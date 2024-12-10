using MediatR;
using TimesheetSystem.Domain.Aggregates;
using TimesheetSystem.Domain.Entities;

namespace TimesheetSystem.Application.UseCases.Commands.Handlers;

public class SubmitTimesheetEntryCommandHandler : IRequestHandler<SubmitTimesheetEntryCommand, Unit>
{
    private readonly TimeSheet _timesheet;
    private readonly ITimesheetRepository _timesheetRepository;

    public SubmitTimesheetEntryCommandHandler(TimeSheet timesheet, ITimesheetRepository timesheetRepository)
    {
        _timesheet = timesheet;
        _timesheetRepository = timesheetRepository;
    }

    public async Task<Unit> Handle(SubmitTimesheetEntryCommand request, CancellationToken cancellationToken)
    {
        var entry = TimesheetEntry.Create(request.UserName, request.Date, request.ProjectName, request.Description,
            request.HoursWorked);

        _timesheet.AddEntry(entry);
        await _timesheetRepository.AddTimesheetEntryAsync(entry);

        return Unit.Value;
    }
}