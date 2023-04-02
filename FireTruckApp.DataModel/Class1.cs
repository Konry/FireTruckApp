
namespace FireTruckApp.DataModel;

public class FireTruck
{
    public List<Items> ItemsOnBoard{get; set;}
    
}

public class Item{
    public string Identifier{get;set;}

    public double Weight{get;set;}

    public List<string> Tags{get;set;}

    public List<string> AlternateWording{get;set;}
}
