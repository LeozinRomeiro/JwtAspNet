using JwtAspNet.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAspNet.Service
{
	public class TokenService
	{
		public string CreateToken(User user)
		{
			var handler = new JwtSecurityTokenHandler();

			var key = Encoding.ASCII.GetBytes(Configuration.PrivateKey);
			var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
			var tokemDescriptor = new SecurityTokenDescriptor
			{
				SigningCredentials = credentials,
				Expires = DateTime.UtcNow.AddHours(1),
				Subject = GenerateClaims(user),
			};

			new Claim(ClaimTypes.Name, "Iron Man");
			new Claim(ClaimTypes.GivenName, "Tony Stark");
			new Claim(ClaimTypes.Email, "PlayBoy@stark.com");
			new Claim(ClaimTypes.Role, "login");

			var token = handler.CreateToken(tokemDescriptor);
			return handler.WriteToken(token);
		}

		private static ClaimsIdentity GenerateClaims(User user)
		{
			var claims = new ClaimsIdentity();

			claims.AddClaim(new Claim(ClaimTypes.Name,user.Email));
			claims.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
			claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));
			claims.AddClaim(new Claim("Id", user.Id.ToString())); 
			claims.AddClaim(new Claim("Image", user.Image));

			foreach (var role in user.Roles)
				claims.AddClaim(new Claim(ClaimTypes.Role, role));

			return claims;
		}
	}
}
