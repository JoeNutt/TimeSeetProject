using TimesheetSystem.Domain.ValueObjects;

namespace TimesheetSystem.Domain.Entities;

public class TimesheetEntry
{
    public string UserName { get; }
    public DateTime Date { get; }
    public string ProjectName { get; }
    public string Description { get; }
    public TimePeriod HoursWorked { get; }

    public TimesheetEntry(string userName, DateTime date, string projectName, string description, decimal hoursWorked)
    {
        if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException("User name cannot be empty.");
        if (hoursWorked <= 0 || hoursWorked > 24) throw new ArgumentException("Hours worked must be between 1 and 24.");
            
        UserName = userName;
        Date = date.Date; 
        ProjectName = projectName;
        Description = description;
        HoursWorked = new TimePeriod(hoursWorked);
    }
}
