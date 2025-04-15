using Business.Models;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces;

public interface IMemberUserService
{
    Task<MemberUserResult<bool>> CreateAsync(MemberUserFormData formData);
    Task<MemberUserResult<bool>> DeleteAsync(MemberUser formData);
    Task<MemberUserResult<MemberUser>> GetMemberUserAsync(string value);
    Task<MemberUserResult<IEnumerable<MemberUser>>> GetMemberUsersAsync();
    Task<MemberUserResult<MemberUser>> GetMemberUserAsync(Guid id);
    Task<MemberUserResult<bool>> UpdateAsync(MemberUserFormData formData);
    Task<MemberUserResult<bool>> ExistsAsync(Guid id);
}