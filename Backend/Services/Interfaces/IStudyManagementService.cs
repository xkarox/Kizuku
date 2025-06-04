using Core.Entities;
using Core.Requests;
using Core.Responses;

namespace Core;

public interface IStudyManagementService
{
    public Task<Result<IEnumerable<Module>>> GetUserModules(Guid userId);
    public Task<Result<Module>> GetModuleWithTopics(Guid moduleId);
    public Task<Result<IEnumerable<Status>>> GetTopicStates();
    public Task<Result<Module>> AddTopicToModule(
        AddTopicToModuleRequest request);
    public Task<Result> RemoveTopicFromModule(Guid moduleId, Guid topicId);
    public Task<Result<Module>> CreateUserModule(CreateModuleRequest createModuleRequest, 
        Guid userId);
    public Task<Result<Module>> UpdateUserModule(UpdateModuleRequest updateModuleRequest, 
        Guid userId);
    public Task<Result> DeleteUserModule(Guid moduleId, Guid userId);
}