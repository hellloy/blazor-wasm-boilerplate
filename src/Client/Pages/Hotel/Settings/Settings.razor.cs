using Microsoft.AspNetCore.Components;

namespace FSH.BlazorWebAssembly.Client.Pages.Hotel.Settings;

public partial class Settings
{
    [Parameter]
    public string Component { get; set; } = default!;

    [Parameter]
    public string Id { get; set; } = default!;

    [Parameter]
    public string Edit { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        if (Component == default)
        {
            NavigationManager.NavigateTo("settings/categories");
        }

    }
}
