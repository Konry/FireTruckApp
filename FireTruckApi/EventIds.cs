// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.


namespace FireTruckApi;

public static class EventIds
{
    // 42 Api
    // 01 Controllers
    internal static readonly EventId SErrorIdUnknownExceptionInDataStorageInitialization = new(42_01_01_00);

    // 02 Data Handling
    internal static readonly EventId SErrorIdUnknownErrorInFireTruckConfiguration = new(42_02_02_00);
}
