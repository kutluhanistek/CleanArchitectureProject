using CleanArchitecture.Application.Services;
using GenericEmailService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services
{
    public sealed class MailService : IMailService
    {
        public async Task SendMailAsync(List<string> emails, string body, string subject, List<Stream> attachments = null)
        {

            EmailConfigurations configurations = new(
            
                Smtp : "",
                Password: "",
                Port: 587,
                SSL: false,
                Html: true
            );

            EmailModel<Stream> model = new(
            
                Configurations:configurations,
                FromEmail:"",
                ToEmails: emails,
                Subject:subject,
                Body:body,
                Attachments:attachments

            );
            await EmailService.SendEmailWithMailKitAsync(model);
                
        }
    }
}
