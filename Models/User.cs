using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ResourceBookingAPI.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("phone")]
        public string? Phone { get; set; }

        [BsonElement("role")]
        public string? Role { get; set; }

        [BsonElement("username")]
        public string? Username { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }

        [BsonElement("institutionId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? InstitutionId { get; set; }
    }
}
