using System.Threading.Tasks;

namespace FamUnion.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
