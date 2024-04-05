using FSH.BlazorWebAssembly.Client.Components.Settings.Hotel.Dialogs;
using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
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

    private async Task SelectRoomsModal()
    {
        var parameters = new DialogParameters();
        parameters.Add("SelectedRooms", Tariff.Rooms?.Select(x => x.RoomId).ToList());

        var options = new DialogOptions()
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            DisableBackdropClick = true
        };

        var dialog = _dialogService.Show<EditTariffRooms>("Modal", parameters, options);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
          // Tariff.Rooms = (List<EditTariffRooms.TreeItemData>)result.Data;
            
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
    public List<TariffRoomDto> Rooms { get; set; }
    public decimal? Price { get; set; }

}
