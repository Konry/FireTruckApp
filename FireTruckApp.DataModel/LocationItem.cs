// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

namespace FireTruckApp.DataModel;

public class LocationItem
{
    public string Identifier { get; set; }
    public int Quantity { get; set; }

    public List<string> AdditionalInformation { get; set; }
}
