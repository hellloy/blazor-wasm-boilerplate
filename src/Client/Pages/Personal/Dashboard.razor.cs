﻿using System.Globalization;
using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Infrastructure.Notifications;
using FSH.BlazorWebAssembly.Client.Shared;
using HMS.Api.Shared.Notifications;
using MediatR.Courier;
using Microsoft.AspNetCore.Components;

namespace FSH.BlazorWebAssembly.Client.Pages.Personal;

public partial class Dashboard
{
    [Parameter]
    public int ProductCount { get; set; }
    [Parameter]
    public int BrandCount { get; set; }
    [Parameter]
    public int UserCount { get; set; }
    [Parameter]
    public int RoleCount { get; set; }

    [Parameter]
    public int RoomCategoriesCount { get; set; }

    [Inject]
    private IDashboardClient DashboardClient { get; set; } = default!;
    [Inject]
    private ICourier Courier { get; set; } = default!;

    private readonly string[] _dataEnterBarChartXAxisLabels = DateTimeFormatInfo.CurrentInfo.AbbreviatedMonthNames;
    private readonly List<MudBlazor.ChartSeries> _dataEnterBarChartSeries = new();
    private bool _loaded;

    protected override async Task OnInitializedAsync()
    {
        Courier.SubscribeWeak<NotificationWrapper<StatsChangedNotification>>(async _ =>
        {
            await LoadDataAsync();
            StateHasChanged();
        });

        await LoadDataAsync();

        _loaded = true;
    }

    private async Task LoadDataAsync()
    {
        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => DashboardClient.GetAsync("1"),
                Snackbar)
            is StatsDto statsDto)
        {
            ProductCount = statsDto.ProductCount;
            BrandCount = statsDto.BrandCount;
            UserCount = statsDto.UserCount;
            RoleCount = statsDto.RoleCount;
            RoomCategoriesCount = statsDto.RoomCategoryCount;
            foreach (var item in statsDto.DataEnterBarChart)
            {
                _dataEnterBarChartSeries
                    .RemoveAll(x => x.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
                _dataEnterBarChartSeries.Add(new MudBlazor.ChartSeries { Name = item.Name, Data = item.Data?.ToArray() });
            }
        }
    }
}
