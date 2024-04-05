using FSH.BlazorWebAssembly.Client.Enums;
using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Models;
using FSH.BlazorWebAssembly.Client.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.BlazorWebAssembly.Client.Components.Settings.Hotel.Dialogs;

public partial class EditTariffRooms
{
    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    [Parameter]
    public List<int> SelectedRooms { get; set; } = new();

    [Inject]
    protected IRoomsClient RoomsClient { get; set; } = default!;

    [Inject]
    protected IRoomTypesClient RoomTypesClient { get; set; } = default!;

    private HashSet<TreeItemData> _roomTypes = new();

    private HashSet<TreeItemData> _selectedValues = new();

    protected override async Task OnInitializedAsync()
    {
        var allRooms = await GetRooms();
        if(allRooms is not null)
        {
            _roomTypes = (ConvertToTree(allRooms) ?? throw new InvalidOperationException()).ToHashSet();
        }
    }

    private void AddRooms()
    {
        MudDialog?.Close(DialogResult.Ok(_selectedValues.Where(x => x.Type == TreeItemType.Room).ToList()));
    }

    private void Cancel()
    {
        MudDialog?.Cancel();
    }

    private async Task<List<RoomDto>?> GetRooms(Guid? roomTypeId = null)
    {
        var response = await ApiHelper.ExecuteCallGuardedAsync(
            () => RoomsClient.SearchAsync("1", new SearchRoomRequest
                {
                    RoomTypeId = roomTypeId,
                    PageNumber = 1,
                    PageSize = 9999,
                }),
            snackbar: Snackbar);
        return response?.Data.ToList();
    }

    private List<TreeItemData>? ConvertToTree(List<RoomDto>? rooms)
    {
        var roomGroups = rooms?.GroupBy(r => r.RoomType.Id);

        var tree = roomGroups?.Select(g => new TreeItemData(
            g.Key,
            g.First().RoomType.Name,
            string.Empty,
            TreeItemType.Category,
            false)
        {
            Rooms = g.Select(r => new TreeItemData(
                r.Id,
                r.Name,
                r.RoomType.Name,
                TreeItemType.Room,
                false)).ToHashSet()
        }).ToList();

        return tree;
    }
}
