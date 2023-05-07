// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

namespace FireTruckApp.DataModel;

public class BareFireTruck
{
    public string Identifier { get; set; }
    public string TruckTypeShort { get; set; }
    public string TruckType { get; set; }
}

public class FireTruck : BareFireTruck
{
    public FireTruck(string identifier)
    {
        Identifier = identifier;
        Locations = new List<Location>();
    }

    public List<Location> Locations { get; }
}

public class Location : FireTruckLocation
{
    public Location()
    {
        Items = new List<LocationItem>();
    }

    public string Identifier { get; set; }
    public List<LocationItem> Items { get; set; }
}
