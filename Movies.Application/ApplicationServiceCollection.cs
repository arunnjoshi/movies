using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Repository;

namespace Movies.Application;

public static class ApplicationServiceCollection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRepository, MovieRepository>();
        return services;
    }
}
