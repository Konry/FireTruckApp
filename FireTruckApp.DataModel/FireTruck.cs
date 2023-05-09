namespace FireTruckApp.DataModel;

public class FireTruck : BareFireTruck
{
    public FireTruck(string identifier)
    {
        Identifier = identifier;
        Locations = new List<Location>();
    }

    public List<Location> Locations { get; }
}