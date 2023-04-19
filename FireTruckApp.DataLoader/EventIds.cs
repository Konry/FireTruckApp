// Copyright (c) Jan Philipp Luehrig.All rights reserved.
// These files are licensed to you under the MIT license.

using Microsoft.Extensions.Logging;

namespace FireTruckApp.DataLoader;

public static class EventIds
{
    // Dataloader 50
    internal static readonly EventId s_errorIdUnknownExceptionInExcelDataLoader = new(50_000_000);
    internal static readonly EventId s_errorIdTruckDataNotFound = new(50_000_002);
    internal static  readonly EventId s_errorIdTruckAlreadyExists = new(50_000_001);
}
