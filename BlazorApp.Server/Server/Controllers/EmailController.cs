using MailKit.Net.Imap;
using MailKit.Net.Pop3;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Server.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly string _pop3Server = "pop.mail.ru";
        private readonly string _imapServer = "imap.mail.ru";
        private readonly int _pop3Port = 995;
        private readonly int _imapPort = 993;
        private readonly string _email = "artemios.sologan@mail.ru";
        private readonly string _password = "URQs6QwzkL9i5nk6C6Mb"; 

        // Метод для получения последнего сообщения с POP3
        [HttpGet("fetch-pop3")]
        public async Task<IActionResult> FetchEmailPop3()
        {
            try
            {
                using (var client = new Pop3Client())
                {
                    await client.ConnectAsync(_pop3Server, _pop3Port, SecureSocketOptions.SslOnConnect);
                    await client.AuthenticateAsync(_email, _password);

                    var messageCount = client.Count;

                    if (messageCount == 0)
                    {
                        return NotFound(new { Message = "No messages found." });
                    }

                    var message = await client.GetMessageAsync(messageCount - 1);

                    var emailResponse = new
                    {
                        From = message.From.ToString(),
                        Subject = message.Subject,
                        Body = message.TextBody
                    };

                    await client.DisconnectAsync(true);

                    return Ok(emailResponse);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        // Метод для получения последнего сообщения с IMAP
        [HttpGet("fetch-imap")]
        public async Task<IActionResult> FetchEmailImap()
        {
            try
            {
                using (var client = new ImapClient())
                {
                    await client.ConnectAsync(_imapServer, _imapPort, SecureSocketOptions.SslOnConnect);
                    await client.AuthenticateAsync(_email, _password);

                    var inbox = client.Inbox;
                    await inbox.OpenAsync(MailKit.FolderAccess.ReadOnly);

                    var messageCount = inbox.Count;

                    if (messageCount == 0)
                    {
                        return NotFound(new { Message = "No messages found." });
                    }

                    var message = await inbox.GetMessageAsync(messageCount - 1);

                    var emailResponse = new
                    {
                        From = message.From.ToString(),
                        Subject = message.Subject,
                        Body = message.TextBody
                    };

                    await client.DisconnectAsync(true);

                    return Ok(emailResponse);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
