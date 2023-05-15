using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FreeCourse.Services.Catalog.Model
{
    public class Course
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
        public string? Picture { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedTime { get; set; }

        public string? UserId { get; set; } // identity server da kullanıcı id si de string olarak tutulacak

        public Feature? Feature { get; set; } // bire bir ilişkiyi tutması için
        [BsonRepresentation(BsonType.ObjectId)]
        public string? CategoryId { get; set; }  // category class ının id sini fk olarak tutması için

        [BsonIgnore]                            // mongodb ye kaydetme demek, serialize edilmesini engeller
        public Category? Category { get; set; } // sadece kursları dönerken kategorileri de dönmesi için
    }
}
