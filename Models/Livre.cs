using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ESCICLibraryManager.Models
{
    public class Livre
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string Titre { get; set; } = null!;
        public string Auteur { get; set; } = null!;
        public bool EstDisponible { get; set; }
    }
}