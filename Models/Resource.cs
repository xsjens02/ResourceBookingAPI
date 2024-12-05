using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ResourceBookingAPI.Models
{
    public class Resource
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("imageUrl")]
        public string? imageUrl { get; set; }

        [BsonElement("institutionId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? InstitutionId { get; set; }
    }
}
