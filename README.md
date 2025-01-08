# Generate Database Objects API:
A .NET 8 project that automatically generates constants for PostgreSQL functions and stored procedures, making database operations more maintainable and type-safe.

## 🎯 The Problem
When working with a Database-First approach in .NET, handling database functions and stored procedures presents several challenges:

### Common Solutions and Their Issues:

1. **Manual String Management in Code:**
   ```csharp
   // Problems:
   // - Prone to typos
   // - No IntelliSense
   // - Hard to maintain
   public async Task<Product> GetProduct(int id)
   {
       return await _context.Products.FromSqlRaw("SELECT * FROM get_product_by_id", id);
   }
   ```
2. **Static String Constants in Classes:**
  ```csharp
    // Problems:
      // - Manual updates needed
      // - No categorization
      // - No relationship between objects
      public static class DatabaseObjects
      {
      public const string GetProduct = "get_product_by_id";
      public const string AddProduct = "add_product";
      // Hundreds of lines like this...
      }
  ```
3. **Resource Files (.resx):**
    ```csharp
    // Problems:
      // - Complex to maintain
      // - No proper structure
      // - Hard to track usage
      public void UpdateProduct()
      {
          var procName = Resources.UpdateProductProcedure;
          // What if the resource name changes?
      }
    ```
4. **Enums with String Conversion:**
   ```csharp
   // Problems:
    // - Limited flexibility
    // - Extra conversion logic needed
    // - Doesn't match database naming
    public enum StoredProcedures
    {
        GetProduct,
        AddProduct
    }
    // Needs conversion logic
    public string GetProcedureName(StoredProcedures proc) { ... }
   ```
   5. **Configuration Files:**
   ```csharp
       // Problems:
      // - No type safety
      // - Runtime errors instead of compile-time
      // - Hard to track usage
      {
        "DatabaseObjects": {
          "Procedures": {
            "GetProduct": "get_product_by_id"
          }
        }
      }
   ```
   6. **Direct Database Queries:**
   ```csharp
    // Problems:
    // - Performance overhead
    // - Extra database calls
    // - No caching
    public async Task<string> GetProcedureName()
    {
        var sql = "SELECT routine_name FROM information_schema.routines";
        // Query database every time
    }
   ```
   7. **Attribute-Based Mapping:**
   ```csharp
     // Problems:
    // - Complex setup
    // - Hard to maintain
    // - Limited flexibility
    [StoredProcedure("get_product_by_id")]
    public class GetProductProcedure { }
   ```
   8. **Code Generation Tools:**
   ```csharp
      // Problems:
    // - Complex setup
    // - Extra build steps
    // - Tool dependencies
    // - Limited customization
    [GeneratedCode("DatabaseToolkit", "1.0")]
    public partial class DatabaseObjects { }
   ```
   9. **Service Layer Abstraction:**
   ```csharp
     // Problems:
    // - Extra abstraction layer
    // - No direct mapping
    // - Maintenance overhead
    public interface IDatabaseObjectProvider
    {
        string GetProcedureName(string key);
    }
   ```
   10. **Hard-Coded Constants in Repository:**
   ```csharp
      // Problems:
    // - Scattered across codebase
    // - No centralized management
    // - Duplicate names
    public class ProductRepository
    {
        private const string GetProductProc = "get_product_by_id";
        private const string AddProductProc = "add_product";
    }
   ```
💡 Our Solution
  This project provides an automated, strongly-typed way to handle database objects:
  1.   Automatic Generation:
     ```csharp
      public static class Functions
      {
          public static class Gets
          {
              public const string ProductById = "get_product_by_id";
              public const string ProductsByTenant = "get_products_by_tenant";
          }
      }
     ```
  2.   Structured Organization:
     -  Categorized by operation type (Gets, Adds, Updates, etc.)
     -  Maintains database naming conventions
     -  Easy to understand hierarchy
    
  3.   Type-Safe Usage:
     ``` csharp
        var result = await _context.Products
    .FromSqlRaw($"SELECT * FROM {Functions.Gets.ProductById}(@p1)",
        new NpgsqlParameter("p1", id));
     ```

Benefits Over Common Solutions:

-  ✅ Compile-time checking
-  ✅ IntelliSense support
-  ✅ Automatic updates
-  ✅ Proper categorization
-  ✅ Easy maintenance
-  ✅ No runtime overhead
-  ✅ No extra dependencies
-  ✅ Centralized management
-  ✅ Clear structure
-  ✅ Easy to track usage


## 🛠️ Built With

- .NET 8
- PostgreSQL
- Entity Framework Core
- Clean Architecture

## 📋 Features

- Automatic generation of database object constants
- Multi-tenant support
- Database-first approach 
- Type-safe database operations
- RESTful API endpoints
- Proper error handling
- Swagger documentation


## 📝 Database Schema
```sql
CREATE TABLE IF NOT EXISTS public."Products"
(
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Name" text COLLATE pg_catalog."default" NOT NULL,
    "Description" text COLLATE pg_catalog."default" NOT NULL,
    "Rate" integer NOT NULL,
    "TenantId" text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "PK_Products" PRIMARY KEY ("Id")
)```

📚 Generated Constants Example:
public static class Functions
{
    public static class Gets
    {
        public const string ProductDetails = "get_product_details";
        public const string ProductById = "get_product_by_id";
        public const string ProductsByTenant = "get_products_by_tenant";
    }
    public static class Searchs
    {
        public const string Products = "search_products";
    }
}

public static class StoredProcedures
{
    public static class Adds
    {
        public const string Product = "add_product";
    }
    public static class Updates
    {
        public const string Product = "update_product";
    }
    public static class Deletes
    {
        public const string Product = "delete_product";
    }
}
```
🔧 Installation

1- Clone the repository:
``` bash
git clone https://github.com/yourusername/GenerateDatabaseObjects.git
```
2- Update the connection string in appsettings.json
```bash
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=your_database;Username=your_username;Password=your_password"
  }
}
```
3- Run the database migrations:
```
if you use Visual studio:
   - open Package manager console and write this command update-database
if you use Visual studio code:
  -  open termenal and write this commant: dotnet ef database update
```
4- Run the application

💡 Key Benefits

- Type-safe database operations
- Automatic constant generation
- Reduced maintenance overhead
- Centralized database object naming
- Easy to track database object usage

🤝 Contributing
Contributions, issues, and feature requests are welcome!
📫 Contact
Shady Khalifa - shadykhalifa.dotnetdev@gmail.com
📄 License
This project is MIT licensed.
🙏 Acknowledgements

.NET Documentation
PostgreSQL Documentation
Entity Framework Core
