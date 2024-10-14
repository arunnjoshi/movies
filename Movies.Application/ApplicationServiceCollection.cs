using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Database;
using Movies.Application.Repository;

namespace Movies.Application;

public static class ApplicationServiceCollection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRepository, MovieRepository>();
        return services;
    }

	public static IServiceCollection AddDatabase(this IServiceCollection services,string connectionString)
    {
		services.AddSingleton<IDbConnectionFactory >(_=>new DbConnectionFactory(connectionString));
		services.AddSingleton<DbInitializer>();
		return services;
	}
}
