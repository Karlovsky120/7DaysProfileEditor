using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Services;

namespace _7DaysProfileEditor.Web {
    /// <summary>
    /// Summary description for Submission
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Submission : WebService {

        [WebMethod]
        public void SubmitFile(string message, string filename, byte[] filecontent) {

            string emailto = System.Configuration.ConfigurationManager.AppSettings["emailto"];
            string emailfrom = System.Configuration.ConfigurationManager.AppSettings["emailfrom"];
            string emailpassword = System.Configuration.ConfigurationManager.AppSettings["emailpassword"];
            string emailserver = System.Configuration.ConfigurationManager.AppSettings["emailserver"];
            string emailport = System.Configuration.ConfigurationManager.AppSettings["emailport"];

            byte[] dataPassword = Convert.FromBase64String(emailpassword);
            string decodedPassword = Encoding.UTF8.GetString(dataPassword);

            var client = new SmtpClient(emailserver, int.Parse(emailport)) {
                Credentials = new NetworkCredential(emailfrom, decodedPassword),
                EnableSsl = true
            };

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(emailfrom);

            foreach (string email in emailto.Split(",".ToCharArray())) {
                mail.To.Add(email);
            }

            mail.Subject = "File Submission: " + filename;
            mail.Body = message;
            mail.IsBodyHtml = true;
            try {
                mail.Attachments.Add(new Attachment(new MemoryStream(filecontent), filename));
            }
            catch {
                //?
            }
            
            client.Send(mail);
        }
    }
}
