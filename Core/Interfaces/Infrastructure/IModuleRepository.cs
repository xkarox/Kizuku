using Core.Entities;

namespace Core;

public interface IModuleRepository : IRepositoryBase<Module>
{
    public Task<Result<IEnumerable<Module>>> GetAllByUserId(Guid userId);
}