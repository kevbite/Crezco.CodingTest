using Crezco.CodingTest.Api;
using Crezco.CodingTest.Api.Location;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMongoDb()
    .AddMassTransit()
    .AddLocation();

var app = builder.Build();

app.MapLocations();

app.Run();

public partial class Program { }