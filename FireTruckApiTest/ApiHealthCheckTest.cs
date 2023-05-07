// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using FireTruckApi;
using FireTruckApi.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;

namespace FireTruckApiTest;

[TestFixture]
public class ApiHealthCheckTest
{
    private ApiHealthCheck _sut = null!;

    [SetUp]
    public void SetUp() => _sut = new ApiHealthCheck();

    [Test]
    public void SetStatusOfService_SettingAServiceWhichIsNotEnteredYet_DoesNotThrowsAnyException() =>
        // Arrange
        // Act
        // Assert
        Assert.DoesNotThrow(() => _sut.SetStatusOfService("ServiceName", HealthStatus.Unhealthy));

    [Test]
    public void SetStatusOfService_SettingAServiceWhichIsEnteredAlready_DoesNotThrowsAnyException()
    {
        // Arrange
        _sut.SetStatusOfService("ServiceName", HealthStatus.Unhealthy);

        // Act
        // Assert
        Assert.DoesNotThrow(() => _sut.SetStatusOfService("ServiceName", HealthStatus.Unhealthy));
    }

    [Test]
    public void SetStatusOfService_NoServiceSendAnyData_ReturnsNormallyHealthy()
    {
        // Arrange

        // Act
        var result = _sut.CheckHealthAsync(It.IsAny<HealthCheckContext>()).GetAwaiter().GetResult();

        // Assert
        Assert.That(result.Status, Is.EqualTo(HealthStatus.Healthy));
    }

    [Test]
    public void SetStatusOfService_AServiceSendUnhealthyData_ReturnsUnhealthy()
    {
        // Arrange
        _sut.SetStatusOfService("ServiceName", HealthStatus.Unhealthy);

        // Act
        var result = _sut.CheckHealthAsync(It.IsAny<HealthCheckContext>()).GetAwaiter().GetResult();

        // Assert
        Assert.That(result.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }


    [Test]
    public void SetStatusOfService_MultipleServicesSendHealthyData_ReturnsHealthy()
    {
        // Arrange
        _sut.SetStatusOfService("ServiceName1", HealthStatus.Healthy);
        _sut.SetStatusOfService("ServiceName2", HealthStatus.Healthy);
        _sut.SetStatusOfService("ServiceName3", HealthStatus.Healthy);

        // Act
        var result = _sut.CheckHealthAsync(It.IsAny<HealthCheckContext>()).GetAwaiter().GetResult();

        // Assert
        Assert.That(result.Status, Is.EqualTo(HealthStatus.Healthy));
    }

    [Test]
    public void SetStatusOfService_MultipleServicesSendHealthyDataOneDegraded_ReturnsDegraded()
    {
        // Arrange
        _sut.SetStatusOfService("ServiceName1", HealthStatus.Healthy);
        _sut.SetStatusOfService("ServiceName2", HealthStatus.Degraded);
        _sut.SetStatusOfService("ServiceName3", HealthStatus.Healthy);

        // Act
        var result = _sut.CheckHealthAsync(It.IsAny<HealthCheckContext>()).GetAwaiter().GetResult();

        // Assert
        Assert.That(result.Status, Is.EqualTo(HealthStatus.Degraded));
    }

    [Test]
    public void SetStatusOfService_MultipleServicesHealthyUnhealthyAndDegraded_ReturnsUnhealthy()
    {
        // Arrange
        _sut.SetStatusOfService("ServiceName1", HealthStatus.Healthy);
        _sut.SetStatusOfService("ServiceName2", HealthStatus.Unhealthy);
        _sut.SetStatusOfService("ServiceName3", HealthStatus.Degraded);

        // Act
        var result = _sut.CheckHealthAsync(It.IsAny<HealthCheckContext>()).GetAwaiter().GetResult();

        // Assert
        Assert.That(result.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }
}
