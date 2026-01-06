using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ESCIC_Library_Manager.Services;
using ESCIC_Library_Manager.UI;

namespace ESCIC_Library_Manager
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string connString = configuration["DatabaseSettings:ConnectionString"] ?? "mongodb://localhost:27017";
            string dbName = configuration["DatabaseSettings:DatabaseName"] ?? "escic_db";

            var dbService = new MongoDBService(connString, dbName);
            var authService = new AuthService(dbService);
            var menu = new MenuManager(dbService, authService);

            await menu.AfficherMenuPrincipal();
        }
    }
}
