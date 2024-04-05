using FSH.BlazorWebAssembly.Client.Components.EntityTable;
using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Infrastructure.Tools;
using HMS.Api.Shared.Authorization;
using Microsoft.AspNetCore.Components;
using Mapster;
using MudBlazor;
using FSH.BlazorWebAssembly.Client.Pages.Identity.Roles;

namespace FSH.BlazorWebAssembly.Client.Components.Settings.Hotel;

public partial class RoomTypes
{
    [Inject]
    protected IRoomTypesClient RoomTypesClient { get; set; } = default!;

    protected EntityServerTableContext<RoomTypeDto, Guid, RoomTypeViewModel> Context { get; set; } = default!;
    private EntityTable<RoomTypeDto, Guid, RoomTypeViewModel> _table = default!;

    protected override void OnInitialized()
    {
        Context = new(
            entityName: L["Room Type"],
            entityNamePlural: L["Room Types"],
            entityResource: FSHResource.RoomTypes,
            fields: new()
            {
                new(roomType => roomType.Name, L["Name"], "Name"),
                new(roomType => roomType.Code, L["Short Code"], "Code"),
                new(roomType => TextTools.TruncateText(roomType.Description, 147), L["Description"], "Description"),
                new(roomType => roomType.BasePrice, L["Base Price"], "BasePrice"),
                new(roomType => roomType.MaxOccupancy, L["Max Occupancy"], "MaxOccupancy"),
            },
            enableAdvancedSearch: false,
            idFunc: roomType => roomType.Id,
            searchFunc: async filter =>
            {
                var roomTypeFilter = filter.Adapt<SearchRoomTypeRequest>();
                roomTypeFilter.Name = SearchRoomTypeName == default ? null : SearchRoomTypeName;

                var result = await RoomTypesClient.SearchAsync("1", roomTypeFilter);
                return result.Adapt<PaginationResponse<RoomTypeDto>>();
            },
            deleteFunc: async id => await RoomTypesClient.DeleteAsync(id, "1"),
            exportFunc: async _ => await RoomTypesClient.ExportAsync("1", new ExportRoomTypesRequest()));
    }

    private string? _searchRoomTypeName;
    private string? SearchRoomTypeName
    {
        get => _searchRoomTypeName;
        set
        {
            _searchRoomTypeName = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private Task OnStartEdit(RoomTypeDto? roomType)
    {
        Navigation.NavigateTo(roomType != null ? $"/settings/types/edit/{roomType.Id}" : "/settings/types/edit");
        return Task.CompletedTask;
    }
}

public class RoomTypeViewModel : UpdateRoomTypeRequest
{
}
