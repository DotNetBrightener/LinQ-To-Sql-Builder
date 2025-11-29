using System.Data;
using Dapper;

namespace DotNetBrightener.LinQToSqlBuilder.Mssql.Tests.IntegrationTests;

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
    ///     Truncates all test tables to reset data between tests.
    /// </summary>
    public static async Task TruncateTablesAsync(IDbConnection connection)
    {
        await connection.ExecuteAsync(@"
            DELETE FROM [dbo].[UsersUserGroup];
            DELETE FROM [dbo].[CloneUserGroup];
            DELETE FROM [dbo].[UsersGroup];
            DELETE FROM [dbo].[Users];
            DBCC CHECKIDENT ('[dbo].[Users]', RESEED, 0);
            DBCC CHECKIDENT ('[dbo].[UsersGroup]', RESEED, 0);
            DBCC CHECKIDENT ('[dbo].[CloneUserGroup]', RESEED, 0);
        ");
    }

    private static async Task CreateUsersTableAsync(IDbConnection connection)
    {
        await connection.ExecuteAsync(@"
            IF OBJECT_ID('dbo.Users', 'U') IS NULL
            CREATE TABLE [dbo].[Users] (
                [Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
                [FirstName] NVARCHAR(100) NOT NULL,
                [LastName] NVARCHAR(100) NOT NULL,
                [Email] NVARCHAR(255) NOT NULL,
                [RecordDeleted] BIT NOT NULL DEFAULT 0,
                [LastChangePassword] DATETIMEOFFSET NULL,
                [ModifiedDate] DATETIMEOFFSET NULL,
                [FailedLogIns] INT NOT NULL DEFAULT 0
            );
        ");
    }

    private static async Task CreateUsersGroupTableAsync(IDbConnection connection)
    {
        await connection.ExecuteAsync(@"
            IF OBJECT_ID('dbo.UsersGroup', 'U') IS NULL
            CREATE TABLE [dbo].[UsersGroup] (
                [Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
                [Name] NVARCHAR(100) NOT NULL,
                [Description] NVARCHAR(500) NULL,
                [IsUndeletable] BIT NOT NULL DEFAULT 0,
                [IsDeleted] BIT NOT NULL DEFAULT 0,
                [CreatedDate] DATETIMEOFFSET NULL,
                [ModifiedDate] DATETIMEOFFSET NULL,
                [CreatedBy] NVARCHAR(100) NULL,
                [ModifiedBy] NVARCHAR(100) NULL
            );
        ");
    }

    private static async Task CreateUsersUserGroupTableAsync(IDbConnection connection)
    {
        await connection.ExecuteAsync(@"
            IF OBJECT_ID('dbo.UsersUserGroup', 'U') IS NULL
            CREATE TABLE [dbo].[UsersUserGroup] (
                [UserId] BIGINT NOT NULL,
                [UserGroupId] BIGINT NOT NULL,
                PRIMARY KEY ([UserId], [UserGroupId]),
                FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id]),
                FOREIGN KEY ([UserGroupId]) REFERENCES [dbo].[UsersGroup]([Id])
            );
        ");
    }

    private static async Task CreateCloneUserGroupTableAsync(IDbConnection connection)
    {
        await connection.ExecuteAsync(@"
            IF OBJECT_ID('dbo.CloneUserGroup', 'U') IS NULL
            CREATE TABLE [dbo].[CloneUserGroup] (
                [Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
                [Name] NVARCHAR(100) NOT NULL,
                [Description] NVARCHAR(500) NULL,
                [IsUndeletable] BIT NOT NULL DEFAULT 0,
                [IsDeleted] BIT NOT NULL DEFAULT 0,
                [CreatedDate] DATETIMEOFFSET NULL,
                [ModifiedDate] DATETIMEOFFSET NULL,
                [CreatedBy] NVARCHAR(100) NULL,
                [ModifiedBy] NVARCHAR(100) NULL,
                [OriginalId] BIGINT NOT NULL
            );
        ");
    }
}

