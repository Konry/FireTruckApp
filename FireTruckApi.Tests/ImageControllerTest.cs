// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using FireTruckApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;

namespace FireTruckApi.Tests;

[TestFixture]
public class ImageControllerTest
{

    private ImageController _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _sut = new ImageController();
    }

    [Test]
    public void GetImage_GetEmptyFireTruckString_Returns400()
    {
        // Arrange
        // Act
        var actionResult = _sut.GetImage("");
        var request = actionResult as IStatusCodeActionResult;

        // Assert
        Assert.That(request, Is.Not.Null);
        Assert.That(request!.StatusCode, Is.EqualTo(400));
    }

    [Test]
    public void GetImage_GetNotExistingFireTruckImage_Returns404()
    {
        // Arrange
        // Act
        var actionResult = _sut.GetImage("NotExists");
        var request = actionResult as IStatusCodeActionResult;

        // Assert
        Assert.That(request, Is.Not.Null);
        Assert.That(request!.StatusCode, Is.EqualTo(404));
    }
}
