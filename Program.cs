using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace API_BAS
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (HttpClient client = new HttpClient())
            {
                var requestBody = new Dictionary<string, string>
            {
                { "username", "6dce2f9ece1e45d1aa0d3507f2eabc86" },
                { "password", "8b7f51d2aef34eccafb7ac00363556f6" },
                { "grant_type", "password" }
            };

                // Create the content for the request
                FormUrlEncodedContent content = new FormUrlEncodedContent(requestBody);
             /*   var postData = new
                {
                    username = "36331792f07047349abc3e606d4e6889",
                    password = "ccd8313e81a44ea39181f2974a59553a",
                    grant_type = "password"
                };

                string json = JsonConvert.SerializeObject(postData);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                */

                // Create the request message
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://mybas.cloud/prod/api/vendor/GetAuthorize")
                {
                    Content = content
                };

                // Add headers to the request message
                // request.Headers.Add("Authorization", ""BASIC", "MzYzMzE3OTJmMDcwNDczNDlhYmMzZTYwNmQ0ZTY4ODk6Y2NkODMxM2U4MWE0NGVhMzkxODFmMjk3NGE1OTU1M2E="");
                request.Headers.Authorization = new AuthenticationHeaderValue("BASIC", "NmRjZTJmOWVjZTFlNDVkMWFhMGQzNTA3ZjJlYWJjODY6OGI3ZjUxZDJhZWYzNGVjY2FmYjdhYzAwMzYzNTU2ZjY=");
               
                try
                {

                    // Send the request
                  
                    HttpResponseMessage response = await client.SendAsync(request);
                      //response.EnsureSuccessStatusCode();

                    string TokenResponse = await response.Content.ReadAsStringAsync();
                  //  Console.WriteLine(TokenResponse);
                    var tokenData = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(TokenResponse);
                  //  Console.WriteLine(tokenData.access_token);
                    await api_search(tokenData.access_token);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Request error: " + e.Message);
                }
            }
            
            
    }
        public static async Task api_search(string access_token)
        {
            using (HttpClient client = new HttpClient())
            {
                Console.WriteLine("");
                Console.WriteLine("START TIME   :" + DateTime.Now);

                for (int i = 0; i < 5500; i=i+50)
                {

                    string jsonBody_1 = @"
                {
                    ""start_index"":";
                    string Count_st = Convert.ToString(i);

                    string jsonBody_2 = @",""max_count"": 50,
                    ""client_code"": ""URBANRISE_LIFESTYLES"",
                    ""site_code"": ""ALL"",
                    ""location_code"": ""ALL"",
                    ""mode"": ""D"",
                    ""fromdate"": ""2024-06-01T19:20:30.45+01:00"",
                    ""todate"": ""2024-06-15T19:20:30.45+01:00""
                }";
                    string jsonBody = jsonBody_1 + Count_st + jsonBody_2;
                    Console.WriteLine(jsonBody);
                    /* 
                       string jsonBody = @"
                       {
                           ""start_index"": 1,
                           ""max_count"": 50,
                           ""client_code"": ""URBANRISE_LIFESTYLES"",
                           ""site_code"": ""ALL"",
                           ""location_code"": ""ALL"",
                           ""mode"": ""D"",
                           ""fromdate"": ""2024-06-01T19:20:30.45+01:00"",
                           ""todate"": ""2024-06-05T19:20:30.45+01:00""
                       }";
                      */

                    // Create the content for the request
                    StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    // Create the request message
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://mybas.cloud/prod/api/vendor/SearchTransaction")
                    {
                        Content = content
                    };
                    // Add the authorization header
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);


                    try
                    {
                        // Make the GET request
                        HttpResponseMessage response = await client.SendAsync(request);

                        // Ensure the request was successful
                        // response.EnsureSuccessStatusCode();

                        // Read the response content as a string
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseBody);
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine("Request error: " + e.Message);
                    }
                    Console.WriteLine("");
                    Console.WriteLine("END TIME   :"+DateTime.Now);
                }

            }
        }
        public class TokenResponse
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public string refresh_token { get; set; }
            public string refresh_token_expires_in { get; set; }
            public string client_id { get; set; }
            public string issued { get; set; }
            public string expires { get; set; }
        }
       

    }
}