using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using petwell_back.Shared.Common;

namespace petwell_back.Models.Entities
{
     public class UserDb
     {
          [BsonId]
          [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
          public string Id { get; set; } = null!;

          [BsonElement("username"), BsonRepresentation(BsonType.String)]
          public required string Username { get; set; } = string.Empty;

          [BsonElement("email"), BsonRepresentation(BsonType.String)]
          public required string Email { get; set; } = string.Empty;

          [BsonElement("password"), BsonRepresentation(BsonType.String)]
          public required string PasswordHash { get; set; } = string.Empty;

          [BsonElement("phone"), BsonRepresentation(BsonType.String)]
          public string? Phone { get; set; }

          [BsonElement("role"), BsonRepresentation(BsonType.String)]
          public UserRole Role { get; set; } = UserRole.Customer;

          [BsonElement("refreshToken"), BsonRepresentation(BsonType.String)]
          public string? RefreshToken { get; set; }

          [BsonElement("refreshTokenExpiry"), BsonRepresentation(BsonType.DateTime)]
          public DateTime? RefreshTokenExpiryTime { get; set; }
     }
}
