using System.Collections.Generic;
using System.Threading.Tasks;
using TimesheetSystem.Domain.Aggregates;
using TimesheetSystem.Domain.Entities;

public interface ITimesheetRepository
{
    Task AddTimesheetEntryAsync(TimesheetEntry entry);
    Task<IEnumerable<TimesheetEntry>> GetAllTimesheetEntriesAsync();
}