namespace FireTruckApp.DataLoader;

public class FireTruckDataNotFoundException : Exception
{
    public FireTruckDataNotFoundException() { }
    public FireTruckDataNotFoundException(string message) : base(message) { }
    public FireTruckDataNotFoundException(string message, Exception inner) : base(message, inner) { }
}