// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using Microsoft.AspNetCore.Mvc;

namespace FireTruckApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemController : ControllerBase
{
    private readonly IDataStorage _storage;

    public ItemController(IDataStorage storage)
    {
        _storage = storage;
    }

    [HttpGet(Name = "GetItem")]
    public Item Get(string itemIdentifier) => _storage.Items.First(x => x.Identifier == itemIdentifier);
}
