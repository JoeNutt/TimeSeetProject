using Moq;
using TimesheetSystem.Application.UseCases.Commands;
using TimesheetSystem.Domain.Services;

namespace TimesheetSystem.UnitTests.UseCases;

[TestFixture]
public class GenerateCsvCommandHandlerTests
{
    private Mock<ITimesheetService> _timesheetServiceMock;
    private GenerateCsvCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        // Create a mock for ITimesheetService
        _timesheetServiceMock = new Mock<ITimesheetService>();

        // Create the handler using the mocked service
        _handler = new GenerateCsvCommandHandler(_timesheetServiceMock.Object);
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
            .Setup(service => service.ExportToCsv())
            .Returns(updatedCsvOutput);

        var command = new GenerateCsvCommand();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.AreEqual(updatedCsvOutput, result);
        _timesheetServiceMock.Verify(service => service.ExportToCsv(), Times.Once);
    }

    
    [Test]
    public void Handle_ShouldThrowArgumentNullException_WhenNoEntriesExist()
    {
        // Arrange
        _timesheetServiceMock
            .Setup(service => service.ExportToCsv())
            .Throws(new ArgumentNullException("No entries found in the timesheet."));

        var command = new GenerateCsvCommand();

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _handler.Handle(command, CancellationToken.None));

        Assert.That(ex.Message, Does.Contain("No entries found in the timesheet."));
        _timesheetServiceMock.Verify(service => service.ExportToCsv(), Times.Once);
    }
}