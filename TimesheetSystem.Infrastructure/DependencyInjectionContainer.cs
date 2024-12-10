using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TimesheetSystem.Infrastructure.Repositories;

namespace TimesheetSystem.Infrastructure;

public static class DependencyInjectionContainer
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
    {
        services.AddDbContext<TimesheetDbContext>(options => 
            options.UseInMemoryDatabase("TimesheetDatabase"));

        services.AddScoped<ITimesheetRepository, TimesheetRepository>();

        return services;
    }
}