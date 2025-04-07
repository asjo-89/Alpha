using Business.Dtos;
using Business.Models;
using System.Linq.Expressions;


namespace Business.Interfaces;

public interface IClientService
{
    Task<bool> CreateAsync(ClientRegForm form);
    Task<IEnumerable<ClientModel>> GetAllAsync();
    Task<ClientModel> GetOneAsync(Expression<Func<ClientModel, bool>> expression);
    Task<bool> UpdateAsync(ClientModel model);
    Task<bool> DeleteAsync(ClientModel model);
}
