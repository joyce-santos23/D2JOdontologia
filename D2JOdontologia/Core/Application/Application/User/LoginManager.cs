using Application.User.Ports;
using UserEntity = Domain.Entities.User;
using Domain.Ports;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.User.Dtos;
using Domain.Security;

namespace Application.User
{
    public class LoginManager : ILoginManager
    {
        private readonly IUserRepository _userRepository;
        private readonly SigningConfigurations _signingConfigurations;
        private readonly TokenConfigurations _tokenConfigurations;

        public LoginManager(IUserRepository userRepository, SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations)
        {
            _userRepository = userRepository;
            _signingConfigurations = signingConfigurations;
            _tokenConfigurations = tokenConfigurations;
        }

        public async Task<string> Authenticate(LoginDto loginDto)
        {
            var user = await _userRepository.Authenticate(loginDto.Email, loginDto.Password);

            if (user == null)
                return null;

            return GenerateToken(user);
        }

        private string GenerateToken(UserEntity user)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, GetRole(user))
            });

            var creationDate = DateTime.UtcNow;
            var expirationDate = creationDate.AddSeconds(_tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = creationDate,
                Expires = expirationDate
            });

            return handler.WriteToken(securityToken);
        }

        private string GetRole(UserEntity user)
        {
            return user.GetType().Name switch
            {
                "Patient" => "Patient",
                "Specialist" => "Specialist",
                _ => "User"
            };
        }
    }
}
