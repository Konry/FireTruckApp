// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using FastExcel;

namespace FireTruckApp.DataLoader;

public interface IWorksheetWrapper
{
    bool Exists { get; }
    IEnumerable<Row> Rows { get; }
    string Name { get; }
    int Index { get; }
    public void Read();
}

public class WorksheetWrapper : IWorksheetWrapper
{
    private Worksheet _worksheet;

    public WorksheetWrapper(Worksheet worksheet)
    {
        _worksheet = worksheet;
    }

    public void Read() => _worksheet.Read();

    public bool Exists => _worksheet.Exists;

    public IEnumerable<Row> Rows => _worksheet.Rows;

    public string Name => _worksheet.Name;

    public int Index => _worksheet.Index;
}

public class WorksheetLoader : IWorksheetLoader
{
    private readonly ICollection<IWorksheetWrapper> _worksheets = new List<IWorksheetWrapper>();

    public void LoadFile(string file)
    {
        FileInfo inputFile = new(file);
        using FastExcel.FastExcel fastExcel = new(inputFile, true);
        _worksheets.Clear();
        foreach (Worksheet fastExcelWorksheet in fastExcel.Worksheets)
        {
            _worksheets.Add(new WorksheetWrapper(fastExcelWorksheet));
        }
    }

    public IEnumerable<IWorksheetWrapper> Worksheets => _worksheets;
}

public interface IWorksheetLoader
{
    public void LoadFile(string file);
    public IEnumerable<IWorksheetWrapper> Worksheets { get; }
}
