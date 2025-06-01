using Blazing.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Entities;
using Core.Requests;
using Core.Responses;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Frontend.Pages.StudyManagement;

public partial class StudyManagementViewModel
    (IStudyManagementServiceUI studyManagementService,
        NavigationManager navigationManager) : ViewModelBase
{
    [ObservableProperty]
    public IEnumerable<Module> _modules = Array.Empty<Module>();
    [ObservableProperty]
    public Module? _selectedModule = null;
    
    [ObservableProperty]
    public bool _showCreateNewModule = false;
    [ObservableProperty]
    public bool _showDeleteModule = false;
    
    [ObservableProperty]
    public string _newModuleName  = string.Empty;
    [ObservableProperty]
    public string _newModuleDescription  = string.Empty;
    [ObservableProperty]
    public Dictionary<string, string>? _createModuleErrors;
    
    [ObservableProperty]
    public Module? _moduleToDelete = null;
    [ObservableProperty]
    public Dictionary<string, string> _deleteModuleErrors = new();
    
    [RelayCommand]
    public async Task FetchModules()
    {
        var result = await studyManagementService.GetModules();
        if (result.IsError)
        {
            return;
        }

        Modules = result!.Value! ?? Array.Empty<Module>();
    }
    
    [RelayCommand]
    public async Task FetchModule(string guid)
    {
        var result = await studyManagementService.GetModules();
        if (result.IsError)
        {
            return;
        }

        Modules = result!.Value! ?? Array.Empty<Module>();
    }
            
    [RelayCommand]
    public void SelectModuleButtonHandler(Module module)
    {
        navigationManager.NavigateTo($"/studyManagement/{module.Id}");
    }
    
    [RelayCommand]
    public void AddModuleButtonHandler()
    {
        ShowCreateNewModule = true;
    }
    
    [RelayCommand]
    public void DeleteModuleButtonHandler(Module module)
    {
        ModuleToDelete = module;
        Console.WriteLine(module.Name);
        ShowDeleteModule = true;
    }
    
    [RelayCommand]
    public async Task CreateNewModule()
    {
        CreateModuleErrors = [];
        if (string.IsNullOrWhiteSpace(NewModuleName) || NewModuleName.Length < 2) { _createModuleErrors.TryAdd("Name", "Module name must be at least 2 characters."); }
        if (CreateModuleErrors.Count > 0)
        {
            return;
        }
            
        var request = new CreateModuleRequest()
        {
            Name = NewModuleName,
            Description = NewModuleDescription,
        };
        var result = await studyManagementService.CreateModule(request);
        if (result.IsError)
        {
            Console.WriteLine("failed to create new module");
            CreateModuleErrors.TryAdd("description", "Failed to create module - server error");
            return;
        }
        _ = FetchModules();
        NewModuleName = string.Empty;
        NewModuleDescription = string.Empty;
        ShowCreateNewModule = false;
    }
    
    [RelayCommand]
    public async Task DeleteModule()
    {
        DeleteModuleErrors = new Dictionary<string, string>();
        var result = await studyManagementService.DeleteModule(ModuleToDelete.Id);
        if (result.IsError)
        {
            Console.WriteLine("failed to delete module");
            DeleteModuleErrors.TryAdd("delete", "Failed to delete module");
        }

        Modules = Modules.Where(module => module.Id != result.Value);
        ShowDeleteModule = false;
    }
    
    
    
}

