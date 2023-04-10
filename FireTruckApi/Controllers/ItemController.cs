// Copyright (c) Jan Philipp Luehrig.All rights reserved.
// These files are licensed to you under the MIT license.

using FireTruckApp.DataModel;
using Microsoft.AspNetCore.Mvc;

namespace FireTruckApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemController : ControllerBase
{
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

    [HttpGet(Name = "GetItem")]
    public Item Get(string itemIdentifier)
    {
        return TestData.Where(x => x.Identifier == itemIdentifier).FirstOrDefault();
    }

}
