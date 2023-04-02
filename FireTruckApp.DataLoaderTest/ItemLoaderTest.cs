// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using BaseLibrary;
using FireTruckApp.DataLoader;
using FireTruckApp.DataModel;

namespace FireTruckApp.DataLoaderTest;

[TestFixture]
public class ItemLoaderTest
{
    [SetUp]
    public void Setup() => _sut = new ItemLoader();

    private ItemLoader _sut = null!;

    [Test]
    public void LoadBaseItems_TwoItems_ReturningTwoItems()
    {
        // Arrange

        List<Item> items = ItemLoader.LoadBaseItems(InternalResourceLoader.GetTextFromEmbeddedResource("ItemsExample.json"));

        // Act
        // Assert
        Assert.That(items, Has.Count.EqualTo(2));
    }
    [Test]
    public void LoadBaseItems_TwoItems_ItemExists()
    {
        // Arrange

        List<Item> items = ItemLoader.LoadBaseItems(InternalResourceLoader.GetTextFromEmbeddedResource("ItemsExample.json"));

        // Act
        // Assert
        Assert.That(items.Where(x => x.Identifier == "Axe"), Is.Not.Null);
    }

    [Test]
    public void LoadFireTrucks_TwoFireTrucks_ReturnsTwoFireTrucks()
    {
        // Arrange

        List<FireTruck> items = ItemLoader.LoadFireTrucks(InternalResourceLoader.GetTextFromEmbeddedResource("FireTruckExample.json"));

        // Act
        // Assert
        Assert.That(items, Has.Count.EqualTo(2));
    }

    [Test]
    public void LoadFireTrucks_HasLadder_ReturnsLadderShort()
    {
        // Arrange

        List<FireTruck> items = ItemLoader.LoadFireTrucks(InternalResourceLoader.GetTextFromEmbeddedResource("FireTruckExample.json"));

        // Act
        // Assert
        Assert.That(items.Where(x => x.TruckTypeShort == "LD"), Is.Not.Null);
    }


    [Test]
    public void LoadFireTrucks_HasLadder_ReturnsLadder()
    {
        // Arrange

        List<FireTruck> items = ItemLoader.LoadFireTrucks(InternalResourceLoader.GetTextFromEmbeddedResource("FireTruckExample.json"));

        // Act
        // Assert
        Assert.That(items.Where(x => x.TruckType == "Ladder"), Is.Not.Null);
    }


    [Test]
    public void LoadFireTruckItems_HasLocations_ReturnTwoLocations()
    {
        // Arrange

        FireTruckItems items = ItemLoader.LoadFireTruckItems(InternalResourceLoader.GetTextFromEmbeddedResource("FireTruckItemsExample.json"));

        // Act
        // Assert
        Assert.That(items.Locations, Has.Count.EqualTo(2));
    }
    [Test]
    public void LoadFireTruckItems_LocationOneHasTwoItems_ReturnTwoItems()
    {
        // Arrange

        FireTruckItems items = ItemLoader.LoadFireTruckItems(InternalResourceLoader.GetTextFromEmbeddedResource("FireTruckItemsExample.json"));

        // Act
        // Assert
        Assert.That(items.Locations.First().Location, Is.EqualTo("G1"));
        Assert.That(items.Locations.First().Items, Has.Count.EqualTo(2));
        Assert.That(items.Locations.First().Items.First().Identifer, Is.EqualTo("Axe 25"));
    }
}
