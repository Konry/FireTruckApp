// Copyright (c) Jan Philipp Luehrig.All rights reserved.
// These files are licensed to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using FastExcel;
using FireTruckApp.DataModel;

[assembly: InternalsVisibleTo("FireTruckApp.DataLoaderTest")]

namespace FireTruckApp.DataLoader;

public class ExcelDataLoader
{
    public void LoadXLSXFile(string file, bool tabPerTruck)
    {
        FileInfo inputFile = new(file);

        // Create an instance of Fast Excel
        using FastExcel.FastExcel fastExcel = new(inputFile, true);

        List<BareFireTruck> fireTrucks = new();
        foreach (Worksheet? worksheet in fastExcel.Worksheets)
        {
            Console.WriteLine("Worksheet Name:{0}, Index:{1}", worksheet.Name, worksheet.Index);
            string fireTruckPattern = @"(\d+\W\d+\W\d+)";

            MatchCollection matches = Regex.Matches(worksheet.Name, fireTruckPattern);
            if (matches.Count > 0)
            {
                // Fire truck handling
                try
                {
                    FireTruck truck = HandleFireTruck(worksheet, ref fireTrucks);
                    fireTrucks.Add(truck);
                }
                catch (TruckAlreadyExistingException tae)
                {
                    Console.WriteLine("Truck is already existing, skip {TruckIdentifier}", matches.First().Value);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                HandleItemList(worksheet);
            }
        }
    }


    internal List<Item> HandleItemList(Worksheet worksheet)
    {
        List<Item> items = new();
        try
        {
            Console.WriteLine("Handle Worksheet for items");
            worksheet.Read();
            Row[] rows = worksheet.Rows.ToArray();
            Console.WriteLine($"Amount of rows : {rows.Count()}");

            foreach (Row row in rows)
            {
                Item item = new Item();
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
                            item.AlternateWording = ((string)cell.Value).Split(s_splitSeparators).ToList();
                            break;
                        case 5:
                            break;
                        case 6:
                            item.Description = (string)cell.Value;
                            break;
                        case 7:
                            item.YoutubeLinks = ((string)cell.Value).Split(s_splitSeparators).ToList();
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
            Console.WriteLine(e);
        }

        Console.WriteLine($"Items found {items.Count}");
        return items;
    }

    private static readonly char[] s_splitSeparators = new List<char> {',', ';', '\n', '\r'}.ToArray();

    internal FireTruck HandleFireTruck(Worksheet worksheet, ref List<BareFireTruck> fireTrucks)
    {
        Console.WriteLine("HandleFireTruck");
        Console.WriteLine(worksheet.Name);
        if (string.IsNullOrWhiteSpace(worksheet.Name))
        {
            throw new WorksheetNotCorrectNamedException("The worksheet is not name is empty");
        }

        if (fireTrucks.Any(truck => truck.Identifier == worksheet.Name))
        {
            throw new TruckAlreadyExistingException($"Truck already inside the list {worksheet.Name}");
        }

        FireTruck truck = new();
        {
            truck.Identifier = worksheet.Name;
            worksheet.Read();
            Row[] rows = worksheet.Rows.ToArray();

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

    private static void HandleLocationsOfTruck(Row[] rows, FireTruck truck)
    {
        Location currentLocation = new();
        var itemGlossary = new Dictionary<int, LocationItem>();
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
                                currentLocation.Items.AddRange(itemGlossary.Values.Where(x => !string.IsNullOrWhiteSpace(x.Identifier)));
                            }

                            truck.Locations.Add(currentLocation);
                            currentLocation = new Location();
                        }

                        currentLocation.Identifier = (string) cell.Value;
                        break;
                    case 2:
                        // Image skip
                        break;
                    case 3:
                        // Item name
                        var item = new LocationItem
                        {
                            Identifier = (string) cell.Value
                        };
                        itemGlossary.Add(row.RowNumber, item);

                        break;
                    case 4:
                        // Quantity
                        itemGlossary[row.RowNumber].Quantity = int.Parse((string) cell.Value);
                        break;
                    case 5:
                        // Additions to the current item
                        itemGlossary[row.RowNumber].AdditionalInformation =
                            ((string) cell.Value).Split(s_splitSeparators).ToList();
                        break;
                    default:
                        Console.WriteLine(
                            $"The current item in row {cell.RowNumber} and column {cell.ColumnNumber} is not implemented, so not used yet.");
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
