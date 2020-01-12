using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosDBTaxiApp.Model;
using CosmosDBTaxiApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CosmosDBTaxiApp
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
            IConfigurationSection configurationSection = Configuration.GetSection("CosmosDb");
            string databaseName = configurationSection.GetSection("DatabaseName").Value;
            string containerName = configurationSection.GetSection("ContainerName").Value;
            string account = configurationSection.GetSection("Account").Value;
            string key = configurationSection.GetSection("Key").Value;
            CosmosClientBuilder clientBuilder = new CosmosClientBuilder(account, key);
            CosmosClient client = clientBuilder
                                .WithConnectionModeDirect()
                                .Build();

            DatabaseResponse database = client.CreateDatabaseIfNotExistsAsync(databaseName).GetAwaiter().GetResult();
            database.Database.CreateContainerIfNotExistsAsync(containerName, "/id").GetAwaiter().GetResult();

            CosmosDBService<Journey> journeyService = new CosmosDBService<Journey>(client, databaseName, containerName);
            services.AddSingleton<ICosmosDBService<Journey>>(journeyService);
            CosmosDBService<Driver> driverService = new CosmosDBService<Driver>(client, databaseName, containerName);
            services.AddSingleton<ICosmosDBService<Driver>>(driverService);
            CosmosDBService<Location> locationService = new CosmosDBService<Location>(client, databaseName, containerName);
            services.AddSingleton<ICosmosDBService<Location>>(locationService);
            CosmosDBService<Vehicle> vehicleService = new CosmosDBService<Vehicle>(client, databaseName, containerName);
            services.AddSingleton<ICosmosDBService<Vehicle>>(vehicleService);

            services.AddControllers();
            BaseDataService.GenerateBaseData(client, databaseName, containerName);
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

        
    }
}
