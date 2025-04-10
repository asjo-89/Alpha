using Business.Models;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces;

public interface IMemberUserService
{
    Task<MemberUserResult<bool>> CreateAsync(MemberUserFormData formData);
    Task<MemberUserResult<bool>> CreateUserAsync(CreateUserFormData formData);
    Task<MemberUserResult<bool>> DeleteAsync(MemberUserFormData formData);
    Task<MemberUserResult<MemberUser>> GetMemberUserAsync(string value);
    Task<MemberUserResult<IEnumerable<MemberUser>>> GetMemberUsersAsync();
    Task<MemberUserResult<bool>> UpdateAsync(MemberUserFormData formData);
}