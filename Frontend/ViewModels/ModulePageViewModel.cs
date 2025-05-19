using Blazing.Mvvm.ComponentModel;
using Core.Entities;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Frontend.ViewModels;

public partial class ModulePageViewModel(NavigationManager navigationManager,
    IFrontendModuleService moduleService
    ) : ViewModelBase
{
    public Module Module { get; set; } = null!;
    public Guid ModuleId { get; set; } = Guid.Empty;
    public void FetchAndSetup(Guid moduleId)
    {
        ModuleId = moduleId;
    }
}