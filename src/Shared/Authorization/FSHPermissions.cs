using System.Collections.ObjectModel;

namespace HMS.Api.Shared.Authorization;

public static class FSHAction
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Export = nameof(Export);
    public const string Generate = nameof(Generate);
    public const string Clean = nameof(Clean);
    public const string UpgradeSubscription = nameof(UpgradeSubscription);
}

public static class FSHResource
{
    public const string Tenants = nameof(Tenants);
    public const string Dashboard = nameof(Dashboard);
    public const string Hangfire = nameof(Hangfire);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
    public const string Products = nameof(Products);
    public const string Brands = nameof(Brands);
    public const string RoomTypes = nameof(RoomTypes);
    public const string RoomBeds = nameof(RoomBeds);
    public const string RoomsCategories = nameof(RoomsCategories);
    public const string Rooms = nameof(Rooms);
    
}

public static class FSHPermissions
{
    private static readonly FSHPermission[] _all = new FSHPermission[]
    {
        new("View Dashboard", FSHAction.View, FSHResource.Dashboard),
        new("View Hangfire", FSHAction.View, FSHResource.Hangfire),
        new("View Users", FSHAction.View, FSHResource.Users),
        new("Search Users", FSHAction.Search, FSHResource.Users),
        new("Create Users", FSHAction.Create, FSHResource.Users),
        new("Update Users", FSHAction.Update, FSHResource.Users),
        new("Delete Users", FSHAction.Delete, FSHResource.Users),
        new("Export Users", FSHAction.Export, FSHResource.Users),
        new("View UserRoles", FSHAction.View, FSHResource.UserRoles),
        new("Update UserRoles", FSHAction.Update, FSHResource.UserRoles),
        new("View Roles", FSHAction.View, FSHResource.Roles),
        new("Create Roles", FSHAction.Create, FSHResource.Roles),
        new("Update Roles", FSHAction.Update, FSHResource.Roles),
        new("Delete Roles", FSHAction.Delete, FSHResource.Roles),
        new("View RoleClaims", FSHAction.View, FSHResource.RoleClaims),
        new("Update RoleClaims", FSHAction.Update, FSHResource.RoleClaims),
        new("View Products", FSHAction.View, FSHResource.Products, IsBasic: true),
        new("Search Products", FSHAction.Search, FSHResource.Products, IsBasic: true),
        new("Create Products", FSHAction.Create, FSHResource.Products),
        new("Update Products", FSHAction.Update, FSHResource.Products),
        new("Delete Products", FSHAction.Delete, FSHResource.Products),
        new("Export Products", FSHAction.Export, FSHResource.Products),
        new("View Brands", FSHAction.View, FSHResource.Brands, IsBasic: true),
        new("Search Brands", FSHAction.Search, FSHResource.Brands, IsBasic: true),
        new("Create Brands", FSHAction.Create, FSHResource.Brands),
        new("Update Brands", FSHAction.Update, FSHResource.Brands),
        new("Delete Brands", FSHAction.Delete, FSHResource.Brands),
        new("Generate Brands", FSHAction.Generate, FSHResource.Brands),
        new("Clean Brands", FSHAction.Clean, FSHResource.Brands),
        new("View Tenants", FSHAction.View, FSHResource.Tenants, IsRoot: true),
        new("Create Tenants", FSHAction.Create, FSHResource.Tenants, IsRoot: true),
        new("Update Tenants", FSHAction.Update, FSHResource.Tenants, IsRoot: true),
        new("Upgrade Tenant Subscription", FSHAction.UpgradeSubscription, FSHResource.Tenants, IsRoot: true),

        new("View Room Types", FSHAction.View, FSHResource.RoomTypes, IsBasic: true),
        new("Search Room Types", FSHAction.Search, FSHResource.RoomTypes, IsBasic: true),
        new("Create Room Types", FSHAction.Create, FSHResource.RoomTypes),
        new("Update Room Types", FSHAction.Update, FSHResource.RoomTypes),
        new("Delete Room Types", FSHAction.Delete, FSHResource.RoomTypes),
        new("Export Room Types", FSHAction.Export, FSHResource.RoomTypes),

        new("View Room Beds", FSHAction.View, FSHResource.RoomBeds, IsBasic: true),
        new("Search Room Beds", FSHAction.Search, FSHResource.RoomBeds, IsBasic: true),
        new("Create Room Beds", FSHAction.Create, FSHResource.RoomBeds),
        new("Update Room Beds", FSHAction.Update, FSHResource.RoomBeds),
        new("Delete Room Beds", FSHAction.Delete, FSHResource.RoomBeds),
        new("Export Room Beds", FSHAction.Export, FSHResource.RoomBeds),

        new("View Rooms", FSHAction.View, FSHResource.Rooms, IsBasic: true),
        new("Search Rooms", FSHAction.Search, FSHResource.Rooms, IsBasic: true),
        new("Create Rooms", FSHAction.Create, FSHResource.Rooms),
        new("Update Rooms", FSHAction.Update, FSHResource.Rooms),
        new("Delete Rooms", FSHAction.Delete, FSHResource.Rooms),
        new("Export Rooms", FSHAction.Export, FSHResource.Rooms),

        new("View Room Categories", FSHAction.View, FSHResource.RoomsCategories, IsBasic: true),
        new("Search Room Categories", FSHAction.Search, FSHResource.RoomsCategories, IsBasic: true),
        new("Create Room Category", FSHAction.Create, FSHResource.RoomsCategories),
        new("Update Room Category", FSHAction.Update, FSHResource.RoomsCategories),
        new("Delete Room Category", FSHAction.Delete, FSHResource.RoomsCategories),
        new("Export Room Categories", FSHAction.Export, FSHResource.RoomsCategories),
    };

    public static IReadOnlyList<FSHPermission> All { get; } = new ReadOnlyCollection<FSHPermission>(_all);
    public static IReadOnlyList<FSHPermission> Root { get; } = new ReadOnlyCollection<FSHPermission>(_all.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<FSHPermission> Admin { get; } = new ReadOnlyCollection<FSHPermission>(_all.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<FSHPermission> Basic { get; } = new ReadOnlyCollection<FSHPermission>(_all.Where(p => p.IsBasic).ToArray());
}

public record FSHPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}
