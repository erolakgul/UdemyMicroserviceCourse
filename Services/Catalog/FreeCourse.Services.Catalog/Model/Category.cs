using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FreeCourse.Services.Catalog.Model
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Name { get; set; }

        // ioption ile property ler implement e edilebilir
        //IOptions<CustomDatabaseSettings>? options { get; set; }
        //public Category()
        //{
        //    string ccname = options.Value.CategoryCollectionName;
        //}
    }
}
