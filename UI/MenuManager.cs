using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESCIC_Library_Manager.Models;
using ESCIC_Library_Manager.Services;
using ESCIC_Library_Manager.Helpers;

namespace ESCIC_Library_Manager.UI
{
    public class MenuManager
    {
        private readonly MongoDBService _dbService;
        private readonly AuthService _authService;

        public MenuManager(MongoDBService db, AuthService auth)
        {
            _dbService = db;
            _authService = auth;
        }

        public async Task AfficherMenuPrincipal()
        {
            bool continuer = true;
            while (continuer)
            {
                Console.Clear();
                Console.WriteLine("=== ESCIC LIBRARY MANAGER ===");
                Console.WriteLine("1. Se connecter");
                Console.WriteLine("2. S'inscrire");
                Console.WriteLine("3. Quitter");
                Console.Write("\nVotre choix : ");

                var choix = Console.ReadLine();
                switch (choix)
                {
                    case "1": await Connexion(); break;
                    case "2": await Inscription(); break;
                    case "3": continuer = false; break;
                }
            }
        }

        private async Task Connexion()
        {
            Console.Write("Email : "); string email = Console.ReadLine();
            Console.Write("Mot de passe : "); string pass = Console.ReadLine();
            
            var user = await _authService.SeConnecterAsync(email, pass);
            if (user != null)
            {
                Logger.LogSuccess($"Connexion réussie : {user.Nom}");
                await MenuUtilisateur();
            }
            else
            {
                Logger.Log("Email ou mot de passe incorrect.", LogLevel.ERROR);
                Console.ReadKey();
            }
        }

        private async Task Inscription()
        {
            Console.Write("Nom : "); string nom = Console.ReadLine();
            Console.Write("Email : "); string email = Console.ReadLine();
            Console.Write("Mot de passe : "); string pass = Console.ReadLine();

            var u = new Utilisateur { Nom = nom, Email = email, DateInscription = DateTime.Now };
            u.DefinirMotDePasse(pass);

            await _dbService.AjouterUtilisateurAsync(u);
            Logger.LogSuccess("Inscription réussie !");
            Console.ReadKey();
        }

        private async Task MenuUtilisateur()
        {
            Console.WriteLine("\nBienvenue dans votre espace !");
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}
