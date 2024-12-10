using TimesheetSystem.Application.UseCases.Queries.Handlers;

namespace TimesheetSystem.Application.UseCases.Queries;

// TimesheetSystem.Application/UseCases/Queries/GetAllEntriesQuery.cs
using MediatR;
using System.Collections.Generic;
using TimesheetSystem.Domain.Entities;

public class GetAllEntriesQuery : IRequest<IEnumerable<TimesheetEntry>>, IRequest<GetAllEntriesQueryHandler>
{
}