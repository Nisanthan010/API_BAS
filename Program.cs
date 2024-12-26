using System.Net.Http.Headers;
using System.Net.Http.Json;
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
         
                // Create the request message
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://mybas.cloud/prod/api/vendor/GetAuthorize")
                {
                    Content = content
                };

                  request.Headers.Authorization = new AuthenticationHeaderValue("BASIC", "ZWMzMjExMGI1YThmNDZmZjg1OTZhYzZlMTIwNDI1Mzk6YzMyN2QyMjI1Mjk4NDA5ZGEyODhhZjU1NzQ0NjcyYWU=");
                
                try
                {


                    HttpResponseMessage response =  client.Send(request);
                   

                    string TokenResponse =  response.Content.ReadAsStringAsync().Result;
                 
                    var  tokenData = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(TokenResponse);
                 
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
            string temp_path = Path.GetTempFileName();
            // Path to the output CSV file
            string csvFilePath = temp_path + "BAS_API.csv";

            try_count c =new try_count();
            c.t_count = try_count;

            c.t_count = try_count;
            
            using (StreamWriter writer = new StreamWriter(temp_path+"BAS_API_LOG_" +Convert.ToString(c.t_count) +".txt"))
            {
                using (HttpClient client = new HttpClient())
             {
                  
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

                           
                                    
                                       string jsonBody_2 = @",
                                       
   
    ""max_count""      : 50,
    ""client_code""    : ""LEIGHTON"",
    ""site_code""      : ""Polaris"",
    ""location_code""  : ""ALL"",
     ""mode""          : ""D"",
    ""fromdate""       : ""2024-12-05T00:00:00.00+01:00"",
    ""todate""         : ""2024-12-07T23:59:59.00+01:00""

}";

                            string jsonBody = jsonBody_1 + Count_st + jsonBody_2;


                                    Program ping = new Program();
                                    ping.IsHostReachable("mybas.cloud");


                                    StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://mybas.cloud/prod/api/vendor/SearchFILOTransaction")
                                    {
                                        Content = content
                                    };

                                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
                                    string responseBody = null;

                                    try
                                    {
                                       
                                        HttpResponseMessage response =  client.Send(request);                
                                       
                                        responseBody =  response.Content.ReadAsStringAsync().Result;
                                       
                                    //    Console.WriteLine("INFO :END TIME CALL  :" + DateTime.Now);
                                        Console.WriteLine(jsonBody);
                                        writer.WriteLine(DateTime.Now + " INFO : " + responseBody);

                                // Deserialize JSON to a list of dictionaries (assuming JSON array)
                                var data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(responseBody);

                             
                                // Convert to CSV and save
                               ConvertJsonToCsv(data, csvFilePath);

                                Console.WriteLine("JSON has been successfully converted to CSV.");

                                if (responseBody == "[]")
                                        {
                                            i = 5500;
                                        }

                                        
                                        response.EnsureSuccessStatusCode();

                                    }

                                    catch (Exception e)
                                    {

                                         c.t_count = try_count + 1;

                                        if (c.t_count >= 3)
                                        {
                                            Console.WriteLine("Error: Request error: " + e.Message );
                                            Console.WriteLine("Error:" + responseBody);
          
                                            Environment.Exit(0);
                                                break;

                                        }
                                        else
                                        {                                       
                                            Program new_token = new Program();
                                            Console.WriteLine("Error: Request error: " + e.Message);
                                            Console.WriteLine("Error:" + responseBody);
                                            writer.WriteLine(DateTime.Now + " Error:" + jsonBody);
                                        
                                     api_search( new_token.Token_gen(), c.in_count, c.t_count, c.date_count);
                                        }
                                    }


                                }

                            }
                           
                                        
                   
                   
               }

            }
           
        }
        static void ConvertJsonToCsv(List<Dictionary<string, object>> data, string outputPath)
        {
            if (data == null || data.Count == 0)
            {
                throw new Exception("No data to write to CSV.");
            }
          if(File.Exists(outputPath))
            {

                using (var writer = new StreamWriter(outputPath, append: true))
                {
                    // Write CSV header
                    var headers = string.Join(",", data[0].Keys);
                    writer.WriteLine(headers);

                    // Write CSV rows
                    foreach (var row in data)
                    {
                        var values = string.Join(",", row.Values);
                        writer.WriteLine(values);
                    }
                }
            }
            else
            {
                using (var writer = new StreamWriter(outputPath))
                {
                    // Write CSV header
                    var headers = string.Join(",", data[0].Keys);
                    writer.WriteLine(headers);

                    // Write CSV rows
                    foreach (var row in data)
                    {
                        var values = string.Join(",", row.Values);
                        writer.WriteLine(values);
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