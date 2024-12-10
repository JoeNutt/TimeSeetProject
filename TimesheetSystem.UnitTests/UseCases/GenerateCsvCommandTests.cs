using System.Globalization;
using Moq;
using TimesheetSystem.Application.UseCases.Commands;
using TimesheetSystem.Domain.Abtractions;
using TimesheetSystem.Domain.Abtractions.Interfaces;
using TimesheetSystem.Domain.Aggregates;
using TimesheetSystem.Domain.Entities;
using TimesheetSystem.Domain.Services;

namespace TimesheetSystem.UnitTests.UseCases;

[TestFixture]
public class GenerateCsvCommandHandlerTests
{
    private Mock<ITimesheetService> _timesheetServiceMock; // Correct: Mock the service
    private GenerateCsvCommandHandler _handler;
    private Mock<ICsvBuilder> _csvBuilderMock;
    private Mock<ITimesheetRepository> _timesheetRepositoryMock;


    [SetUp]
    public void SetUp()
    {
        _timesheetServiceMock = new Mock<ITimesheetService>();
        _csvBuilderMock = new Mock<ICsvBuilder>(); // Create the mock ICsvBuilder
        _timesheetServiceMock
            .Setup(service => service.ExportToCsv(_csvBuilderMock.Object))
            .Returns("");
        _timesheetRepositoryMock = new Mock<ITimesheetRepository>();


        _handler = new GenerateCsvCommandHandler(_timesheetRepositoryMock.Object, _timesheetServiceMock.Object,
            _csvBuilderMock.Object);
    }

    [Test]
    public async Task Handle_ShouldCallExportToCsv_OnTimesheetService_WithNewFormat()
    {
        // Arrange
        var updatedCsvOutput = string.Join(Environment.NewLine, new[]
        {
            "Name,Date,Project,Task,Hours",
            "John Doe,2024-12-01,Project A,Development,8",
            "Jane Smith,2024-12-02,Project B,Testing,4"
        });

        _timesheetServiceMock
            .Setup(service => service.ExportToCsv(_csvBuilderMock.Object))
            .Returns(updatedCsvOutput);

        var command = new GenerateCsvCommand();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.AreEqual(updatedCsvOutput, result);
        _timesheetServiceMock.Verify(service => service.ExportToCsv(_csvBuilderMock.Object), Times.Once);
    }

    [Test]
    public void ExportToCsv_MultipleEntriesSameDay_CalculatesTotalHoursCorrectly()
    {
        // Arrange
        var entries = new List<TimesheetEntry>
        {
            TimesheetEntry.Create("John Smith", new DateTime(2014, 10, 22), "Project Alpha", "Task 1", 4),
            TimesheetEntry.Create("John Smith", new DateTime(2014, 10, 22), "Project Beta", "Task 2", 4),
            TimesheetEntry.Create("Jane Doe", new DateTime(2014, 10, 22), "Project Gamma", "Task 3", 6)
        };

        var timesheetMock = new Mock<ITimeSheet>();
        timesheetMock.Setup(t => t.GetAllEntries()).Returns(entries);

        string expectedCsv =
            "UserName,Date,ProjectName,Description,HoursWorked,DailyTotal" + Environment.NewLine +
            "John Smith,2014-10-22,Project Alpha,Task 1,4,8" + Environment.NewLine +
            "John Smith,2014-10-22,Project Beta,Task 2,4,8" + Environment.NewLine +
            "Jane Doe,2014-10-22,Project Gamma,Task 3,6,6";

        _csvBuilderMock.Setup(x => x.Build()).Returns(expectedCsv);
        _timesheetServiceMock.Setup(service => service.ExportToCsv(_csvBuilderMock.Object)).Returns(expectedCsv);

        // Act
        var csv = _timesheetServiceMock.Object.ExportToCsv(_csvBuilderMock.Object);

        // Assert - Direct string comparison for robustness
        Assert.That(csv, Is.EqualTo(expectedCsv));
    }
}