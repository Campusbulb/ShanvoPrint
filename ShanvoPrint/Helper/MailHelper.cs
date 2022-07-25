﻿using MailKit.Net.Smtp;
using MailKit.Security;

using Microsoft.Extensions.Configuration;

using MimeKit;
using System.Threading.Tasks;

namespace ShanvoPrint.Helper
{
    public class MailHelper
    {
        private readonly IConfiguration _configuration;

        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Send(string from, string to, string subject, string content)
        {
            try
            {
                var username = _configuration["Gmail:Username"];
                var password = _configuration["Gmail:Password"];


                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(from));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = "Get Quote Request";
                var builder = new BodyBuilder
                {
                    HtmlBody = string.Format(content)
                };


                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 465, SecureSocketOptions.Auto);
                smtp.Authenticate(username, password);
                smtp.Send(email);
                smtp.Disconnect(true);

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}

