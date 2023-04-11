// Copyright (c) Jan Philipp Luehrig.All rights reserved.
// These files are licensed to you under the MIT license.

namespace FireTruckApp.DataModel;

public class Item
{
    public string Identifier { get; set; } = "";

    public string Type { get; set; } = "";

    public double Weight { get; set; }

    public List<string> Tags { get; set; } = new();

    public List<string> AlternateWording { get; set; } = new();

    public List<(string Name, string Info)> AdditionalInfo { get; set; } = new();

    public string Description { get; set; } = "";
    public List<string> YoutubeLinks { get; set; } = new();
}
