using Business.Models;
using Domain.Dtos;

namespace Business.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult<bool>> SignInAsync(SignInFormData formData);
        Task<AuthResult<bool>> SignOutAsync(MemberUserFormData formData);
        Task<MemberUserResult<bool>> CreateUserAsync(CreateUserFormData formData);
    }
}