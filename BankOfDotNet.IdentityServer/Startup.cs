using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BankOfDotNet.IdentityServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Plug-in our identity server middleware
            services.AddIdentityServer()
                // We are not going to use natural certificate to sign-in tokens and everything
                // The idea is to use developer mode sign-in credentials
                .AddDeveloperSigningCredential()
                // We have Users (Human users) need to use Clients (PC, Phone) and those Clients have access
                // to resources.  Our resource in our case is the BankOfDotNet.API and our client is one is 
                // Postman and the other is console app.
                // We will be setting up a config file as parameter
                .AddInMemoryApiResources(Config.GetAllApiResources())
                // We have clients that will be registered to our Identity service and those clients
                // are managed by IdentiyServer4 and have permissions to access some resources.
                .AddInMemoryClients(Config.GetClients())
                // Add the in-memory test users for testing and to be used
                // For the GrantTypes.ResourceOwnerPassword grant types in the BankOfDotNet.ConsoleResourceOwner project
                .AddTestUsers(Config.GetTestUsers());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // We are going to plug-in the pipeline and use the identity service
            app.UseIdentityServer();
        }
    }
}
