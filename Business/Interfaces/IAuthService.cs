using Business.Models;
using Domain.Dtos;

namespace Business.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult<bool>> SignInAsync(SignInDto formData);
        Task<AuthResult<bool>> SignOutAsync(MemberUserDto formData);
        Task<MemberUserResult<bool>> CreateUserAsync(CreateAccountDto dto);
    }
}