using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Wonderlust.Infrastructure.Data.Mongo;

public static class MongoConfigurator
{
    public static void Configure()
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
    }
}