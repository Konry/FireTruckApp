using System.Net;
using System.Web.Http;
using FireTruckApi.Controllers;
using FireTruckApi.DataHandling;
using FireTruckApp.DataModel;
using Moq;

namespace FireTruckApiTest;

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

    [Test]
    public void Get_NotExistingItem_ThrowsException()
    {
        _storage.Setup(x => x.FireTrucks).Returns(new List<FireTruck>());
        var exception = Assert.Throws<HttpResponseException>(() => _sut.Get("NotExisting"));
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception!.Response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public void Get_ExistingItem_ReturnsFireTruck()
    {
        _storage.Setup(x => x.FireTrucks).Returns(new List<FireTruck> {new() {Identifier = "Item"}});
        var fireTruck = _sut.Get("Item");
        Assert.That(fireTruck, Is.Not.Null);
    }
}
