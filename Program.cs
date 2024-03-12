using JwtAspNet.Service;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddTransient<TokenService>();

app.MapGet("/", (TokenService service) => service.CreateToken);

app.Run();
