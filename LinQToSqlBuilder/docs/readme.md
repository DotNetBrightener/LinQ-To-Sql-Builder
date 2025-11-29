# LinQ to SQL Builder - Core Library

A small .NET library that supports creating SQL queries and commands in a strongly typed fashion using LINQ lambda expressions.

&copy; 2025 [DotNet Brightener](mailto:admin@dotnetbrightener.com)

![NuGet Version](https://img.shields.io/nuget/v/DotNetBrightener.LinQToSqlBuilder)

## Overview

This is the **core library** that provides the base infrastructure for LinQ to SQL Builder. It contains the abstract adapter interfaces and common query building logic that database-specific providers extend.

### Architecture

LinQ to SQL Builder uses a **provider-based architecture**:

- **Core Library** (`DotNetBrightener.LinQToSqlBuilder`) - Contains base classes, interfaces, and query building logic
- **Database Providers** - Implement database-specific SQL syntax:
  - [`DotNetBrightener.LinQToSqlBuilder.Mssql`](https://www.nuget.org/packages/DotNetBrightener.LinQToSqlBuilder.Mssql) - SQL Server provider
  - [`DotNetBrightener.LinQToSqlBuilder.Npgsql`](https://www.nuget.org/packages/DotNetBrightener.LinQToSqlBuilder.Npgsql) - PostgreSQL provider

> **Important**: You must install and initialize a database-specific provider before using the `SqlBuilder` class. The core library alone does not generate valid SQL for any specific database.

## Installation

Install the core library along with your preferred database provider:

### For SQL Server
```bash
dotnet add package DotNetBrightener.LinQToSqlBuilder.Mssql
```

### For PostgreSQL
```bash
dotnet add package DotNetBrightener.LinQToSqlBuilder.Npgsql
```

## Initialization

Before using any `SqlBuilder` methods, you must initialize the appropriate database adapter once at application startup:

### SQL Server
```csharp
using DotNetBrightener.LinQToSqlBuilder.Mssql;

// Call once at application startup
SqlServerSqlBuilder.Initialize();
```

### PostgreSQL
```csharp
using DotNetBrightener.LinQToSqlBuilder.Npgsql;

// Call once at application startup
NpgsqlSqlBuilder.Initialize();
```

## Usage

#### Simple Select

This basic example queries the database for 10 User and order them by their registration date using **Dapper**:
```csharp
var query = SqlBuilder.Select<User>()
                      .OrderBy(_ => _.RegistrationDate)
                      .Take(10);
                      
var results = Connection.Query<User>(query.CommandText, query.CommandParameters);
```

As you can see the CommandText property will return the SQL string itself, while the CommandParameters property refers to a dictionary of SQL parameters. 

#### Select query with Join
The below example performs a query to the User table, and join it with UserGroup table to returns a many to many relationship mapping specified using **Dapper** mapping API

```csharp
var query = SqlBuilder.Select<User>()
                    //.Where(user => user.Email == email)
                      .Join<UserUserGroup>((@user, @group) => user.Id == group.UserId)
                      .Join<UserGroup>((group,     g) => group.UserGroupId == g.Id)
                      .Where(group => group.Id == groupId);

var result = new Dictionary<long, User>();
var results = Connection.Query<User, UserGroup, User>(query.CommandText,
                                                        (user, group) =>
                                                        {
                                                            if (!result.ContainsKey(user.Id))
                                                            {
                                                                user.Groups = new List<UserGroup>();
                                                                result.Add(user.Id, user);
                                                            }

                                                            result[user.Id].Groups.Add(group);
                                                            return user;
                                                        },
                                                        query.CommandParameters,
                                                        splitOn: "UserId,UserGroupId")
                        .ToList();
```


#### Insert single record

The example below will generate an insert command with one record.

```csharp
var query = SqlBuilder.Insert<UserGroup>(_ => new UserGroup
            {
                CreatedBy   = "TestSystem",
                CreatedDate = DateTimeOffset.Now,
                Description = "Created from Test System",
                Name        = "TestUserGroup",
                IsDeleted   = false
            });

var results = Connection.Execute(query.CommandText, query.CommandParameters);
```

#### Insert with Output Identity

When inserting a record, you can retrieve the auto-generated identity value using `OutputIdentity()`:

```csharp
var query = SqlBuilder.Insert<UserGroup>(_ => new UserGroup
            {
                CreatedBy   = "TestSystem",
                CreatedDate = DateTimeOffset.Now,
                Description = "Created from Test System",
                Name        = "TestUserGroup",
                IsDeleted   = false
            })
            .OutputIdentity();

// SQL Server generates: INSERT INTO ... OUTPUT Inserted.[Id] VALUES ...
// PostgreSQL generates: INSERT INTO ... VALUES ... RETURNING "Id"

var newId = Connection.ExecuteScalar<long>(query.CommandText, query.CommandParameters);
```

#### Insert multiple records

The example below will generate an insert command with multiple records.

```csharp
var query = SqlBuilder.InsertMany<UserGroup>(_ => new []
            {
                new UserGroup
                {
                    CreatedBy   = "TestSystem",
                    CreatedDate = DateTimeOffset.Now,
                    Description = "Created from Test System",
                    Name        = "TestUserGroup",
                    IsDeleted   = false
                },

                new UserGroup
                {
                    CreatedBy   = "TestSystem",
                    CreatedDate = DateTimeOffset.Now,
                    Description = "Created from Test System",
                    Name        = "TestUserGroup2",
                    IsDeleted   = false
                },

                new UserGroup
                {
                    CreatedBy   = "TestSystem",
                    CreatedDate = DateTimeOffset.Now,
                    Description = "Created from Test System",
                    Name        = "TestUserGroup3",
                    IsDeleted   = false
                }
            });

var results = Connection.Execute(query.CommandText, query.CommandParameters);
```

#### Insert by copying from another table

Sometimes we need to copy a bunch of records from one table to another. For instance, if we have an order that contains few products, and the quantity of the products are being updated before the order gets finalized. So we need to keep the inventory history records of all products that are being updated from time to time. Using Entity Framework, we could have loaded all inventory records of the specified products, then create a copied object and insert them to the inventory history. The more products you have, the slower performance you will suffer because you will have to deal with the data that are in memory versus the data that are being processed by other request(s).

```csharp
var query = SqlBuilder.InsertFrom<Inventory, InventoryHistory>(inventory => new InventoryHistory()
                                   {
                                       CreatedBy        = "Cloning System",
                                       CreatedDate      = DateTimeOffset.Now,
                                       StockQuantity    = inventory.StockQuantity,
                                       ReservedQuantity = inventory.ReservedQuantity,
                                       IsDeleted        = inventory.IsDeleted,
                                       InventoryId      = inventory.Id,
                                       ProductId        = inventory.ProductId
                                   })
                                  .WhereIsIn(inventory => inventory.ProductId, new long[] { /*... obmited values, describes the list of product ids */ });

Assert.AreEqual("INSERT INTO [dbo].[InventoryHistory] ([CreatedBy], [CreatedDate], [StockQuantity], [ReservedQuantity], [IsDeleted], [InventoryId], [ProductId]) " +
                "SELECT " +
                "@Param1 as [CreatedBy], " +
                "@Param2 as [CreatedDate], " +
                "[dbo].[Inventory].[StockQuantity] as [StockQuantity], " +
                "[dbo].[Inventory].[ReservedQuantity] as [ReservedQuantity], " +
                "[dbo].[Inventory].[IsDeleted] as [IsDeleted], " +
                "[dbo].[Inventory].[Id] as [InventoryId] " +
                "[dbo].[Inventory].[ProductId] as [ProductId] " +
                "FROM [dbo].[Inventory] " +
                "WHERE [dbo].[Inventory].[ProductId] IS IN @Param3",
                query.CommandText);
```

#### Update a record
The example below will generate a command to update the User table, provides 3 properties to be updated, where `user.Id` equals the given value `userId`
```csharp
var query = SqlBuilder.Update<User>(_ => new User
                                   {
                                       Email              = _.Email.Replace("@domain1.com", "@domain2.com"),
                                       LastChangePassword = DateTimeOffset.Now,
                                       FailedLogIns       = _.FailedLogIns + 1
                                   })
                    .Where(user => user.Id == userId);

var result = Connection.Execute(query.CommandText, query.CommandParameters);
// this will return the affected rows of the query
```

#### Delete a record / multiple records by condition

The example below will generate a command to delete from User table where the `user.Email` equals the specified `userEmail` value:
```csharp
string userEmail = "query_email@domain1.com";

var query = SqlBuilder.Delete<User>()
                    .Where(user => user.Email == userEmail);
                    // .Where(user => user.Email.Contains("part_of_email_to_search"));

var result = Connection.Execute(query.CommandText, query.CommandParameters);
```

## Extending with Custom Adapters

You can create custom database adapters by implementing the `ISqlAdapter` interface or extending the `SqlAdapterBase` class. See the SQL Server and PostgreSQL providers as reference implementations.

## Inspiration

I am a big fan of ORM, and I have been using Entity Framework since the first day I started my career as a .Net Developer back in 2009.

Some time ago I worked on a project and needed to deal with 2 databases at the same time. For some reasons, I was not supposed to use Entity Framework for the second database which is dynamically configured in a tenant-based setting at runtime. So I had to choose either to come back to ADO.Net and making queries using string concatenation and SqlCommand, or to come up with something that is friendly with Entity Framework usage, which is using LINQ lambda expressions to describe the query or command we want to process with the database.

Finally, I found the open-source repository at [https://github.com/DomanyDusan/lambda-sql-builder](https://github.com/DomanyDusan/lambda-sql-builder) and it's very close to what I was looking for. I decided to reference his code and make a modified version, adding support for INSERT, UPDATE, DELETE in addition to SELECT queries, and refactoring to support multiple database providers.

This project is not meant to replace or cover the entire SQL world. The purpose is to provide the most basic and commonly used CRUD operations, which I believe covers 70% of simple data access operations in most applications.

Continuing the spirit of the original library, this library can be used to generate queries and parameters for use with ADO.Net **SqlCommand** or with **Dapper** for simple mapping back to your entities.

## References

- [https://github.com/DomanyDusan/lambda-sql-builder](https://github.com/DomanyDusan/lambda-sql-builder)
- [https://github.com/mladenb/sql-query-builder](https://github.com/mladenb/sql-query-builder)
