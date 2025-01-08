using Npgsql;
using System.Text;

namespace GenerateDatabaseObjects.Services;

// Services/DatabaseObjectGenerator.cs
public class DatabaseObjectGenerator
{
    private readonly string _connectionString;
    private readonly string _outputPath;

    public DatabaseObjectGenerator(string connectionString, string outputPath)
    {
        _connectionString = connectionString;
        _outputPath = outputPath;
    }

    public async Task GenerateDatabaseObjects()
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var procedures = await GetStoredProcedures(conn);
        var functions = await GetFunctions(conn);

        await GenerateStoredProceduresClass(procedures);
        await GenerateFunctionsClass(functions);
    }

    private async Task<Dictionary<string, List<string>>> GetStoredProcedures(NpgsqlConnection conn)
    {
        var procedures = new Dictionary<string, List<string>>();

        const string sql = @"
            SELECT routine_name 
            FROM information_schema.routines 
            WHERE routine_type = 'PROCEDURE' 
            AND routine_schema = 'public'";

        using var cmd = new NpgsqlCommand(sql, conn);
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var name = reader.GetString(0);
            var category = GetCategory(name);

            if (!procedures.ContainsKey(category))
                procedures[category] = new List<string>();

            procedures[category].Add(name);
        }

        return procedures;
    }

    private async Task<Dictionary<string, List<string>>> GetFunctions(NpgsqlConnection conn)
    {
        var functions = new Dictionary<string, List<string>>();

        const string sql = @"
            SELECT routine_name 
            FROM information_schema.routines 
            WHERE routine_type = 'FUNCTION' 
            AND routine_schema = 'public'";

        using var cmd = new NpgsqlCommand(sql, conn);
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var name = reader.GetString(0);
            var category = GetCategory(name);

            if (!functions.ContainsKey(category))
                functions[category] = new List<string>();

            functions[category].Add(name);
        }

        return functions;
    }

    private string GetCategory(string name)
    {
        // Assuming naming convention: category_action_name
        // Example: product_get_by_id -> Products
        var parts = name.Split('_');
        if (parts.Length > 0)
        {
            return char.ToUpper(parts[0][0]) + parts[0].Substring(1) + "s";
        }
        return "Common";
    }

    private async Task GenerateStoredProceduresClass(Dictionary<string, List<string>> procedures)
    {
        var sb = new StringBuilder();
        sb.AppendLine("public static class StoredProcedures");
        sb.AppendLine("{");

        foreach (var category in procedures)
        {
            sb.AppendLine($"    public static class {category.Key}");
            sb.AppendLine("    {");

            foreach (var proc in category.Value)
            {
                var constantName = ToCamelCase(proc.Replace(category.Key.ToLower().TrimEnd('s') + "_", ""));
                sb.AppendLine($"        public const string {constantName} = \"{proc}\";");
            }

            sb.AppendLine("    }");
            sb.AppendLine();
        }

        sb.AppendLine("}");

        await File.WriteAllTextAsync(Path.Combine(_outputPath, "StoredProcedures.cs"), sb.ToString());
    }

    private async Task GenerateFunctionsClass(Dictionary<string, List<string>> functions)
    {
        var sb = new StringBuilder();
        sb.AppendLine("public static class Functions");
        sb.AppendLine("{");

        foreach (var category in functions)
        {
            sb.AppendLine($"    public static class {category.Key}");
            sb.AppendLine("    {");

            foreach (var func in category.Value)
            {
                var constantName = ToCamelCase(func.Replace(category.Key.ToLower().TrimEnd('s') + "_", ""));
                sb.AppendLine($"        public const string {constantName} = \"{func}\";");
            }

            sb.AppendLine("    }");
            sb.AppendLine();
        }

        sb.AppendLine("}");

        await File.WriteAllTextAsync(Path.Combine(_outputPath, "Functions.cs"), sb.ToString());
    }

    private string ToCamelCase(string name)
    {
        var parts = name.Split('_');
        var result = new StringBuilder();

        foreach (var part in parts)
        {
            if (string.IsNullOrEmpty(part)) continue;
            result.Append(char.ToUpper(part[0]) + part.Substring(1).ToLower());
        }

        return result.ToString();
    }
}