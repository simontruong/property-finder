using System.IO;
using ServiceStack.Text;
using System.Collections.Generic;
public class CsvHelper
{
    public void WriteCsv(List<SuburbProfile> suburbs, string filepath)
    {
        var csvContents = CsvSerializer.SerializeToCsv<SuburbProfile>(suburbs);
        File.WriteAllText(filepath, csvContents);
    }
}