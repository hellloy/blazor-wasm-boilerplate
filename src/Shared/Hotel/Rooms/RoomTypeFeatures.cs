namespace HMS.Api.Shared.Hotel.Rooms;

[Flags]
public enum RoomTypeFeatures
{
    None = 0,
    SmokingAllowed = 1 << 0,
    PetFriendly = 1 << 1,
    Accessible = 1 << 2,
    Balcony = 1 << 3,
    Terrace = 1 << 4,
    SeaView = 1 << 5,
    PoolView = 1 << 6,
    MountainView = 1 << 7,
    CityView = 1 << 8,
    GardenView = 1 << 9
}

