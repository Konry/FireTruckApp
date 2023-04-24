// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using FireTruckApp.DataModel;
using Newtonsoft.Json;

namespace FireTruckApi.DataHandling;

public class DataStorage : IDataStorage
{
    private static readonly string CommonAppData =
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

    private static readonly string BasePath =
        $"{CommonAppData}{Path.DirectorySeparatorChar}FireTruckApi{Path.DirectorySeparatorChar}";

    private const string ItemStorageFileName = "items.json";
    private const string TruckStorageFileName = "trucks.json";
    private readonly ILogger<DataStorage> _logger;

    public DataStorage(ILogger<DataStorage> logger)
    {
        _logger = logger;
        try
        {
            LoadFromDisk();
        }
        catch (Exception e)
        {
            _logger.LogCritical(EventIds.SErrorIdUnknownExceptionInDataStorageInitialization, e,
                "Critical Exception in data handling");
        }
    }

    public List<Item> Items { get; private set; } = new();
    public List<FireTruck> FireTrucks { get; private set; } = new();

    public void Update(List<Item> items)
    {
        if (items.Count > 0)
        {
            Items = items;
        }

        StoreToDisk(BasePath, ItemStorageFileName, Items);
    }

    public void Update(List<FireTruck> fireTrucks)
    {
        if (fireTrucks.Count > 0)
        {
            FireTrucks = fireTrucks;
        }

        StoreToDisk(BasePath, TruckStorageFileName, FireTrucks);
    }

    private void LoadFromDisk()
    {
        if (File.Exists(BasePath + ItemStorageFileName))
        {
            List<Item>? loaded =
                JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText(BasePath + ItemStorageFileName));
            if (loaded != null)
            {
                Items = loaded;
            }
        }

        if (File.Exists(BasePath + TruckStorageFileName))
        {
            List<FireTruck>? loaded =
                JsonConvert.DeserializeObject<List<FireTruck>>(File.ReadAllText(BasePath + TruckStorageFileName));
            if (loaded != null)
            {
                FireTrucks = loaded;
            }
        }
    }


    private void StoreToDisk(string path, string filename, object obj)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        using StreamWriter file = File.CreateText(path + filename);

        JsonSerializer serializer = new();
        serializer.Serialize(file, obj);
    }
}
