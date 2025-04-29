using Business.Interfaces;
using Data.Entities;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Models;
using System.Linq.Expressions;

namespace Business.Services;

public class ProjectMemberService(IProjectMemberRepository repository) : IProjectMemberService
{
    private readonly IProjectMemberRepository _repository = repository;

    public async Task<List<ProjectMember>> ExistingAsync(ProjectDto dto)
    {
        if (dto == null)
            return new List<ProjectMember>();

        var projectMembers = await _repository.GetAllAsync(
            filterBy: x => x.ProjectId == dto.Id);

        var list = projectMembers.Data?.Select(pm => new ProjectMember
        {
            ProjectId = pm.ProjectId,
            MemberId = pm.MemberId
        }).ToList() ?? [];

        return list;

    }

    public async Task<bool> AddAsync(ProjectMemberDto dto)
    {
        if (dto == null)
            return false;

        var entity = new ProjectMemberEntity
        {
            ProjectId = dto.ProjectId,
            MemberId = dto.MemberId
        };

        try
        {
            await _repository.BeginTransactionAsync();

            var result = await _repository.AddAsync(entity);
            if (!result)
                return false;

            await _repository.CommitTransactionAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to add ProjectMember: {ex.Message}");
            await _repository.RollbackTransactionAsync();
            return false;
        }
    }

    public async Task<IEnumerable<ProjectMember>> GetProjectMembersAsync(Guid id)
    {
        if (id == Guid.Empty) 
            return new List<ProjectMember>();

        var pmList = await _repository.GetAllAsync(
            filterBy: x => x.ProjectId == id || x.MemberId == id,
            includes: x => x.Member);

        return pmList.Data.Select(member => new ProjectMember
        {
            ProjectId = member.ProjectId,
            MemberId = member.MemberId
        });
    }

    public async Task<IEnumerable<MemberUser>> GetProjectMembersWithDetailsAsync(Guid id)
    {
        if (id == Guid.Empty)
            return new List<MemberUser>();

        var pmList = await _repository.GetAllAsync(
            filterBy: x => x.ProjectId == id || x.MemberId == id,
            includes:
            [
                x => x.Member,
                x => x.Member.Picture
            ]);

        return pmList.Data.Select(member => new MemberUser
        {
            Id = member.Member.Id,
            FirstName = member.Member.FirstName,
            LastName = member.Member.LastName,
            ImageUrl = member.Member.Picture.ImageUrl
        }) ?? new List<MemberUser>();
    }

    public async Task<bool> DeleteAsync(IEnumerable<ProjectMember> projectMembers)
    {
        if (projectMembers == null)
            return false;

        var pm = projectMembers.Select(pm => new ProjectMemberEntity
        {
            ProjectId = pm.ProjectId,
            MemberId = pm.MemberId
        }).ToList();

        try
        {
            await _repository.BeginTransactionAsync();

            var result = await _repository.DeleteAsync(pm);
            if (!result)
                return false;

            await _repository.CommitTransactionAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to remove ProjectMember: {ex.Message}");
            await _repository.RollbackTransactionAsync();
            return false;
        }
    }
}
