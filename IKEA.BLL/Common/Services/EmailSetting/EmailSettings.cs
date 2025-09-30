using IKEA.DAL.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Common.Services.EmailSetting
{
    public class EmailSettings : IEmailSettings
    {
        public void SendEmail(EmailSend emailSend)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            // sender - reciver
            //reciver = user try password
            client.Credentials = new NetworkCredential("alimazen09811@gmail.com", "rzjmbvexuyhtwpgr");//Generate password
            client.Send("alimazen09811@gmail.com",emailSend.To,emailSend.Subject,emailSend.Body);

        }
    }
}
