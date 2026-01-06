using ESCIC_Library_Manager.Models;
using System.Threading.Tasks;

namespace ESCIC_Library_Manager.Services
{
    public class AuthService
    {
        private readonly MongoDBService _dbService;

        public AuthService(MongoDBService dbService)
        {
            _dbService = dbService;
        }

        public async Task<Utilisateur?> SeConnecterAsync(string email, string motDePasse)
        {
            // On appelle la méthode ASYNC qu'on a créée dans MongoDBService
            var utilisateur = await _dbService.RechercherUtilisateurParEmailAsync(email);

            if (utilisateur == null)
                return null;

            // Vérification simple du mot de passe (à améliorer avec BCrypt plus tard)
            if (utilisateur.MotDePasse == motDePasse)
                return utilisateur;

            return null;
        }
    }
}
