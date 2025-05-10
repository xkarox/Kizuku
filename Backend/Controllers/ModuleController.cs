using Core;
using Core.Errors;
using Core.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModuleController(
    IModuleService moduleService
    ) : ControllerBase
{
    
    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModulesResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IError))]
    public async Task<IActionResult> GetModules()
    {
        var id = User.Claims.FirstOrDefault(c => c.Type == "nameidentifier")?.Value;
        if (string.IsNullOrEmpty(id))
            return BadRequest(new Error("User does not have an identifier claim"));
        var guid = new Guid(id);
        var modulesResult = await moduleService.GetUserModules(guid);
        if (modulesResult.IsError)
        {
            return BadRequest(modulesResult.Error);
        }
        return Ok(modulesResult.Value!.ToUserModulesResponse(guid));
    }
}