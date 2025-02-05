using TimesheetSystem.Domain.ValueObjects;

namespace TimesheetSystem.UnitTests
{
    using TimesheetSystem.Domain.Aggregates;
    using TimesheetSystem.Domain.Entities;

    [TestFixture]
    public class TimesheetTests
    {
        [Test]
        public void AddEntry_ValidEntry_UpdatesDailySummary()
        {
            // Arrange 
            var timesheet = new TimeSheet();
            var entry = TimesheetEntry.Create("John Doe", DateTime.Today, "Project A", "Development", 8);

            // Act 
            timesheet.AddEntry(entry);

            // Assert
            var totalHours = timesheet.GetDailySummary("John Doe", DateTime.Today);
            Assert.AreEqual(8, totalHours);
        }

        [Test]
        public void AddEntry_MultipleEntriesForSameDay_AggregatesTotalHours()
        {
            // Arrange
            var timesheet = new TimeSheet();
            var entry1 = TimesheetEntry.Create("John Doe", DateTime.Today, "Project A", "Development", 5);
            var entry2 = TimesheetEntry.Create("John Doe", DateTime.Today, "Project B", "Testing", 3);

            // Act
            timesheet.AddEntry(entry1);
            timesheet.AddEntry(entry2);

            // Assert
            var totalHours = timesheet.GetDailySummary("John Doe", DateTime.Today);
            Assert.AreEqual(8, totalHours);
        }

        [Test]
        public void AddEntry_InvalidHoursWorked_ThrowsArgumentException()
        {
            // Arrange
            var timesheet = new TimeSheet();

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                timesheet.AddEntry(TimesheetEntry.Create("John Doe", DateTime.Today, "Project A", "Development", 0)));

            Assert.Throws<ArgumentException>(() =>
                timesheet.AddEntry(TimesheetEntry.Create("John Doe", DateTime.Today, "Project A", "Development", -5)));

            Assert.Throws<ArgumentException>(() =>
                timesheet.AddEntry(TimesheetEntry.Create("John Doe", DateTime.Today, "Project A", "Development", 25)));
        }
        
    }
}