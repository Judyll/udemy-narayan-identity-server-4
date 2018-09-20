using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BankOfDotNet.ConsoleClient
{
    // A console client which uses the GrantTypes.ClientCredentials
    class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            // Get the discovery end points using metadata of the identity server
            var discovery = await DiscoveryClient.GetAsync("http://localhost:5000");

            if (discovery.IsError)
            {
                Console.WriteLine(discovery.Error);
                return;
            }

            // Grab a bearer token
            // Values are configured in BankOfDotNet.IdentityServer.Config.cs
            // This is the same as what we did in Postman request named "Request Token From BankOfDotNet.IdentityServer"
            var tokenClient = new TokenClient(discovery.TokenEndpoint, "Client", "secretpass");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("BankOfDotNetAPI");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // Consume our BankOfDotNet.API
            var client = new HttpClient();
            // In Postman, this is the Authorization token we defined in the Headers
            client.SetBearerToken(tokenResponse.AccessToken);

            var customerInfo = new StringContent(
                JsonConvert.SerializeObject(
                    new { Id = 10, FirstName = "Judyll", LastName = "Agan"}),
                Encoding.UTF8,
                "application/json");

            var createCustomerResponse = await client.PostAsync("http://localhost:51085/api/customers", 
                customerInfo);

            if (!createCustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("Create Customer: " + createCustomerResponse.StatusCode);
            }

            var getCustomerResponse = await client.GetAsync("http://localhost:51085/api/customers");

            if (!getCustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("Get Customer: " + getCustomerResponse.StatusCode);
            }
            else
            {
                var content = await getCustomerResponse.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }

            Console.Read();

        }
    }
}
