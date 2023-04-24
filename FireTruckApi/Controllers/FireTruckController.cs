// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using System.Web.Http;
using Microsoft.AspNetCore.Mvc;

using AspNet = Microsoft.AspNetCore.Mvc;

namespace FireTruckApi.Controllers;

[ApiController]
[AspNet.Route("[controller]")]
public class FireTruckController : ControllerBase
{
    private readonly IDataStorage _dataStorage;

    public FireTruckController(IDataStorage dataStorage)
    {
        _dataStorage = dataStorage ?? throw new ArgumentNullException(nameof(dataStorage));
    }

    [Microsoft.AspNetCore.Mvc.HttpGet(Name = "GetFireTrucks")]
    public BareFireTruck Get(string identifier)
    {
        var fireTrucks = _dataStorage.FireTrucks.Where(x => x.Identifier == identifier).ToList();

        if (fireTrucks == null || !fireTrucks.Any())
        {
            var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent($"No fire truck with ID {identifier}"),
                ReasonPhrase = "Fire Truck ID Not Found"
            };
            throw new HttpResponseException(resp);
        }
        if (fireTrucks.Count > 1)
        {
            var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent($"There are several firetrucks with the same id {identifier}"),
                ReasonPhrase = "Fire Truck ID Ambiguous"
            };
            throw new HttpResponseException(resp);
        }

        return fireTrucks.First();
    }

    [Microsoft.AspNetCore.Mvc.HttpGet(Name = "GetAllFireTrucks")]
    public IEnumerable<BareFireTruck> GetAll() => _dataStorage.FireTrucks;
}
