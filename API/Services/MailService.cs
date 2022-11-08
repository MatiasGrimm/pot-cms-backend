using PotShop.API.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotShop.API.Services
{
    public class MailService : IMailService
    {
        private readonly IMailSenderService _sender;
        private readonly SiteOptions _siteOptions;

        public MailService(IMailSenderService sender, IOptions<SiteOptions> siteOptions)
        {
            _sender = sender;
            _siteOptions = siteOptions.Value;
        }

        public async Task SendPasswordResetEmailAsync(string email, string username, string token)
        {
            string resetLink = $"{_siteOptions.FrontEndUrl}/resetPassword?token={System.Web.HttpUtility.UrlEncode(token)}";

            await _sender.SendEmailAsync(new EmailSendOptions()
            {
                TargetAddresses = new string[] { email },
                BodyIsHtml = true,
                Subject = "PotShop password reset",
                Body = $@"Hi {username},<br>
We received a request to reset your password for the PotShop license system.<br>
<br>
Use the below link to confirm your request to reset your password.<br>
<a href=""{resetLink}"">{resetLink}</a><br>
<br>
If the link does not work, you may need to copy it into your browser.<br>
<br>
Please contact us at <a href=""{_siteOptions.ContactEmailAddress}"">{_siteOptions.ContactEmailAddress}</a> if you encounter issues with your account.
<br>
<strong>Note: Disregard this email if you did not request your password to be reset.</strong><br>
<br>
Kind regards,<br>
PotShop",
            });
        }

        public async Task SendNewUserEmailAsync(string email, string username, string token)
        {
            string activateLink = $"{_siteOptions.FrontEndUrl}/activate?token={System.Web.HttpUtility.UrlEncode(token)}&email={System.Web.HttpUtility.UrlEncode(email)}";

            await _sender.SendEmailAsync(new EmailSendOptions()
            {
                TargetAddresses = new string[] { email },
                BodyIsHtml = true,
                Subject = "PotShop account created",
                Body = $@"Hi {username},<br>
You have just been created as a user in the PotShop license system.<br>
<br>
Use the below link to activate your account.<br>
<a href=""{activateLink}"">Activate account</a><br>
<br>
If the link does not work, you may need to copy it into your browser.<br>
<br>
Please contact us at <a href=""{_siteOptions.ContactEmailAddress}"">{_siteOptions.ContactEmailAddress}</a> if you encounter any issues with the system or have any questions.
<br>
<br>
Kind regards,<br>
PotShop",
            });

            await _sender.SendEmailAsync(new EmailSendOptions()
            {
                TargetAddresses = _siteOptions.AdminEmailAdresses,
                Subject = $"PotShop: new user {email} created",
                BodyIsHtml = false,
                Body = "",
            });
        }

        public async Task SendAdminErrorEmailAsync(string title, string text, Exception exception)
        {
            string body;

            if (exception != null)
            {
                body = text + Environment.NewLine + Environment.NewLine + exception?.ToString();
            }
            else
            {
                body = text;
            }

            await _sender.SendEmailAsync(new EmailSendOptions()
            {
                TargetAddresses = _siteOptions.AdminEmailAdresses,
                Subject = $"PotShop error: {title}",
                BodyIsHtml = false,
                Body = body,
            });
        }
    }
}
