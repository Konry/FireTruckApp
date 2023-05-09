// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

namespace FireTruckApi.DataHandling;

public interface IDataStorage
{
    public List<Item> Items { get; }
    public List<FireTruck> FireTrucks { get; }

    void Update(List<Item> items);
    void Update(List<FireTruck> fireTrucks);
}
