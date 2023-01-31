using PotShop.API.Auth;
using PotShop.API.Data;
using PotShop.API.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PotShop.API.Models
{
    public class AuthModel : IAuthModel
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly ApiDbContext _context;
        private readonly ILogger<AuthModel> _logger;
        private readonly ClaimsPrincipal _user;

        public AuthModel(UserManager<ApiUser> userManager, IJwtFactory jwtFactory, ApiDbContext context, ILogger<AuthModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _context = context;
            _logger = logger;
            _user = httpContextAccessor.HttpContext.User;
        }

        public async Task<AuthResponse> AuthenticateWithCredentialsAsync(string username, string password)
        {
            ClaimsIdentity identity = null;
            IList<string> roles = null;

            var user = await _userManager.Users
                .Where(x => x.NormalizedUserName == username).FirstOrDefaultAsync();

            if (user != null)
            {
                if (user.IsDisabled)
                {
                    _logger.LogWarning("User {username} {userId} is disabled", user.UserName, user.Id);
                    throw new InvalidOperationException("User is disabled");
                }

                // check the credentials
                if (await _userManager.CheckPasswordAsync(user, password))
                {
                    _logger.LogInformation("Valid login for user {username} {userId}", user.UserName, user.Id);

                    roles = await _userManager.GetRolesAsync(user);

                    identity = _jwtFactory.GenerateClaimsIdentity(user.UserName, user.Id, roles);
                }
                else
                {
                    _logger.LogInformation("Wrong password for {username} {userId}", user.UserName, user.Id);
                }
            }
            else
            {
                _logger.LogInformation("Unknown user login attempt {username}", username);
            }

            if (identity == null)
            {
                throw new InvalidOperationException("Invalid username or password");
            }

            var refreshToken = _context.AuthRefreshTokens.Where(x => x.User == user && x.Enabled).FirstOrDefault();

            if (refreshToken == null)
            {
                refreshToken = new AuthRefreshToken()
                {
                    User = user,
                    Enabled = true
                };

                _context.AuthRefreshTokens.Add(refreshToken);

                _context.SaveChanges();
            }

            return new AuthResponse()
            {
                User = user,
                Identity = identity,
                Roles = roles,
                RefreshTokenId = Convert.ToBase64String(refreshToken.Id.ToByteArray()),
            };
        }


        public async Task<AuthResponse> AuthenticateWithRefreshTokenAsync(string refreshTokenId)
        {
            Guid targetGuid = new(Convert.FromBase64String(refreshTokenId));

            var token = await _context.AuthRefreshTokens
                .Include(x => x.User)
                .Where(x => x.Id == targetGuid)
                .FirstOrDefaultAsync();

            if (token == null)
            {
                _logger.LogWarning("Unknown refresh token {tokenId}", refreshTokenId);

                throw new NotAuthorizedException();
            }

            if (!token.Enabled)
            {
                _logger.LogWarning("Attempt to use refresh token {tokenId} for user {username} that is not enabled", refreshTokenId, token.User.UserName);

                throw new NotAuthorizedException();
            }

            _logger.LogDebug("Generating token for user {username} {userId} from valid refresh token {tokenId}", token.User.UserName, token.User.Id, token.Id);

            IList<string> roles = await _userManager.GetRolesAsync(token.User);

            return new AuthResponse()
            {
                User = token.User,
                Identity = _jwtFactory.GenerateClaimsIdentity(token.User.UserName, token.User.Id, roles),
                RefreshTokenId = Convert.ToBase64String(token.Id.ToByteArray()),
                Roles = roles,
            };
        }

        public async Task<IdentityResult> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            var user = _user.GetUser(_context);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            _logger.LogInformation("User {username} password change. Succeeded: {success}", user.UserName, result.Succeeded);

            return result;
        }

        public async Task<bool> SendPasswordResetAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                _logger.LogWarning("Cannot send reset password to user with username {username} that could not be found", user);

                return false;
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            //await _mailService.SendPasswordResetEmailAsync(user.Email, user.Name ?? user.Email, token);

            _logger.LogInformation("Sent password reset token for {userId} {email}", user.Id, user.Email);

            return true;
        }

        public async Task<bool> ResetPasswordWithTokenAsync(string username, string token, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                _logger.LogWarning("Cannot reset password with token for username {username} that could not be found", username);
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, token, password);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Bad request for password reset with token for {userId} {email}: {error}", user.Id, user.Email, string.Join(", ", result.Errors.Select(x => x.Description)));
                return false;
            }

            _logger.LogInformation("Password reset for {userId} {email} with token", user.Id, user.Email);

            return true;
        }
    }
}
