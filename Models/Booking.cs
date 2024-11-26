using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ResourceBookingAPI.Models
{
    public class Booking
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserId { get; set; }

        [BsonElement("resourceId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ResourceId { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("startTime")]
        public string? StartTime { get; set; }

        [BsonElement("endTime")]
        public string? EndTime { get; set; }

        [BsonElement("active")]
        public bool Active { get; set; }
    }
}
