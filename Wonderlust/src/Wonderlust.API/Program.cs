using MongoDB.Bson;
using MongoDB.Driver;
using Wonderlust.Domain.Entities;
using Wonderlust.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddMongoDb(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", async (IMongoDatabase db) => // получаем IMongoDatabase - базу данных "test" через DI
{
    var collection = db.GetCollection<Comment>("comments"); // получаем коллекцию users
    if (await collection.CountDocumentsAsync("{}") == 0)
    {
        await collection.InsertOneAsync(new Comment("ABCD", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()));
    }

    var users = await collection.Find("{}").ToListAsync();
    return users.ToJson(); // отправляем клиенту все документы из коллекции
});
app.Run();