using NullableFox.AoXiangToDoList.Services.Interfaces;
using NullableFox.AoXiangToDoList.Services.Transmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Protection.PlayReady;

namespace NullableFox.AoXiangToDoList.Services
{
    internal class HttpService
    {
        string url;
        HttpClient client;
        public HttpService(int backEndPort)
        {
            url = $"http://127.0.0.1:{backEndPort}/";
            client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(300);
        }

        public event EventHandler<NotificationPacket> NotificationReceived;

        public async Task<ResponsePacket> RequestAsync(RequestPacket requestPacket)
        {
            string str = requestPacket.ToJsonString();
            var content = new StringContent(str);

            // Set the content type header to text/plain
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");

            // Send a POST request to the specified URL and get the response
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            // Read the response body content as a string and return it
            var result = await response.Content.ReadAsStringAsync();

            var responsePacket = Utilities.JsonHelper.ObjectFromJsonString<ResponsePacket>(result);
            return responsePacket;
        }

        public async Task<ResponsePacket> RequestAsync(RequestType type, string content)
        {
            return await RequestAsync(new RequestPacket() { RequestType = type, Content = content });
        }
    }
}
