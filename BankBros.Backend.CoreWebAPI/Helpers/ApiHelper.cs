using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BankBros.Backend.CoreWebAPI.Helpers
{
    /// <summary>
    /// This Helper is an API requester for REST Apis'.
    /// Modified for .net core environment.
    /// Developed by Cengiz "TheJengo" Cebeci
    /// </summary>
    /// <typeparam name="T">Response Class Type</typeparam>
    public static class ApiHelper<T>
    {
        private static string baseURL = "http://bankbroscredit.azurewebsites.net/";
        public static async Task<T> Get(string pathname, string token = "")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", token);
                var userAgent = "d-fens HttpClient";
                client.DefaultRequestHeaders.Add("User-Agent", userAgent);

                HttpResponseMessage response = await client.GetAsync(pathname);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("json : "+json);
                    var items = JsonSerializer.Deserialize<T>(json);

                    // now use you have the date on Items !
                    return items;
                }
                else
                {
                    // deal with error or here ...
                    return default(T);
                }
            }
        }

        public static async Task<T> Post(object entity, string pathname, string token = "")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                if (token != "")
                    client.DefaultRequestHeaders.Add("Authorization", token);

                var userAgent = "d-fens HttpClient";
                client.DefaultRequestHeaders.Add("User-Agent", userAgent);

                HttpResponseMessage response = await client.PostAsync(pathname, entity,new JsonMediaTypeFormatter());
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("json : " + json);
                    var items = JsonSerializer.Deserialize<T>(json);

                    return items;
                }
                else
                {
                    // deal with error or here ...
                    return default(T);
                }
            }
        }

        public static async Task<T> Put(object entity, string pathname, string token = "")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                if (token != "")
                    client.DefaultRequestHeaders.Add("Authorization", token);

                var userAgent = "d-fens HttpClient";
                client.DefaultRequestHeaders.Add("User-Agent", userAgent);

                HttpResponseMessage response = await client.PutAsync(pathname, entity, new JsonMediaTypeFormatter());
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("json : " + json);
                    var items = JsonSerializer.Deserialize<T>(json);

                    return items;
                }
                else
                {
                    // deal with error or here ...
                    return default(T);
                }
            }
        }

        public static async Task<T> Delete(string pathname, string token = "")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                if (token != "")
                    client.DefaultRequestHeaders.Add("Authorization", token);

                var userAgent = "d-fens HttpClient";
                client.DefaultRequestHeaders.Add("User-Agent", userAgent);

                HttpResponseMessage response = await client.DeleteAsync(pathname);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("json : " + json);
                    var items = JsonSerializer.Deserialize<T>(json);

                    return items;
                }
                else
                {
                    // deal with error or here ...
                    return default(T);
                }
            }
        }
    }
}
