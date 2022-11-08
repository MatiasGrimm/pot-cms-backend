using AutoMapper;
using PotShop.API.Auth;
using PotShop.API.Data;
using PotShop.API.Helpers;
using PotShop.API.Models;
using PotShop.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PotShop.API.Controllers.Admin
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthModel _authModel;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;

        public AuthController(IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, IAuthModel authModel)
        {
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _authModel = authModel;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authModel.AuthenticateWithCredentialsAsync(credentials.UserName, credentials.Password);

            return Ok(await GetJwtResult(response));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Post([FromBody] string refreshTokenId)
        {
            var response = await _authModel.AuthenticateWithRefreshTokenAsync(refreshTokenId);

            return Ok(await GetJwtResult(response));
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] string username)
        {
            bool result = await _authModel.SendPasswordResetAsync(username);

            return new OkObjectResult(result);
        }

        [HttpPost("resetPasswordToken")]
        public async Task<IActionResult> ResetPasswordToken([FromBody] ResetPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result = await _authModel.ResetPasswordWithTokenAsync(viewModel.Username, viewModel.Token, viewModel.Password);

            if (!result)
            {
                return BadRequest();
            }

            return new OkObjectResult(true);
        }

        [Authorize]
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authModel.ChangePasswordAsync(viewModel.CurrentPassword, viewModel.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(ErrorHelper.AddErrorsToModelState(result, ModelState));
            }

            return Ok();
        }

        private async Task<AuthResponseViewModel> GetJwtResult(AuthResponse response)
        {
            return new AuthResponseViewModel
            {
                Id = response.User.Id,
                Token = await _jwtFactory.GenerateEncodedToken(response.User.UserName, response.Identity),
                Expires = DateTimeOffset.Now.AddSeconds(_jwtOptions.ValidFor.TotalSeconds),
                RefreshToken = response.RefreshTokenId,
                Username = response.User.UserName,
                Name = response.User.Name,
                Roles = response.Roles.ToList(),
            };
        }
    }
}
