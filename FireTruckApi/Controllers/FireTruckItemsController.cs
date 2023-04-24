// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace FireTruckApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FireTruckItemsController : ControllerBase
{
    private readonly IDataStorage _dataStorage;

    public FireTruckItemsController(IDataStorage dataStorage)
    {
        _dataStorage = dataStorage;
    }

    [HttpGet(Name = "GetFireTruckItems")]
    public IEnumerable<LocationItem> Get(string fireTruck, string location) => _dataStorage.FireTrucks
        .First(x => x.Identifier == fireTruck).Locations.First(y => y.Identifier == location).Items;
}
