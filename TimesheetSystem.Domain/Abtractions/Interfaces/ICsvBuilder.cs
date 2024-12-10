namespace TimesheetSystem.Domain.Abtractions.Interfaces;

public interface ICsvBuilder
{
    /// <summary>
    /// Adds a header row to the CSV data.
    /// </summary>
    /// <param name="headers">An array of strings representing the column headers to be added to the CSV.</param>
    void AddHeader(params string[] headers);

    /// <summary>
    /// Adds a data row to the CSV data.
    /// </summary>
    /// <param name="values">An array of strings representing the values of the row to be added to the CSV.</param>
    void AddRow(params string[] values);

    /// <summary>
    /// Builds and returns the resulting CSV data as a string.
    /// </summary>
    /// <returns>A string containing the generated CSV content.</returns>
    string Build();
}