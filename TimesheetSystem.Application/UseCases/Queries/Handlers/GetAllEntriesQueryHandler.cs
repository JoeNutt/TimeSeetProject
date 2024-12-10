namespace TimesheetSystem.Application.UseCases.Queries.Handlers;

using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TimesheetSystem.Domain.Entities;
using TimesheetSystem.Domain.Abtractions;

public class GetAllEntriesQueryHandler : IRequestHandler<GetAllEntriesQuery, IEnumerable<TimesheetEntry>>
{
    private readonly ITimesheetRepository _repository;

    public GetAllEntriesQueryHandler(ITimesheetRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TimesheetEntry>> Handle(GetAllEntriesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllTimesheetEntriesAsync();
    }
}