using FSH.BlazorWebAssembly.Client.Components.EntityTable;
using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using HMS.Api.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;

namespace FSH.BlazorWebAssembly.Client.Components.Settings.Hotel;

public partial class EditRoomCategories
{
    [Inject]
    protected IRoomCategoriesClient RoomCategoriesClient { get; set; } = default!;

    protected EntityServerTableContext<RoomCategoryDto, Guid, RoomCategoryViewModel> Context { get; set; } = default!;
    private EntityTable<RoomCategoryDto, Guid, RoomCategoryViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: L["Room Category"],
            entityNamePlural: L["Room Category"],
            entityResource: FSHResource.RoomsCategories,
            fields: new()
            {
                new(category => category.Name, L["Name"], "Name"),
                new(category => category.Code, L["Code"], "Code"),
                new(category => category.Description, L["Description"], "Description"),
            },
            enableAdvancedSearch: false,
            idFunc: category => category.Id,
            searchFunc: async filter =>
            {
                var categoryFilter = filter.Adapt<SearchRoomCategoryRequest>();
                categoryFilter.Name = SearchCategoryName == default ? null : SearchCategoryName;

                var result = await RoomCategoriesClient.SearchAsync("1",categoryFilter);
                return result.Adapt<PaginationResponse<RoomCategoryDto>>();
            },
            createFunc: async category =>
            {
                await RoomCategoriesClient.CreateAsync("1", category.Adapt<CreateRoomCategoryRequest>());
            },
            updateFunc: async (id, category) =>
            {
                await RoomCategoriesClient.UpdateAsync(id, "1", category.Adapt<UpdateRoomCategoryRequest>());
            },
            deleteFunc: async id => await RoomCategoriesClient.DeleteAsync(id,"1")
        );

    private string? _searchCategoryName;
    private string? SearchCategoryName
    {
        get => _searchCategoryName;
        set
        {
            _searchCategoryName = value;
            _ = _table.ReloadDataAsync();
        }
    }
}

public class RoomCategoryViewModel : UpdateRoomCategoryRequest
{
}

