using Abp.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CaznerMarketplaceBackendApp.Users.Dto;

namespace CaznerMarketplaceBackendApp.Users
{
 
    public class EmailHelper
    {
        private IConfiguration _configuration;
        public EmailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmail(EmailDto Model)
        {
            try
            {
                string UserName = string.Empty;
                string Password = string.Empty;
                string Host = string.Empty;
                string Port = string.Empty;
                string IsEnableSSL = string.Empty;
                string FromAddress = string.Empty;

             
                     UserName = _configuration.GetValue<string>("EmailSMTPSettings:username");
                     Password = _configuration.GetValue<string>("EmailSMTPSettings:password");
                     Host = _configuration.GetValue<string>("EmailSMTPSettings:host");
                     Port = _configuration.GetValue<string>("EmailSMTPSettings:port");
                     IsEnableSSL = _configuration.GetValue<string>("EmailSMTPSettings:IsSSL");
                     FromAddress = _configuration.GetValue<string>("EmailSMTPSettings:FromAddress");
                

                MailMessage Message = new MailMessage();               
                SmtpClient emailClient = new SmtpClient(Host);

                if (Model != null)
                {
                    Message.To.Add(Model.EmailTo);
                    Message.From = new MailAddress(FromAddress);

                    Message.Subject = Model.Subject;
                    Message.Body = Model.Body;

                    Message.IsBodyHtml = true;

                    emailClient.Port = Convert.ToInt16(Port);
                    emailClient.Credentials = new NetworkCredential(UserName, string.IsNullOrEmpty(Password)? "" : Password);

                    if (string.IsNullOrEmpty(IsEnableSSL))
                    {
                        emailClient.EnableSsl = false;
                    }
                    else
                    {
                        emailClient.EnableSsl = IsEnableSSL.ToLower() == "true" ? true : false;
                    }

                    emailClient.Send(Message);                   

                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {             
                return false;
            }
        }
    }
}
