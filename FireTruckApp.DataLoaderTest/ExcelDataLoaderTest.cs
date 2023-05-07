// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using FastExcel;
using FireTruckApp.DataLoader;
using FireTruckApp.DataModel;
using Microsoft.Extensions.Logging.Abstractions;

namespace FireTruckApp.DataLoaderTest;

[TestFixture]
public class ExcelDataLoaderTest
{
    private const string WorksheetName = "01-01-01";

    [Test, Ignore("This is a DEV Test, working only locally")]
    [Category("DEV")]
    public void LoadExcelFile_MultipleTabs_LoadDifferentFireTrucks()
    {
        // Arrange
        ExcelDataLoader sut = new(new NullLogger<ExcelDataLoader>());

        // Act
        sut.LoadXlsxFile(@"E:\Onedrive\Feuerwehr\Fahrzeuge + Ausstattung\Ausstattung.xlsx");
        // Assert

        Assert.Fail();
    }

    [Test]
    public void HandleFireTruck_TwoSheetsWithSameName_ThrowsException()
    {
        // Arrange
        Worksheet worksheet = new() {Name = WorksheetName};
        List<Row> emptyRows = new();
        worksheet.Rows = emptyRows;
        ExcelDataLoader sut = new(new NullLogger<ExcelDataLoader>());
        List<FireTruck> list = new()
        {
            new FireTruck(worksheet.Name)
        };
        // Act

        // Assert
        Assert.Throws<TruckAlreadyExistingException>(() => sut.HandleFireTruck(worksheet, ref list));
    }


    [Test]
    public void HandleFireTruck_NoWorksheetName_ThrowsNoCorrectNameException()
    {
        // Arrange
        Worksheet worksheet = new();
        List<Row> emptyRows = new();
        worksheet.Rows = emptyRows;

        ExcelDataLoader sut = new(new NullLogger<ExcelDataLoader>());
        List<FireTruck> list = new() {new FireTruck(worksheet.Name)};
        // Act

        // Assert
        Assert.Throws<WorksheetNotCorrectNamedException>(() => sut.HandleFireTruck(worksheet, ref list));
    }

    [Test]
    [Ignore("Currently not working, worksheet needs to be read.")]
    public void HandleFireTruck_FirstRowContainsHeaderSecondEmpty_ThrowsDataNotFoundException()
    {
        // Arrange
        Worksheet worksheet = new() {Name = WorksheetName};
        List<Row> correctFilledRows = new();
        worksheet.Rows = correctFilledRows;

        ExcelDataLoader sut = new(new NullLogger<ExcelDataLoader>());
        List<FireTruck> list = new();
        // Act

        // Assert
        Assert.Throws<FireTruckDataNotFoundException>(() => sut.HandleFireTruck(worksheet, ref list));
    }

    [Test]
    [Ignore("Currently not working, worksheet needs to be read.")]
    public void HandleFireTruck_FirstRowContainsHeaderSecondData_ReturnsATruck()
    {
        // Arrange
        Worksheet worksheet = new() {Name = WorksheetName};
        List<Row> correctFilledRows = new();
        worksheet.Rows = correctFilledRows;

        ExcelDataLoader sut = new(new NullLogger<ExcelDataLoader>());
        List<FireTruck> list = new();
        // Act
        FireTruck result = sut.HandleFireTruck(worksheet, ref list);

        // Assert
        Assert.That(result, Is.Not.Null);
    }
}
