// Copyright (c) Jan Philipp Luehrig.All rights reserved.
// These files are licensed to you under the MIT license.

using FireTruckApp.DataModel;
using Microsoft.AspNetCore.Mvc;

namespace FireTruckApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FireTruckController : ControllerBase
{
    private static readonly List<BareFireTruck> TestData = new()
    {
        new BareFireTruck
        {
            Identifier = "01-11-01",
            TruckType = "Einsatzleitwagen",
            TruckTypeShort = "ELW"
        },
        new BareFireTruck
        {
            Identifier = "01-44-01",
            TruckType = "Löschgruppenfahrzeug",
            TruckTypeShort = "LF 8/6"
        },
        new BareFireTruck
        {
            Identifier = "01-48-01",
            TruckType = "Hilfeleistungslöschgruppenfahzeug",
            TruckTypeShort = "HLF 20/16"
        }
    };

    [HttpGet(Name = "GetFireTrucks")]
    public IEnumerable<BareFireTruck> Get()
    {
        return TestData;
    }

}
