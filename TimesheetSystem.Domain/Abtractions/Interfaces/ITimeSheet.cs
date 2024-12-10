using TimesheetSystem.Domain.Entities;

namespace TimesheetSystem.Domain.Abtractions.Interfaces;

public interface ITimeSheet
{
    /// <summary>
    /// Adds a new entry to the timesheet, ensuring that the total hours worked for a given day does not exceed 24.
    /// </summary>
    /// <param name="entry">The timesheet entry to be added.</param>
    /// <exception cref="ArgumentNullException">Thrown when the provided entry is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when adding the entry would cause the total hours for the date to exceed 24.</exception>
    void AddEntry(TimesheetEntry entry);

    /// <summary>
    /// Retrieves the total hours worked by a user for a specific date.
    /// </summary>
    /// <param name="userName">The name of the user whose daily summary is being retrieved.</param>
    /// <param name="date">The date for which the summary of hours worked is requested.</param>
    /// <returns>The total number of hours worked by the user on the specified date.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided user name is null, empty, or whitespace.</exception>
    decimal GetDailySummary(string userName, DateTime date);
    /// <summary>
    /// Retrieves all timesheet entries available in the current timesheet.
    /// </summary>
    /// <returns>A collection of <see cref="TimesheetEntry"/> representing all the entries in the timesheet.</returns>
    IEnumerable<TimesheetEntry> GetAllEntries();
}