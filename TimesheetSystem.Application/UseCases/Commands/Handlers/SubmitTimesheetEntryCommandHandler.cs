using MediatR;
using TimesheetSystem.Domain.Aggregates;
using TimesheetSystem.Domain.Entities;

namespace TimesheetSystem.Application.UseCases.Commands.Handlers
{
    public class SubmitTimesheetEntryCommandHandler : IRequestHandler<SubmitTimesheetEntryCommand>
    {
        private readonly TimeSheet _timesheet;

        public SubmitTimesheetEntryCommandHandler(TimeSheet timesheet)
        {
            _timesheet = timesheet;
        }

        public async Task Handle(SubmitTimesheetEntryCommand request, CancellationToken cancellationToken)
        {
            // Create a new timesheet entry from the command data
            var entry = new TimesheetEntry(request.UserName, request.Date, request.ProjectName, request.Description, request.HoursWorked);

            // Add the entry to the timesheet
            await Task.Run(() => _timesheet.AddEntry(entry), cancellationToken);
        }
    }
}