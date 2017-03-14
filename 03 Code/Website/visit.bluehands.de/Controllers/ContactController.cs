using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Mail;
using System.Web.Http;

namespace visit.bluehands.de.Controllers
{
    public class ContactController : ApiController
    {
        public string Post(FormDataCollection formData)
        {
            try
            {
                var email = new MailMessage();
                var from = formData.GetValues("email").FirstOrDefault();
                if (!string.IsNullOrEmpty(from))
                {
                    try
                    {
                        email.From = new MailAddress(from);
                    }
                    catch (Exception)
                    {
                    }
                }
                email.To.Add("feedback@bluehands.de");
                email.Subject = "Stadtgeist Karlsruhe Feedback";
                email.Body = formData.GetValues("name").FirstOrDefault() + Environment.NewLine + formData.GetValues("message").FirstOrDefault();

                var client = new SmtpClient("mxa.expurgate.de");
                client.Send(email);

                return "success";
            }
            catch (Exception)
            {
                return "error";
            }
        }

        public string Get()
        {
            return "Hello World";
        }
    }
}
