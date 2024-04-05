using FSH.BlazorWebAssembly.Client.Components.EntityTable;
using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Infrastructure.Tools;
using FSH.BlazorWebAssembly.Client.Shared;
using HMS.Api.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static MudBlazor.CategoryTypes;

namespace FSH.BlazorWebAssembly.Client.Components.Settings.Hotel;

public partial class EditRooms
{
    [Inject]
    protected IRoomsClient RoomsClient { get; set; } = default!;

    protected EntityServerTableContext<RoomDto, Guid, RoomViewModel> Context { get; set; } = default!;
    private EntityTable<RoomDto, Guid, RoomViewModel> _table = default!;

    private List<RoomDto> Rooms { get; set; } = new List<RoomDto>();

    private TableGroupDefinition<RoomDto> _groupDefinition = new TableGroupDefinition<RoomDto>()
    {
        GroupName = "Category",
        Indentation = true,
        Expandable = true,
        Selector = (e) => e.RoomCategory.Name,
        InnerGroup = new TableGroupDefinition<RoomDto>()
        {
            GroupName = "Room Type",
            Expandable = true,
            Selector = (e) => e.RoomType.Name
        }

    };

    protected override void OnInitialized()
    {
        Context = new(
            entityName: L["Room"],
            entityNamePlural: L["Rooms"],
            entityResource: FSHResource.Rooms,
            fields: new()
            {
                new(room => room.Name, L["Name"], "Name"),
                new(room => room.Code, L["Code"], "Code"),
                new(room => TextTools.TruncateText(room.RoomType.Name, 147), L["Room Type"], "RoomType.Name"),
                new(room => room.RoomType.BasePrice, L["Base Price"], "RoomType.BasePrice"),
                new(room => room.RoomCategory.Name, L["Category"], "RoomCategory.Name"),
            },
            enableAdvancedSearch: false,
            idFunc: room => room.Id,
            searchFunc: async filter =>
            {
                var roomFilter = filter.Adapt<SearchRoomRequest>();
                //roomFilter.Name = SearchRoomName;

                var result = await RoomsClient.SearchAsync("1", roomFilter);
                return result.Adapt<PaginationResponse<RoomDto>>();
            },
            createFunc: async roomTypeProperty =>
            {
                var createRequest = new CreateRoomRequest
                {
                    Count = roomTypeProperty.Count,
                    Name = roomTypeProperty.Name,
                    RoomCategoryId = roomTypeProperty.RoomCategory.Id,
                    RoomTypeId = roomTypeProperty.RoomType.Id,
                };
                await RoomsClient.CreateAsync("1", createRequest);
            },
            updateFunc: async (id, roomTypeProperty) =>
            {
                await RoomsClient.UpdateAsync(id, "1", roomTypeProperty.Adapt<UpdateRoomRequest>());
            },
            deleteFunc: async id => await RoomsClient.DeleteAsync(id, "1"));

    }

    private string? _searchRoomName;
    private string? SearchRoomName
    {
        get => _searchRoomName;
        set
        {
            _searchRoomName = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private async Task<TableData<RoomDto>> ServerReload(TableState state)
    {
        var response = await ApiHelper.ExecuteCallGuardedAsync(
            () => RoomsClient.SearchAsync("1", new SearchRoomRequest
        {
            PageNumber = state.Page+1,
            PageSize = state.PageSize,
        }),
            snackbar: Snackbar);

        return new TableData<RoomDto>() { TotalItems = response.TotalCount, Items = response.Data};
    }
}

public class RoomViewModel : UpdateRoomRequest
{
    public RoomTypeDto RoomType { get;  set; } = new();
    public RoomStatus RoomStatus { get; set; } = new();
    public RoomStateDto RoomState { get;  set; }
    public new RoomCategoryDto RoomCategory { get; set; } = new();
    public int Count { get; set; } = 1;
}
