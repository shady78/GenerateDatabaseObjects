using System;
using System.Collections.Generic;

namespace GenerateDatabaseObjects.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Rate { get; set; }

    public string TenantId { get; set; } = null!;
}
