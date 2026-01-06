using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ESCICLibraryManager.Services;
using ESCICLibraryManager.UI;
using ESCICLibraryManager.Models;
using MongoDB.Driver;

namespace ESCICLibraryManager
{
    public class Program
    {
        private static MongoDBService _mongoService = null!;
        private static AuthService _authService = null!;
        private static IConfiguration _config = null!;

        public static async Task Main(string[] args)
        {
            try
            {
                _config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                Console.WriteLine(" [SYSTÈME] Démarrage de ESCIC-LIBRARY-MANAGER...");

                string connString = _config["DatabaseSettings:ConnectionString"] ?? "mongodb://localhost:27017";
                string dbName = _config["DatabaseSettings:DatabaseName"] ?? "escic_db";

                _mongoService = new MongoDBService(connString, dbName);
                _authService = new AuthService(_mongoService);

                await _mongoService.VerifierConnexionAsync();
                Console.WriteLine(" [OK] Connexion MongoDB établie.");

                await CreerAdminParDefautAsync();

                var menu = new MenuManager(_mongoService, _authService);
                await menu.BouclePrincipaleAsync();
            }
            catch (MongoException ex)
            {
                Console.WriteLine($"\n [ERREUR CRITIQUE DATABASE] : {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n [ERREUR FATALE] : {ex.Message}");
                File.AppendAllText("error_log.txt", $"{DateTime.Now} : {ex}\n");
            }

            Console.WriteLine("\n Fin du programme. Au revoir !");
        }

        static async Task CreerAdminParDefautAsync()
        {
            string email = _config["AdminSettings:DefaultEmail"];
            string password = _config["AdminSettings:DefaultPassword"];

            var admin = await _mongoService.RechercherUtilisateurParEmailAsync(email);
            if (admin == null)
            {
                var nouvelAdmin = new Utilisateur
                {
                    Nom = "Admin Système",
                    Email = email,
                    Role = RoleUtilisateur.Admin,
                    EstActif = true,
                    DateInscription = DateTime.Now
                };
                nouvelAdmin.DefinirMotDePasse(password);
                await _mongoService.AjouterUtilisateurAsync(nouvelAdmin);
                Console.WriteLine(" [INFO] Premier compte admin créé.");
            }
        }
    }
}