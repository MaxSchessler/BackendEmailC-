using System;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailService : ControllerBase
    {

        private readonly IConfiguration _config;
        
        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        
        [HttpOptions("send-email")]
        public IActionResult Preflight()
        {
            return Ok(); // Respond to OPTIONS requests
        }

        [HttpPost("/send-email")]
        public async Task<IActionResult> SendEmail([FromBody] EmailModel emailModel)
        {
            if (emailModel == null)
            {
                return BadRequest("Invalid email request.");
            }
            
            var response = await SendEmailAsync(emailModel);
            if (response != null)
            {
                return BadRequest("response");
            }
            else
            {
                return Ok("Email sent successfully.");
            }
            
        }

        private async Task<string> SendEmailAsync(EmailModel emailModel)
        {
            try {
                var smtpClient = new SmtpClient("smtp.gmail.com") 
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_config["Email"], _config["APPPassword"]),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage 
                {
                    From = new MailAddress(_config["Email"]),
                    Subject = emailModel.Subject,
                    Body = emailModel.Body,
                    IsBodyHtml = false,
                };

                mailMessage.To.Add(_config["Email"]);
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email send");

                return null!;


            } catch (Exception ex) {
                return ex.Message;
            }
        }
    }
}
