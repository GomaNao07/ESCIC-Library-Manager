using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ESCIC_Library_Manager.Models
{
    public class Livre
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("titre")]
        public string Titre { get; set; } = string.Empty;

        [BsonElement("auteur")]
        public string Auteur { get; set; } = string.Empty;

        [BsonElement("isbn")]
        public string ISBN { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("disponible")]
        public bool EstDisponible { get; set; } = true;

        [BsonElement("dateAjout")]
        public DateTime DateAjout { get; set; } = DateTime.Now;
    }
}
