using Blazing.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Entities;
using Frontend.Services.Interfaces;

namespace Frontend.Pages.StudyManagement.Pages.ModuleDetails;

public partial class ModuleDetailsViewModel
    (IStudyManagementServiceUI studyManagementService) : ViewModelBase
{
    [ObservableProperty] private Module? _module = null;

    [RelayCommand]
    public async Task FetchModule(Guid moduleId)
    {
        var result = await studyManagementService.GetModules();
        if (result.IsError)
        {
            SetDummyModule();
        }
        Module = result!.Value!.FirstOrDefault(module => module.Id == moduleId);
    }

    private void SetDummyModule()
    {
        Module = new Module()
        {
            Id = Guid.NewGuid(),
            Name = "Failed to fetch module",
        };
    }
}