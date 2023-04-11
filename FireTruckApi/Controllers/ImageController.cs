// Copyright (c) Jan Philipp Luehrig.All rights reserved.
// These files are licensed to you under the MIT license.

using Microsoft.AspNetCore.Mvc;

namespace FireTruckApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageController : ControllerBase
{
    private static readonly string commonAppData =
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

    private static readonly string basePath =
        $"{commonAppData}{Path.DirectorySeparatorChar}FireTruckApi{Path.DirectorySeparatorChar}";

    private static readonly string imagePath =
        $"{basePath}{Path.DirectorySeparatorChar}images{Path.DirectorySeparatorChar}";

    private static readonly string baseImageName = "base.jpg";

    [HttpGet(Name = "GetFireTruckImage")]
    public IActionResult Get(string fireTruck, string? location = null)
    {
        if (location != null)
        {
            return PhysicalFile(imagePath + fireTruck + Path.DirectorySeparatorChar + location + ".jpg", "image/jpeg");
        }
        return PhysicalFile(imagePath + fireTruck + Path.DirectorySeparatorChar + baseImageName, "image/jpeg");
    }

}
