using TimesheetSystem.Domain.Abtractions.Interfaces;
using TimesheetSystem.Domain.Entities;

namespace TimesheetSystem.Domain.Aggregates;

public class TimeSheet : ITimeSheet
{
    private readonly List<TimesheetEntry> _entries;

    public TimeSheet()
    {
        _entries = new List<TimesheetEntry>();
    }

    public void AddEntry(TimesheetEntry entry)
    {
        if (entry == null) throw new ArgumentNullException(nameof(entry));
        
        var totalHoursForDay = _entries
            .Where(e => e.UserName == entry.UserName && e.Date == entry.Date)
            .Sum(e => e.HoursWorked.HoursWorked) + entry.HoursWorked.HoursWorked;

        if (totalHoursForDay > 24)
            throw new InvalidOperationException("Total hours for the day cannot exceed 24.");

        _entries.Add(entry);
    }

    public decimal GetDailySummary(string userName, DateTime date)
    {
        if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException("User name cannot be empty.");

        return _entries
            .Where(e => e.UserName == userName && e.Date == date.Date)
            .Sum(e => e.HoursWorked.HoursWorked);
    }

    public IEnumerable<TimesheetEntry> GetAllEntries()
    {
        return _entries.AsReadOnly(); 
    }

}