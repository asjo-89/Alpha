using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;

public interface ICreateMemberService
{
    Task<MemberModel> AddMember(CreateMemberRegForm form);
    Task<IEnumerable<MemberModel>> GetAllMembers();
    Task<MemberModel> GetMember(MemberModel model);
    Task<MemberModel> UpdateMember(MemberModel model);
    Task<bool> DeleteMember(MemberModel model);
}
