// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using Microsoft.Extensions.Logging;

namespace FireTruckApp.DataLoader;

public static class EventIds
{
    // Dataloader 50
    internal static readonly EventId ErrorIdUnknownExceptionInExcelDataLoader = new(50_000_000);
    internal static readonly EventId ErrorIdTruckAlreadyExists = new(50_000_001);
    internal static readonly EventId ErrorIdTruckDataNotFound = new(50_000_002);
    internal static readonly EventId ErrorIdWorkSheetNameNotSufficient = new(50_000_003);
}
