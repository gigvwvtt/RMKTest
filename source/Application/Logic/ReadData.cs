using System.Text;
using OfficeOpenXml;

namespace Application.Logic;

public class ReadData
{
    public static List<string> ReadExcel(string filename)
    {
        try
        {
            var excelData = new List<string>();
            var exeDir = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(exeDir, filename);
            var fileInfo = new FileInfo(path);
            var bin = File.ReadAllBytes(path);
            
            using (var stream = new MemoryStream(bin))
            using (var ep = new ExcelPackage(stream))
            {
                var firstWorksheet = ep.Workbook.Worksheets[0];

                for (int i = firstWorksheet.Dimension.Start.Row; i <= firstWorksheet.Dimension.End.Row; i++)
                {
                    var sb = new StringBuilder();
                    for (int j = firstWorksheet.Dimension.Start.Column; j <= firstWorksheet.Dimension.End.Column; j++)
                    {
                        if (firstWorksheet.Cells[i, j].Value != null)
                        {
                            sb.Append($"{firstWorksheet.Cells[i, j].Value}");
                        }
                        sb.Append(j != firstWorksheet.Dimension.End.Column ? "\t" : "");
                    }
                    
                    excelData.Add(sb.ToString());
                }
                
                return excelData;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error reading data: {e.Message}");
            return new List<string>();
        }
    }
}