﻿using HMS.Api.Shared.Notifications;

namespace FSH.BlazorWebAssembly.Client.Infrastructure.Notifications;

public record ConnectionStateChanged(ConnectionState State, string? Message) : INotificationMessage;