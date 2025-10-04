using ECommerce.Application.IServices;
using ECommerce.Domain.AuthenticationHepler;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _configuration;
        public EmailService(IOptions<EmailConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }
        public async Task SendEmailAsync(string mailTo, string subject, string body)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_configuration.Email),
                Subject = subject,
            };

            email.To.Add(MailboxAddress.Parse(mailTo));

            var builder = new BodyBuilder();
            builder.HtmlBody = $@"
                    <div style=""background-color: #f9f9f9; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border-radius: 12px; box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);"">
                    <div style=""background-color: #1e3d58; color: #ffffff; padding: 15px; text-align: center; border-top-left-radius: 12px; border-top-right-radius: 12px; font-size: 22px; font-weight: bold;"">
                        <span style=""letter-spacing: 1px;"">ECommerce</span>
                    </div>
                    <div style=""color: #333333; padding: 30px; font-size: 16px; line-height: 1.6;"">
                        <p style=""margin-bottom: 10px;"">Dear User,</p>
                        <p style=""margin-bottom: 20px;"">Thank you for showing interest in our ECommerce platform! We’re excited to have you with us.</p>
                        <p style=""text-align: center;"">
                            <a href=""{body}"" style=""background-color: #1e3d58; color: #fff; padding: 12px 25px; text-decoration: none; border-radius: 8px; font-size: 16px; font-weight: bold; box-shadow: 0 5px 15px rgba(0, 0, 0, 0.2);"">Confirm Your Email</a>
                        </p>
                        <p style=""margin-top: 20px;"">Once confirmed, we will notify you of our decision within the next 24 hours. Stay tuned!</p>
                        <p style=""margin-top: 20px;"">Best Regards,</p>
                        <p>The ECommerce Team</p>
                        <div style=""text-align: center; margin-top: 30px; font-size: 12px; color: #999999;"">
                            <p>We're here to assist you anytime. Feel free to reach out with any questions!</p>
                        </div>
                    </div>
                </div>
                ";

            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_configuration.DisplayName, _configuration.Email));

            using var smtp = new SmtpClient();
            smtp.Connect(_configuration.Host, _configuration.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration.Email, _configuration.Password);
            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }
    }
}
