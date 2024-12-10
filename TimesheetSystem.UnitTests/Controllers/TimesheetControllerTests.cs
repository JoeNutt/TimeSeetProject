using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TimesheetSystem.Application.UseCases.Commands;
using TimesheetSystem.Presentation.Controllers;

namespace TimesheetSystem.UnitTests.Controllers;

[TestFixture]
public class TimesheetControllerTests
{
    private Mock<IMediator> _mediatorMock;
    private TimesheetController _controller;

    [SetUp]
    public void SetUp()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new TimesheetController(_mediatorMock.Object);

        // Default setup for Send (can be overridden in individual tests)
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<IRequest<object>>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<object>(null));
    }

    [Test]
    public async Task SubmitTimesheetEntry_ValidRequest_ReturnsOk()
    {
        // Arrange
        var command = new SubmitTimesheetEntryCommand(
            "John Smith", new DateTime(2014, 10, 22), "Project Alpha", "Developed new feature X", 4);

        // Act
        var result = await _controller.SubmitTimesheetEntry(command);

        // Assert
        Assert.IsInstanceOf<OkResult>(result);
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task ExportToCsv_ReturnsCsvData()
    {
        // Arrange
        var expectedCsv = "User Name\tDate\tProject\tDescription of Tasks\tHours Worked\tTotal Hours for the Day\n" +
                  "John Smith\t22/10/2014\tProject Alpha\tDeveloped new feature X\t4\t8\n" +
                  "John Smith\t22/10/2014\tProject Beta\tFixed bugs in module Y\t4\t8\n" +
                  "Jane Doe\t22/10/2014\tProject Gamma\tConducted user testing\t6\t6";
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GenerateCsvCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedCsv); // Override default setup for this specific command

        // Act
        var result = await _controller.ExportTimesheet(); // Use correct method name from your updated controller

        // Assert
        var fileResult = result as FileContentResult;
        Assert.That(fileResult, Is.Not.Null); // Ensure the cast was successful
        Assert.That(fileResult.ContentType, Is.EqualTo("text/csv"));
        Assert.That(fileResult.FileDownloadName, Is.EqualTo("timesheet.csv"));
        Assert.That(System.Text.Encoding.UTF8.GetString(fileResult.FileContents), Is.EqualTo(expectedCsv));
    }
}