using Blazing.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Entities;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Frontend.ViewModels;

public partial class ModulesPageViewModel
    (IFrontendModuleService moduleService) : ViewModelBase
{
    [Parameter]
    public IEnumerable<Module> Modules { get; set; } = Array.Empty<Module>();
    
    public async Task FetchModules()
    {
        var result = await moduleService.GetModules();
        if (result.IsError)
        {
            return;
        }
        Modules = result!.Value! ?? Array.Empty<Module>();
    } 
}