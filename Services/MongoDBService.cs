using MongoDB.Driver;
using ESCICLibraryManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace ESCICLibraryManager.Services
{
    public class MongoDBService
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Utilisateur> _utilisateurs;
        private readonly IMongoCollection<Livre> _livres;
        private readonly IMongoCollection<Emprunt> _emprunts;

        public MongoDBService(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);

            _utilisateurs = _database.GetCollection<Utilisateur>("Utilisateurs");
            _livres = _database.GetCollection<Livre>("Livres");
            _emprunts = _database.GetCollection<Emprunt>("Emprunts");
        }

        public async Task VerifierConnexionAsync()
        {
            await _database.RunCommandAsync((Command<MongoDB.Bson.BsonDocument>)"{ping:1}");
        }

        public async Task<Utilisateur> RechercherUtilisateurParEmailAsync(string email)
        {
            return await _utilisateurs.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task AjouterUtilisateurAsync(Utilisateur utilisateur)
        {
            await _utilisateurs.InsertOneAsync(utilisateur);
        }

        public async Task<List<Utilisateur>> RecupererTousLesUtilisateursAsync()
        {
            return await _utilisateurs.Find(_ => true).ToListAsync();
        }

        public async Task<List<Livre>> RecupererTousLesLivresAsync()
        {
            return await _livres.Find(_ => true).ToListAsync();
        }

        public async Task<Livre> RecupererLivreParIdAsync(string id)
        {
            return await _livres.Find(l => l.Id == id).FirstOrDefaultAsync();
        }

        public async Task AjouterLivreAsync(Livre livre)
        {
            await _livres.InsertOneAsync(livre);
        }

        public async Task ModifierLivreAsync(string id, Livre livre)
        {
            await _livres.ReplaceOneAsync(l => l.Id == id, livre);
        }

        public async Task AjouterEmpruntAsync(Emprunt emprunt)
        {
            await _emprunts.InsertOneAsync(emprunt);
        }

        public async Task<List<Emprunt>> RecupererTousLesEmpruntsAsync()
        {
            return await _emprunts.Find(_ => true).ToListAsync();
        }

        public async Task ModifierEmpruntAsync(string id, Emprunt emprunt)
        {
            await _emprunts.ReplaceOneAsync(e => e.Id == id, emprunt);
        }
    }
}