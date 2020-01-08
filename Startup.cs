using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using TodoApi.Models;

namespace TodoApi
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
            // TODO: FOUND OUT IT WAS USING IN MEMORY DATABASE SO IT WAS NEVER GOING TO MAKE AN SQL CALLS 
            services.AddDbContext<TodoContext>(opt =>
                opt.UseSqlServer(BuildConnectionString()));

            services.AddControllers();

            // AUTO-GENERATED CODE
            // services.AddDbContext<TodoContext>(options =>
            //         options.UseSqlServer(Configuration.GetConnectionString("TodoContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        private static string GetConnectionStringByName(string name) 
        {
            string returnValue = null;

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];

            if(settings != null) 
            {
                returnValue = settings.ConnectionString;
            }

            return returnValue;
        }

        public static string BuildConnectionString()
        {
            // TODO: Re-evaluate this later to determine if this is the correct way to get the config
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var test = configuration.GetConnectionString("TodoContext");

            // Retrieve the partial connection string named databaseConnection
            // from the application's app.config or web.config file.
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;

            if (settings != null)
            {
                foreach(ConnectionStringSettings cs in settings)
                {
                    var name = cs.Name;
                    var providerName = cs.ProviderName;
                    var connectionString = cs.ConnectionString;
                }
                // // Retrieve the partial connection string.
                // string connectString = settings.ConnectionString;

                // // Create a new SqlConnectionStringBuilder based on the
                // // partial connection string retrieved from the config file.
                // SqlConnectionStringBuilder builder =
                //     new SqlConnectionStringBuilder(connectString);

                // // // Supply the additional values.
                // // builder.DataSource = GetConnectionStringByName("DataSource"); 
                // // builder.UserID = GetConnectionStringByName("User ID");
                // // builder.Password = GetConnectionStringByName("Password");

                // return settings.ConnectionString;
                // return builder.ConnectionString;
                return test;
            }

            // TODO: Find out why the Configuration is not being loaded from the web.config
            return "Server";
        }
    }
}
