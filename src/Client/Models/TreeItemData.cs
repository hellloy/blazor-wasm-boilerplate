using FSH.BlazorWebAssembly.Client.Enums;

namespace FSH.BlazorWebAssembly.Client.Models;

public class TreeItemData(Guid id, string name, string roomTypeName, TreeItemType type, bool selected)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string RoomTypeName { get; set; } = roomTypeName;
    public TreeItemType Type { get; set; } = type;
    public bool Selected { get; set; } = selected;
    public HashSet<TreeItemData>? Rooms { get; set; }
}