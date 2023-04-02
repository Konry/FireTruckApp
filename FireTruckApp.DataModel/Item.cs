// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

namespace FireTruckApp.DataModel;

public class Item
{
    public string Identifier { get; set; }

    public string Type { get; set; }

    public double Weight { get; set; }

    public List<string> Tags { get; set; }

    public List<string> AlternateWording { get; set; }

    public List<(string Name, string Info)> AdditionalInfo { get; set; }
}
