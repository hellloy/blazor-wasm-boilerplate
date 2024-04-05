using FSH.BlazorWebAssembly.Client.Components.Settings.Hotel.Dialogs;
using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Models;
using MudBlazor;

namespace FSH.BlazorWebAssembly.Client.Components.Settings.Hotel;

public partial class EditTariff
{
    private TariffVm Tariff = new();

    protected override void OnInitialized()
    {
        
        //Tariff.Name = "Тест 1";
        //Tariff.StartDate = DateTime.Now;
        //Tariff.EndDate = DateTime.Now.AddMonths(1);
        //Tariff.Price = 1000;
        base.OnInitialized();
    }

    [Obsolete]
    private async Task SelectRoomsModal()
    {
        var parameters = new DialogParameters
        {
            { "SelectedRooms", Tariff.Rooms?.Select(x => x.Id).ToList() }
        };
        var options = new DialogOptions()
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            DisableBackdropClick = true
        };

        var dialog = await _dialogService.ShowAsync<EditTariffRooms>("Modal", parameters, options);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
          Tariff.Rooms = (List<TreeItemData>)result.Data;
        }
    }
}

public class TariffVm
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Enable { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool Extended { get; set; }
    public List<TariffDayDto> Days { get; set; }
    public List<TreeItemData> Rooms { get; set; }
    public decimal? Price { get; set; }

}
