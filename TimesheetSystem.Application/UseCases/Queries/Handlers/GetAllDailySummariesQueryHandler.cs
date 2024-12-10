using MediatR;
using TimesheetSystem.Domain.Aggregates;
using TimesheetSystem.Domain.Entities;

namespace TimesheetSystem.Application.UseCases.Queries.Handlers;

public class GetAllDailySummariesQueryHandler : IRequestHandler<GetAllDailySummariesQuery, IEnumerable<TimesheetEntry>>
{
    private readonly ITimesheetRepository _dailySummaryRepository;

    public GetAllDailySummariesQueryHandler(ITimesheetRepository dailySummaryRepository)
    {
        _dailySummaryRepository =
            dailySummaryRepository ?? throw new ArgumentNullException(nameof(dailySummaryRepository));
    }

    public async Task<IEnumerable<TimesheetEntry>> Handle(GetAllDailySummariesQuery request,
        CancellationToken cancellationToken)
    {
        return await _dailySummaryRepository.GetAllTimesheetEntriesAsync();
    }
}
