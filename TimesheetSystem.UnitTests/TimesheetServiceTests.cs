using TimesheetSystem.Domain.Aggregates;
using TimesheetSystem.Domain.Entities;
using TimesheetSystem.Domain.Services;

namespace TimesheetSystem.UnitTests
{
    [TestFixture]
    public class TimesheetServiceTests
    {
        [Test]
        public void ExportToCsv_ValidEntries_GeneratesCorrectCsv()
        {
            // Arrange
            var timesheet = new TimeSheet();
            var entry1 = new TimesheetEntry("John Doe", new DateTime(2024, 12, 1), "Project A", "Development", 8);
            var entry2 = new TimesheetEntry("John Doe", new DateTime(2024, 12, 1), "Project B", "Testing", 4);
            var entry3 = new TimesheetEntry("Jane Smith", new DateTime(2024, 12, 2), "Project C", "Design", 6);

            timesheet.AddEntry(entry1);
            timesheet.AddEntry(entry2);
            timesheet.AddEntry(entry3);

            var service = new TimesheetService(timesheet);

            // Act
            var csvContent = service.ExportToCsv();

            // Assert
            var lines = csvContent.Split(Environment.NewLine);

            // Check headers
            Assert.AreEqual("UserName,Date,ProjectName,Description,HoursWorked,DailyTotal", lines[0]);

            // Check rows
            Assert.IsTrue(lines[1].Contains("John Doe,2024-12-01,Project A,Development,8,12"));
            Assert.IsTrue(lines[2].Contains("John Doe,2024-12-01,Project B,Testing,4,12"));
            Assert.IsTrue(lines[3].Contains("Jane Smith,2024-12-02,Project C,Design,6,6"));
        }

        [Test]
        public void ExportToCsv_NoEntries_ReturnsOnlyHeaders()
        {
            // Arrange
            var timesheet = new TimeSheet();
            var service = new TimesheetService(timesheet);

            // Act
            var csvContent = service.ExportToCsv();

            // Assert
            Assert.AreEqual("UserName,Date,ProjectName,Description,HoursWorked,DailyTotal", csvContent.Trim());
        }
    }
}
