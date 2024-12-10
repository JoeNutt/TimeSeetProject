using Moq;
using TimesheetSystem.Domain.Abtractions;
using TimesheetSystem.Domain.Abtractions.Interfaces;
using TimesheetSystem.Domain.Aggregates;
using TimesheetSystem.Domain.Builders;
using TimesheetSystem.Domain.Entities;
using TimesheetSystem.Domain.Services;

namespace TimesheetSystem.UnitTests
{
    [TestFixture]
    public class TimesheetServiceTests
    {
        private Mock<ICsvBuilder> _csvBuilderMock;

        [SetUp]
        public void Setup()
        {
            _csvBuilderMock = new Mock<ICsvBuilder>();
        }
        
        [Test]
        public void ExportToCsv_ValidEntries_GeneratesCorrectCsv()
        {
            // Arrange
            var timesheet = new TimeSheet();
            var entry1 = TimesheetEntry.Create("John Doe", new DateTime(2024, 12, 1), "Project A", "Development", 8);
            var entry2 = TimesheetEntry.Create("John Doe", new DateTime(2024, 12, 1), "Project B", "Testing", 4);
            var entry3 = TimesheetEntry.Create("Jane Smith", new DateTime(2024, 12, 2), "Project C", "Design", 6);

            timesheet.AddEntry(entry1);
            timesheet.AddEntry(entry2);
            timesheet.AddEntry(entry3);

            var service = new TimesheetService();
            service.SetEntries(new List<TimesheetEntry> { entry1, entry2, entry3 });

            _csvBuilderMock.Setup(x => x.AddHeader(It.IsAny<string[]>()));
            _csvBuilderMock.Setup(x => x.AddRow(It.IsAny<string[]>()));
            _csvBuilderMock.Setup(x => x.Build()).Returns(
                "UserName,Date,ProjectName,Description,HoursWorked,DailyTotal" + Environment.NewLine +
                "John Doe,2024-12-01,Project A,Development,8,12" + Environment.NewLine +
                "John Doe,2024-12-01,Project B,Testing,4,12" + Environment.NewLine +
                "Jane Smith,2024-12-02,Project C,Design,6,6" + Environment.NewLine
            );

            // Act
            var csvContent = service.ExportToCsv(_csvBuilderMock.Object);

            // Assert
            var lines = csvContent.Split(Environment.NewLine);

            // Check headers
            Assert.That(lines[0], Is.EqualTo("UserName,Date,ProjectName,Description,HoursWorked,DailyTotal"));

            // Check rows
            Assert.IsTrue(lines[1].Contains("John Doe,2024-12-01,Project A,Development,8,12"));
            Assert.IsTrue(lines[2].Contains("John Doe,2024-12-01,Project B,Testing,4,12"));
            Assert.IsTrue(lines[3].Contains("Jane Smith,2024-12-02,Project C,Design,6,6"));
        }

        [Test]
        public void ExportToCsv_NoEntries_ReturnsOnlyHeaders()
        {
            // Arrange
            var service = new TimesheetService();
            service.SetEntries(Enumerable.Empty<TimesheetEntry>());

            _csvBuilderMock.Setup(x => x.AddHeader(It.IsAny<string[]>())); // Important setup
            _csvBuilderMock.Setup(x => x.Build())
                .Returns("UserName,Date,ProjectName,Description,HoursWorked,DailyTotal"); // Setup return

            // Act
            var csvContent = service.ExportToCsv(_csvBuilderMock.Object);

            // Assert
            Assert.That(csvContent.Trim(), Is.EqualTo("UserName,Date,ProjectName,Description,HoursWorked,DailyTotal"));
        }
    }
}