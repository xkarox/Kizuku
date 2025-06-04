using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Core;
using Core.Entities;
using Core.Errors;
using Core.Requests;
using Core.Responses;

namespace Frontend.Services.Interfaces;

public class StudyManagementServiceUi(
    IHttpClientFactory httpClientFactory,
    ILogger<StudyManagementServiceUi> logger)
    : IStudyManagementServiceUI
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("API");
    
    public async Task<Result<Module>> CreateModule(CreateModuleRequest createModuleRequest)
    {
        if (createModuleRequest == null)
        {
            return Result<Module>.Failure(new Error("Invalid request"));
        }
        var response = await _httpClient.PostAsJsonAsync("api/study_management/module", createModuleRequest);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>();
            logger.Log(LogLevel.Debug, error != null ? error.Message : "Unknown error");
            return Result<Module>.Failure(error ?? new Error("No error provided"));
        }
        var createModuleResponse = await response.Content.ReadFromJsonAsync<CreateModuleResponse>();
        if (createModuleResponse == null)
        {
            logger.Log(LogLevel.Debug, $"Module creation failed for module: {createModuleRequest.Name}");
            return Result<Module>.Failure(
                new Error("No module returned after creation"));
        }
        logger.Log(LogLevel.Debug, createModuleResponse.ToString());
        return Result<Module>.Success(createModuleResponse.ToModule());
    }

    public async Task<Result<IEnumerable<Module>>> GetModules()
    {
        var response = await _httpClient.GetAsync("api/study_management/module");
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>();
            logger.Log(LogLevel.Debug, error != null ? error.Message : "Unknown error");
            return Result<IEnumerable<Module>>.Failure(error ?? new Error("No error provided"));
        }
        var getModulesResponse = await response.Content.ReadFromJsonAsync<GetModulesResponse>();
        if (getModulesResponse == null)
        {
            logger.Log(LogLevel.Debug, $"Get Modules returned nothing");
            return Result<IEnumerable<Module>>.Failure(
                new Error("Get Modules returned nothing"));
        }
        logger.Log(LogLevel.Debug, getModulesResponse.ToString());
        return Result<IEnumerable<Module>>.Success(getModulesResponse.Modules);
    }

    public async Task<Result<IEnumerable<Status>>> GetTopicStates()
    {
        var response = await _httpClient.GetAsync("api/study_management/topic/states");
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>();
            logger.Log(LogLevel.Debug, error != null ? error.Message : "Unknown error");
            return Result<IEnumerable<Status>>.Failure(error ?? new Error("No error provided"));
        }
        var getStatesResponse = await response.Content.ReadFromJsonAsync<IEnumerable<Status>>();
        if (getStatesResponse == null)
        {
            logger.Log(LogLevel.Debug, $"Get Modules returned nothing");
            return Result<IEnumerable<Status>>.Failure(
                new Error("Get Modules returned nothing"));
        }
        logger.Log(LogLevel.Debug, getStatesResponse.ToString());
        return Result<IEnumerable<Status>>.Success(getStatesResponse);
    }

    public async Task<Result<Module>> UpdateModule(UpdateModuleRequest updateModuleRequest)
    {
        if (updateModuleRequest == null)
        {
            return Result<Module>.Failure(new Error("Invalid request"));
        }
        var response = await _httpClient.PutAsJsonAsync("api/study_management", updateModuleRequest);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>();
            logger.Log(LogLevel.Debug, error != null ? error.Message : "Unknown error");
            return Result<Module>.Failure(error ?? new Error("No error provided"));
        }
        var updateModuleResponse = await response.Content.ReadFromJsonAsync<UpdateModuleResponse>();
        if (updateModuleResponse == null)
        {
            logger.Log(LogLevel.Debug, $"Module update failed for module: {updateModuleRequest.Name}");
            return Result<Module>.Failure(
                new Error($"Module update failed for module: {updateModuleRequest.Name}"));
        }
        logger.Log(LogLevel.Debug, updateModuleResponse.ToString());
        return Result<Module>.Success(updateModuleResponse.ToModule());
    }

    public async Task<Result<Guid>> DeleteModule(Guid id)
    {
        if (id == Guid.Empty || id == null)
        {
            return Result<Guid>.Failure(new Error("Invalid ModuleId"));
        }
        var response = await _httpClient.DeleteAsync($"api/study_management/{id}");
        if (!response.IsSuccessStatusCode)
        {
            logger.Log(LogLevel.Debug, $"Module deletion failed for module: {id}");
            return Result<Guid>.Failure(
                new Error($"Module deletion failed for module: {id}"));
        }
        logger.Log(LogLevel.Debug, $"Deleted Module: {id}");
        return Result<Guid>.Success(id);
    }

    public async Task<Result<Module>> GetModule(Guid moduleId)
    {
        if (moduleId == Guid.Empty || moduleId == null)
        {
            return Result<Module>.Failure(new Error("Invalid ModuleId"));
        }
        
        var response = await _httpClient.GetAsync($"api/study_management/module/{moduleId}");
        if (!response.IsSuccessStatusCode)
        {
            logger.Log(LogLevel.Debug, $"Failed to get Module: {moduleId}");
            return Result<Module>.Failure(
                new Error($"Failed to get Module: {moduleId}"));
        }
        logger.Log(LogLevel.Debug, $"Got Module: {moduleId}");
        var module = await response.Content.ReadFromJsonAsync<Module>();
        if (module == null)
        {
            logger.Log(LogLevel.Debug, $"Failed to deserialize Module: {moduleId}");
            return Result<Module>.Failure(
                new Error($"Failed to deserialize Module: {moduleId}"));
        }
        return Result<Module>.Success(module);
    }
    
    public async Task<Result<Module>> AddTopic(AddTopicToModuleRequest addTopicRequest)
    {
        if (addTopicRequest == null)
        {
            logger.Log(LogLevel.Debug, $"Invalid Request");
            return Result<Module>.Failure(new Error("Invalid Request"));
        }
       
        logger.Log(LogLevel.Debug, $"Adding Topic to Module: {addTopicRequest.TopicName}");
        var response = await _httpClient.PostAsJsonAsync(
            $"api/study_management/module/addTopic", 
            addTopicRequest);
        if (!response.IsSuccessStatusCode)
        {
            logger.Log(LogLevel.Debug, $"Failed to add Topic {addTopicRequest.TopicName} to Module: {addTopicRequest.ModuleId}");
            return Result<Module>.Failure(
                new Error($"Failed to add Topic {addTopicRequest.TopicName} to Module: {addTopicRequest.ModuleId}"));
        }
        logger.Log(LogLevel.Debug, $"Topic Added to Module: {addTopicRequest.TopicName}");
        var module = await response.Content.ReadFromJsonAsync<Module>();
        if (module == null)
        {
            logger.Log(LogLevel.Debug, $"Failed to deserialize Module after Add Topic: {addTopicRequest.TopicName}");
            return Result<Module>.Failure(
                new Error($"Failed to deserialize Module after Add Topic: {addTopicRequest.TopicName}"));
        }
        return Result<Module>.Success(module);
    }

    public async Task<Result> RemoveTopicFromModule(Guid moduleId, Guid topicId)
    {
        if (moduleId == null || topicId == null)
        {
            logger.Log(LogLevel.Debug, $"Invalid Request");
            return Result.Failure(new Error("Invalid Request"));
        }
        logger.Log(LogLevel.Debug, $"Removing Topic from Module: {topicId} from {moduleId}");
        var response = await _httpClient.DeleteAsync(
            $"api/study_management/module/removeTopic/{moduleId}/{topicId}");
        if (!response.IsSuccessStatusCode)
        {
            logger.Log(LogLevel.Debug, $"Failed to delete Topic from Module: {topicId} from {moduleId}");
            return Result.Failure(
                new Error($"Failed to delete Topic from Module: {topicId} from {moduleId}"));
        }
        logger.Log(LogLevel.Debug, $"Topic deleted from Module: {topicId} from {moduleId}");
        return Result.Success();
    }
}

