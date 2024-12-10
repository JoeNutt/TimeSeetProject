using TimesheetSystem.Domain.Entities;

namespace TimesheetSystem.Domain.Abtractions.Interfaces;

public interface ITimesheetService
{
    /// <summary>
    /// Exports timesheet entries into a CSV formatted string using the provided CSV builder.
    /// </summary>
    /// <param name="csvBuilder">An instance of <see cref="ICsvBuilder"/> used to construct the CSV output.</param>
    /// <returns>A string representing the timesheet data formatted as CSV.</returns>
    string ExportToCsv(ICsvBuilder csvBuilder);

    /// <summary>
    /// Sets the collection of timesheet entries to be used in subsequent operations.
    /// </summary>
    /// <param name="entries">A collection of <see cref="TimesheetEntry"/> objects representing timesheet data.</param>
    void SetEntries(IEnumerable<TimesheetEntry> entries);
}