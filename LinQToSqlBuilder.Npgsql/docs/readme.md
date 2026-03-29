# LinQ to SQL Builder - PostgreSQL Provider

Copyright © 2017 - 2026 Vampire Coder (formerly DotnetBrightener)

PostgreSQL (Npgsql) provider for the LinQ to SQL Builder library.


![NuGet Version](https://img.shields.io/nuget/v/DotNetBrightener.LinQToSqlBuilder.Npgsql)

## Overview

This package provides PostgreSQL-specific SQL generation for the LinQ to SQL Builder library. It implements the `ISqlAdapter` interface to generate PostgreSQL compatible queries with proper syntax for identifiers, pagination, and identity output.

### PostgreSQL-Specific Syntax

| Feature | PostgreSQL Syntax |
|---------|------------------|
| **Schema** | `"public"` |
| **Identifier quoting** | `"Table"."Column"` |
| **Pagination** | `LIMIT n OFFSET m` |
| **Top N records** | `SELECT ... LIMIT n` |
| **Identity output** | `RETURNING "Id"` |

### SQL Server vs PostgreSQL Comparison

| Feature | SQL Server | PostgreSQL |
|---------|------------|------------|
| **Identifier quoting** | `[Table].[Column]` | `"Table"."Column"` |
| **Schema** | `[dbo]` | `"public"` |
| **Pagination** | `OFFSET n ROWS FETCH NEXT m ROWS ONLY` | `LIMIT m OFFSET n` |
| **Top N** | `SELECT TOP(n)` | `SELECT ... LIMIT n` |
| **Identity output** | `OUTPUT Inserted.[Id]` | `RETURNING "Id"` |

## Installation

```bash
dotnet add package DotNetBrightener.LinQToSqlBuilder.Npgsql
```

This package automatically includes the core `DotNetBrightener.LinQToSqlBuilder` package as a dependency.

## Initialization

Initialize the PostgreSQL adapter once at application startup before using any `SqlBuilder` methods:

```csharp
using DotNetBrightener.LinQToSqlBuilder.Npgsql;

// Call once at application startup (e.g., in Program.cs or Startup.cs)
NpgsqlSqlBuilder.Initialize();
```

## Usage Examples

### Select Query

```csharp
var query = SqlBuilder.Select<User>()
                      .OrderBy(_ => _.RegistrationDate)
                      .Take(10);

// Generated SQL:
// SELECT "public"."User".* FROM "public"."User"
// ORDER BY "public"."User"."RegistrationDate" LIMIT 10
```

### Select with Pagination

```csharp
var query = SqlBuilder.Select<User>()
                      .OrderBy(_ => _.RegistrationDate)
                      .Skip(20)
                      .Take(10);

// Generated SQL:
// SELECT "public"."User".* FROM "public"."User"
// ORDER BY "public"."User"."RegistrationDate" LIMIT 10 OFFSET 20
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
// INSERT INTO "public"."UserGroup" ("Name", "Description", "IsDeleted")
// VALUES (@Param1, @Param2, @Param3) RETURNING "Id"

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
// UPDATE "public"."User" SET "Email" = @Param1, "ModifiedDate" = @Param2
// WHERE "public"."User"."Id" = @Param3
```

### Delete

```csharp
var query = SqlBuilder.Delete<User>()
                      .Where(user => user.Email == userEmail);

// Generated SQL:
// DELETE FROM "public"."User" WHERE "public"."User"."Email" = @Param1
```

## Entity Configuration

Use `[Table]` and `[Column]` attributes from `System.ComponentModel.DataAnnotations.Schema` to configure entity mappings:

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("users", Schema = "public")]
public class User
{
    [Key]
    public long Id { get; set; }

    [Column("email_address")]
    public string Email { get; set; }

    [Column("first_name")]
    public string FirstName { get; set; }

    [Column("last_name")]
    public string LastName { get; set; }

    [Column("registration_date")]
    public DateTime RegistrationDate { get; set; }
}
```

> **Note**: PostgreSQL conventionally uses lowercase table and column names with underscores (snake_case). Use the `[Table]` and `[Column]` attributes to map your C# PascalCase properties to PostgreSQL snake_case names.

## Integration with Dapper and Npgsql

This library works seamlessly with Dapper and the Npgsql ADO.NET provider:

```csharp
using Dapper;
using Npgsql;
using DotNetBrightener.LinQToSqlBuilder;
using DotNetBrightener.LinQToSqlBuilder.Npgsql;

// Initialize once at startup
NpgsqlSqlBuilder.Initialize();

// Create connection
using var connection = new NpgsqlConnection(connectionString);

// Build and execute queries
var query = SqlBuilder.Select<User>()
                      .Where(u => u.IsActive)
                      .OrderBy(u => u.LastName)
                      .Take(100);

var users = connection.Query<User>(query.CommandText, query.CommandParameters);
```

## See Also

- [Core Library Documentation](https://www.nuget.org/packages/DotNetBrightener.LinQToSqlBuilder)
- [SQL Server Provider](https://www.nuget.org/packages/DotNetBrightener.LinQToSqlBuilder.Mssql)

