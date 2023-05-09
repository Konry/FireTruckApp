namespace FireTruckApp.DataModel;

public class Location : FireTruckLocation
{
    public Location(string identifier) : base(identifier)
    {
        Items = new List<LocationItem>();
    }

    public List<LocationItem> Items { get; set; }
}
