using System.Data;
using Dapper;

namespace DotNetBrightener.LinQToSqlBuilder.Npgsql.Tests.IntegrationTests;

/// <summary>
///     Provides database schema setup and teardown utilities for integration tests.
/// </summary>
public static class DatabaseSchemaSetup
{
    /// <summary>
    ///     Creates all required tables for integration tests.
    /// </summary>
    public static async Task CreateTablesAsync(IDbConnection connection)
    {
        await CreateUsersTableAsync(connection);
        await CreateUsersGroupTableAsync(connection);
        await CreateUsersUserGroupTableAsync(connection);
        await CreateCloneUserGroupTableAsync(connection);
    }

    /// <summary>
    ///     Drops all test tables if they exist.
    /// </summary>
    public static async Task DropTablesAsync(IDbConnection connection)
    {
        await connection.ExecuteAsync(@"
            DROP TABLE IF EXISTS ""UsersUserGroup"" CASCADE;
            DROP TABLE IF EXISTS ""CloneUserGroup"" CASCADE;
            DROP TABLE IF EXISTS ""UsersGroup"" CASCADE;
            DROP TABLE IF EXISTS ""Users"" CASCADE;
        ");
    }

    /// <summary>
    ///     Truncates all test tables to reset data between tests.
    /// </summary>
    public static async Task TruncateTablesAsync(IDbConnection connection)
    {
        await connection.ExecuteAsync(@"
            TRUNCATE TABLE ""UsersUserGroup"" CASCADE;
            TRUNCATE TABLE ""CloneUserGroup"" CASCADE;
            TRUNCATE TABLE ""UsersGroup"" CASCADE;
            TRUNCATE TABLE ""Users"" CASCADE;
        ");
    }

    private static async Task CreateUsersTableAsync(IDbConnection connection)
    {
        await connection.ExecuteAsync(@"
            CREATE TABLE IF NOT EXISTS ""Users"" (
                ""Id"" BIGSERIAL PRIMARY KEY,
                ""FirstName"" VARCHAR(100) NOT NULL,
                ""LastName"" VARCHAR(100) NOT NULL,
                ""Email"" VARCHAR(255) NOT NULL,
                ""RecordDeleted"" BOOLEAN NOT NULL DEFAULT FALSE,
                ""LastChangePassword"" TIMESTAMPTZ NULL,
                ""ModifiedDate"" TIMESTAMPTZ NULL,
                ""FailedLogIns"" INT NOT NULL DEFAULT 0
            );
        ");
    }

    private static async Task CreateUsersGroupTableAsync(IDbConnection connection)
    {
        await connection.ExecuteAsync(@"
            CREATE TABLE IF NOT EXISTS ""UsersGroup"" (
                ""Id"" BIGSERIAL PRIMARY KEY,
                ""Name"" VARCHAR(100) NOT NULL,
                ""Description"" VARCHAR(500) NULL,
                ""IsUndeletable"" BOOLEAN NOT NULL DEFAULT FALSE,
                ""IsDeleted"" BOOLEAN NOT NULL DEFAULT FALSE,
                ""CreatedDate"" TIMESTAMPTZ NULL,
                ""ModifiedDate"" TIMESTAMPTZ NULL,
                ""CreatedBy"" VARCHAR(100) NULL,
                ""ModifiedBy"" VARCHAR(100) NULL
            );
        ");
    }

    private static async Task CreateUsersUserGroupTableAsync(IDbConnection connection)
    {
        await connection.ExecuteAsync(@"
            CREATE TABLE IF NOT EXISTS ""UsersUserGroup"" (
                ""UserId"" BIGINT NOT NULL,
                ""UserGroupId"" BIGINT NOT NULL,
                PRIMARY KEY (""UserId"", ""UserGroupId""),
                FOREIGN KEY (""UserId"") REFERENCES ""Users""(""Id""),
                FOREIGN KEY (""UserGroupId"") REFERENCES ""UsersGroup""(""Id"")
            );
        ");
    }

    private static async Task CreateCloneUserGroupTableAsync(IDbConnection connection)
    {
        await connection.ExecuteAsync(@"
            CREATE TABLE IF NOT EXISTS ""CloneUserGroup"" (
                ""Id"" BIGSERIAL PRIMARY KEY,
                ""Name"" VARCHAR(100) NOT NULL,
                ""Description"" VARCHAR(500) NULL,
                ""IsUndeletable"" BOOLEAN NOT NULL DEFAULT FALSE,
                ""IsDeleted"" BOOLEAN NOT NULL DEFAULT FALSE,
                ""CreatedDate"" TIMESTAMPTZ NULL,
                ""ModifiedDate"" TIMESTAMPTZ NULL,
                ""CreatedBy"" VARCHAR(100) NULL,
                ""ModifiedBy"" VARCHAR(100) NULL,
                ""OriginalId"" BIGINT NOT NULL
            );
        ");
    }
}

