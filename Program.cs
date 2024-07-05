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
            int workerThreads = 4;
            int completionPortThreads = 4;

            ThreadPool.SetMinThreads(workerThreads, completionPortThreads);
            ThreadPool.SetMaxThreads(workerThreads, completionPortThreads);
            using (HttpClient client = new HttpClient())
            {

                var requestBody = new Dictionary<string, string>
            {
                { "username", "ec32110b5a8f46ff8596ac6e12042539" },
                { "password", "c327d2225298409da288af55744672ae" },
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
                request.Headers.Authorization = new AuthenticationHeaderValue("BASIC", "ZWMzMjExMGI1YThmNDZmZjg1OTZhYzZlMTIwNDI1Mzk6YzMyN2QyMjI1Mjk4NDA5ZGEyODhhZjU1NzQ0NjcyYWU=");
               
                try
                {

                    // Send the request
                  
                    HttpResponseMessage response = await client.SendAsync(request);
                      //response.EnsureSuccessStatusCode();

                    string TokenResponse = await response.Content.ReadAsStringAsync();
                  //  Console.WriteLine(TokenResponse);
                    var tokenData = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(TokenResponse);
                  //  Console.WriteLine(tokenData.access_token);
                    await api_search(tokenData.access_token,0,0);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Request error: " + e.Message);
                }
            }
            
            
    }
        public static async Task api_search(string access_token,int count,int try_count)
        {
            Thread thread = new Thread(new ThreadStart(DoWork));
            thread.Start();
            thread.Join();
            using (StreamWriter writer = new StreamWriter("E:\\API\\LOG.txt"))
            {
                using (HttpClient client = new HttpClient())
             {
                    writer.WriteLine(" ");
                    writer.WriteLine("START TIME   :" + DateTime.Now);
                    for(int j=0; j<=23; j++)
                    {
                        
                        int e_TIME = j + 1;
                        

                        for (int i = count; i < 5500; i=i+50)
                {

                    string jsonBody_1 = @"
                {
                    ""start_index"":";
                            string Count_st = Convert.ToString(i);

                            string jsonBody_2 = @",""max_count"": 50,
                    ""client_code"": ""LEIGHTON"",
                    ""site_code"": ""ALL"",
                    ""location_code"": ""ALL"",
                    ""mode"": ""D"",
                    ""fromdate"": ""2024-04-04T";
                            string s_date;
                            if (j < 10)
                            {
                                s_date = "0" + Convert.ToString(j);
                            }
                            else
                            {
                                s_date = Convert.ToString(j);
                            }
                            string jsonBody_3 = @":00:00.00+01:00"",
                    ""todate"": ""2024-04-04T";
                            string E_date ;
                            if (j < 10)
                            {
                                 E_date = "0"+Convert.ToString(e_TIME);
                            }
                            else
                            {
                                E_date = Convert.ToString(e_TIME);
                            }
                            string jsonBody_4 = @":59:59.00+01:00""
                }";
                            string jsonBody = jsonBody_1 + Count_st + jsonBody_2+s_date+ jsonBody_3+ E_date + jsonBody_4;
                    Console.WriteLine(jsonBody);
                        writer.WriteLine(jsonBody);
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

                                // Write the content to the file
                                if (responseBody == "[]")
                                {
                                    i = 5500;
                                }

                                writer.WriteLine(responseBody);

                            }

                            catch (Exception e)
                            {
                                writer.WriteLine("Request error: " + e.Message + " Try again " + try_count + " start_index " + count);

                                Console.WriteLine("Request error: " + e.Message + " Try again " + try_count + " start_index " + count);
                                if (try_count >= 3)
                                {

                                    writer.WriteLine(" ");
                                    writer.WriteLine("END TIME   :" + DateTime.Now);
                                    break;
                                }
                                else
                                {
                                    writer.WriteLine(" ");
                                    writer.WriteLine("END TIME   :" + DateTime.Now);
                                    await api_search(access_token, count, try_count);
                                }
                            }
                        }
                        

                        Console.WriteLine("Thread has completed.");
                        Console.WriteLine("");
                    Console.WriteLine("END TIME   :"+DateTime.Now);
                    writer.WriteLine(" ");
                    writer.WriteLine("END TIME   :" + DateTime.Now);
                    }
                   
               }

            }
            static void DoWork()
            {
                // Simulate work
                Thread.Sleep(5000);
                Console.WriteLine("Work completed.");
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