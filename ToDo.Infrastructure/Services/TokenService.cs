using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDo.Application.Contracts.Repositories;
using ToDo.Application.Contracts.Services;
using ToDo.Application.DTOs;
using static BCrypt.Net.BCrypt;

namespace ToDo.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IUserRepository _userRepository;

        public TokenService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<string> GenerateJwtToken(UserRequest user)
        {
            if (!(await _userRepository.VerifyUserAndPassword(user)))
                throw new SecurityTokenValidationException("User or password not correct");

            var userBank = await _userRepository.GetByEmail(user.Email);
            var claims = new[]
            {
             new Claim(JwtRegisteredClaimNames.Email, user.Email),
             new Claim(JwtRegisteredClaimNames.Jti, userBank.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("7154d034-394b-4a4b-96d6-50eaad7cf9ab"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void VerifyUserTokenIsEqualsUserRequest(ClaimsPrincipal user, Guid idUser) 
        { 
            var userToken =  user?.Claims?.FirstOrDefault(c => c.Type.Equals("jti", StringComparison.OrdinalIgnoreCase))?.Value;
            if (userToken != null && !string.Equals(userToken, idUser.ToString(), StringComparison.OrdinalIgnoreCase))
                throw new SecurityTokenException("This token belongs to another user");
        }
    }
}
