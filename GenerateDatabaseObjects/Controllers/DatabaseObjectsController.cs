using GenerateDatabaseObjects.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenerateDatabaseObjects.Controllers;
// Controllers/DatabaseObjectsController.cs
[ApiController]
[Route("api/[controller]")]
public class DatabaseObjectsController : ControllerBase
{
    private readonly IDatabaseObjectsService _dbObjectsService;

    public DatabaseObjectsController(IDatabaseObjectsService dbObjectsService)
    {
        _dbObjectsService = dbObjectsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDatabaseObjects()
    {
        var (procedures, functions) = await _dbObjectsService.GetDatabaseObjects();

        return Ok(new
        {
            StoredProcedures = procedures,
            Functions = functions
        });
    }
}