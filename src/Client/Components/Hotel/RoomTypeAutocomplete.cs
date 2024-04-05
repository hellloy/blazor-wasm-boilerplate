using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace FSH.BlazorWebAssembly.Client.Components.Hotel;

public class RoomTypeAutocomplete : MudAutocomplete<Guid>
{
    [Inject]
    private IStringLocalizer<RoomTypeAutocomplete> L { get; set; } = default!;

    [Inject]
    private IRoomTypesClient RoomTypesClient { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<RoomTypeDto> _roomTypes = new();

    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["Room Type"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchRoomTypes;
        ToStringFunc = GetRoomTypeName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => RoomTypesClient.GetAsync(_value, "1"), Snackbar) is { } roomType)
        {
            _roomTypes.Add(roomType);
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<Guid>> SearchRoomTypes(string value)
    {
        var filter = new SearchRoomTypeRequest()
        {
            PageSize = 10,
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value}
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => RoomTypesClient.SearchAsync("1", filter), Snackbar)
            is PaginationResponseOfRoomTypeDto response)
        {
            _roomTypes = response.Data.ToList();
        }

        return _roomTypes.Select(x => x.Id);
    }

    private string GetRoomTypeName(Guid id) =>
        _roomTypes.Find(rt => rt.Id == id)?.Name ?? string.Empty;
}
