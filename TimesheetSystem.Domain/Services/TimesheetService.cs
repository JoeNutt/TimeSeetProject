using System.Globalization;
using TimesheetSystem.Domain.Abtractions;
using TimesheetSystem.Domain.Abtractions.Interfaces;
using TimesheetSystem.Domain.Entities;

namespace TimesheetSystem.Domain.Services;

public class TimesheetService : ITimesheetService
{
    private IEnumerable<TimesheetEntry> _entries;

    public void SetEntries(IEnumerable<TimesheetEntry> entries)
    {
        _entries = entries ?? throw new ArgumentNullException(nameof(entries));
    }

    public string ExportToCsv(ICsvBuilder csvBuilder)
    {
        var entries = _entries.ToList();
        if (!entries.Any())
        {
            csvBuilder.AddHeader("UserName", "Date", "ProjectName", "Description", "HoursWorked", "DailyTotal");
            return csvBuilder.Build();
        }

        var groupedEntries = entries.GroupBy(e => new { e.UserName, e.Date })
            .Select(g => new
            {
                g.Key.UserName,
                g.Key.Date,
                DailyTotal = g.Sum(e => e.HoursWorked.HoursWorked),
                Entries = g.ToList()
            });

        csvBuilder.AddHeader("UserName", "Date", "ProjectName", "Description", "HoursWorked", "DailyTotal");

        var rows = groupedEntries.SelectMany(group =>
            group.Entries.Select(entry => new
            {
                group.UserName,
                Date = group.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                entry.ProjectName,
                entry.Description,
                HoursWorked = entry.HoursWorked.HoursWorked.ToString(CultureInfo.InvariantCulture),
                DailyTotal = group.DailyTotal.ToString(CultureInfo.InvariantCulture)
            }));

        foreach (var row in rows)
        {
            csvBuilder.AddRow(row.UserName, row.Date, row.ProjectName, row.Description, row.HoursWorked, row.DailyTotal);
        }

        return csvBuilder.Build();
    }
}