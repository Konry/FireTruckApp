// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using Microsoft.AspNetCore.Mvc;

namespace FireTruckApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageController : ControllerBase
{
    private const string ContentTypeJpeg = "image/jpeg";
    private const string FileEndingJpg = ".jpg";
    private const string FolderNameImages = "images";

    private static readonly string CommonAppData =
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

    private static readonly string BasePath =
        $"{CommonAppData}{Path.DirectorySeparatorChar}FireTruckApi{Path.DirectorySeparatorChar}";

    private static readonly string ImagePath =
        $"{BasePath}{Path.DirectorySeparatorChar}{FolderNameImages}{Path.DirectorySeparatorChar}";

    private static readonly string BaseImageName = "base" + FileEndingJpg;

    [HttpGet(Name = "GetFireTruckImage")]
    public IActionResult Get(string fireTruck, string? location = null)
    {
        if (location != null)
        {
            return PhysicalFile(ImagePath + fireTruck + Path.DirectorySeparatorChar + location + FileEndingJpg,
                ContentTypeJpeg);
        }

        return PhysicalFile(ImagePath + fireTruck + Path.DirectorySeparatorChar + BaseImageName, ContentTypeJpeg);
    }
}
