using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ESCICLibraryManager.Models
{
    public enum RoleUtilisateur { Admin, Client }

    public class Utilisateur
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string Nom { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string MotDePasseHash { get; set; } = null!;
        public RoleUtilisateur Role { get; set; }
        public bool EstActif { get; set; }
        public DateTime DateInscription { get; set; }

        public void DefinirMotDePasse(string mdp)
        {
            MotDePasseHash = BCrypt.Net.BCrypt.HashPassword(mdp);
        }

        public bool VerifierMotDePasse(string mdp)
        {
            return BCrypt.Net.BCrypt.Verify(mdp, MotDePasseHash);
        }
    }
}