using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESCICLibraryManager.Services;
using ESCICLibraryManager.Models;

namespace ESCICLibraryManager.UI
{
    public class MenuManager
    {
        private readonly MongoDBService _mongoService;
        private readonly AuthService _authService;

        public MenuManager(MongoDBService mongoService, AuthService authService)
        {
            _mongoService = mongoService;
            _authService = authService;
        }

        public async Task BouclePrincipaleAsync()
        {
            int choix = -1;
            while (choix != 0)
            {
                AfficherMenuPrincipal();
                Console.Write("\n > Entrez votre choix : ");

                if (int.TryParse(Console.ReadLine(), out choix))
                {
                    await TraiterChoixPrincipalAsync(choix);
                }
                else
                {
                    Console.WriteLine(" [!] Erreur : Saisie invalide.");
                    await Task.Delay(1000);
                }
            }
        }

        private void AfficherMenuPrincipal()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                 ESCIC-LIBRARY-MANAGER                      ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

            if (_authService.EstConnecte)
            {
                Console.WriteLine($"  Session : {_authService.UtilisateurConnecte.Nom} ({_authService.UtilisateurConnecte.Role})");
                Console.WriteLine("  ------------------------------------------------------------");
                Console.WriteLine("  1. Liste des livres");
                Console.WriteLine("  2. Emprunter un livre");
                Console.WriteLine("  3. Statistiques et Analytics");
                Console.WriteLine("  4. Mon Profil (Mes emprunts / Retourner)");

                if (_authService.EstAdmin)
                {
                    Console.WriteLine("  5. Administration des utilisateurs");
                    Console.WriteLine("  6. Ajouter un nouveau livre");
                }

                Console.WriteLine("  7. Déconnexion");
                Console.WriteLine("  0. Quitter");
            }
            else
            {
                Console.WriteLine("  1. Se connecter");
                Console.WriteLine("  2. S'inscrire");
                Console.WriteLine("  0. Quitter");
            }
        }

        private async Task TraiterChoixPrincipalAsync(int choix)
        {
            try
            {
                if (!_authService.EstConnecte)
                {
                    switch (choix)
                    {
                        case 1: await LoginAsync(); break;
                        case 2: await InscriptionAsync(); break;
                        case 0: Environment.Exit(0); break;
                    }
                }
                else
                {
                    switch (choix)
                    {
                        case 1: await ListeLivresAsync(); break;
                        case 2: await EmprunterLivreAsync(); break;
                        case 3: await AfficherStatistiquesAsync(); break;
                        case 4: await ProfilUtilisateurAsync(); break;
                        case 5: if (_authService.EstAdmin) await MenuAdminUtilisateursAsync(); break;
                        case 6: if (_authService.EstAdmin) await AjouterLivreAsync(); break;
                        case 7: _authService.Deconnexion(); break;
                        case 0: Environment.Exit(0); break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n [ERREUR] : {ex.Message}");
                Console.ResetColor();
                Console.WriteLine(" Appuyez sur une touche...");
                Console.ReadKey();
            }
        }

        private async Task LoginAsync()
        {
            Console.Clear();
            Console.WriteLine("--- AUTHENTIFICATION ---");
            Console.Write(" Email : ");
            string email = Console.ReadLine();
            Console.Write(" Mot de passe : ");
            string mdp = LireMotDePasseMasque();

            if (await _authService.SeConnecterAsync(email, mdp))
                Console.WriteLine(" Succès.");
            else
                Console.WriteLine(" Échec de connexion.");
            await Task.Delay(1000);
        }

        private async Task InscriptionAsync()
        {
            Console.Clear();
            Console.WriteLine("--- CRÉATION DE COMPTE ---");
            Console.Write(" Nom : ");
            string nom = Console.ReadLine();
            Console.Write(" Email : ");
            string email = Console.ReadLine();
            Console.Write(" Mot de passe : ");
            string mdp = LireMotDePasseMasque();

            var user = new Utilisateur { Nom = nom, Email = email, Role = RoleUtilisateur.Client, DateInscription = DateTime.Now, EstActif = true };
            user.DefinirMotDePasse(mdp);
            await _mongoService.AjouterUtilisateurAsync(user);
            Console.WriteLine(" Compte créé.");
            await Task.Delay(1000);
        }

        private async Task ListeLivresAsync()
        {
            Console.Clear();
            var livres = await _mongoService.RecupererTousLesLivresAsync();
            Console.WriteLine("--- CATALOGUE ---");
            foreach (var l in livres)
                Console.WriteLine($" - {l.Titre} (Par {l.Auteur}) | {(l.EstDisponible ? "Disponible" : "Indisponible")}");
            Console.WriteLine("\nAppuyez sur une touche pour revenir...");
            Console.ReadKey();
        }

        private async Task EmprunterLivreAsync()
        {
            Console.Clear();
            Console.WriteLine("--- EMPRUNTER UN LIVRE ---");

            var livres = await _mongoService.RecupererTousLesLivresAsync();
            var disponibles = livres.FindAll(l => l.EstDisponible);

            if (disponibles.Count == 0)
            {
                Console.WriteLine(" Aucun livre disponible.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < disponibles.Count; i++)
                Console.WriteLine($" {i + 1}. {disponibles[i].Titre}");

            Console.Write("\n Choisissez le numéro (0 pour annuler) : ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= disponibles.Count)
            {
                var livre = disponibles[index - 1];
                var emprunt = new Emprunt
                {
                    LivreId = livre.Id,
                    UtilisateurId = _authService.UtilisateurConnecte.Id,
                    DateEmprunt = DateTime.Now,
                    DateRetourPrevue = DateTime.Now.AddDays(14),
                    EstTermine = false
                };

                livre.EstDisponible = false;
                await _mongoService.ModifierLivreAsync(livre.Id, livre);
                await _mongoService.AjouterEmpruntAsync(emprunt);
                Console.WriteLine($" Succès ! À rendre le {emprunt.DateRetourPrevue:dd/MM/yyyy}");
            }
            await Task.Delay(1500);
        }

        private async Task ProfilUtilisateurAsync()
        {
            Console.Clear();
            var user = _authService.UtilisateurConnecte;
            Console.WriteLine($"--- PROFIL : {user.Nom} ---");

            var tousEmprunts = await _mongoService.RecupererTousLesEmpruntsAsync();
            var mesEmprunts = tousEmprunts.FindAll(e => e.UtilisateurId == user.Id && !e.EstTermine);

            if (mesEmprunts.Count == 0)
            {
                Console.WriteLine(" Vous n'avez aucun emprunt en cours.");
            }
            else
            {
                Console.WriteLine(" Vos emprunts en cours :");
                for (int i = 0; i < mesEmprunts.Count; i++)
                {
                    var livre = await _mongoService.RecupererLivreParIdAsync(mesEmprunts[i].LivreId);
                    Console.WriteLine($" {i + 1}. {livre?.Titre} (Retour prévu : {mesEmprunts[i].DateRetourPrevue:dd/MM/yyyy})");
                }

                Console.Write("\n Entrez le numéro pour rendre un livre (0 pour quitter) : ");
                if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= mesEmprunts.Count)
                {
                    var emp = mesEmprunts[idx - 1];
                    emp.DateRetourEffective = DateTime.Now;
                    emp.EstTermine = true;

                    var livre = await _mongoService.RecupererLivreParIdAsync(emp.LivreId);
                    livre.EstDisponible = true;

                    await _mongoService.ModifierEmpruntAsync(emp.Id, emp);
                    await _mongoService.ModifierLivreAsync(livre.Id, livre);
                    Console.WriteLine(" Livre rendu avec succès.");
                }
            }
            Console.ReadKey();
        }

        private async Task AjouterLivreAsync()
        {
            Console.Clear();
            Console.WriteLine("--- NOUVEAU LIVRE ---");
            Console.Write(" Titre : "); string t = Console.ReadLine();
            Console.Write(" Auteur : "); string a = Console.ReadLine();
            await _mongoService.AjouterLivreAsync(new Livre { Titre = t, Auteur = a, EstDisponible = true });
            Console.WriteLine(" Livre ajouté au catalogue.");
            await Task.Delay(1000);
        }

        private async Task MenuAdminUtilisateursAsync()
        {
            Console.Clear();
            var users = await _mongoService.RecupererTousLesUtilisateursAsync();
            Console.WriteLine("--- LISTE DES MEMBRES ---");
            foreach (var u in users)
                Console.WriteLine($" [{u.Role}] {u.Nom} - {u.Email}");
            Console.ReadKey();
        }

        private async Task AfficherStatistiquesAsync()
        {
            Console.Clear();
            Console.WriteLine("--- DASHBOARD ANALYTICS ---");
            var livres = await _mongoService.RecupererTousLesLivresAsync();
            var emprunts = await _mongoService.RecupererTousLesEmpruntsAsync();

            Console.WriteLine($" Total livres : {livres.Count}");
            Console.WriteLine($" Livres empruntés : {livres.FindAll(l => !l.EstDisponible).Count}");
            Console.WriteLine($" Total transactions d'emprunt : {emprunts.Count}");
            Console.WriteLine("\nAppuyez sur une touche...");
            Console.ReadKey();
        }

        public static string LireMotDePasseMasque()
        {
            string pass = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    pass = pass[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return pass;
        }
    }
}