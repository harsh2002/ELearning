using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using ELearning.Model;
using Microsoft.Extensions.Configuration;

namespace ELearning.Utility
{

    public class EmailHelper
    {
        private AppConfiguration settings;
        MailAddress fromAddress;
        MailAddress toAddress;
        MailMessage message;
        public EmailHelper(IConfiguration config)
        {
            settings = new AppConfiguration(config);
        }

        public void SendEmail(UserModel emailModel)
        {
            fromAddress = new MailAddress(settings.FromEmailAddress);
            toAddress = new MailAddress(emailModel.Email);
            message = new MailMessage(fromAddress, toAddress);
            message.Subject = "Password Request";

            message.Body = EmailBody(emailModel);
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient()
            {
                Host = settings.SMTPServer,
                Port = settings.SMTPPort,
                EnableSsl = true,
                UseDefaultCredentials = false,

                Credentials = new NetworkCredential(settings.FromEmailAddress, settings.smtpPassword)
            };

            client.Send(message);
        }

        private string EmailBody(UserModel emailModel)
        {
            StringBuilder templateString = new StringBuilder();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Documents","template.html");
            FileStream fileStream = new FileStream(path, FileMode.Open);
            string readString;
            using (StreamReader reader = new StreamReader(fileStream))
            {
                readString = reader.ReadToEnd();
            }
            templateString.Append(readString);
            Dictionary<string, string> keyValuePair = new Dictionary<string, string>();

            keyValuePair.Add("__Name__", emailModel.Name);
            keyValuePair.Add("__Email__", emailModel.Email);
            keyValuePair.Add("__Password__", emailModel.Password);

            foreach (var obj in keyValuePair)
            {
                templateString.Replace(obj.Key, obj.Value);
            }

            return templateString.ToString();
        }


        public void SendContactEmail(ContactModel emailModel)
        {
            fromAddress = new MailAddress(emailModel.Email);
            toAddress = new MailAddress(settings.adminEmailAddress);
            message = new MailMessage(fromAddress, toAddress);
            message.Subject = "Contact";

            message.Body = EmailBody(emailModel);
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient()
            {
                Host = settings.SMTPServer,
                Port = settings.SMTPPort,
                EnableSsl = true,
                UseDefaultCredentials = false,

                Credentials = new NetworkCredential(settings.FromEmailAddress, settings.smtpPassword)
            };

            client.Send(message);
        }

        private string EmailBody(ContactModel emailModel)
        {
            StringBuilder templateString = new StringBuilder();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Documents/templatecontact.html");
            FileStream fileStream = new FileStream(path, FileMode.Open);
            string readString;
            using (StreamReader reader = new StreamReader(fileStream))
            {
                readString = reader.ReadLine();
            }

            Dictionary<string, string> keyValuePair = new Dictionary<string, string>();

            keyValuePair.Add("__Name__", emailModel.Name);
            keyValuePair.Add("__Email__", emailModel.Email);
            keyValuePair.Add("__Phone__", emailModel.Phone);
            keyValuePair.Add("__Message__", emailModel.Message);

            templateString.Append(readString);

            foreach (var obj in keyValuePair)
            {
                templateString.Replace(obj.Key, obj.Value);
            }

            return templateString.ToString();
        }
    }
}
