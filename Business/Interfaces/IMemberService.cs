using Business.Dtos;
using Business.Models;
using Data.Entities;
using System.Linq.Expressions;

namespace Business.Interfaces;

public interface IMemberService
{
    Task<MemberModel> AddMemberAsync(CreateMemberRegForm form);
    Task<IEnumerable<MemberModel>> GetAllMembersAsync();
    Task<MemberModel> GetMemberAsync(Expression<Func<MemberUserEntity, bool>> expression);
    Task<bool> ExistsAsync(Expression<Func<MemberUserEntity, bool>> expression);
    Task<MemberModel> UpdateMember(MemberModel model);
    Task<bool> DeleteMember(Guid id);
}
