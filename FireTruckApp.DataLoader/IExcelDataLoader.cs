using FireTruckApp.DataModel;

namespace FireTruckApp.DataLoader;

public interface IExcelDataLoader
{
    (List<Item> Items, List<FireTruck> Trucks) LoadXlsxFile(string file, bool tabPerTruck = true);
}