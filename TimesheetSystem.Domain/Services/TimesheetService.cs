using System.Globalization;
using System.Text;
using TimesheetSystem.Domain.Aggregates;

namespace TimesheetSystem.Domain.Services
{
    public class TimesheetService
    {
        private readonly TimeSheet _timesheet;

        public TimesheetService(TimeSheet timesheet)
        {
            _timesheet = timesheet ?? throw new ArgumentNullException(nameof(timesheet));
        }

        public string ExportToCsv()
        {
            var entries = _timesheet.GetAllEntries();

            if (!entries.Any())
                return "UserName,Date,ProjectName,Description,HoursWorked,DailyTotal";

            var dailyTotals = entries
                .GroupBy(e => new { e.UserName, e.Date })
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(e => e.HoursWorked.HoursWorked)
                );

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("UserName,Date,ProjectName,Description,HoursWorked,DailyTotal");

            foreach (var entry in entries)
            {
                var dailyTotal = dailyTotals[new { entry.UserName, entry.Date }];
                csvBuilder.AppendLine(string.Join(",", new[]
                {
                    entry.UserName,
                    entry.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    entry.ProjectName,
                    entry.Description,
                    entry.HoursWorked.HoursWorked.ToString(CultureInfo.InvariantCulture),
                    dailyTotal.ToString(CultureInfo.InvariantCulture)
                }));
            }

            return csvBuilder.ToString();
        }
    }
}