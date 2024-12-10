using TimesheetSystem.Domain.ValueObjects;

namespace TimesheetSystem.Domain.Entities;

public class TimesheetEntry
{
    public int Id { get; set; }
    public string UserName { get; private set; }
    public DateTime Date { get; private set; }
    public string ProjectName { get; private set; }
    public string Description { get; private set; }
    public TimePeriod HoursWorked { get; private set; }


    public TimesheetEntry()
    {
    } 


    public static TimesheetEntry Create(string userName, DateTime date, string projectName, string description,
        decimal hoursWorked)
    {
        if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException("User name cannot be empty.");
        if (hoursWorked <= 0 || hoursWorked > 24) throw new ArgumentException("Hours worked must be between 1 and 24.");

        var entry = new TimesheetEntry()
        {
            UserName = userName,
            Date = date.Date,
            ProjectName = projectName,
            Description = description,
            HoursWorked = new TimePeriod(hoursWorked)
        };

        return entry;
    }
}