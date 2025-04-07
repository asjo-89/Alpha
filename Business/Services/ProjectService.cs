using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System.Linq.Expressions;

namespace Business.Services;

public class ProjectService(IBaseRepository<ProjectEntity> baseRepsitory) : IProjectService
{
    private readonly IBaseRepository<ProjectEntity> _baseRepsitory = baseRepsitory;

    public async Task<bool> CreateAsync(ProjectRegForm form)
    {
        if (form == null) return false;

        var picture = new PictureEntity
        {
            PictureUrl = form.ProjectImage
        };


        try
        {
            ProjectEntity entity = ProjectFactory.CreateEntityFromDto(form, picture);

            await _baseRepsitory.BeginTransactionAsync();

            var result = await _baseRepsitory.CreateAsync(entity);

            if (result)
            {
                await _baseRepsitory.SaveChangesAsync();
                await _baseRepsitory.CommitTransactionAsync();

                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to create project: {ex.Message}");
            await _baseRepsitory.RollbackTransactionAsync();
            return false;
        }
    }


    public async Task<IEnumerable<ProjectModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }


    public async Task<ProjectModel> GetOneAsync(Expression<Func<ProjectModel, bool>> expression)
    {
        throw new NotImplementedException();
    }


    public async Task<bool> UpdateAsync(ProjectModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(ProjectModel model)
    {
        throw new NotImplementedException();
    }
}
