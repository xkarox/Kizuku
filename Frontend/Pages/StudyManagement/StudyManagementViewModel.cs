using Blazing.Mvvm.ComponentModel;
using Core.Entities;
using Core.Requests;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Frontend.Pages.StudyManagement;

public class StudyManagementViewModel
    (IStudyManagementServiceUI studyManagementService) : ViewModelBase
{
    public IEnumerable<Module> Modules { get; set; } = Array.Empty<Module>();
    public bool ShowCreateNewModule { get; set; } = false;
    
    public async Task FetchModules()
    {
        var result = await studyManagementService.GetModules();
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

    public async Task<bool> CreateNewModule(string title, string description)
    {
        var request = new CreateModuleRequest()
        {
            Name = title,
            Description = description,
        };
        var result = await studyManagementService.CreateModule(request);
        if (result.IsError)
        {
            Console.WriteLine("failed to create new module");
            return false;
        }
        
        await FetchModules();
        ShowCreateNewModule = false;
        return true;
    }
    
}