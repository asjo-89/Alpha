using Data.Entities;
using Domain.Models;

namespace Data.Interfaces;

public interface IPictureRepository : IBaseRepository<PictureEntity, Picture>
{
}
