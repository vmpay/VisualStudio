using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;

namespace WebAPIProgram
{
    static class Program
    {
        static void Main()
        {
            MakeRequest(); ;
            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }

        static async void MakeRequest()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            //String queryString = null;

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "{subscription key}");

            var uri = "https://calc274102.azure-api.net/Calc/xpowy?a={a}&b={b}&" + queryString;

            var response = await client.GetAsync(uri);
        }
    }
}
