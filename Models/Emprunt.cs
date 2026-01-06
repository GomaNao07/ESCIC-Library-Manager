using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ESCICLibraryManager.Models
{
    public class Emprunt
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string LivreId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string UtilisateurId { get; set; } = null!;

        public DateTime DateEmprunt { get; set; }
        public DateTime DateRetourPrevue { get; set; }
        public DateTime? DateRetourEffective { get; set; }
        public bool EstTermine { get; set; }
    }
}