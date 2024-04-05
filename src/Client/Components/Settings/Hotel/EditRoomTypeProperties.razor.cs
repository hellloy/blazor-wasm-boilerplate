using FSH.BlazorWebAssembly.Client.Components.EntityTable;
using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using HMS.Api.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;

namespace FSH.BlazorWebAssembly.Client.Components.Settings.Hotel;

public partial class EditRoomTypeProperties
{
    [Inject]
    protected IRoomTypePropertiesClient RoomTypePropertiesClient { get; set; } = default!;

    protected EntityServerTableContext<RoomTypePropertyDto, Guid, RoomTypePropertyViewModel> Context { get; set; } = default!;
    private EntityTable<RoomTypePropertyDto, Guid, RoomTypePropertyViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: L["Room Type Property"],
            entityNamePlural: L["Room Properties"],
            entityResource: FSHResource.RoomTypes,
            fields: new()
            {
                new(roomTypeProperty => roomTypeProperty.Name, L["Name"], "Name"),
                new(roomTypeProperty => roomTypeProperty.Description, L["Description"], "Description"),
            },
            enableAdvancedSearch: false,
            idFunc: roomTypeProperty => roomTypeProperty.Id,
            searchFunc: async filter =>
            {
                var roomTypePropertyFilter = filter.Adapt<SearchRoomTypePropertyRequest>();
                roomTypePropertyFilter.Name = SearchRoomTypePropertyName == default ? null : SearchRoomTypePropertyName;

                var result = await RoomTypePropertiesClient.SearchAsync("1",roomTypePropertyFilter);
                return result.Adapt<PaginationResponse<RoomTypePropertyDto>>();
            },
            createFunc: async roomTypeProperty =>
            {
                await RoomTypePropertiesClient.CreateAsync("1",roomTypeProperty.Adapt<CreateRoomTypePropertyRequest>());
            },
            updateFunc: async (id, roomTypeProperty) =>
            {
                await RoomTypePropertiesClient.UpdateAsync(id,"1", roomTypeProperty.Adapt<UpdateRoomTypePropertyRequest>());
            },
            deleteFunc: async id => await RoomTypePropertiesClient.DeleteAsync(id,"1")
        );

    private string? _searchRoomTypePropertyName;
    private string? SearchRoomTypePropertyName
    {
        get => _searchRoomTypePropertyName;
        set
        {
            _searchRoomTypePropertyName = value;
            _ = _table.ReloadDataAsync();
        }
    }
}

public class RoomTypePropertyViewModel : UpdateRoomTypePropertyRequest
{
}
