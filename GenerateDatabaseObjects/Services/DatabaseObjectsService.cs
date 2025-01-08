using Npgsql;

namespace GenerateDatabaseObjects.Services;

public class DatabaseObjectsService : IDatabaseObjectsService
{
    private readonly string _connectionString;

    public DatabaseObjectsService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<(List<string> Procedures, List<string> Functions)> GetDatabaseObjects()
    {
        var procedures = new List<string>();
        var functions = new List<string>();

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        // Get Stored Procedures
        var procCommand = new NpgsqlCommand(@"
            SELECT routine_name 
            FROM information_schema.routines 
            WHERE routine_type = 'PROCEDURE' 
            AND routine_schema = 'public'", conn);

        using (var reader = await procCommand.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                procedures.Add(reader.GetString(0));
            }
        }

        // Get Functions
        var funcCommand = new NpgsqlCommand(@"
            SELECT routine_name 
            FROM information_schema.routines 
            WHERE routine_type = 'FUNCTION' 
            AND routine_schema = 'public'", conn);

        using (var reader = await funcCommand.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                functions.Add(reader.GetString(0));
            }
        }

        return (procedures, functions);
    }
}