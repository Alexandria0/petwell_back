using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace petwell_back.Models.Entities
{
     public class ServiceDb
     {
          [BsonId]
          [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
          public string Id { get; set; } = null!;

          [BsonElement("_userId"), BsonRepresentation(BsonType.ObjectId)]
          public required string UserId { get; set; }

          [BsonElement("company"), BsonRepresentation(BsonType.String)]
          public required string Company { get; set; }

          [BsonElement("type"), BsonRepresentation(BsonType.String)]
          public required string Type { get; set; }

          [BsonElement("location"), BsonRepresentation(BsonType.String)]
          public required string Location { get; set; }

          [BsonElement("animal"), BsonRepresentation(BsonType.String)]
          public required string Animal { get; set; }

          [BsonElement("description"), BsonRepresentation(BsonType.String)]
          public required string Description { get; set; }

          [BsonElement("prices")]
          public required List<ServicePrice> Prices { get; set; } = new();

          [BsonElement("contacts")]
          public required Contacts Contacts { get; set; } = new() { Phone = string.Empty };

          [BsonElement("createdAt"), BsonRepresentation(BsonType.DateTime)]
          public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
     }

     public class ServicePrice
     {
          [BsonElement("name"), BsonRepresentation(BsonType.String)]
          public required string Name { get; set; }

          [BsonElement("price"), BsonRepresentation(BsonType.Decimal128)]
          public required decimal Price { get; set; }

          [BsonElement("currency"), BsonRepresentation(BsonType.String)]
          public required string Currency { get; set; }
     }

     public class Contacts
     {
          [BsonElement("address"), BsonRepresentation(BsonType.String)]
          public string? Address { get; set; }

          [BsonElement("phone"), BsonRepresentation(BsonType.String)]
          public required string Phone { get; set; }

          [BsonElement("email"), BsonRepresentation(BsonType.String)]
          public string? Email { get; set; }

          [BsonElement("website"), BsonRepresentation(BsonType.String)]
          public string? Website { get; set; }
     }
}
