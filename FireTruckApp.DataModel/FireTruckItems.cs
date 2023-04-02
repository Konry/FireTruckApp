// Copyright (c) Jan Philipp Luehrig.All rights reserved.
// These files are licensed to you under the MIT license.

namespace FireTruckApp.DataModel;

public class FireTruckItems
{
    public string FireTruckIdentifier { get; set; }
    public List<FireTruckLocation> Locations { get; set; }
}

public class FireTruckLocation
{
    public string Location { get; set; }
    public List<LocationItem> Items { get; set; }
}

public record LocationItem
{
    public string Identifer { get; set; }
    public int Quantity { get; set; }
}
