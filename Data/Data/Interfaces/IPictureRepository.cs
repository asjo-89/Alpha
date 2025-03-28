using Data.Entities;
using System.Linq.Expressions;

namespace Data.Interfaces;

public interface IPictureRepository
{
    Task<PictureEntity> GetOrAddAsync(string url);
}
