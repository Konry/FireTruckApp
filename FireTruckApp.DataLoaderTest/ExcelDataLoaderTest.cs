// Copyright (c) Jan Philipp Luehrig.All rights reserved.
// These files are licensed to you under the MIT license.

using FastExcel;
using FireTruckApp.DataLoader;
using FireTruckApp.DataModel;

namespace FireTruckApp.DataLoaderTest;

[TestFixture]
public class ExcelDataLoaderTest
{
    private const string WorksheetName = "01-01-01";

    [Test, Category("DEV")]
    public void LoadExcelFile_MultipleTabs_LoadDifferentFireTrucks()
    {
        // Arrange
        ExcelDataLoader sut = new();

        // Act
        sut.LoadXLSXFile(@"E:\Onedrive\Feuerwehr\Fahrzeuge + Ausstattung\Ausstattung.xlsx",tabPerTruck: true);
        // Assert
    }

    [Test]
    public void HandleFireTruck_TwoSheetsWithSameName_ThrowsException()
    {
        // Arrange
        var worksheet = new Worksheet
        {
            Name = WorksheetName
        };
        var emptyRows = new List<Row>();
        worksheet.Rows = emptyRows;
        var sut = new ExcelDataLoader();
        var list = new List<BareFireTruck>
        {
            new()
            {
                Identifier = worksheet.Name
            }
        };
        // Act

        // Assert
        Assert.Throws<TruckAlreadyExistingException>(() => sut.HandleFireTruck(worksheet, ref list));
    }


    [Test]
    public void HandleFireTruck_NoWorksheetName_ThrowsNoCorrectNameException()
    {
        // Arrange
        var worksheet = new Worksheet();
        var emptyRows = new List<Row>();
        worksheet.Rows = emptyRows;

        var sut = new ExcelDataLoader();
        var list = new List<BareFireTruck>
        {
            new()
            {
                Identifier = worksheet.Name
            }
        };
        // Act

        // Assert
        Assert.Throws<WorksheetNotCorrectNamedException>(() => sut.HandleFireTruck(worksheet, ref list));
    }

    [Test, Ignore("Currently not working, worksheet needs to be read.")]
    public void HandleFireTruck_FirstRowContainsHeaderSecondEmpty_ThrowsDataNotFoundException()
    {
        // Arrange
        var worksheet = new Worksheet()
        {
            Name = WorksheetName
        };
        var correctFilledRows = new List<Row>();
        worksheet.Rows = correctFilledRows;

        var sut = new ExcelDataLoader();
        var list = new List<BareFireTruck>();
        // Act

        // Assert
        Assert.Throws<FireTruckDataNotFoundException>(() => sut.HandleFireTruck(worksheet, ref list));
    }

    [Test, Ignore("Currently not working, worksheet needs to be read.")]
    public void HandleFireTruck_FirstRowContainsHeaderSecondData_ReturnsATruck()
    {
        // Arrange
        var worksheet = new Worksheet()
        {
            Name = WorksheetName
        };
        var correctFilledRows = new List<Row>();
        worksheet.Rows = correctFilledRows;

        var sut = new ExcelDataLoader();
        var list = new List<BareFireTruck>();
        // Act
        var result = sut.HandleFireTruck(worksheet, ref list);

        // Assert
        Assert.That(result, Is.Not.Null);
    }
}
