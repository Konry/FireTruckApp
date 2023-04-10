// Copyright (c) Jan Philipp Luehrig.All rights reserved.
// These files are licensed to you under the MIT license.

using FireTruckApp.DataModel;
using Microsoft.AspNetCore.Mvc;

namespace FireTruckApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FireTruckLocationController : ControllerBase
{
    private static readonly List<FireTruckLocation> s_testDataHlf = new()
    {
        new FireTruckLocation
        {
            Identifier = "G1",
        },
        new FireTruckLocation
        {
            Identifier = "G2",
        },
        new FireTruckLocation
        {
            Identifier = "G3",
        }
    };
    private static readonly List<FireTruckLocation> s_testDataLf = new()
    {
        new FireTruckLocation
        {
            Identifier = "G1",
        },
        new FireTruckLocation
        {
            Identifier = "G3",
        },
        new FireTruckLocation
        {
            Identifier = "G6",
        }
    };
    private static readonly List<FireTruckLocation> s_testDataElw = new()
    {
        new FireTruckLocation
        {
            Identifier = "G1",
        },
        new FireTruckLocation
        {
            Identifier = "G3",
        },
        new FireTruckLocation
        {
            Identifier = "G6",
        }
    };

    [HttpGet(Name = "GetFireTruckLocation")]
    public IEnumerable<FireTruckLocation> Get(string fireTruck)
    {
        Console.WriteLine(fireTruck);
        switch (fireTruck)
        {
            case "01-48-01":
                return s_testDataHlf;
            case "01-42-01":
                return s_testDataLf;
            default:
                return s_testDataElw;
        }
    }

}
