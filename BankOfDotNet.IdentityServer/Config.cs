using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;

namespace BankOfDotNet.IdentityServer
{
    public class Config
    {
        // Used for Open-ID Connect Identity scopes
        public static IEnumerable<IdentityResource> GetidentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        // Used for OAuth Identity scopes
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "Judyll",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "Krysia",
                    Password = "password"
                }
            };
        }

        // Used for OAuth Identity scopes
        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource>
            {
                // This is the resource definition for the BankOfDotNet.API project
                new ApiResource("BankOfDotNetAPI", "Customer API for BankOfDotNet")
            };
        }

        // Used for OAuth Identity scopes
        public static IEnumerable<Client> GetClients()
        {
            // What is a grant-type -- Is a way the clients can communicate with the resources (APIs) or 
            // communicate through the IdentityServer.  It is a method such that the client can gain
            // access token from the IdentityServer4 and use that access token to make request 
            // to the resources.

            // Oauth 2.0 or Oidc (Open-ID Connect) are authorization protocols/frameworks that describe
            // different grants or methods for our clients to acquire different access tokens in order
            // to talk to the resources            

            // What is a trusted 1st party source -- For example, a manufacturing company develop the app
            // XYZ and they trust the app to securely store username and password and they also trust the apps storage
            // of client information and user information.

            // GrantTypes.ClientCredentials -- Used by machine to machine, trusted 1st party sources, server-to-server.
            // It is not recommended to use these client credentials on web-browser based apps or javascript apps
            // or single-page apps because they can't maintain the client id and client secret which exposes codes
            // to the public.  IMPORTANT NOTE: NO USER/RESOURCE OWNER/USER NAME/USER PASSWORD INVOLVED. JUST THE
            // CLIENT ID AND CLIENT SECRET

            // GrantTypes.ResourceOwnerPassword -- The client not only sends cliend id and client secret to the 
            // IdentityServer but also needs to send the user name and password of the resource owner (Human Users).
            // Used by users/resource owner with username and password, trusted 1st party resources.

            // GrantTypes.Code -- You can use Google, Facebook to log-in to an app and these 3rd party authorization 
            // platforms will return authorization code.  This can be used by user/resource owner, web app (server app, mvc),
            // pure client side app (core), or a 3rd party native app (ios app, android app)

            // GrantTypes.Implicit -- The client re-directs the user immediately to IdentityServer4, and IS4 will ask the user
            // to log-in using his username/password, and IS4 uses a callback url to present the user a consent page asking 
            // the user "do you recognize or approve of this client to start making calls to these resources ?".  Once consent
            // is confirmed by the user, IS4 will provide a token that can be used by the client to access resources. Kinda 
            // optimized for the browser-based apps 

            // GrantTypes.Hybrid -- Is a combination with the GranTypes.Implicit and GrantTypes.Code.  IS4 will return an 
            // Identity token back and transmitted to the browser and contains various artifacts like authorization code, etc.
            // For example, if your site has a log-in link to Google/Facebook, you can add that to your hybrid
            // flow and set-up in IS4.  During a request, you can  not just get the authorization code from IS4, but you will get
            // the identity token which is encrypted/signed and contains the authorization code from Google/Facebook
            // as well. Users are involved and can be used by server apps, native desktop apps, native mobile
            // apps.

            // A typical question, when to use a grant type ?
            // 1. Is there a user/resource owner involved ?  
            // 2. If no, then you want to go with GrantTypes.ClientCredentials.
            // 3. If yes, you might want to go with GrantTypes.ResourceOwnerPassword or GrantTypes.Implicit.
            // 5. Are you involving Google/Facebook ? 
            // 6. If yes, then you might want to go with GrantTypes.Code or GrantTypes.Hybrid


            return new List<Client>
            {
                // Used by BankOfDotNet.ConsoleClient
                // Client-Credential based grant type 
                new Client
                {
                    // The Client ID called  "Client".  Used when requesting an access token from IdentityServer
                    ClientId = "Client",
                    // Use a simple client credential grant types
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // Set-up a hashed password called "secretpass".  Used when requesting an access token from IdentityServer
                    ClientSecrets =
                    {
                        new Secret("secretpass".Sha256())
                    },
                    // Define the scope of this client (might be a console client, native app, ios app, web app)
                    // We are only allowing the client to access BankOfDotNetAPI resource
                    AllowedScopes = { "BankOfDotNetAPI" }
                },

                // Used by BankOfDotNet.ConsoleResourceOwner
                // GrantTypes.ResourceOwnerPassword grant type
                new Client
                {
                    ClientId = "ResOwnerPwd-Client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secretpass".Sha256())
                    },
                    AllowedScopes = { "BankOfDotNetAPI" }
                },

                // Used by BankOfDotNet.MvcClient
                // GrantTypes.Implicit grant type
                new Client
                {
                    ClientId = "Mvc-Client",
                    ClientName = "MCV Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    // For Implicit flow, the user is redirected from the client to the IS4
                    // So we need a redirect URI which means how the user will be redirected "back"
                    // after successful authentication with IS4
                    // We are setting our MVC client port to 5003 
                    // We are making use of signin-oidc which is for Open-ID connect
                    RedirectUris = { "http://localhost:5003/signin-oidc" },

                    // If the user logs-out from IS4, we need a post-logout redirect URI
                    PostLogoutRedirectUris = { "http://localhost:5003/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }

            };
        }
    }
}
