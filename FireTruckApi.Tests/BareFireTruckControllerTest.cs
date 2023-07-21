// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using FireTruckApi.Controllers;
using FireTruckApi.DataHandling;
using FireTruckApp.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FireTruckApi.Tests;

public class BareFireTruckControllerTest
{
    private Mock<IDataStorage> _storage = null!;
    private BareFireTruckController _sut = null!;

    [SetUp]
    public void Setup()
    {
        _storage = new Mock<IDataStorage>();
        _sut = new BareFireTruckController(_storage.Object);
    }

    [Test]
    public void Get_NotExistingItem_ThrowsException()
    {
        _storage.Setup(x => x.FireTrucks).Returns(new List<FireTruck>());
        var response = _sut.GetSingleBareFireTruck("NotExisting").Result;


        Assert.That(response, Is.Not.Null);
        Assert.That(response, Is.TypeOf<NotFoundObjectResult>());

        var notFound = response as NotFoundObjectResult;
        Assert.That(notFound, Is.Not.Null);
        Assert.That(notFound!.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
    }


    [Test]
    public void GetSingleFireTruck_ExistingItem_ReturnsFireTruck()
    {
        // Arrange
        FireTruck truck = new("Item");
        _storage.Setup(x => x.FireTrucks).Returns(new List<FireTruck> {truck});

        // Act

        var response = _sut.GetSingleBareFireTruck("Item").Result;

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response, Is.TypeOf<OkObjectResult>());

        // var okResult = response as OkObjectResult;
        // Assert.That(okResult, Is.Not.Null);
        // var fireTruck = okResult!.Value as BareFireTruck;
        // Assert.Multiple(() =>
        // {
        //     Assert.That(fireTruck, Is.Not.Null);
        //     Assert.That(fireTruck!.Identifier, Is.EqualTo(truck.Identifier));
        // });
    }

    [Test]
    public void GetSingleFireTruck_MultipleExistingItems_ReturnsFireTruck()
    {
        // Arrange
        FireTruck truck = new( "Item");
        _storage.Setup(x => x.FireTrucks).Returns(new List<FireTruck>
        {
            truck,
            new("70/11/04"),
        });

        // Act
        var response = _sut.GetSingleBareFireTruck("Item").Result;

        // Assert
        // TODO can this be easier?
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
}
