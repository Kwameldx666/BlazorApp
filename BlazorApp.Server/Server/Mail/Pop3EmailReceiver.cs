using MailKit.Net.Pop3;

namespace BlazorApp.Server.Mail
{
    public class Pop3EmailReceiver
    {
        private readonly string _pop3Server = "pop.mail.ru";
        private readonly int _pop3Port = 995;
        private readonly string _email = "artemios.sologan@mail.ru";
        private readonly string _password = "URQs6QwzkL9i5nk6C6Mb";

        public void ReceiveEmails()
        {
            using (var client = new Pop3Client())
            {
                client.Connect(_pop3Server, _pop3Port, true); // Подключаемся через POP3 (SSL)
                client.Authenticate(_email, _password); // Аутентификация

                // Получаем все письма
                var messageCount = client.Count;
                for (int i = 0; i < messageCount; i++)
                {
                    var email = client.GetMessage(i);
                    Console.WriteLine($"Сообщение от {email.From}: {email.Subject}");
                    // Здесь можно добавить логику обработки входящих писем
                }

                client.Disconnect(true); // Отключаемся от сервера
            }
        }
    }
}
