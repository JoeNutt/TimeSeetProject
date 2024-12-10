using MediatR;
using TimesheetSystem.Domain.Abtractions;
using TimesheetSystem.Domain.Abtractions.Interfaces;
using TimesheetSystem.Domain.Services;

namespace TimesheetSystem.Application.UseCases.Commands;

public class GenerateCsvCommandHandler : IRequestHandler<GenerateCsvCommand, string>
{
    private readonly ITimesheetRepository _timesheetRepository;
    private readonly ITimesheetService _timesheetService;
    private readonly ICsvBuilder _csvBuilder;

    public GenerateCsvCommandHandler(ITimesheetRepository timesheetRepository, ITimesheetService timesheetService, ICsvBuilder csvBuilder)
    {
        _timesheetRepository = timesheetRepository ?? throw new ArgumentNullException(nameof(timesheetRepository));
        _timesheetService = timesheetService ?? throw new ArgumentNullException(nameof(timesheetService));
        _csvBuilder = csvBuilder ?? throw new ArgumentNullException(nameof(csvBuilder));
    }

    public async Task<string> Handle(GenerateCsvCommand request, CancellationToken cancellationToken)
    {
        var entries = await _timesheetRepository.GetAllTimesheetEntriesAsync();
        _timesheetService.SetEntries(entries);
        return await Task.FromResult(_timesheetService.ExportToCsv(_csvBuilder));
    }
}