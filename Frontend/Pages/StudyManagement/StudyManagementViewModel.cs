using Blazing.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Entities;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Frontend.ViewModels;

public class StudyManagementViewModel
    (IStudyManagementServiceUI moduleServiceUi) : ViewModelBase
{
    [Parameter]
    public IEnumerable<Module> Modules { get; set; } = Array.Empty<Module>();
    public bool ShowCreateNewModule { get; set; } = false;
    
    public async Task FetchModules()
    {
        var result = await moduleServiceUi.GetModules();
        if (result.IsError)
        {
            return;
        }
        Modules = result!.Value! ?? Array.Empty<Module>();
    }

    public void AddModuleButtonHandler()
    {
        ShowCreateNewModule = true;
    }
    
}