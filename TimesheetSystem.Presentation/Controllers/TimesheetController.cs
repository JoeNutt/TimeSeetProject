using MediatR;
using Microsoft.AspNetCore.Mvc;
using TimesheetSystem.Application.UseCases.Commands;
using TimesheetSystem.Application.UseCases.Queries;

namespace TimesheetSystem.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class TimesheetController : ControllerBase
{
    private readonly IMediator _mediator;

    public TimesheetController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("submit")]
    
    public async Task<IActionResult> SubmitTimesheetEntry([FromBody] SubmitTimesheetEntryCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _mediator.Send(command);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("export")]
    public async Task<IActionResult> ExportTimesheet()
    {
        var csv = await _mediator.Send(new GenerateCsvCommand()); 
        return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", "timesheet.csv");
    }
    
    [HttpGet("entries")]
    public async Task<IActionResult> GetAllEntries()
    {
        var entries = await _mediator.Send(new GetAllEntriesQuery());
        return Ok(entries);
    }
}