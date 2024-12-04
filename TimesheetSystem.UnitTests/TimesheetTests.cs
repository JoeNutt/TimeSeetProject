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
            var entry = new TimesheetEntry("John Doe", DateTime.Today, "Project A", "Development", 8);

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
            var entry1 = new TimesheetEntry("John Doe", DateTime.Today, "Project A", "Development", 5);
            var entry2 = new TimesheetEntry("John Doe", DateTime.Today, "Project B", "Testing", 3);

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
                timesheet.AddEntry(new TimesheetEntry("John Doe", DateTime.Today, "Project A", "Development", 0)));

            Assert.Throws<ArgumentException>(() =>
                timesheet.AddEntry(new TimesheetEntry("John Doe", DateTime.Today, "Project A", "Development", -5)));

            Assert.Throws<ArgumentException>(() =>
                timesheet.AddEntry(new TimesheetEntry("John Doe", DateTime.Today, "Project A", "Development", 25)));
        }
        
        [Test]
        public void CreateTimePeriod_ValidHours_Success()
        {
            // Arrange & Act
            var timePeriod = new TimePeriod(8);

            // Assert
            Assert.AreEqual(8, timePeriod.HoursWorked);
        }

        [Test]
        public void CreateTimePeriod_InvalidHours_ThrowsArgumentException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => new TimePeriod(0));
            Assert.Throws<ArgumentException>(() => new TimePeriod(25));
        }
    } 
}