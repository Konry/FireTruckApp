// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using AspNet = Microsoft.AspNetCore.Mvc;

namespace FireTruckApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FireTruckController : ControllerBase
{
    private readonly IDataStorage _dataStorage;

    public FireTruckController(IDataStorage dataStorage)
    {
        _dataStorage = dataStorage ?? throw new ArgumentNullException(nameof(dataStorage));
    }

    [HttpGet("{identifier}", Name = nameof(GetSingleFireTruck))]
    [ProducesResponseType(typeof(FireTruck), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<FireTruck> GetSingleFireTruck(string identifier)
    {
        var fireTrucks = _dataStorage.FireTrucks.Where(x => x.Identifier == identifier).ToList();

        if (!fireTrucks.Any())
        {
            return NotFound($"Fire Truck {identifier} not found");
        }
        if (fireTrucks.Count > 1)
        {
            return BadRequest($"There are several firetrucks with the same id {identifier}");
        }

        return Ok(fireTrucks.First());
    }

    [HttpGet(Name = nameof(GetAllFireTrucks))]
    [ProducesResponseType(typeof(IEnumerable<FireTruck>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<FireTruck>> GetAllFireTrucks() => Ok(_dataStorage.FireTrucks);
}
