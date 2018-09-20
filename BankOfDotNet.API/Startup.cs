using BankOfDotNet.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankOfDotNet.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Set-up the plumming for our Identity server
            // We use "Bearer" which is the same output from Postman "token_type": "Bearer"
            // when we call http://localhost:5000/connect/token to grab a token
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    // This is the BankOfDotNet.IdentityServer which is running on port 5000
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "BankOfDotNetAPI";
                });

            // Add a reference to the BankContext and use in-memory database
            services.AddDbContext<BankContext>(options =>
                options.UseInMemoryDatabase("BankingDb"));

            // If we need to use Sqlite
            //services.AddDbContext<BankContext>(opts =>
            //    opts.UseSqlite(Configuration.GetConnectionString("Users")));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Finally, we can make a call to our authentication service
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
