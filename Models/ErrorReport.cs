using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ResourceBookingAPI.Models
{
    public class ErrorReport
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("resourceId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ResourceId { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserId { get; set; }

        [BsonElement("resolved")]
        public bool Resolved { get; set; }
    }
}
