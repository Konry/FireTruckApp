using Newtonsoft.Json;
using FireTruckApp.DataModel;


namespace FireTruckApp.DataLoader;
public class ItemLoader
{
    string items = "";
    public List<Item> LoadBaseItems(string text){
        return JsonConvert.DeserializeObject<LoadBaseItems<Item>(text);
    }
}
