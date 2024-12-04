using MediatR;

namespace TimesheetSystem.Application.UseCases.Commands;

public class SubmitTimesheetEntryCommand : IRequest
{
    public string UserName { get; }
    public DateTime Date { get; }
    public string ProjectName { get; }
    public string Description { get; }
    public decimal HoursWorked { get; }

    public SubmitTimesheetEntryCommand(string userName, DateTime date, string projectName, string description, decimal hoursWorked)
    {
        UserName = userName;
        Date = date;
        ProjectName = projectName;
        Description = description;
        HoursWorked = hoursWorked;
    }
}