using Microsoft.EntityFrameworkCore;
using TimesheetSystem.Domain.Aggregates;
using TimesheetSystem.Domain.Entities;

namespace TimesheetSystem.Infrastructure.Repositories;

public class TimesheetRepository : ITimesheetRepository
{
    private readonly TimesheetDbContext _context;

    public TimesheetRepository(TimesheetDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddTimesheetEntryAsync(TimesheetEntry entry)
    {
        if (entry == null)
            throw new ArgumentNullException(nameof(entry));

        _context.TimesheetEntries.Add(entry);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TimesheetEntry>> GetAllTimesheetEntriesAsync()
    {
        return await _context.TimesheetEntries.ToListAsync();
    }
}