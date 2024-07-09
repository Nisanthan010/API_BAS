using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using static API_BAS.Program;

namespace API_BAS
{
    public class Program
    {
        static void Main(string[] args)
        {
          
            Program new_token = new Program();
                     api_search(new_token.Token_gen(), 0,0,0);
                  
        }
        public string Token_gen()
        {
            string token = null;
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

                    HttpResponseMessage response =  client.Send(request);
                    //response.EnsureSuccessStatusCode();

                    string TokenResponse =  response.Content.ReadAsStringAsync().Result;
                    //  Console.WriteLine(TokenResponse);
                    var  tokenData = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(TokenResponse);
                    //  Console.WriteLine(tokenData.access_token);
                  //  await api_search(tokenData.access_token, 0, 0,0);

                    token= tokenData.access_token;
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR : Request error: " + e.Message);
                }
               
            }
            return token;
        }
        public static  void api_search(string access_token,int count,int try_count,int date_count)
        {
           try_count c =new try_count();
            c.t_count = try_count;

            c.t_count = try_count;
           using (StreamWriter writer = new StreamWriter("E:\\API\\LOG"+Convert.ToString(c.t_count) +".txt"))
            {
                using (HttpClient client = new HttpClient())
             {
                  
                    Console.WriteLine( "INFO : START TIME   :" + DateTime.Now + "  TRY COUNT :"+ c.t_count);
                    writer.WriteLine(DateTime.Now + " INFO : START TIME   :" + DateTime.Now + "  TRY COUNT :" + c.t_count);

                    for (int j= date_count; j<=23; j++)
                    {
                        c.date_count = j;
                       
                           for (int M = 0; M <= 58; M=M+10)
                        {
                           // for (int s = 0; s <= 58; s = s + 30)
                            {
                                for (int i = count; i < 5500; i = i + 50)
                                {
                                    
                                    c.in_count = i;
                                    c.actual_count = 1 + c.actual_count;
                                    Console.WriteLine(" START TIME CALL  :" + DateTime.Now + " INDEX COUNT ON " +c.t_count+" TRY :" + c.actual_count);
                                    writer.WriteLine(DateTime.Now + " INFO : START TIME CALL  :" + DateTime.Now + " INDEX COUNT ON " + c.t_count + " TRY :" + c.actual_count);


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
                                    string jsonBody_3 = @":";
                                    
                                    string s_min;
                                    if (M < 10)
                                    {
                                        s_min = "0" + Convert.ToString(M);
                                    }
                                    else
                                    {
                                        s_min = Convert.ToString(M);
                                    }
                                    
                                    string jsonBody_4 = @":00";
                                     /*      string s_sec;
                                    if (s < 10)
                                    {
                                        s_sec = "0" + Convert.ToString(s);
                                    }
                                    else
                                    {
                                        s_sec = Convert.ToString(s);
                                    }
                                    */
                                    string jsonBody_5 = @".00+01:00"",
                    ""todate"": ""2024-04-04T";
                                    string E_date;
                                    if (j < 10)
                                    {
                                        E_date = "0" + Convert.ToString(j);
                                    }
                                    else
                                    {
                                        E_date = Convert.ToString(j);
                                    }
                                    string jsonBody_6 = @":";
                                        string E_min;


                                    if (M >= 50)
                                    {
                                        E_min = Convert.ToString(59);
                                    }
                                    else
                                    {
                                      /* if (M < 10)
                                        {
                                            E_min = "0" + Convert.ToString(M);
                                        }
                                        else*/
                                        { 
                                            E_min = Convert.ToString(M + 10);
                                        }
                                           
                                    }

                                    
                                    string jsonBody_7 = @":59";
                                    /*                                  string E_sec;
                                    if (s >= 30)
                                    {
                                        E_sec = Convert.ToString(59);
                                    }
                                    else
                                    {
                                        E_sec = Convert.ToString(s+30 );
                                    }
*/
                                    string jsonBody_8 = @".00+01:00""
                                      }";
                                    string jsonBody = jsonBody_1 + 
                                        Count_st +
                                        jsonBody_2 + 
                                        s_date +
                                        jsonBody_3 +
                                        s_min +
                                        jsonBody_4 +
                                     //   s_sec+
                                        jsonBody_5 +
                                        E_date + 
                                        jsonBody_6 + 
                                      E_min + 
                                        jsonBody_7 + 
                                       // E_sec+
                                        jsonBody_8;
                                    Console.WriteLine("INFO :" +jsonBody);
                                    writer.WriteLine(DateTime.Now + " INFO :" + jsonBody);
                                    
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
                                           ""todate"": ""2024-06-05T19:59:59.00+01:00""
                                       }";
                                      */
                                    Program ping = new Program();
                                    ping.IsHostReachable("mybas.cloud");



                                    // Create the content for the request
                                    StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                                    // Create the request message
                                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://mybas.cloud/prod/api/vendor/SearchTransaction")
                                    {
                                        Content = content
                                    };
                                    // Add the authorization header
                                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
                                    string responseBody = null;

                                    try
                                    {
                                       
                                        HttpResponseMessage response =  client.Send(request);                
                                       
                                        responseBody =  response.Content.ReadAsStringAsync().Result;
                                       
                                        Console.WriteLine("INFO :END TIME CALL  :" + DateTime.Now);
                                        Console.WriteLine(responseBody);
                                        writer.WriteLine(DateTime.Now + " INFO :END TIME CALL  :" + DateTime.Now);

                                        if (responseBody == "[]")
                                        {
                                            i = 5500;
                                        }

                                        // writer.WriteLine(DateTime.Now + " INFO responseBody);
                                     //   Thread.Sleep(5000);
                                  

                                        response.EnsureSuccessStatusCode();

                                    }

                                    catch (Exception e)
                                    {

                                         c.t_count = try_count + 1;

                                        if (c.t_count >= 3)
                                        {
                                            Console.WriteLine("Error: Request error: " + e.Message );
                                            Console.WriteLine("Error:" +  responseBody );
                                            Console.WriteLine("INFO :  END TIME   :" + DateTime.Now + " Try again :" + c.t_count + " start_index :" + c.in_count + " DAY HOURS  :" + c.date_count);
                                            writer.WriteLine(DateTime.Now + " Error: Request error: " + e.Message);
                                            writer.WriteLine(DateTime.Now + " Error:" + responseBody);
                                            writer.WriteLine(DateTime.Now + " INFO :  END TIME   :" + DateTime.Now + " Try again :" + c.t_count + " start_index :" + c.in_count + " DAY HOURS  :" + c.date_count);

                                            Environment.Exit(0);
                                                break;

                                        }
                                        else
                                        {                                       
                                            Program new_token = new Program();
                                            Console.WriteLine("Error: Request error: " + e.Message);
                                            Console.WriteLine("Error:" + responseBody);
                                            writer.WriteLine(DateTime.Now + " Error: Request error: " + e.Message);
                                            writer.WriteLine(DateTime.Now + " Error:" + responseBody);
                                            Console.WriteLine("INFO : END TIME WAITING FOR 2 MINS   :" + DateTime.Now );
                                            writer.WriteLine(DateTime.Now + " INFO : END TIME WAITING FOR 5 MINS   :" + DateTime.Now);
                                            Console.WriteLine("INFO : END TIME WAITING FOR 2 (START) MINS   :" + DateTime.Now);

                                            Thread.Sleep(600000);
                                            Console.WriteLine("INFO : END TIME WAITING FOR 1 (END)MINS   :" + DateTime.Now);
                                            Thread.Sleep(600000);
                                            Console.WriteLine("INFO : END TIME WAITING FOR 2 (END)MINS   :" + DateTime.Now);
                                            Thread.Sleep(600000);
                                            Console.WriteLine("INFO : END TIME WAITING FOR 2 (END)MINS   :" + DateTime.Now);
                                            api_search( new_token.Token_gen(), c.in_count, c.t_count, c.date_count);
                                        }
                                    }


                                }

                            }
                           
                        }                 
                        
                        Console.WriteLine("END TIME   :"+DateTime.Now);
                        writer.WriteLine(DateTime.Now + " INFO :END TIME   :" + DateTime.Now);
                    }
                   
               }

            }
           
        }
        public bool IsHostReachable(string hostNameOrAddress)
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(hostNameOrAddress);

                    if (reply.Status == IPStatus.Success)
                    {
                        Console.WriteLine($"{DateAndTime.Now} INFO :Ping to {hostNameOrAddress} successful.");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"{DateAndTime.Now} INFO :Ping to {hostNameOrAddress} failed: {reply.Status}");
                        return false;
                    }
                }
            }
            catch (PingException ex)
            {
                Console.WriteLine($"{DateAndTime.Now} INFO :Ping to {hostNameOrAddress} failed with exception: {ex.Message}");
                return false;
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
        public class try_count
        {
            public int t_count { get; set; }
            public int in_count { get; set; }
            public int date_count { get; set; }
            public int actual_count { get; set; }=0;

        }

    }
}