//using Business.Interfaces;
//using Business.Models;
//using Business.Factories;
//using Data.Entities;
//using Data.Interfaces;
//using Domain.Dtos;
//using Domain.Models;
//using System.Diagnostics;

//namespace Business.Services;

//public class ProjectNoteService(IProjectNoteRepository projectNoteRepository) : IProjectNoteService
//{
//    private readonly IProjectNoteRepository _projectNoteRepository = projectNoteRepository;

//    public async Task<ProjectNoteResult> CreateAsync(ProjectNoteDto formData)
//    {
//        if (formData == null)
//            return new ProjectNoteResult { Succeeded = false, StatusCode = 400, ErrorMessage = "All required fields must be completed." };

//        try
//        {
//            var started = await _projectNoteRepository.BeginTransactionAsync();

//            var entity = formData.MapTo<ProjectNoteEntity>();

//            var result = await _projectNoteRepository.CreateAsync(entity);
//            if (!result.Success)
//                return new ProjectNoteResult { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Failed to create note." };

//            await _projectNoteRepository.CommitTransactionAsync();
//            return new ProjectNoteResult { Succeeded = true, StatusCode = 201 };
//        }
//        catch (Exception ex)
//        {
//            var rollback = await _projectNoteRepository.RollbackTransactionAsync();
//            Debug.WriteLine($"**********\n{ex.Message}\n**********");
//            return new ProjectNoteResult { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to create note: {ex.Message} " };
//        }
//    }


//    public async Task<ProjectNoteResult<IEnumerable<ProjectNote>>> GetNotesAsync()
//    {
//        var entities = await _projectNoteRepository.GetAllAsync(
//            orderByDescending: true,
//            orderBy: x => x.Created,
//            includes: x => x.Member);

//        if (entities.Data == null || !entities.Success)
//            return new ProjectNoteResult<IEnumerable<ProjectNote>> { Succeeded = false, StatusCode = 404, ErrorMessage = "No project notes were found." };

//        var notes = entities.Data.Select(ProjectNoteFactory.CreateModelFromEntity).ToList();

//        return entities.Success
//            ? new ProjectNoteResult<IEnumerable<ProjectNote>> { Succeeded = true, StatusCode = 200, Data = notes }
//            : new ProjectNoteResult<IEnumerable<ProjectNote>> { Succeeded = false, StatusCode = entities.StatusCode, ErrorMessage = entities.Error };
//    }


//    public async Task<ProjectNoteResult<ProjectNote>> GetNoteAsync(Guid projectId)
//    {
//        var noteEntity = await _projectNoteRepository.GetAsync(
//            filterBy: x => x.ProjectId == projectId,
//            includes: x => x.Member);

//        if (noteEntity.Data == null || !noteEntity.Success)
//            return new ProjectNoteResult<ProjectNote> { Succeeded = false, StatusCode = 404, ErrorMessage = "No project note was found." };

//        var note = ProjectNoteFactory.CreateModelFromEntity(noteEntity.Data);
//        return note != null
//            ? new ProjectNoteResult<ProjectNote> { Succeeded = true, StatusCode = 200, Data = note }
//            : new ProjectNoteResult<ProjectNote> { Succeeded = false, StatusCode = 400, ErrorMessage = "Unable to retrieve project note." };
//    }


//    public async Task<ProjectNoteResult> UpdateAsync(ProjectNoteDto formData)
//    {
//        if (formData == null)
//            return new ProjectNoteResult { Succeeded = false, StatusCode = 400, ErrorMessage = "All reaquired fields are not completed." };

//        var entity = formData.MapTo<ProjectNoteEntity>();

//        try
//        {
//            await _projectNoteRepository.BeginTransactionAsync();

//            var result = await _projectNoteRepository.UpdateAsync(entity);

//            if (!result.Success)
//                return new ProjectNoteResult { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Unable to update note." };

//            await _projectNoteRepository.CommitTransactionAsync();
//            return new ProjectNoteResult { Succeeded = true, StatusCode = 200 };
//        }
//        catch (Exception ex)
//        {
//            var rollback = await _projectNoteRepository.RollbackTransactionAsync();
//            Debug.WriteLine($"**********\n{ex.Message}\n**********");
//            return new ProjectNoteResult { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to update note: {ex.Message} " };
//        }
//    }


//    public async Task<ProjectNoteResult> DeleteAsync(ProjectNoteDto formData)
//    {
//        var entity = formData.MapTo<ProjectNoteEntity>();

//        try
//        {
//            await _projectNoteRepository.BeginTransactionAsync();

//            var result = await _projectNoteRepository.DeleteAsync(entity);
//            if (!result.Success)
//                return new ProjectNoteResult { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Unable to delete note." };

//            await _projectNoteRepository.CommitTransactionAsync();

//            return new ProjectNoteResult { Succeeded = true, StatusCode = 200 };
//        }
//        catch (Exception ex)
//        {
//            var rollback = await _projectNoteRepository.RollbackTransactionAsync();
//            Debug.WriteLine($"**********\n{ex.Message}\n**********");
//            return new ProjectNoteResult { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to delete note: {ex.Message} " };
//        }
//    }
//}
