using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace EFreshStoreCore.Api.Utility
{
    public class Email
    {
        public static void SendEmail(string subject, string body, MailAddress toMailAddress)
        {
            var fromAddress = new MailAddress("meghna.ecommerce@gmail.com", "Meghna e-Commerce");
            //var toAddress = new MailAddress("to@example.com", "To Name");
            var receivers = new List<MailAddress>
            {
                toMailAddress
            };
            const string fromPassword = "Meghna2020";
            //const string subject = "Subject";
            //const string body = "Body";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            foreach (var toAddress in receivers)
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            //using (var message = new MailMessage(fromAddress, toAddress)
            //{
            //    Subject = subject,
            //    Body = body
            //})
            //{
            //    smtp.Send(message);
            //}
        }

        //check internet connection
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static void SendEmailWithAttachment(string subject, string body, MailAddress toMailAddress, Attachment pdf)
        {
            var fromAddress = new MailAddress("meghna.ecommerce@gmail.com", "Meghna e-Commerce");           
            var receivers = new List<MailAddress>
            {
                toMailAddress
            };
            const string fromPassword = "Meghna2020";           
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            foreach (var toAddress in receivers)
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    message.Attachments.Add(pdf);
                    smtp.Send(message);
                }
            }
        }
    }
}