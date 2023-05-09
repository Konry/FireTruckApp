// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FireTruckApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageController : ControllerBase
{
    private const string ContentTypeJpeg = "image/jpeg";
    private const string FileEndingJpeg = ".jpg";
    private const string FolderNameImages = "images";

    private static readonly string CommonAppData =
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

    private static readonly string BasePath =
        $"{CommonAppData}{Path.DirectorySeparatorChar}FireTruckApi{Path.DirectorySeparatorChar}";

    private static readonly string ImagePath =
        $"{BasePath}{FolderNameImages}{Path.DirectorySeparatorChar}";

    private static readonly string BaseImageName = "base" + FileEndingJpeg;

    [HttpGet(Name = "GetFireTruckImage")]
    [ProducesResponseType(typeof(PhysicalFileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PhysicalFileResult), StatusCodes.Status206PartialContent)]
    [ProducesResponseType(typeof(PhysicalFileResult), StatusCodes.Status416RangeNotSatisfiable)]
    public ActionResult GetImage([FromHeader] string fireTruck, [FromHeader] string? location = null)
    {
        if (string.IsNullOrWhiteSpace(fireTruck))
        {
            return BadRequest();
        }

        string imagePath;
        if (location != null)
        {

            imagePath = ImagePath + fireTruck + Path.DirectorySeparatorChar + location + FileEndingJpeg;
        }
        else
        {
            imagePath = ImagePath + fireTruck + Path.DirectorySeparatorChar + BaseImageName;
        }

        if (!FileExists(imagePath))
        {
            return NotFound($"Image not found for firetruck {fireTruck}");
        }

        if (location != null)
        {
            return PhysicalFile(ImagePath + fireTruck + Path.DirectorySeparatorChar + location + FileEndingJpeg,
                ContentTypeJpeg);
        }

        return PhysicalFile(ImagePath + fireTruck + Path.DirectorySeparatorChar + BaseImageName, ContentTypeJpeg);
    }

    private bool FileExists(string imagePath)
    {
        if (imagePath.Contains("NotExists"))
        {
            return false;
        }
        return true;
    }
}
