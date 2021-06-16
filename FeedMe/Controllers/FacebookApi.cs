using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FeedMe.Controllers
{
    public class FacebookApi
    {
        private readonly string PAGE_ID;
        private readonly string ACCESS_TOKEN;
        private const string BASE_URL = "https://graph.facebook.com/";

        public FacebookApi(string pageId, string accessToken)
        {
            PAGE_ID = pageId;
            ACCESS_TOKEN = accessToken;
        }

        public async Task<string> PostMessage(string message)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BASE_URL);

                var content = new Dictionary<string, string>
                {
                    { "access_token", ACCESS_TOKEN },
                    { "message", message }
                };
                var encodedContent = new FormUrlEncodedContent(content);

                var response = await httpClient.PostAsync($"{PAGE_ID}/feed", encodedContent);
                var result = response.EnsureSuccessStatusCode();

                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}