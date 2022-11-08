using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace PotShop.API.Models
{
    public interface IAuthModel
    {
        Task<AuthResponse> AuthenticateWithCredentialsAsync(string username, string password);
        Task<AuthResponse> AuthenticateWithRefreshTokenAsync(string refreshTokenId);
        Task<IdentityResult> ChangePasswordAsync(string currentPassword, string newPassword);
        Task<bool> ResetPasswordWithTokenAsync(string username, string token, string password);
        Task<bool> SendPasswordResetAsync(string username);
    }
}