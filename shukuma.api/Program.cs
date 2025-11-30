using shukuma.domain.Interfaces;
using shukuma.persistence.firebase;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.ConnectFirestore(config);

// Swagger/OpenAPI: https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
app.MapGet("/users-registered", async (IUserService userService) =>
{
    var users = new List<User>();
    var response = await userService.GetUsers();

    response.ForEach(u => users.Add(
        new User($"{u.FirstName} {u.LastName}",
        $"{DateTime.Parse(u.CreatedBy).ToLongDateString()}, " +
        $"{DateTime.Parse(u.CreatedBy).ToLongTimeString()}")));
    return new UsersVm<User>(users.Count, users);
});

app.MapGet("/users-progress", async (IUserService userService) =>
{
    var users = new List<Champ>();
    var response = await userService.GetUsers();

    response.Where(u => !string.IsNullOrEmpty(u.TimeCompleted)).ToList().ForEach(u => users.Add(
        new Champ($"{u.FirstName} {u.LastName}", u.CardsCompleted,
        $"{Math.Round((int.Parse(u.CardsCompleted) / 52m) * 100)}%",
        u.TimeCompleted,
        $"{DateTime.Parse(u.CompletedBy).ToLongDateString()}, " +
        $"{DateTime.Parse(u.CompletedBy).ToLongTimeString()}", u.Review ?? "")));
    return new UsersVm<Champ>(users.Count, users);
});

app.Run();

internal record UsersVm<T>(int NumberOfUsers, List<T> Users) { }
internal record User(string Fullname, string RegistrationDate) { }
internal record Champ(string Fullname, string CardsCompleted, string Progress, string TimesElapsed, string CompletionDate, string review) { }
