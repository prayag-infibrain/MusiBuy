using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusiBuy.Common.DB;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.ResponseModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthenticationRepository : IAuthenticate
{
    private readonly IConfiguration _configuration;
    private readonly MusiBuyDB_Connection _context;

    public AuthenticationRepository(IConfiguration configuration, MusiBuyDB_Connection context)
    {
        _configuration = configuration;
        _context = context;
    }

    public TokenResult AuthenticateByEmail(string EmailId, string password)
    {
        var expirationTime = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"]));

        var claims = new[]
        {
        new Claim(ClaimTypes.Email, EmailId),
        new Claim(ClaimTypes.NameIdentifier, EmailId)
        };

        string keyValue = _configuration["Jwt:SecretKey"];

        if (string.IsNullOrEmpty(keyValue))
            throw new Exception("JWT SecretKey is missing from configuration");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expirationTime,
            signingCredentials: creds);

        return new TokenResult
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expirationTime
        };
    }
}


