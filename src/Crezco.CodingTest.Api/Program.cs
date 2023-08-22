using Crezco.CodingTest.Api.Location;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocation();

var app = builder.Build();

app.MapLocations();

app.Run();

public partial class Program { }