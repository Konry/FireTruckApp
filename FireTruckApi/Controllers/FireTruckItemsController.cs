// Copyright (c) Jan Philipp Luehrig.All rights reserved.
// These files are licensed to you under the MIT license.

using FireTruckApp.DataModel;
using Microsoft.AspNetCore.Mvc;

namespace FireTruckApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FireTruckItemsController : ControllerBase
{
    private static readonly List<LocationItem> TestData = new()
    {
        new LocationItem
        {
            Identifier = "Axt",
            Quantity = 1
        },
        new LocationItem
        {
            Identifier = "Schlauch",
            Quantity = 5
        },
        new LocationItem
        {
            Identifier = "Strahlrohr B",
            Quantity = 5
        },
        new LocationItem
        {
            Identifier = "Schuttmulde",
            Quantity = 2
        }
    };
    private static readonly List<LocationItem> TestDataG1 = new()
    {
        new LocationItem
        {
            Identifier = "Axe",
            Quantity = 1
        },
        new LocationItem
        {
            Identifier = "Hose",
            Quantity = 5
        },
        new LocationItem
        {
            Identifier = "Strahlrohr C",
            Quantity = 5
        },
        new LocationItem
        {
            Identifier = "Schuttmulde",
            Quantity = 2
        }
    };

    [HttpGet(Name = "GetFireTruckItems")]
    public IEnumerable<LocationItem> Get(string location)
    {
        switch (location)
        {
            case "G1":
                return TestDataG1;
            default:
                return TestData;
        }
    }

}
