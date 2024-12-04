using TimesheetSystem.Application.UseCases.Commands;
using TimesheetSystem.Application.UseCases.Commands.Handlers;
using TimesheetSystem.Domain.Aggregates;

namespace TimesheetSystem.UnitTests.UseCases;

[TestFixture]
public class SubmitTimesheetEntryTests
{
    [Test]
    public async Task SubmitTimesheetEntryCommand_ValidEntry_AddsToTimesheet()
    {
        // Arrange
        var timesheet = new TimeSheet();
        var command = new SubmitTimesheetEntryCommand("John Doe", DateTime.Today, "Project A", "Development", 8);
        var handler = new SubmitTimesheetEntryCommandHandler(timesheet);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        var totalHours = timesheet.GetDailySummary("John Doe", DateTime.Today);
        Assert.That(totalHours, Is.EqualTo(8));
    }
}