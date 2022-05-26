using System;
using System.Net.Http;
using DemoClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace DemoClient.Services
{
    public class DbService
    {
        
        private HttpClient httpClient;
        public DbService()
        {
            httpClient = new HttpClient(GetInsecureHandler());
            httpClient.BaseAddress = new Uri("http://localhost:5265/");
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<User>> getUsers()
        {
            var content = await httpClient.GetStringAsync("users");
            var users  = JsonConvert.DeserializeObject<List<User>>(content);
            return users;
        }

        public HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }
    }
}

