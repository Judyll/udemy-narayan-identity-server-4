using IdentityModel.Client;
using System;
using System.Threading.Tasks;

namespace BankOfDotNet.ConsoleResourceOwner
{
    // A console client which uses the GrantTypes.ResourceOwnerPassword
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

            // Grab a bearer token using GrantTypes.ResourceOwnerPassword
            // Values are configured in BankOfDotNet.IdentityServer.Config.cs
            // This is the same as what we did in Postman request named "Request Token From BankOfDotNet.IdentityServer"
            var tokenClient = new TokenClient(discovery.TokenEndpoint, "ResOwnerPwd-Client", "secretpass");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("Judyll", 
                "password", "BankOfDotNetAPI");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

        }
    }
}
