using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ESCIC_Library_Manager.Models
{
    public class Emprunt
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string LivreId { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string UtilisateurId { get; set; } = string.Empty;

        public DateTime DateEmprunt { get; set; } = DateTime.Now;
        public DateTime DateRetourPrevue { get; set; }
        public bool EstTermine { get; set; } = false;
    }
}
