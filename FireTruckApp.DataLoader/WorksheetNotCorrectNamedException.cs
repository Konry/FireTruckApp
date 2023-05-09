namespace FireTruckApp.DataLoader;

public class WorksheetNotCorrectNamedException : Exception
{
    public WorksheetNotCorrectNamedException() { }
    public WorksheetNotCorrectNamedException(string message) : base(message) { }
    public WorksheetNotCorrectNamedException(string message, Exception inner) : base(message, inner) { }
}