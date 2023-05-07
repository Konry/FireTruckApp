// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.


namespace FireTruckApi;

public static class EventIds
{
    // 42 Api
    // 01 Controllers
    internal static readonly EventId ErrorIdUnknownExceptionInDataStorageInitialization = new(42_01_01_00);

    // 02 Data Handling
    internal static readonly EventId ErrorIdUnknownErrorInFireTruckConfiguration = new(42_02_02_00);

    // 02 Data Handling
    internal static readonly EventId ErrorIdUnknownErrorInStoreData = new(42_02_03_01);
    internal static readonly EventId ErrorIdUnknownErrorInLoadData = new(42_02_03_02);


    internal static readonly EventId ErrorIdUnknownExceptionInUploadXLSX = new(42_01_04_00);
}
