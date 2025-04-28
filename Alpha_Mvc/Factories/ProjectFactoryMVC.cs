using Alpha_Mvc.Models;
using Domain.Dtos;
using Domain.Models;

namespace Alpha_Mvc.Factories;

public static class ProjectFactoryMVC
{
    public static ProjectDto CreateDtoFromCreateForm(CreateProjectFormModel formModel) => new ProjectDto
    {
        ProjectTitle = formModel.ProjectTitle,
        Description = formModel.Description ?? "",
        StartDate = formModel.StartDate,
        EndDate = formModel.EndDate,
        Budget = formModel.Budget,
        ImageUrl = formModel.ImageUrl,
        ClientId = Guid.Empty,
        //Members = formModel.SelectedMembers.Select(member =>
        //    new MemberUserDto
        //    {
        //        Id = member
        //    }).ToList()
    };

    public static ProjectDto CreateDtoFromEditForm(EditProjectFormModel formModel) => new ProjectDto
    {
        Id = formModel.Id,
        ProjectTitle = formModel.ProjectTitle,
        Description = formModel.Description ?? "",
        StartDate = formModel.StartDate,
        EndDate = formModel.EndDate,
        Budget = formModel.Budget,
        ImageUrl = formModel.ImageUrl,
        ClientId = formModel.ClientId
    };

    public static ProjectCardModel CreateCardFromDomainModel(Project project) => new ProjectCardModel
    {
        Id = project.Id,
        ProjectTitle = project.ProjectTitle,
        Description = project.Description ?? "",
        ClientName = project.Client?.ClientName ?? "Unknown",
        ImageUrl = project.ImageUrl,
        StartDate = project.StartDate,
        EndDate = project.EndDate,
        StatusName = project.StatusName,
        Budget = project.Budget,
        MemberUsers = project.ProjectMembers?.Select(member => new MemberUser
        {
            Id = member.Id,
            ImageUrl = member.ImageUrl,
            FirstName = member.FirstName,
            LastName = member.LastName
        }).ToList() ?? []
    };

    public static ProjectDetailsModel CreateDetailsFromDomainModel(Project project) => new ProjectDetailsModel
    {
        Id = project.Id,
        ProjectTitle = project.ProjectTitle,
        Description = project.Description ?? "",
        StartDate = project.StartDate,
        EndDate = project.EndDate,
        Client = new ClientModel
        {
            Id = project.Client.Id,
            ClientName = project.Client.ClientName,
            Email = project.Client.Email,
            PhoneNumber = project.Client.PhoneNumber ?? "Unknown"
        },
        StatusName = project.StatusName,
        Budget = project.Budget ?? 0,
        ImageUrl = project.ImageUrl,
        Members = project.ProjectMembers?.Select(member => new MemberUserModel
        {
            Id = member.Id,
            ImageUrl = member.ImageUrl,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Email = member.Email,
            PhoneNumber = member.PhoneNumber ?? "Unknown"
        }).ToList() ?? [],
        ProjectNotes = project.ProjectNotes?.Select(note => new ProjectNotesModel
        {
            Id = note.Id,
            Created = note.Created,
            Note = note.Content,
            Member = new MemberUserModel
            {
                Id = note.Member.Id,
                FirstName = note.Member.FirstName,
                LastName = note.Member.LastName
            },
            ProjectId = project.Id
        }).ToList() ?? []
    };
}
