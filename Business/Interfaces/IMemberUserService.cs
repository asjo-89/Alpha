using Business.Models;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces;

public interface IMemberUserService
{
    Task<MemberUserResult<bool>> CreateAsync(MemberUserDto formData);
    Task<MemberUserResult<bool>> DeleteAsync(Guid id);
    Task<MemberUserResult<MemberUser>> GetMemberUserAsync(string value);
    Task<MemberUserResult<IEnumerable<MemberUser>>> GetMemberUsersAsync();
    Task<MemberUserResult<MemberUser>> GetMemberUserAsync(Guid id);
    Task<MemberUserResult<bool>> UpdateAsync(MemberUserDto formData);
    Task<MemberUserResult<bool>> ExistsAsync(Guid id);
}