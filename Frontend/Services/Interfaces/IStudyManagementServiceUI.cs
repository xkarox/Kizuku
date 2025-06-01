using Core;
using Core.Entities;
using Core.Requests;
using Core.Responses;

namespace Frontend.Services.Interfaces;

public interface IStudyManagementServiceUI
{
    public Task<Result<Module>> CreateModule(CreateModuleRequest createModuleRequest);
    public Task<Result<IEnumerable<Module>>> GetModules();
    public Task<Result<Module>> UpdateModule(UpdateModuleRequest updateModuleRequest);
    public Task<Result<Guid>> DeleteModule(Guid id);

    public Task<Result<Module>> AddTopic(
        AddTopicToModuleRequest addTopicRequest);

    public Task<Result<Module>> GetModule(Guid moduleId);


}