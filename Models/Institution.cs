using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ResourceBookingAPI.Models
{
    public class Institution
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("imageUrl")]
        public string? imageUrl { get; set; }

        [BsonElement("openTime")]
        public string? OpenTime { get; set; }

        [BsonElement("closeTime")]
        public string? CloseTime { get; set; }

        [BsonElement("bookingInterval")]
        public int BookingInterval { get; set; }
    }
}
