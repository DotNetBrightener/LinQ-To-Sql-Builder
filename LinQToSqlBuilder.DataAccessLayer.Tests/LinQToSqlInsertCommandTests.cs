﻿using DotNetBrightener.LinQToSqlBuilder;
using LinQToSqlBuilder.DataAccessLayer.Tests.Entities;
using Shouldly;
using Xunit;

namespace LinQToSqlBuilder.DataAccessLayer.Tests;

public class LinQToSqlInsertCommandTests
{
    [Fact]
    public void InsertSingleRecord()
    {
        var query = SqlBuilder.Insert<UserGroup>(_ => new UserGroup
        {
            CreatedBy   = "TestSystem",
            CreatedDate = DateTimeOffset.Now,
            Description = "Created from Test System",
            Name        = "TestUserGroup",
            IsDeleted   = false
        });

        query.CommandText
             .ShouldBe("INSERT INTO [dbo].[UsersGroup] ([CreatedBy], [CreatedDate], [Description], [Name], [IsDeleted]) " +
                       "VALUES (@Param1, @Param2, @Param3, @Param4, @Param5)");
    }

    [Fact]
    public void InsertSingleRecordWithOutputIdentity()
    {
        var query = SqlBuilder.Insert<UserGroup>(_ => new UserGroup
                               {
                                   CreatedBy   = "TestSystem",
                                   CreatedDate = DateTimeOffset.Now,
                                   Description = "Created from Test System",
                                   Name        = "TestUserGroup",
                                   IsDeleted   = false
                               })
                              .OutputIdentity();

        query.CommandText
             .ShouldBe("INSERT INTO [dbo].[UsersGroup] ([CreatedBy], [CreatedDate], [Description], [Name], [IsDeleted]) " +
                       "OUTPUT Inserted.[Id] " +
                       "VALUES (@Param1, @Param2, @Param3, @Param4, @Param5)");
    }

    [Fact]
    public void InsertMultipleRecords()
    {
        var query = SqlBuilder.InsertMany<UserGroup>(_ => new[]
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


        query.CommandText
             .ShouldBe("INSERT INTO [dbo].[UsersGroup] ([CreatedBy], [CreatedDate], [Description], [Name], [IsDeleted]) " +
                       "VALUES (@Param1, @Param2, @Param3, @Param4, @Param5), " +
                       "(@Param6, @Param7, @Param8, @Param9, @Param10), " +
                       "(@Param11, @Param12, @Param13, @Param14, @Param15)");
    }

    [Fact]
    public void InsertRecordFromAnotherTables()
    {
        var query = SqlBuilder.InsertFrom<UserGroup, CloneUserGroup>(userGroup => new CloneUserGroup()
                               {
                                   CreatedBy     = "Cloning System",
                                   CreatedDate   = DateTimeOffset.Now,
                                   Description   = userGroup.Description,
                                   Name          = userGroup.Name,
                                   IsDeleted     = userGroup.IsDeleted,
                                   IsUndeletable = userGroup.IsUndeletable,
                                   OriginalId    = userGroup.Id
                               })
                              .Where(group => group.IsDeleted == false);

        query.CommandText
             .ShouldBe("INSERT INTO [dbo].[CloneUserGroup] ([CreatedBy], [CreatedDate], [Description], [Name], [IsDeleted], [IsUndeletable], [OriginalId]) " +
                       "SELECT " +
                       "@Param1 as [CreatedBy], " +
                       "@Param2 as [CreatedDate], " +
                       "[dbo].[UsersGroup].[Description] as [Description], " +
                       "[dbo].[UsersGroup].[Name] as [Name], " +
                       "[dbo].[UsersGroup].[IsDeleted] as [IsDeleted], " +
                       "[dbo].[UsersGroup].[IsUndeletable] as [IsUndeletable], " +
                       "[dbo].[UsersGroup].[Id] as [OriginalId] " +
                       "FROM [dbo].[UsersGroup] " +
                       "WHERE [dbo].[UsersGroup].[IsDeleted] = @Param3");
    }
}