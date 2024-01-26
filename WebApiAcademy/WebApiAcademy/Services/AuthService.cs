using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using WebApiAcademy.DTOs;
using WebApiAcademy;

namespace WebApiAcademy.Services
{
    public class AuthService: IAuthService
    {
        protected readonly ApplicationDbContext _context;
        protected IConfiguration _config;

        public AuthService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public UserSession? Authenticate(UserCredentials credentials)
        {
            var user = _context.Users
                .Where(u => u.Email == credentials.Email && u.Password == HashPassword(credentials.Password))
                .Select(u => new UserSession
                {
                    Name = u.Person.Name,
                    LastName = u.Person.LastName,
                    Phone = u.Person.Phone,
                    Email = u.Email,
                    PersonId = u.PersonId.ToString(),
                    Token = ""
                })
                .FirstOrDefault();
            if (user == null) return null; 
            user.Token = GenerateJwtToken(user)!;
            return user;            
        }

        public static string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);
            System.Text.StringBuilder builder = new();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }
            return builder.ToString();
        }
        public string? GenerateJwtToken(UserSession user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                claims: new Claim[]
                {
                    new ("name", user.Name),
                    new ("lastName", user.LastName),
                    new ("email", user.Email),
                    new ("phone", user.Phone),
                    new ("personId", user.PersonId)
                }
            );
            var token = tokenHandler.WriteToken(tokenDescriptor);
            return token;
        }
        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            token = token[7..];
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public string RegenerateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            token = token[7..];
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                claims: tokenHandler.ReadJwtToken(token).Claims
            );
            var newToken = tokenHandler.WriteToken(tokenDescriptor);
            return newToken;
        }
        public static IEnumerable<Claim> GetTokenClaims(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            return jwtToken.Claims;
        }
        public UserSession? GetUserFromToken(string token)
        {
            var claims = GetTokenClaims(token);
            var user = new UserSession
            {
                Name     = claims.FirstOrDefault(c => c.Type == "name")?.Value,
                LastName = claims.FirstOrDefault(c => c.Type == "lastName")?.Value,
                Email    = claims.FirstOrDefault(c => c.Type == "email")?.Value,
                Phone    = claims.FirstOrDefault(c => c.Type == "phone")?.Value,
                PersonId = claims.FirstOrDefault(c => c.Type == "personId")?.Value,
                Token    = token
            };
            return user;
        }
    }

    public interface IAuthService
    {
        UserSession? Authenticate(UserCredentials credentials);
        bool ValidateToken(string token);
        string? GenerateJwtToken(UserSession user);
        string RegenerateToken(string token);
        UserSession? GetUserFromToken(string token);

    }
}
