using MailKit;
using MailKit.Net.Imap;

namespace BlazorApp.Server.Mail
{
    public class EmailReceiver
    {
        private readonly string _imapServer = "imap.mail.ru";
        private readonly int _imapPort = 993;
        private readonly string _email = "artemios.sologan@mail.ru";
        private readonly string _password = "URQs6QwzkL9i5nk6C6Mb";

        public void ReceiveEmails()
        {
            using (var client = new ImapClient())
            {
                client.Connect(_imapServer, _imapPort, true); // Подключаемся через IMAP (SSL)
                client.Authenticate(_email, _password); // Аутентификация

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly); // Открываем папку входящих сообщений

                // Поиск новых писем
                var messages = inbox.Fetch(0, -1, MessageSummaryItems.Full | MessageSummaryItems.Flags);

                foreach (var message in messages)
                {
                    var email = inbox.GetMessage(message.UniqueId);
                    Console.WriteLine($"Сообщение от {email.From}: {email.Subject}");
                    // Здесь можно добавить логику обработки входящих писем
                }

                client.Disconnect(true); // Отключаемся от сервера
            }
        }
    }
}
