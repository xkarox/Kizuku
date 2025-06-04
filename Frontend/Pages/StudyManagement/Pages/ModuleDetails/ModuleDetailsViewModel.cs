using Blazing.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Entities;
using Core.Requests;
using Frontend.Services.Interfaces;

namespace Frontend.Pages.StudyManagement.Pages.ModuleDetails;

public partial class ModuleDetailsViewModel
    (IStudyManagementServiceUI studyManagementService) : ViewModelBase
{
    [ObservableProperty] private Module? _module = null;
    [ObservableProperty] private bool _showAddTopic = false;
    [ObservableProperty] private string _newTopicName = string.Empty;
    [ObservableProperty] private string? _newTopicDescription = string.Empty;
    [ObservableProperty] private Dictionary<string, string> _addTopicErrors= new();
    [ObservableProperty] private bool _showTopicInfo = false;
    [ObservableProperty] private Topic? _selectedTopic = null;
    [ObservableProperty] private ICollection<Status>? _availableTopicStates = null;
    [ObservableProperty] private Status? _currentSelectedState = null;
    

    [RelayCommand]
    public async Task FetchModule(Guid moduleId)
    {
        var result = await studyManagementService.GetModule(moduleId);
        if (result.IsError)
        {
            Console.WriteLine(result.Error);
            SetDummyModule();
        }
        Module = result!.Value!;
    }
    
    [RelayCommand]
    public async Task FetchTopicStates()
    {
        var result = await studyManagementService.GetTopicStates();
        if (result.IsError)
        {
            Console.WriteLine($"Error fetching topic states: {result.Error}");
        }
        AvailableTopicStates = result!.Value! as ICollection<Status>;
    }

    [RelayCommand]
    public void AddTopicButtonHandler()
    {
        ShowAddTopic = true; 
    }
    
    [RelayCommand]
    public async Task AddTopic()
    {
        AddTopicErrors = [];
        if (string.IsNullOrWhiteSpace(NewTopicName)) { AddTopicErrors.TryAdd("Name", "Name is required"); }
        if (Module == null) { AddTopicErrors.TryAdd("Module", "No module selected"); }
        if (AddTopicErrors.Count != 0) return;
        
        var request = new AddTopicToModuleRequest()
        {
            ModuleId = Module!.Id,
            TopicName = NewTopicName,
            TopicDescription = NewTopicDescription ?? string.Empty,
        };
        
        var result = await studyManagementService.AddTopic(request);
        if (result.IsError)
        {
            Console.WriteLine("Failed to add topic");
            AddTopicErrors.TryAdd("description", "Failed to create module - server error");
            return;
        }
        Module = result!.Value!;
        NewTopicName = string.Empty;
        NewTopicDescription = string.Empty;
        ShowAddTopic = false;
    }
    
    private void SetDummyModule()
    {
        Module = new Module()
        {
            Id = Guid.NewGuid(),
            Name = "Failed to fetch module",
        };
    }

    [RelayCommand]
    public void TopicInfoHandler(Topic topic)
    {
        CurrentSelectedState = topic.Status;
        if (AvailableTopicStates == null)
            _ = FetchTopicStates();
        SelectedTopic = topic;
        ShowTopicInfo = true;
    }
    
    [RelayCommand]
    public void TopicDialogCloseHandler()
    {
        SelectedTopic = null;
        ShowTopicInfo = false;
    }

    [RelayCommand]
    public async Task RemoveTopicButtonHandler()
    {
        if (SelectedTopic == null) { return; }
        var moduleId = SelectedTopic.ModuleId;
        var topicId = SelectedTopic.Id;
        await studyManagementService.RemoveTopicFromModule(moduleId, topicId);
        _ = FetchModule(Module!.Id);
        ShowTopicInfo = false;
        SelectedTopic = null;
    }
    

}