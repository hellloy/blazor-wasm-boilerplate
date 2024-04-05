using System.Collections.ObjectModel;

namespace HMS.Api.Shared.Hotel.Rooms;

public static class DefaultRoomStates
{
    public const string Available = nameof(Available);              //(Доступный) - номер доступен для бронирования и нахождения гостей.
    public const string Occupied  = nameof(Occupied);               //Occupied (Занятый) - номер занят гостями и не доступен для бронирования.
    public const string Reserved  = nameof(Reserved);               //(Зарезервированный) - номер зарезервирован, но гости еще не заехали.
    public const string Maintenance  = nameof(Maintenance);         //(Техническое обслуживание) - номер находится в процессе технического обслуживания и недоступен для бронирования и нахождения гостей.
    public const string OutofService  = nameof(OutofService);       //(Неисправный) - номер неисправен и недоступен для бронирования и нахождения гостей.
    public const string Dirty  = nameof(Dirty);                     //(Грязный) - номер был освобожден гостями, но еще не был убран и не готов к заселению новых гостей.
    public const string Clean  = nameof(Clean);                     //(Чистый) - номер был убран и готов для заселения новых гостей.
    public const string Inspected  = nameof(Inspected);             //(Проверенный) - номер был убран и проверен на наличие неисправностей и готов к заселению новых гостей.
    public const string Overbooked  = nameof(Overbooked);           //(Перебронированный) - номер забронирован более чем на одного гостя и недоступен для нахождения гостей.
    public const string NoShow  = nameof(NoShow);                   //гость не явился в отель, не отменив бронь и не связавшись с отелем.
    public const string CheckedOut  = nameof(CheckedOut);           //гость покинул номер и оплатил все свои счета.

    public static IReadOnlyList<string> DefaultStates { get; } = new ReadOnlyCollection<string>(new[]
    {
        Available,
        Occupied,
        Reserved,
        Maintenance,
        OutofService,
        Dirty,
        Clean,
        Inspected,
        Overbooked,
        NoShow,
        CheckedOut
    });

    public static bool IsDefault(string stateName) => DefaultStates.Any(r => r == stateName);
}
