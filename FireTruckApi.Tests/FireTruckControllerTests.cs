using FireTruckApi.Controllers;
using FireTruckApi.DataHandling;
using FireTruckApp.DataModel;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FireTruckApi.Tests;

public class FireTruckControllerTests
{
    private Mock<IDataStorage> _storage = null!;
    private FireTruckController _sut = null!;

    [SetUp]
    public void Setup()
    {
        _storage = new Mock<IDataStorage>();
        _sut = new FireTruckController(_storage.Object);
    }

    // Todo fix this issue
    [Test, Ignore("TODO fix this ")]
    public void Get_NotExistingItem_ThrowsException()
    {
        _storage.Setup(x => x.FireTrucks).Returns(new List<FireTruck>());
        // var exception = Assert.Throws<HttpResponseException>(() => _sut.Get("NotExisting"));
        // Assert.That(exception, Is.Not.Null);
        // Assert.That(exception!.Response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }


    [Test]
    public void GetSingleFireTruck_ExistingItem_ReturnsFireTruck()
    {
        // Arrange
        FireTruck truck = new("Item");
        _storage.Setup(x => x.FireTrucks).Returns(new List<FireTruck> {truck});

        // Act
        var response = _sut.GetSingleFireTruck("Item").Result;

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response, Is.TypeOf<OkObjectResult>());

        var okResult = response as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var fireTruck = okResult!.Value as BareFireTruck;
        Assert.Multiple(() =>
        {
            Assert.That(fireTruck, Is.Not.Null);
            Assert.That(fireTruck!.Identifier, Is.EqualTo(truck.Identifier));
        });
    }

    [Test]
    public void GetSingleFireTruck_MultipleExistingItems_ReturnsFireTruck()
    {
        // Arrange
        FireTruck truck = new( "Item");
        _storage.Setup(x => x.FireTrucks).Returns(new List<FireTruck>
        {
            truck,
            new FireTruck("70/11/04"),
        });

        // Act
        var response = _sut.GetSingleFireTruck("Item").Result;

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response, Is.TypeOf<OkObjectResult>());

        var okResult = response as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var fireTruck = okResult!.Value as BareFireTruck;
        Assert.Multiple(() =>
        {
            Assert.That(fireTruck, Is.Not.Null);
            Assert.That(fireTruck!.Identifier, Is.EqualTo(truck.Identifier));
        });
    }

    [Test]
    public void METHOD()
    {
        // Arrange


        // Act
        // Assert
    }
}
