﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using Microsoft.Xrm.Sdk.Query;
using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using DynamicsApplication.Services;

namespace DynamicsApplication
{
    public class Program
    {

        static void Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var services = serviceCollection.BuildServiceProvider();

            int countToCreate = HandleInput();

            var rentGenerator = services.GetService<RentGenerator>();
            var rents = rentGenerator.GenerateRents(countToCreate);

            var rentService = services.GetService<RentsService>();

            for (int i = 0; i < rents.Count; i++)
            {
                WriteOneLine($"Created {i}/{rents.Count} rents");
                rentService.CreateRent(rents[i]);
            }
            WriteOneLine($"Created {rents.Count} rents");
        }

        private static void WriteOneLine(string text)
        {
            int currLine = Console.CursorTop;
            Console.SetCursorPosition(0, currLine);
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currLine);
            Console.Write(text);
        }

        private static int HandleInput()
        {
            do
            {
                Console.WriteLine("How many rents to create?");
                var input = Console.ReadLine();

                if (int.TryParse(input, out int result))
                {
                    return result;
                }
            } while (true);
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DynamicsConnection"].ConnectionString;

            var orgService = new CrmServiceClient(connectionString);
            serviceCollection.AddSingleton<IOrganizationService, CrmServiceClient>(_ => orgService);
            serviceCollection.AddSingleton<DataRepository>(GetDataRepository);
            serviceCollection.AddSingleton<Random>(_ => new Random());
            serviceCollection.AddTransient<RentGenerator>();
            serviceCollection.AddTransient<RentsService>();
        }

        private static DataRepository GetDataRepository(IServiceProvider services)
        {
            var orgService = services.GetService<IOrganizationService>();
            var repository = new DataRepository(orgService);
            repository.RetreiveData();

            return repository;
        }
    }
}
