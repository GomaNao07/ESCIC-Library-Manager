using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ESCIC_Library_Manager.Models
{
    public class Utilisateur
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("nom")]
        public string Nom { get; set; } = string.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("motDePasse")]
        public string MotDePasse { get; set; } = string.Empty;

        [BsonElement("type")]
        public TypeUtilisateur Type { get; set; }

        [BsonElement("dateInscription")]
        public DateTime DateInscription { get; set; } = DateTime.Now;

        public void DefinirMotDePasse(string pass) => MotDePasse = pass;
    }
}
