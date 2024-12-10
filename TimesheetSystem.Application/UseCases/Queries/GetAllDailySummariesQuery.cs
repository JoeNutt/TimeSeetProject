using MediatR;
using TimesheetSystem.Domain.Aggregates;
using TimesheetSystem.Domain.Entities;

namespace TimesheetSystem.Application.UseCases.Queries;

public class GetAllDailySummariesQuery : IRequest<IEnumerable<TimesheetEntry>>, IRequest<IEnumerable<TimeSheet>>
{
}