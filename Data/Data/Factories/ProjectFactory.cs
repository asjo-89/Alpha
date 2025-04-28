//using Data.Entities;
//using Domain.Helpers;
//using Domain.Models;

//namespace Data.Factories;

//public static class ProjectFactory
//{
//    public static ProjectEntity CreateEntityFromModel(Project model)
//    {
//        return new ProjectEntity
//        {
//            ProjectTitle = model.ProjectTitle,
//            Description = model.Description,
//            StartDate = model.StartDate,
//            EndDate = model.EndDate,
//            Budget = model.Budget,
//            Created = model.Created,
//            PictureId = model.PictureId,
//            ClientId = model.Client.Id,
//            StatusId = StatusHelper.SetStatus(S),
//            ProjectMembers = [.. model.ProjectMembers!.Select(member =>
//                new ProjectMemberEntity
//                {
//                    MemberId = member.Id,
//                    ProjectId = model.Id
//                })]
//        };
//    }

//    public static Project CreateModelFromEntity(ProjectEntity entity)
//    {
//        return new Project
//        {
//            Id = entity.Id,
//            ProjectTitle = entity.ProjectTitle,
//            Description = entity.Description,
//            StartDate = entity.StartDate,
//            EndDate = entity.EndDate,
//            Budget = entity.Budget,
//            Created = entity.Created,
//            PictureId = entity.PictureId,
//            Client = new Client
//            {
//                Id = entity.Client.Id,
//                ClientName = entity.Client.ClientName,
//                Email = entity.Client.Email,
//                PhoneNumber = entity.Client.PhoneNumber
//            },
//            Status = new Status
//            {
//                Id = entity.Status.Id,
//                StatusName = entity.Status.StatusName
//            }
//        };
//    }
//}
