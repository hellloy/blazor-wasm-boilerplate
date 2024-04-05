using FSH.BlazorWebAssembly.Client.Components.EntityTable;
using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using HMS.Api.Shared.Authorization;
using Microsoft.AspNetCore.Components;
using Mapster;

namespace FSH.BlazorWebAssembly.Client.Components.Settings.Hotel;

public partial class EditRoomBeds
{
    [Inject]
    protected IRoomBedsClient RoomBedsClient { get; set; } = default!;

    protected EntityServerTableContext<RoomBedDto, Guid, RoomBedViewModel> Context { get; set; } = default!;
    private EntityTable<RoomBedDto, Guid, RoomBedViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: L["Room Bed"],
            entityNamePlural: L["Room Beds"],
            entityResource: FSHResource.RoomBeds,
            fields: new()
            {
                new(bed => bed.Name, L["Name"], "Name"),
                new(bed => bed.MaxOccupancy, L["MaxOccupancy"], "Max Occupancy"),
                new(bed => bed.Description, L["Description"], "Description"),
            },
            enableAdvancedSearch: false,
            idFunc: bed => bed.Id,
            searchFunc: async filter =>
            {
                var bedFilter = filter.Adapt<SearchRoomBedRequest>();
                bedFilter.Name = SearchBedName == default ? null : SearchBedName;

                var result = await RoomBedsClient.SearchAsync("1",bedFilter);
                return result.Adapt<PaginationResponse<RoomBedDto>>();
            },
            createFunc: async bed =>
            {
                await RoomBedsClient.CreateAsync("1",bed.Adapt<CreateRoomBedRequest>());
            },
            updateFunc: async (id, bed) =>
            {
                await RoomBedsClient.UpdateAsync(id,"1", bed.Adapt<UpdateRoomBedRequest>());
            },
            deleteFunc: async id => await RoomBedsClient.DeleteAsync(id,"1")
        );
    private string? _searchBedName;
    private string? SearchBedName
    {
        get => _searchBedName;
        set
        {
            _searchBedName = value;
            _ = _table.ReloadDataAsync();
        }
    }
}

public class RoomBedViewModel : UpdateRoomBedRequest
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int MaxOccupancy { get; set; } = default!;

}
