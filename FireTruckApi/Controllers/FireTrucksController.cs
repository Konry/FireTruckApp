// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using FireTruckApi.DataHandling;
using FireTruckApp.DataModel;
using Microsoft.AspNetCore.Mvc;

namespace FireTruckApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FireTrucksController : ControllerBase
{
    private readonly IDataStorage _dataStorage;

    public FireTrucksController(IDataStorage dataStorage)
    {
        _dataStorage = dataStorage;
    }

    [HttpGet(Name = "GetFireTrucks")]
    public IEnumerable<BareFireTruck> Get() => _dataStorage.FireTrucks;
}
