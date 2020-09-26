using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scrape.Utility
{
    public static class WebHelper
    {

        public async static System.Threading.Tasks.Task<string> GetHttpContent(string requestUri)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(requestUri);
                Console.Out.WriteLine(response.Headers.ToString());
                HttpContent content = response.Content;
                string result = await content.ReadAsStringAsync();
                return result;

            }
        }



    }
}
