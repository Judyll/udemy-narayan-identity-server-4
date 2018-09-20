using IdentityServer4.Models;
using System.Collections.Generic;

namespace BankOfDotNet.IdentityServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource>
            {
                // This is the resource definition for the BankOfDotNet.API project
                new ApiResource("BankOfDotNetAPI", "Customer API for BankOfDotNet")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    // The Client ID is the user name called  "Client"
                    ClientId = "Client",
                    // Use a simple client credential grant types
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // Set-up a hashed password called "secretpass"
                    ClientSecrets =
                    {
                        new Secret("secretpass".Sha256())
                    },
                    // Define the scope of this client (might be a console client, native app, ios app, web app)
                    // We are only allowing the client to access BankOfDotNetAPI resource
                    AllowedScopes = { "BankOfDotNetAPI" }
                }
            };
        }
    }
}
