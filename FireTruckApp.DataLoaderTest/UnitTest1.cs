namespace FireTruckApp.DataLoaderTest;

[TestFixture]
public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void LoadBaseItems_LoadRight_Right()
    {
        ItemLoader.LoadBaseItems(GetTextFromEmbeddedFile("ItemsExample.json"));
        Assert.Pass();
    }

    public string GetTextFromEmbeddedFile(string filename){
        
        string returnValue;
    }
}