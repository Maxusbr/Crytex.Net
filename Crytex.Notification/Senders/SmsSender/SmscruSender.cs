using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Crytex.Notification
{
    public class SmscruSender : ISmsSender
    {
        private string _login;
        private string _password;

        public SmscruSender(string serviceLogin, string servicePassword)
        {
            _login = serviceLogin;
            _password = servicePassword;
        }

        public async Task Send(string phoneNumber, string messageText)
        {
            var requestUri = GetRequestUri(phoneNumber, messageText);

            var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };

            using (var client = new HttpClient(handler))
            {
                var requestTask = client.GetAsync(requestUri);
                await requestTask;

                var response = requestTask.Result;
                Console.WriteLine($"Response status code: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException($"Request failed witd status code {(int)response.StatusCode}({response.StatusCode})");
                }
            }
        }

        private string GetRequestUri(string phoneNumber, string messageText)
        {
            var uri = "https://smsc.ru/sys/send.php?login="+ _login + "&psw=" + _password +"&phones=" + phoneNumber + "&mes=" + messageText;

            return uri;
        }
    }
}
