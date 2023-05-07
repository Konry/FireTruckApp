// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using FastExcel;
using FireTruckApp.DataModel;
using Microsoft.Extensions.Logging;
using static System.Text.RegularExpressions.Regex;

[assembly: InternalsVisibleTo("FireTruckApp.DataLoaderTest")]

namespace FireTruckApp.DataLoader;

public interface IExcelDataLoader
{
    (List<Item> Items, List<FireTruck> Trucks) LoadXlsxFile(string file, bool tabPerTruck = true);
}

public class ExcelDataLoader : IExcelDataLoader
{
    private static readonly char[] SSplitSeparators = new List<char> {',', ';', '\n', '\r'}.ToArray();
    private readonly ILogger<ExcelDataLoader> _logger;

    public ExcelDataLoader(ILogger<ExcelDataLoader> logger)
    {
        _logger = logger;
    }

    public (List<Item> Items, List<FireTruck> Trucks) LoadXlsxFile(string file, bool tabPerTruck = true)
    {
        FileInfo inputFile = new(file);

        // Create an instance of Fast Excel
        using FastExcel.FastExcel fastExcel = new(inputFile, true);

        List<FireTruck> fireTrucks = new();
        List<Item> items = new();
        foreach (Worksheet? worksheet in fastExcel.Worksheets)
        {
            _logger.LogDebug("Worksheet Name:{Name}, Index:{TableIndex}", worksheet.Name, worksheet.Index);
            const string fireTruckPattern = @"(\d+\W\d+\W\d+)";

            MatchCollection matches = Matches(worksheet.Name, fireTruckPattern, RegexOptions.None,
                new TimeSpan(0, 0, 3));
            if (matches.Count > 0)
            {
                // Fire truck handling
                try
                {
                    FireTruck truck = HandleFireTruck(worksheet, ref fireTrucks);
                    fireTrucks.Add(truck);
                }
                catch (FireTruckDataNotFoundException dtdnfou)
                {
                    _logger.LogError(EventIds.SErrorIdTruckDataNotFound, dtdnfou, "Skip worksheet {Name}",
                        worksheet.Name);
                }
                catch (TruckAlreadyExistingException tae)
                {
                    _logger.LogError(EventIds.SErrorIdTruckAlreadyExists, tae,
                        "Truck is already existing, skip {TruckName}", matches.First().Value);
                }
                catch (Exception e)
                {
                    _logger.LogCritical(EventIds.SErrorIdUnknownExceptionInExcelDataLoader, e, "Critical exception");
                    throw;
                }
            }
            else
            {
                items = HandleItemList(worksheet);
            }
        }

        return (items, fireTrucks);
    }


    internal List<Item> HandleItemList(Worksheet worksheet)
    {
        List<Item> items = new();
        try
        {
            _logger.LogDebug("Handle Worksheet for items");
            worksheet.Read();
            Row[] rows = worksheet.Rows.ToArray();
            _logger.LogDebug("Amount of rows : {RowCount}", rows.Length);

            foreach (Row row in rows)
            {
                Item item = new();
                foreach (Cell cell in row.Cells)
                {
                    if (row.RowNumber == 1)
                    {
                        continue;
                    }

                    if (cell.Value == null)
                    {
                        continue;
                    }

                    switch (cell.ColumnNumber)
                    {
                        case 1:
                            item.Identifier = (string)cell.Value;
                            break;
                        case 2:
                            item.Weight = double.Parse((string)cell.Value);
                            break;
                        case 3:
                            // Image
                            break;
                        case 4:
                            item.AlternateWording = ((string)cell.Value).Split(SSplitSeparators).ToList();
                            break;
                        case 5:
                            break;
                        case 6:
                            item.Description = (string)cell.Value;
                            break;
                        case 7:
                            item.YoutubeLinks = ((string)cell.Value).Split(SSplitSeparators).ToList();
                            break;
                    }
                }

                if (!string.IsNullOrWhiteSpace(item.Identifier))
                {
                    items.Add(item);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogCritical(EventIds.SErrorIdUnknownExceptionInExcelDataLoader, e, "Critical exception");
        }

        _logger.LogInformation("Items found {ItemCount}", items.Count);
        return items;
    }

    internal FireTruck HandleFireTruck(Worksheet worksheet, ref List<FireTruck> fireTrucks)
    {
        _logger.LogDebug("HandleFireTruck {WorksheetName}", worksheet.Name);
        if (string.IsNullOrWhiteSpace(worksheet.Name))
        {
            throw new WorksheetNotCorrectNamedException("The worksheet is not name is empty");
        }

        if (fireTrucks.Any(truck => truck.Identifier == worksheet.Name))
        {
            throw new TruckAlreadyExistingException($"Truck already inside the list {worksheet.Name}");
        }

        FireTruck truck = new(worksheet.Name);
        {
            worksheet.Read();
            Row[] rows = worksheet.Rows.ToArray();

            if (rows.Length < 2)
            {
                throw new FireTruckDataNotFoundException(
                    $"There are no data for a firetruck in worksheet {worksheet.Name}");
            }

            Row row = rows[1]; // Row 1 is containing all information about the car

            if (!row.Cells.Any())
            {
                throw new FireTruckDataNotFoundException(
                    $"There are no data for a firetruck in the row {row.RowNumber}");
            }

            foreach (Cell cell in row.Cells)
            {
                switch (cell.ColumnNumber)
                {
                    case 1:
                        truck.TruckTypeShort = (string)cell.Value;
                        break;
                    case 2:
                        truck.TruckType = (string)cell.Value;
                        break;
                }
            }

            HandleLocationsOfTruck(rows, truck);
        }
        return truck;
    }

    private void HandleLocationsOfTruck(Row[] rows, FireTruck truck)
    {
        Location currentLocation = new();
        Dictionary<int, LocationItem> itemGlossary = new();
        for (int index = 3; index < rows.Length; index++)
        {
            Row row = rows[index];
            if (!row.Cells.Any())
            {
                continue;
            }

            foreach (Cell cell in row.Cells)
            {
                switch (cell.ColumnNumber)
                {
                    case 1:
                        // new Location
                        if (!string.IsNullOrWhiteSpace(currentLocation.Identifier))
                        {
                            // Add current items to the location
                            if (itemGlossary.Any())
                            {
                                currentLocation.Items.AddRange(
                                    itemGlossary.Values.Where(x => !string.IsNullOrWhiteSpace(x.Identifier)));
                            }

                            truck.Locations.Add(currentLocation);
                            currentLocation = new Location();
                        }

                        currentLocation.Identifier = (string)cell.Value;
                        break;
                    case 2:
                        // Image skip
                        break;
                    case 3:
                        // Item name
                        LocationItem item = new() {Identifier = (string)cell.Value};
                        itemGlossary.Add(row.RowNumber, item);

                        break;
                    case 4:
                        // Quantity
                        itemGlossary[row.RowNumber].Quantity = int.Parse((string)cell.Value);
                        break;
                    case 5:
                        // Additions to the current item
                        itemGlossary[row.RowNumber].AdditionalInformation =
                            ((string)cell.Value).Split(SSplitSeparators).ToList();
                        break;
                    default:
                        _logger.LogWarning(
                            "The current item in row {RowNumber} and column {ColumnNumber} is not implemented, so not used yet",
                            cell.RowNumber, cell.ColumnNumber);
                        break;
                }
            }
        }
    }
}

public class WorksheetNotCorrectNamedException : Exception
{
    public WorksheetNotCorrectNamedException() { }
    public WorksheetNotCorrectNamedException(string message) : base(message) { }
    public WorksheetNotCorrectNamedException(string message, Exception inner) : base(message, inner) { }
}

public class TruckAlreadyExistingException : Exception
{
    public TruckAlreadyExistingException() { }
    public TruckAlreadyExistingException(string message) : base(message) { }
    public TruckAlreadyExistingException(string message, Exception inner) : base(message, inner) { }
}

public class FireTruckDataNotFoundException : Exception
{
    public FireTruckDataNotFoundException() { }
    public FireTruckDataNotFoundException(string message) : base(message) { }
    public FireTruckDataNotFoundException(string message, Exception inner) : base(message, inner) { }
}
