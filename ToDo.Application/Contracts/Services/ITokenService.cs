using System.Security.Claims;
using ToDo.Application.DTOs;

namespace ToDo.Application.Contracts.Services
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(UserRequest user);
        void VerifyUserTokenIsEqualsUserRequest(ClaimsPrincipal user, Guid idUser);
    }
}