// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using FireTruckApi.DataHandling;
using FireTruckApp.DataModel;
using Microsoft.AspNetCore.Mvc;

namespace FireTruckApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FireTruckLocationController : ControllerBase
{
    private readonly IDataStorage _storage;

    public FireTruckLocationController(IDataStorage storage)
    {
        _storage = storage;
    }

    [HttpGet(Name = "GetFireTruckLocation")]
    public IEnumerable<FireTruckLocation> Get(string fireTruck) =>
        _storage.FireTrucks.First(x => x.Identifier == fireTruck).Locations;
}
