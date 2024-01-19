using System.Net.Mail;

namespace CleanArchitecture.Application.Services;

public interface IMailService
{
    Task SendMailAsync(List<string> emails, string body, string subject, List<Stream> attachments = null);
}
