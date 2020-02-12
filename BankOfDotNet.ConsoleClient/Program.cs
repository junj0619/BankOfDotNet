using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BankOfDotNet.ConsoleClient
{
    class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            // Discover all the EndPoints using metadata of identity server
            var disco = await DiscoveryClient.GetAsync("http://localhost:5001");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            //Grab a bearer token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            //Request scope
            var tokenReponse = await tokenClient.RequestClientCredentialsAsync("bankOfDotNetApi");
            if (tokenReponse.IsError)
            {
                Console.WriteLine(tokenReponse.Error);
                return;
            }

            //console JWT token
            Console.WriteLine(tokenReponse.Json);
            Console.WriteLine("\n\n");

            //Consume Customer API
            var client = new HttpClient();
            client.SetBearerToken(tokenReponse.AccessToken);


            //Create customer
            var cusomerInfo = new StringContent(
                JsonConvert.SerializeObject(
                    new { Id = 10, FirstName = "JA", LastName = "NY" }
                    ), Encoding.UTF8, "application/json");

            var createCustomerResponse = await client.PostAsync("http://localhost:2457/api/customers", cusomerInfo);

            if (!createCustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(createCustomerResponse.StatusCode);
            }

            //Get customer
            var getCustomerResponse = await client.GetAsync("http://localhost:2457/api/customers");
            if (!getCustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(getCustomerResponse.StatusCode);
            } else
            {
                var content = await getCustomerResponse.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }

            Console.Read();
        }
    }
}
