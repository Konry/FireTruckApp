// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using System.IO.Abstractions;
using FireTruckApi.DataHandling;
using FireTruckApp.DataModel;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace FireTruckApi.Tests;

[TestFixture]
public class DataStorageTest
{
    private DataStorage _sut = null!;
    private Mock<IFileSystem> _mockFileSystem = null!;
    private Mock<IDirectory> _directoryMock;
    private Mock<IFile> _fileMock;


    [SetUp]
    public void SetUp()
    {
        _directoryMock = new Mock<IDirectory>();
        _fileMock = new Mock<IFile>();
        _mockFileSystem = new Mock<IFileSystem>();
        _mockFileSystem.Setup(x => x.Directory).Returns(_directoryMock.Object);
        _mockFileSystem.Setup(x => x.File).Returns(_fileMock.Object);
        _sut = new DataStorage(new NullLogger<DataStorage>(), _mockFileSystem.Object);
    }

    [Test]
    public void FireTrucks_ListUpdatedWithOneEntry_ReturnsOneEntry()
    {
        // Arrange
        _sut.Update(new List<FireTruck>()
        {
            new("hello")
        });

        // Act
        // Assert
        Assert.That(_sut.FireTrucks, Has.Count.EqualTo(1));
    }
    [Test]
    public void FireTrucks_ListUpdatedWithSameOneEntry_ReturnsOneEntry()
    {
        // Arrange
        _sut.Update(new List<FireTruck>()
        {
            new("hello")
        });
        _sut.Update(new List<FireTruck>()
        {
            new("hello")
        });

        // Act
        // Assert
        Assert.That(_sut.FireTrucks, Has.Count.EqualTo(1));
    }

    [Test]
    public void Update_DirectoryThrowsIOException_DoesNotThrow()
    {
        // Arrange
        _directoryMock.Setup(x => x.CreateDirectory(It.IsAny<string>())).Throws<IOException>();
        // Act
        // Assert
        Assert.DoesNotThrow(() => _sut.Update(new List<FireTruck>()));
    }

    [Test]
    public void Update_DirectoryThrowsUnauthorizedAccessException_DoesNotThrow()
    {
        // Arrange
        _directoryMock.Setup(x => x.CreateDirectory(It.IsAny<string>())).Throws<UnauthorizedAccessException>();
        // Act
        // Assert
        Assert.DoesNotThrow(() => _sut.Update(new List<FireTruck>()));
    }

    [Test]
    public void Update_DirectoryThrowsArgumentException_DoesNotThrow()
    {
        // Arrange
        _directoryMock.Setup(x => x.CreateDirectory(It.IsAny<string>())).Throws<ArgumentException
        >();
        // Act
        // Assert
        Assert.DoesNotThrow(() => _sut.Update(new List<FireTruck>()));
    }

    [Test]
    public void Update_DirectoryThrowsArgumentNullException_DoesNotThrow()
    {
        // Arrange
        _directoryMock.Setup(x => x.CreateDirectory(It.IsAny<string>())).Throws<ArgumentNullException>();
        // Act
        // Assert
        Assert.DoesNotThrow(() => _sut.Update(new List<FireTruck>()));
    }

    [Test]
    public void Update_DirectoryThrowsPathTooLongException_DoesNotThrow()
    {
        // Arrange
        _directoryMock.Setup(x => x.CreateDirectory(It.IsAny<string>())).Throws<PathTooLongException>();
        // Act
        // Assert
        Assert.DoesNotThrow(() => _sut.Update(new List<FireTruck>()));
    }

    [Test]
    public void Update_DirectoryThrowsDirectoryNotFoundException_DoesNotThrow()
    {
        // Arrange
        _directoryMock.Setup(x => x.CreateDirectory(It.IsAny<string>())).Throws<DirectoryNotFoundException>();
        // Act
        // Assert
        Assert.DoesNotThrow(() => _sut.Update(new List<FireTruck>()));
    }

    [Test]
    public void Update_DirectoryThrowsNotSupportedException_DoesNotThrow()
    {
        // Arrange
        _directoryMock.Setup(x => x.CreateDirectory(It.IsAny<string>())).Throws<NotSupportedException>();
        // Act
        // Assert
        Assert.DoesNotThrow(() => _sut.Update(new List<FireTruck>()));
    }
}
