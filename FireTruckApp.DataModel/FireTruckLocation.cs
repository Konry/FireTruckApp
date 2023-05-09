// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

namespace FireTruckApp.DataModel;

public class FireTruckLocation
{
    public FireTruckLocation(string identifier)
    {
        Identifier = identifier;
    }
    public string Identifier { get; set; }
}
