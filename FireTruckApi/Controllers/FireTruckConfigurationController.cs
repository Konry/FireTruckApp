// Copyright (c) Jan Philipp Luehrig.All rights reserved.
// These files are licensed to you under the MIT license.

using System.Net;
using System.Web.Http;
using FireTruckApi.DataHandling;
using FireTruckApp.DataLoader;
using Microsoft.AspNetCore.Mvc;

namespace FireTruckApi.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("[controller]")]
public class FireTruckConfigurationController : ControllerBase
{
    private readonly IDataStorage _storage;

    public FireTruckConfigurationController(IDataStorage storage)
    {
        _storage = storage;
    }

    [Microsoft.AspNetCore.Mvc.HttpPost(Name = "UploadXLSX")]
    public async Task<HttpResponseMessage> Post()
    {
        HttpResponseMessage result = new()
        {
            StatusCode = HttpStatusCode.BadRequest
        };

        try
        {
            var httpStream = HttpContext.Request.Body;
            string tempFileName = Path.GetTempFileName();

            if (HttpContext.Request.Headers.ContentType !=
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                throw new HttpRequestException("The file in the given format will not be accepted");
            }
            await using (FileStream outputFileStream = new(tempFileName, FileMode.Create))
            {
                await httpStream.CopyToAsync(outputFileStream);
            }
            var loader = new ExcelDataLoader();
            await Task.Run(() =>
            {
                try
                {
                    var res = loader.LoadXLSXFile(tempFileName);
                    _storage.Update(res.Items);
                    _storage.Update(res.Trucks);
                    Console.WriteLine( $"{res.Items.Count} {res.Trucks.Count}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }).ConfigureAwait(false);

            result.StatusCode = HttpStatusCode.OK;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            result.StatusCode = HttpStatusCode.InternalServerError;
            result.ReasonPhrase = e.Message; // Currently posting error directly, not a good case, refactor to better
            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }

        return result;
    }


}
