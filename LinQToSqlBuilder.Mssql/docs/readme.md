# LinQ to SQL Builder - SQL Server Provider

SQL Server (MSSQL) provider for the LinQ to SQL Builder library.

&copy; 2025 [DotNet Brightener](mailto:admin@dotnetbrightener.com)

![NuGet Version](https://img.shields.io/nuget/v/DotNetBrightener.LinQToSqlBuilder.Mssql)

## Overview

This package provides SQL Server-specific SQL generation for the LinQ to SQL Builder library. It implements the `ISqlAdapter` interface to generate SQL Server compatible queries with proper syntax for identifiers, pagination, and identity output.

### SQL Server-Specific Syntax

| Feature | SQL Server Syntax |
|---------|------------------|
| **Schema** | `[dbo]` |
| **Identifier quoting** | `[Table].[Column]` |
| **Pagination** | `OFFSET n ROWS FETCH NEXT m ROWS ONLY` |
| **Top N records** | `SELECT TOP(n)` |
| **Identity output** | `OUTPUT Inserted.[Id]` |

## Installation

```bash
dotnet add package DotNetBrightener.LinQToSqlBuilder.Mssql
```

This package automatically includes the core `DotNetBrightener.LinQToSqlBuilder` package as a dependency.

## Initialization

Initialize the SQL Server adapter once at application startup before using any `SqlBuilder` methods:

```csharp
using DotNetBrightener.LinQToSqlBuilder.Mssql;

// Call once at application startup (e.g., in Program.cs or Startup.cs)
SqlServerSqlBuilder.Initialize();
```

## Usage Examples

### Select Query

```csharp
var query = SqlBuilder.Select<User>()
                      .OrderBy(_ => _.RegistrationDate)
                      .Take(10);

// Generated SQL:
// SELECT TOP(10) [dbo].[User].* FROM [dbo].[User] ORDER BY [dbo].[User].[RegistrationDate]
```

### Select with Pagination

```csharp
var query = SqlBuilder.Select<User>()
                      .OrderBy(_ => _.RegistrationDate)
                      .Skip(20)
                      .Take(10);

// Generated SQL:
// SELECT [dbo].[User].* FROM [dbo].[User] 
// ORDER BY [dbo].[User].[RegistrationDate] 
// OFFSET 20 ROWS FETCH NEXT 10 ROWS ONLY
```

### Insert with Output Identity

```csharp
var query = SqlBuilder.Insert<UserGroup>(_ => new UserGroup
            {
                Name        = "Administrators",
                Description = "Admin group",
                IsDeleted   = false
            })
            .OutputIdentity();

// Generated SQL:
// INSERT INTO [dbo].[UserGroup] ([Name], [Description], [IsDeleted]) 
// OUTPUT Inserted.[Id] VALUES (@Param1, @Param2, @Param3)

var newId = Connection.ExecuteScalar<long>(query.CommandText, query.CommandParameters);
```

### Update

```csharp
var query = SqlBuilder.Update<User>(_ => new User
                                   {
                                       Email        = "new@email.com",
                                       ModifiedDate = DateTimeOffset.Now
                                   })
                      .Where(user => user.Id == userId);

// Generated SQL:
// UPDATE [dbo].[User] SET [Email] = @Param1, [ModifiedDate] = @Param2 
// WHERE [dbo].[User].[Id] = @Param3
```

### Delete

```csharp
var query = SqlBuilder.Delete<User>()
                      .Where(user => user.Email == userEmail);

// Generated SQL:
// DELETE FROM [dbo].[User] WHERE [dbo].[User].[Email] = @Param1
```

## Entity Configuration

Use `[Table]` and `[Column]` attributes from `System.ComponentModel.DataAnnotations.Schema` to configure entity mappings:

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Users", Schema = "dbo")]
public class User
{
    [Key]
    public long Id { get; set; }

    [Column("EmailAddress")]
    public string Email { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime RegistrationDate { get; set; }
}
```

## Integration with Dapper

This library works seamlessly with Dapper:

```csharp
using Dapper;
using DotNetBrightener.LinQToSqlBuilder;
using DotNetBrightener.LinQToSqlBuilder.Mssql;

// Initialize once at startup
SqlServerSqlBuilder.Initialize();

// Build and execute queries
var query = SqlBuilder.Select<User>()
                      .Where(u => u.IsActive)
                      .OrderBy(u => u.LastName)
                      .Take(100);

var users = connection.Query<User>(query.CommandText, query.CommandParameters);
```

## See Also

- [Core Library Documentation](https://www.nuget.org/packages/DotNetBrightener.LinQToSqlBuilder)
- [PostgreSQL Provider](https://www.nuget.org/packages/DotNetBrightener.LinQToSqlBuilder.Npgsql)

