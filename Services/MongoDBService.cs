using MongoDB.Driver;
using ESCIC_Library_Manager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESCIC_Library_Manager.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Utilisateur> _utilisateurs;
        private readonly IMongoCollection<Livre> _livres;

        public MongoDBService(string connectionString, string dbName)
        {
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(dbName);
            _utilisateurs = db.GetCollection<Utilisateur>("Utilisateurs");
            _livres = db.GetCollection<Livre>("Livres");
        }

        public async Task<Utilisateur> RechercherUtilisateurParEmailAsync(string email) 
            => await _utilisateurs.Find(u => u.Email == email).FirstOrDefaultAsync();

        public async Task AjouterUtilisateurAsync(Utilisateur u) 
            => await _utilisateurs.InsertOneAsync(u);

        public async Task<List<Livre>> RecupererTousLesLivresAsync() 
            => await _livres.Find(_ => true).ToListAsync();
    }
}
