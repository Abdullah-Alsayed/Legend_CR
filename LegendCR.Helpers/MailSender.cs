using System;
using System.Net.Mail;
//

namespace LegendCR.BLL
{
    public class MailSender
    {
        //string host = "mail.mostakhlas.com";
        //int port = 8889;
        ////int port = 25;
        //string password = "010MostakhlaS010@";
        //string fromMail = "noreply@mostakhlas.com";

        string host = "smtp-mail.outlook.com";
        int port = 587;
        //int port = 25;
        string password = "";
        string fromMail = "ahmedexe2007@gmail.com";

        bool enableSsl = false;
        bool useDefaultCredentials = false;

        public MailSender()
        {
        }

        //Satrt Send Email Function
        public string SendMail(string to, string subject, string body, bool isBodyHtml = true)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();
                string msg = string.Empty;
                MailAddress fromAddress = new MailAddress(this.fromMail);
                message.From = fromAddress;
                var toList = to.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < toList.Length; i++)
                {
                    message.To.Add(toList[i]);
                }
                message.Subject = subject;
                message.IsBodyHtml = isBodyHtml;
                message.Body = body;
                smtpClient.Host = this.host;
                smtpClient.Port = this.port;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = this.useDefaultCredentials;
                smtpClient.Credentials = new System.Net.NetworkCredential(this.fromMail, this.password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(message);
                return "true";
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return msg;
            }



            
        }
        //End Send Email Function

    }
}
