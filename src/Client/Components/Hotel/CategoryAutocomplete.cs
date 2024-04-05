using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace FSH.BlazorWebAssembly.Client.Components.Hotel;

public class CategoryAutocomplete : MudAutocomplete<Guid>
{
    [Inject]
    private IStringLocalizer<CategoryAutocomplete> L { get; set; } = default!;

    [Inject]
    private IRoomCategoriesClient RoomCategoriesClient { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<RoomCategoryDto> _roomCategories = new();

    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["Room Category"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchCategories;
        ToStringFunc = GetCategoryName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one category to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => RoomCategoriesClient.GetAsync(_value, "1"), Snackbar) is { } category)
        {
            _roomCategories.Add(category);
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<Guid>> SearchCategories(string value)
    {
        var filter = new SearchRoomCategoryRequest()
        {
            PageSize = 10,
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => RoomCategoriesClient.SearchAsync("1", filter), Snackbar)
            is PaginationResponseOfRoomCategoryDto response)
        {
            _roomCategories = response.Data.ToList();
        }

        return _roomCategories.Select(x => x.Id);
    }

    private string GetCategoryName(Guid id) =>
        _roomCategories.Find(b => b.Id == id)?.Name ?? string.Empty;
}
