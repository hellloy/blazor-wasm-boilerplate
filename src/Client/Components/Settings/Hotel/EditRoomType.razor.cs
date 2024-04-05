using FSH.BlazorWebAssembly.Client.Components.Common;
using FSH.BlazorWebAssembly.Client.Components.Dialogs;
using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Infrastructure.Tools;
using FSH.BlazorWebAssembly.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static FSH.BlazorWebAssembly.Client.Components.Settings.Hotel.SelectRoomBedsDialog;

namespace FSH.BlazorWebAssembly.Client.Components.Settings.Hotel;

public partial class EditRoomType
{
    [Inject]
    protected IRoomTypesClient RoomTypesClient { get; set; } = default!;

    [Inject]
    protected IRoomTypePropertiesClient RoomTypePropertiesClient { get; set; } = default!;

    [Parameter]
    public string Id { get; set; } = default!;

    private CustomValidation? _customValidation;

    private EditRoomTypeViewModel RoomTypeDto { get; set; } = new ();
    private bool _loading;

    protected override async Task OnInitializedAsync()
    {
        if (!string.IsNullOrEmpty(Id))
        {
            RoomTypeDto = await LoadRoomType(Guid.Parse(Id));
        }
        else
        {
            RoomTypeDto.RoomTypeBeds = new List<RoomBedRoomTypeDto>();
            RoomTypeDto.RoomTypeProperties = new List<RoomPropertyRoomTypeDto>();
        }
    }

    private async Task<EditRoomTypeViewModel> LoadRoomType(Guid id)
    {
        _loading = true;
        var result = await ApiHelper.ExecuteCallGuardedAsync(() => RoomTypesClient.GetAsync(id, "1"), Snackbar, _customValidation);
        _loading = false;
        return result!.Adapt<EditRoomTypeViewModel>();
    }

    private void BackToList()
    {
        Navigation.NavigateTo("/settings/types");
    }

    private async Task SaveRoomTypeAsync()
    {
        if (!string.IsNullOrEmpty(Id))
        {
            var data = new UpdateRoomTypeRequest
            {
                RoomTypeBeds = RoomTypeDto.RoomTypeBeds,
                MaxOccupancy = RoomTypeDto.MaxOccupancy,
                RoomTypeProperties = RoomTypeDto.RoomTypeProperties,
                Description = RoomTypeDto.Description,
                BasePrice = RoomTypeDto.BasePrice,
                Id = RoomTypeDto.Id,
                Name = RoomTypeDto.Name,
                Code = RoomTypeDto.Code
            };
            _loading = true;
            var result = await ApiHelper.ExecuteCallGuardedAsync(() => RoomTypesClient.UpdateAsync(Guid.Parse(Id), "1", data), Snackbar, _customValidation, Localizer["Room type has been updated"]);
            _loading = false;
            if (result != default)
            {
                NavigateToList();
            }
        }
        else
        {
            _loading = true;
            var result = await ApiHelper.ExecuteCallGuardedAsync(
                () => RoomTypesClient.CreateAsync("1", RoomTypeDto.Adapt<CreateRoomTypeRequest>()), Snackbar, _customValidation, Localizer["Room type has been created"]);
            _loading = false;
            if (result != default)
            {
                NavigateToList();
            }
        }

    }

    private void NavigateToList()
    {
        Navigation.NavigateTo("/settings/types");
    }

    private async Task OpenAddRoomBedsTypeDialogAsync()
    {
        var parameters = new DialogParameters { { "RoomBedVmsParam", RoomTypeDto.RoomTypeBeds } };
        var result = await DialogService.Show<SelectRoomBedsDialog>("Add beds to the type room", parameters).Result;

        if (!result.Cancelled)
        {
            var data = (List<RoomBedDtoWithQuantity>)result.Data;
            RoomTypeDto.RoomTypeBeds = data.Select(x => new RoomBedRoomTypeDto
            {
                Quantity = x.Quantity,
                RoomBedId = x.Id,
                RoomTypeId = RoomTypeDto.Id,
                RoomBed = x.Adapt<RoomBedDto>()
            }).ToList();
            RoomTypeDto.MaxOccupancy = RoomTypeDto.RoomTypeBeds.Sum(x => x.RoomBed?.MaxOccupancy * x.Quantity ?? RoomTypeDto.MaxOccupancy);
        }
    }

    private async Task OpenAddRoomTypesPropertiesDialog()
    {
        var parameters = new DialogParameters { { "RoomTypePropertiesParam", RoomTypeDto.RoomTypeProperties }, {"RoomTypeId", RoomTypeDto.Id} };
        var result = await DialogService.Show<SelectRoomTypeProperties>("Add properties to the type room", parameters).Result;
        if (!result.Cancelled)
        {
            var data = (List<RoomPropertyRoomTypeDto>)result.Data;
            RoomTypeDto.RoomTypeProperties = data;
        }
    }

    private async Task Delete()
    {
        string deleteContent = Localizer["Confirm deletion of this Room Type?"];
        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), deleteContent }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
        var dialog = DialogService.Show<DeleteConfirmation>(Localizer["Delete"], parameters, options);
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            _loading = true;
            var dialogResult = await ApiHelper.ExecuteCallGuardedAsync(
                () => RoomTypesClient.DeleteAsync(RoomTypeDto.Id, "1"), Snackbar, _customValidation, Localizer["Room type has been deleted"]);
            _loading = false;
            if (dialogResult != default)
            {
                NavigateToList();
            }
        }
    }

    private void Test(string obj)
    {
        Console.WriteLine(ShortCodeGenerator.GenerateShortCode(obj));
    }
}

public class EditRoomTypeViewModel : UpdateRoomTypeRequest
{
    public string Name { get; set; }
    public string Code { get; set; }
}