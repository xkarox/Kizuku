using Core;
using Core.Errors;
using Core.Requests;
using Core.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/study_management")]
public class StudyManagementController(
    IStudyManagementService studyManagementService
    ) : ControllerBase
{
    
    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetModulesResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IError))]
    public async Task<IActionResult> GetModules()
    {
        var id = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(id))
            return BadRequest(new Error("User does not have an identifier claim"));
        var guid = new Guid(id);
        var modulesResult = await studyManagementService.GetUserModules(guid);
        if (modulesResult.IsError)
        {
            return BadRequest(modulesResult.Error);
        }
        return Ok(modulesResult.Value!.ToUserModulesResponse(guid));
    }
    
    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateModuleResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IError))]
    public async Task<IActionResult> CreateModule([FromBody] CreateModuleRequest createModuleRequest)
    {
        var id = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(id))
            return BadRequest(new Error("User does not have an identifier claim"));
        var guid = new Guid(id);
        var modulesResult = await studyManagementService.CreateUserModule(createModuleRequest, guid);
        if (modulesResult.IsError)
        {
            return BadRequest(modulesResult.Error);
        }
        return Ok(modulesResult.Value!.ToCreateModuleResponse());
    }
    
    [Authorize]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateModuleResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IError))]
    public async Task<IActionResult> UpdateModule([FromBody] UpdateModuleRequest updateModuleRequest)
    {
        var id = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(id))
            return BadRequest(new Error("User does not have an identifier claim"));
        var guid = new Guid(id);
        var modulesResult = await studyManagementService.UpdateUserModule(updateModuleRequest, guid);
        if (modulesResult.IsError)
        {
            return BadRequest(modulesResult.Error);
        }
        return Ok(modulesResult.Value!.ToUpdateModuleResponse());
    }
    
    [Authorize]
    [HttpDelete("/{moduleId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteModuleResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IError))]
    public async Task<IActionResult> DeleteModule(string moduleId)
    {
        var id = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(id))
            return BadRequest(new Error("User does not have an identifier claim"));
        var userGuid = new Guid(id);
        var moduleGuid = new Guid(moduleId);
        var modulesResult = await studyManagementService.DeleteUserModule(moduleGuid, userGuid);
        if (modulesResult.IsError)
        {
            return BadRequest(modulesResult.Error);
        }
        return Ok();
    }
}