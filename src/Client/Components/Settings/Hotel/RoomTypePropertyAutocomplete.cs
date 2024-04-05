using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace FSH.BlazorWebAssembly.Client.Components.Settings.Hotel;

public class RoomTypePropertyAutocomplete:MudAutocomplete<Guid>
{
    [Inject]
    private IStringLocalizer<RoomTypePropertyAutocomplete> L { get; set; } = default!;

    [Inject]
    private IRoomTypePropertiesClient RoomTypePropertiesClient { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<RoomTypePropertyDto> _roomTypeProperties = new();

    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["Room Type Properties"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchProperties;
        ToStringFunc = GetRoomTypePropertyName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => RoomTypePropertiesClient.GetAsync(_value, "1"), Snackbar) is { } property)
        {
            _roomTypeProperties.Add(property);
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<Guid>> SearchProperties(string value)
    {
        var filter = new SearchRoomTypePropertyRequest()
        {
            PageSize = 10,
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => RoomTypePropertiesClient.SearchAsync("1",filter), Snackbar)
            is PaginationResponseOfRoomTypePropertyDto response)
        {
            _roomTypeProperties = response.Data.ToList();
        }

        return _roomTypeProperties.Select(x => x.Id);
    }

    private string GetRoomTypePropertyName(Guid id) =>
        _roomTypeProperties.Find(b => b.Id == id)?.Name ?? string.Empty;
}
