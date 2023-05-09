namespace FireTruckApp.DataLoader;

public class TruckAlreadyExistingException : Exception
{
    public TruckAlreadyExistingException() { }
    public TruckAlreadyExistingException(string message) : base(message) { }
    public TruckAlreadyExistingException(string message, Exception inner) : base(message, inner) { }
}