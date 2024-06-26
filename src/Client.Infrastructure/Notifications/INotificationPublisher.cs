using HMS.Api.Shared.Notifications;

namespace FSH.BlazorWebAssembly.Client.Infrastructure.Notifications;

public interface INotificationPublisher
{
    Task PublishAsync(INotificationMessage notification);
}