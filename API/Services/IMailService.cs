using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotShop.API.Services
{
    public interface IMailService
    {
        Task SendAdminErrorEmailAsync(string title, string text, Exception exception);

        Task SendNewUserEmailAsync(string email, string username, string token);

        Task SendPasswordResetEmailAsync(string email, string username, string token);
    }
}
