// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using FastExcel;
using FireTruckApp.DataModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace FireTruckApp.DataLoader.Tests;

[TestFixture]
public class ExcelDataLoaderTest
{
    [SetUp]
    public void SetUp()
    {
        _worksheetLoaderMock = new Mock<IWorksheetLoader>();
        _sut = new ExcelDataLoader(new NullLogger<ExcelDataLoader>(), _worksheetLoaderMock.Object);
    }

    private ExcelDataLoader _sut = null!;
    private Mock<IWorksheetLoader> _worksheetLoaderMock = null!;

    [Test]
    [Ignore("This is a DEV Test, working only locally")]
    [Category("DEV")]
    public void LoadExcelFile_MultipleTabs_LoadDifferentFireTrucks()
    {
        // Arrange
        ExcelDataLoader sut = new(new NullLogger<ExcelDataLoader>(), new WorksheetLoader());

        // Act
        sut.LoadXlsxFile(@"E:\Onedrive\Feuerwehr\Fahrzeuge + Ausstattung\Ausstattung.xlsx");
        // Assert

        Assert.Fail();
    }

    [Test]
    public void HandleFireTruck_TwoSheetsWithSameName_ThrowsException()
    {
        // Arrange
        Mock<IWorksheetWrapper> worksheetTabAMock = GenerateCorrectFireTruckWorksheetTab("01-01-01");

        // Act
        List<FireTruck> list = new() {new FireTruck("01-01-01")};

        // Assert
        Assert.Throws<TruckAlreadyExistingException>(() => _sut.HandleFireTruck(worksheetTabAMock.Object, ref list));
    }

    [Test]
    public void HandleFireTruck_GetItemsForATruck_ReturnsTheContent()
    {
        // Arrange
        Mock<IWorksheetWrapper> worksheetTabAMock = GenerateCorrectFireTruckWorksheetTab("01-01-01");

        // Act
        List<FireTruck> list = new();
        var res =  _sut.HandleFireTruck(worksheetTabAMock.Object, ref list);

        var res1 = res.Locations.Where(x => x.Identifier == "G3").ToList().First().Items;
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(res, Is.Not.Null);
            Assert.That(res.Identifier, Is.EqualTo("01-01-01"));
            Assert.That(res.Locations, Has.Count.EqualTo(4));
            Assert.That(res.Locations.First(x => x.Identifier == "G3").Items, Has.Count.EqualTo(2));
            Assert.That(res.Locations.First(x => x.Identifier == "G1").Items, Has.Count.EqualTo(0));
            Assert.That(res.Locations.First(x => x.Identifier == "G4").Items, Has.Count.EqualTo(0));
            Assert.That(res.Locations.First(x => x.Identifier == "G3").Items.First(x => x.Identifier == "B-Schlauch 20m").Quantity, Is.EqualTo(6));
        });
    }

    [Test]
    public void LoadXlsxFile_TwoSheetsWithSameName_LogsErrorButContinuingLoadTrucks()
    {
        // Arrange
        Mock<IWorksheetWrapper> worksheetTabAMock = GenerateCorrectFireTruckWorksheetTab("01-01-01");
        Mock<IWorksheetWrapper> worksheetTabCopyOfAMock = GenerateCorrectFireTruckWorksheetTab("01-01-01");
        Mock<IWorksheetWrapper> worksheetTabCMock = GenerateCorrectFireTruckWorksheetTab("01-01-02");
        Mock<ILogger<ExcelDataLoader>> loggerMock = new();

        Mock<IWorksheetLoader> worksheetLoaderMock = new();
        worksheetLoaderMock.Setup(x => x.Worksheets).Returns(new List<IWorksheetWrapper>
        {
            worksheetTabAMock.Object, worksheetTabCopyOfAMock.Object, worksheetTabCMock.Object
        });

        ExcelDataLoader sut = new(loggerMock.Object, worksheetLoaderMock.Object);

        // Act
        sut.LoadXlsxFile("any");

        // Assert
        loggerMock.Verify(
            x => x.Log(LogLevel.Error, EventIds.ErrorIdTruckAlreadyExists, It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }


    [Test]
    public void HandleFireTruck_NoWorksheetName_ThrowsNoCorrectNameException()
    {
        // Arrange
        Worksheet worksheet = new();
        List<Row> emptyRows = new();
        worksheet.Rows = emptyRows;

        Mock<IWorksheetWrapper> worksheetMock = new();
        worksheetMock.Setup(x => x.Rows).Returns(emptyRows);
        ExcelDataLoader sut = new(new NullLogger<ExcelDataLoader>(), null);
        List<FireTruck> list = new() {new FireTruck(worksheet.Name)};
        // Act

        // Assert
        Assert.Throws<WorksheetNotCorrectNamedException>(() => sut.HandleFireTruck(worksheetMock.Object, ref list));
    }

    [Test]
    public void HandleFireTruck_EmptyWorkSheet_LogsEmptyWorksheetError()
    {
        // Arrange
        Mock<IWorksheetWrapper> worksheetTabAMock = HeaderOnly("");
        Mock<IWorksheetLoader> worksheetLoaderMock = new();
        worksheetLoaderMock.Setup(x => x.Worksheets).Returns(new List<IWorksheetWrapper>
        {
            worksheetTabAMock.Object
        });
        Mock<ILogger<ExcelDataLoader>> loggerMock = new();

        ExcelDataLoader sut = new(loggerMock.Object, worksheetLoaderMock.Object);

        // Act
        sut.LoadXlsxFile("any");

        // Assert
        loggerMock.Verify(
            x => x.Log(LogLevel.Error, EventIds.ErrorIdWorkSheetNameNotSufficient, It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Test]
    public void HandleFireTruck_FirstRowContainsHeaderSecondEmpty_LogsErrorTruckDataDataNotFound()
    {
        // Arrange
        Mock<IWorksheetWrapper> worksheetTabAMock = HeaderOnly("01-01-01");
        List<FireTruck> list = new();

        // Act

        // Assert
        Assert.Throws<FireTruckDataNotFoundException>(() => _sut.HandleFireTruck(worksheetTabAMock.Object, ref list));
    }

    [Test]
    public void HandleFireTruck_FirstRowContainsHeaderSecondData_ReturnsATruck()
    {
        // Arrange
        Mock<IWorksheetWrapper> worksheetTabAMock = GenerateCorrectFireTruckWorksheetTab("01-01-01");
        List<FireTruck> list = new();
        // Act
        FireTruck result = _sut.HandleFireTruck(worksheetTabAMock.Object, ref list);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Identifier, Is.EqualTo("01-01-01"));
    }

    [Test]
    public void HandleItemList_CorrectItemListIsGiven_HandleTheCorrectItemListAndReturnIt()
    {
        // Arrange
        var workerMapperMock = GenerateCorrectToolWorksheetTab();

        // Act
        var res = _sut.HandleItemList(workerMapperMock.Object);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(res, Is.Not.Null);
            Assert.That(res, Has.Count.EqualTo(4));;
        });
    }

    private static Mock<IWorksheetWrapper> GenerateCorrectFireTruckWorksheetTab(string truckIdentifier)
    {
        Mock<IWorksheetWrapper> worksheetTabAMock = new();
        worksheetTabAMock.Setup(x => x.Name).Returns(truckIdentifier);
        worksheetTabAMock.Setup(x => x.Index).Returns(1);
        worksheetTabAMock.Setup(x => x.Rows).Returns(new List<Row>
        {
            new(1, new List<Cell>
            {
                new(1, "Fhzg."),
                new(2, "Fhzg Name Lang"),
                new(3, "Funkkennung"),
                new(4, "Fahrgestell"),
                new(5, "Indienstnahme"),
                new(6, "Tags"),
            }),
            new(2, new List<Cell>
            {
                new(1, "ELW"),
                new(2, "Einsatzleitwagen"),
                new(3, truckIdentifier),
                new(4, "Mercedes-Benz"),
                new(5, "01.12.2014"),
                new(6, "ELW, First Responder"),
            }),
            new(3, new List<Cell>()),
            new(4, new List<Cell>
            {
                new(1, "Fach"),
                new(2, "Bilder"),
                new(3, "Geräte"),
                new(4, "Anzahl"),
                new(5, "Besonderheit"),
            }),
            new(5, new List<Cell>()),
            new(6, new List<Cell> {new Cell(1, "G1")}),
            new(7, new List<Cell>()),
            new(8, new List<Cell> {new Cell(1, "G2")}),
            new(9, new List<Cell>()),
            new(10, new List<Cell>()),
            new(11, new List<Cell>
            {
                new Cell(1, "G3"),
                new Cell(3, "B-Schlauch 20m"),
                new Cell(4, 6),
                new Cell(5, "6x 20m"),
            }),
            // no Location
            new(12, new List<Cell>
            {
                new Cell(3, "C-Schlauch 15m"),
                new Cell(4, "12"),
                new Cell(5, "neon"),
            }),
            new(13, new List<Cell>()),
            new(14, new List<Cell> {new Cell(1, "G4")}),
        });
        return worksheetTabAMock;
    }

    private static Mock<IWorksheetWrapper> GenerateCorrectToolWorksheetTab(string title = "Geräteübersicht")
    {
        Mock<IWorksheetWrapper> worksheetTabAMock = new();
        worksheetTabAMock.Setup(x => x.Name).Returns(title);
        worksheetTabAMock.Setup(x => x.Index).Returns(1);
        worksheetTabAMock.Setup(x => x.Rows).Returns(new List<Row>
        {
            new(1, new List<Cell>
            {
                new(1, "Name"),
                new(2, "Gewicht"),
                new(3, "Bild"),
                new(4, "Alternative Bezeichnung"),
                new(5, "Einsatzzweck"),
                new(6, "Beschreibung"),
                new(7, "Youtube"),
            }),
            new(2, new List<Cell>
            {
                new(1, "Feuerwehraxt"),
                new(2, "3,6"),
                new(4, "Axt"),
                new(5, "Alle"),
                new(6, "Multifunktionswehrzeug, beim Innenangriff immer dabei"),
                new(7, "https://www.youtube.com/watch?v=xdZlUGlo2iU"),
            }),
            new(3, new List<Cell>()),
            new(4, new List<Cell>
            {
                new(1, "Feuerwehraxt"),
                new(2, "3,6"),
                new(4, "Axt"),
                new(5, "Alle"),
                new(6, "Multifunktionswehrzeug, beim Innenangriff immer dabei"),
                new(7, "https://www.youtube.com/watch?v=xdZlUGlo2iU"),
            }),
            new(5, new List<Cell>
            {
                new(1, "something"),
                new(2, "3.6"),
                new(4, "Axt"),
                new(5, "Alle"),
                new(6, "Multifunktionswehrzeug, beim Innenangriff immer dabei"),
            }),
            new(6, new List<Cell>
            {
                new(1, "something else"),
                new(2, 3.6),
                new(4, "Axt"),
                new(5, "Alle"),
            }),
        });
        return worksheetTabAMock;
    }
    private static Mock<IWorksheetWrapper> HeaderOnly(string truckIdentifier)
    {
        Mock<IWorksheetWrapper> worksheetTabAMock = new();
        worksheetTabAMock.Setup(x => x.Name).Returns(truckIdentifier);
        worksheetTabAMock.Setup(x => x.Index).Returns(1);
        worksheetTabAMock.Setup(x => x.Rows).Returns(new List<Row> {new(1, new List<Cell> {new(1, "A")})});
        return worksheetTabAMock;
    }
}
