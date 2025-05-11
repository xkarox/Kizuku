using System.Net.Http.Json;
using Core;
using Core.Entities;
using Core.Errors;
using Core.Requests;
using Core.Responses;

namespace Frontend.Services.Interfaces;

public class FrontendModuleService(
    IHttpClientFactory httpClientFactory,
    ILogger<FrontendModuleService> logger)
    : IFrontendModuleService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("API");
    
    public async Task<Result<Module>> CreateModule(CreateModuleRequest createModuleRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Module", createModuleRequest);
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
        var response = await _httpClient.GetAsync("api/Module");
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

    public async Task<Result<Module>> UpdateModule(UpdateModuleRequest updateModuleRequest)
    {
        var response = await _httpClient.PutAsJsonAsync("api/Module", updateModuleRequest);
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
        var response = await _httpClient.DeleteAsync("api/Module");
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>();
            logger.Log(LogLevel.Debug, error != null ? error.Message : "Unknown error");
            return Result<Guid>.Failure(error ?? new Error("No error provided"));
        }
        var updateModuleResponse = await response.Content.ReadFromJsonAsync<UpdateModuleResponse>();
        if (updateModuleResponse == null)
        {
            logger.Log(LogLevel.Debug, $"Module deletion failed for module: {id}");
            return Result<Guid>.Failure(
                new Error($"Module deletion failed for module: {id}"));
        }
        logger.Log(LogLevel.Debug, updateModuleResponse.ToString());
        return Result<Guid>.Success(updateModuleResponse.ModuleId);
    }
}