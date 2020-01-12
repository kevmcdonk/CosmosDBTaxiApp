using CosmosDBTaxiApp.Model;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDBTaxiApp.Services
{
    public class BaseDataService
    {
        public static async void GenerateBaseData(CosmosClient client, string databaseName, string containerName)
        {
                Location StarbucksCoffee = new Location()
                {
                    Id = Guid.NewGuid(),
                    Name = "Starbucks Coffee",
                    AddressLine1 = "424 Park Ave S",
                    Town = "New York",
                    County = "NY",
                    Country = "US",
                    Postcode = "10016"
                };
                Location ZephyrCafe = new Location() { Id = Guid.NewGuid(), Name = "Zephyr Cafe", AddressLine1 = "1767 W. Wilson Ave.", Town = "Chicago", County = "IL", Country = "US", Postcode = "60625" };
                Location FosterAvenue = new Location() { Id = Guid.NewGuid(), Name = "Foster Avenue Beach", AddressLine1 = "Lake Michigan at Foster Avenue (5200 N.)", Town = "Chicago", County = "IL", Country = "US", Postcode = "60640" };

                Driver GregorFenton = new Driver()
                {
                    Id = Guid.NewGuid(),
                    Name = "Gregor Fenton",
                    Age = 55,
                    DriverLicenseNumber = "D3380W",
                    HomeLocation = FosterAvenue
                };
                Driver RoxyKeller = new Driver() { Id = Guid.NewGuid(), Name = "Roxy Keller", Age = 28, DriverLicenseNumber = "D6372E", HomeLocation = FosterAvenue };
                Driver ZakariahAmin = new Driver() { Id = Guid.NewGuid(), Name = "Zakariah Amin", Age = 38, DriverLicenseNumber = "D7129M", HomeLocation = FosterAvenue };

                Vehicle FordFocus = new Vehicle()
                {
                    Id = Guid.NewGuid(),
                    Name = "Ford Focus",
                    VehicleMake = "Ford",
                    VehicleType = "Focus"
                };
                Vehicle HyundaiSolaris = new Vehicle() { Id = Guid.NewGuid(), Name = "Hyundai Solaris", VehicleMake = "Hyundai", VehicleType = "Solaris" };
                Vehicle RenaultFluence = new Vehicle() { Id = Guid.NewGuid(), Name = "Renault Fluence", VehicleMake = "Renault", VehicleType = "Fluence" };

                Journey journey1 = new Journey()
                {
                    Id = Guid.NewGuid(),
                    Name = "StarbucksToZephyr",
                    StartPoint = StarbucksCoffee,
                    EndPoint = ZephyrCafe,
                    JourneyDate = DateTime.Now.AddHours(-3487),
                    VehicleDriver = GregorFenton,
                    VehicleUsed = FordFocus
                };

                CosmosDBService<Journey> journeyService = new CosmosDBService<Journey>(client, databaseName, containerName);

                //journeyService.AddItemAsync(journey1).GetAwaiter().GetResult();
        }
    }
}
