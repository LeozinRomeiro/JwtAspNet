using JwtAspNet;
using JwtAspNet.Models;
using JwtAspNet.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<TokenService>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(x =>
{
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
	x.TokenValidationParameters = new TokenValidationParameters
	{
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.PrivateKey)),
		ValidateAudience = false,
		ValidateIssuer = false,
	};
});

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/login", (TokenService service) =>
{
	var user = new User(1, "Tony Stark", "PlayBoy@stark.com", "https://fotos.com/", "123", new[] { "Rico", "Vingador" });
	return service.CreateToken(user);

});

app.MapGet("/restrito", (TokenService service) =>"Acesso aprovado").RequireAuthorization();

app.Run();
