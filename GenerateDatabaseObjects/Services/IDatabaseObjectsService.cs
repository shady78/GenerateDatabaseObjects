namespace GenerateDatabaseObjects.Services;

public interface IDatabaseObjectsService
{
    Task<(List<string> Procedures, List<string> Functions)> GetDatabaseObjects();
}