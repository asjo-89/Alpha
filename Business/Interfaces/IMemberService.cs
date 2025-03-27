using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;

public interface IMemberService
{
    Task<MemberModel> AddMember(CreateMemberRegForm form);
    Task<IEnumerable<MemberModel>> GetAllMembers();
    Task<MemberModel> GetMember(MemberModel model);
    Task<MemberModel> UpdateMember(MemberModel model);
    Task<bool> DeleteMember(MemberModel model);
}
