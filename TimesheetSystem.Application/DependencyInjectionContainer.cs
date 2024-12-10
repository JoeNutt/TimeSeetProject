using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TimesheetSystem.Application.UseCases.Commands;
using TimesheetSystem.Application.UseCases.Commands.Handlers;
using TimesheetSystem.Application.UseCases.Queries;
using TimesheetSystem.Application.UseCases.Queries.Handlers;
using TimesheetSystem.Domain.Abtractions;
using TimesheetSystem.Domain.Abtractions.Interfaces;
using TimesheetSystem.Domain.Aggregates;
using TimesheetSystem.Domain.Builders;
using TimesheetSystem.Domain.Entities;
using TimesheetSystem.Domain.Services;

namespace TimesheetSystem.Application;

public static class DependencyInjectionContainer
{
    public static IServiceCollection AddProjectDependencies(this IServiceCollection services)
{
services.AddScoped<IRequestHandler<SubmitTimesheetEntryCommand, Unit>, SubmitTimesheetEntryCommandHandler>();    services.AddScoped<IRequestHandler<GenerateCsvCommand, string>, GenerateCsvCommandHandler>();
    services.AddScoped<IRequestHandler<GetAllEntriesQuery, IEnumerable<TimesheetEntry>>, GetAllEntriesQueryHandler>(); // Corrected line
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SubmitTimesheetEntryCommand).Assembly));
    services.AddScoped<ITimeSheet, TimeSheet>();
    services.AddScoped<ITimesheetService, TimesheetService>();
    services.AddScoped<ICsvBuilder, CsvBuilder>();
    services.AddScoped<TimeSheet>(); // Register TimeSheet
    return services;
}
}