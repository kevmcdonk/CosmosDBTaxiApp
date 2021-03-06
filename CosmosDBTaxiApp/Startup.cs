using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CosmosDBTaxiApp.Model;
using CosmosDBTaxiApp.Services;
using Microsoft.AspNet.OData.Extensions;
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
            services.AddOData();
            services.AddControllers(mvcOptions =>
                mvcOptions.EnableEndpointRouting = false);
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
            database.Database.CreateContainerIfNotExistsAsync(containerName, "/entity").GetAwaiter().GetResult();

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
            StartChangeFeedProcessorAsync(client, Configuration).GetAwaiter().GetResult(); ;
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

            /*
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            */

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Item}/{action=Index}/{id?}");

                routes.EnableDependencyInjection();
                routes.Select().Filter().OrderBy().Expand();
            });

        }

        private static async Task<ChangeFeedProcessor> StartChangeFeedProcessorAsync(
            CosmosClient cosmosClient,
            IConfiguration configuration)
        {
            string databaseName = configuration["SourceDatabaseName"];
            string sourceContainerName = configuration["SourceContainerName"];
            string leaseContainerName = configuration["LeasesContainerName"];

            Container leaseContainer = cosmosClient.GetContainer(databaseName, leaseContainerName);
            ChangeFeedProcessor changeFeedProcessor = cosmosClient.GetContainer(databaseName, sourceContainerName)
                .GetChangeFeedProcessorBuilder<Journey>("changeFeedSample", HandleChangesAsync)
                    .WithInstanceName("consoleHost")
                    .WithLeaseContainer(leaseContainer)
                    .Build();

            Console.WriteLine("Starting Change Feed Processor...");
            await changeFeedProcessor.StartAsync();
            Console.WriteLine("Change Feed Processor started.");
            return changeFeedProcessor;
        }

        static async Task HandleChangesAsync(IReadOnlyCollection<Journey> changes, CancellationToken cancellationToken)
        {
            Console.WriteLine("Started handling changes...");
            foreach (Journey item in changes)
            {
                Console.WriteLine($"Detected operation for item with id {item.Id}, created on {item.JourneyDate}.");
                // Simulate some asynchronous operation
                await Task.Delay(10);
            }

            Console.WriteLine("Finished handling changes.");
        }
    }
}
