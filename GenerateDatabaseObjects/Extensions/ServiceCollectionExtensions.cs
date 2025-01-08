using GenerateDatabaseObjects.Services;

namespace GenerateDatabaseObjects.Extensions;

// Extensions/ServiceCollectionExtensions.cs
public static class ServiceCollectionExtensions
{
    public static async Task GenerateDatabaseObjects(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "Constants");

        // Create directory if it doesn't exist
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        var generator = new DatabaseObjectGenerator(connectionString!, outputPath);
        await generator.GenerateDatabaseObjects();
    }
}