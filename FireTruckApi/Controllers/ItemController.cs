// Copyright (c) Jan Philipp Luehrig.All rights reserved.
// These files are licensed to you under the MIT license.

using FireTruckApi.DataHandling;
using FireTruckApp.DataModel;
using Microsoft.AspNetCore.Mvc;

namespace FireTruckApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemController : ControllerBase
{
    private readonly IDataStorage _storage;

    private static readonly List<Item> TestData = new()
    {
        new Item
        {
            Identifier = "Axt",
            Weight = 1
        },
        new Item
        {
            Identifier = "Schlauch",
            Weight = 5
        },
        new Item
        {
            Identifier = "Strahlrohr B",
            Weight = 5
        },
        new Item
        {
            Identifier = "Schuttmulde",
            Weight = 2
        }
    };

    public ItemController(IDataStorage storage)
    {
        _storage = storage;
    }
    [HttpGet(Name = "GetItem")]
    public Item Get(string itemIdentifier)
    {
        return _storage.Items.First(x => x.Identifier == itemIdentifier);
    }

}
