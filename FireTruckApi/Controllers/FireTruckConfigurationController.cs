// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using System.Net;
using System.Web.Http;
using FireTruckApi.DataHandling;
using FireTruckApp.DataLoader;
using FireTruckApp.DataModel;
using Microsoft.AspNetCore.Mvc;

namespace FireTruckApi.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("[controller]")]
public class FireTruckConfigurationController : ControllerBase
{
    private const string ContentTypeOpenXml = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    private readonly IExcelDataLoader _excelDataLoader;

    private readonly ILogger<FireTruckConfigurationController> _logger;
    private readonly IDataStorage _storage;

    public FireTruckConfigurationController(ILogger<FireTruckConfigurationController> logger,
        IExcelDataLoader excelDataLoader, IDataStorage storage)
    {
        _logger = logger;
        _excelDataLoader = excelDataLoader;
        _storage = storage;
    }

    [Microsoft.AspNetCore.Mvc.HttpPost(Name = "UploadXLSX")]
    public async Task<HttpResponseMessage> Post()
    {
        HttpResponseMessage result = new() {StatusCode = HttpStatusCode.BadRequest};

        try
        {
            Stream httpStream = HttpContext.Request.Body;
            string tempFileName = Path.GetTempPath() + Path.DirectorySeparatorChar + Path.GetRandomFileName();

            if (HttpContext.Request.Headers.ContentType != ContentTypeOpenXml)
            {
                throw new HttpRequestException("The file in the given format will not be accepted");
            }

            await using (FileStream outputFileStream = new(tempFileName, FileMode.Create))
            {
                await httpStream.CopyToAsync(outputFileStream);
            }

            await Task.Run(() =>
            {
                try
                {
                    (List<Item> Items, List<FireTruck> Trucks) res = _excelDataLoader.LoadXlsxFile(tempFileName);
                    _storage.Update(res.Items);
                    _storage.Update(res.Trucks);
                    _logger.LogTrace("{ItemCount} {TruckCount}", res.Items.Count, res.Trucks.Count);
                }
                catch (Exception e)
                {
                    _logger.LogCritical(EventIds.s_errorIdUnknownErrorInFireTruckConfiguration, e,
                        "Critical exception in update storage");
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
