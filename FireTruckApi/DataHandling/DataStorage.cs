// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using System.IO.Abstractions;
using Newtonsoft.Json;

namespace FireTruckApi.DataHandling;

internal class DataStorage : IDataStorage
{
    private static readonly string CommonAppData =
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

    private static readonly string BasePath =
        $"{CommonAppData}{Path.DirectorySeparatorChar}FireTruckApi{Path.DirectorySeparatorChar}";

    private const string ItemStorageFileName = "items.json";
    private const string TruckStorageFileName = "trucks.json";
    private readonly ILogger<DataStorage> _logger;
    private readonly IFileSystem _fileSystem;

    public DataStorage(ILogger<DataStorage> logger, IFileSystem fileSystem)
    {
        _logger = logger;
        _fileSystem = fileSystem;
        try
        {
            LoadFromDisk();
        }
        catch (Exception e)
        {
            _logger.LogCritical(EventIds.ErrorIdUnknownErrorInLoadData, e,
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

        try
        {
            StoreToDisk(BasePath, TruckStorageFileName, FireTrucks);
        }
        catch (Exception e)
        {
            _logger.LogCritical(EventIds.ErrorIdUnknownErrorInStoreData, e, "Can not store data in the database");
        }
    }

    private void LoadFromDisk()
    {
        if (_fileSystem.File.Exists(BasePath + ItemStorageFileName))
        {
            List<Item>? loaded =
                JsonConvert.DeserializeObject<List<Item>>(_fileSystem.File.ReadAllText(BasePath + ItemStorageFileName));
            if (loaded != null)
            {
                Items = loaded;
            }
        }

        if (_fileSystem.File.Exists(BasePath + TruckStorageFileName))
        {
            List<FireTruck>? loaded =
                JsonConvert.DeserializeObject<List<FireTruck>>(_fileSystem.File.ReadAllText(BasePath + TruckStorageFileName));
            if (loaded != null)
            {
                FireTrucks = loaded;
            }
        }
    }


    private void StoreToDisk(string path, string filename, object obj)
    {
        if (!_fileSystem.Directory.Exists(path))
        {
            _fileSystem.Directory.CreateDirectory(path);
        }

        using StreamWriter file = _fileSystem.File.CreateText(path + filename);

        JsonSerializer serializer = new();
        serializer.Serialize(file, obj);
    }
}
