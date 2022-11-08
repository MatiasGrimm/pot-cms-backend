using System.Threading.Tasks;

namespace PotShop.API.Services
{
    public interface IMailSenderService
    {
        Task SendEmailAsync(EmailSendOptions request);
    }
}
