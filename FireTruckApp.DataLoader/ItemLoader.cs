// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using FireTruckApp.DataModel;
using Newtonsoft.Json;

namespace FireTruckApp.DataLoader;

public class ItemLoader
{
    public static List<Item> LoadBaseItems(string text) => JsonConvert.DeserializeObject<List<Item>>(text) ?? new List<Item>();

    public static List<FireTruck> LoadFireTrucks(string text) => JsonConvert.DeserializeObject<List<FireTruck>>(text) ?? new List<FireTruck>();

    public static FireTruckItems LoadFireTruckItems(string fireTruck) =>
        JsonConvert.DeserializeObject<FireTruckItems>(fireTruck) ?? new FireTruckItems();
}
