using System.Text;
using TimesheetSystem.Domain.Abtractions;
using TimesheetSystem.Domain.Abtractions.Interfaces;

namespace TimesheetSystem.Domain.Builders;

public class CsvBuilder : ICsvBuilder
{
    private readonly StringBuilder _csvBuilder;

    public CsvBuilder()
    {
        _csvBuilder = new StringBuilder();
    }

    public void AddHeader(params string[] headers)
    {
        _csvBuilder.AppendLine(string.Join(",", headers));
    }

    public void AddRow(params string[] values)
    {
        _csvBuilder.AppendLine(string.Join(",", values));
    }

    public string Build()
    {
        return _csvBuilder.ToString();
    }
}
