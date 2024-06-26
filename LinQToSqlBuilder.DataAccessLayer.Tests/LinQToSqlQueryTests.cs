﻿using DotNetBrightener.LinQToSqlBuilder;
using LinQToSqlBuilder.DataAccessLayer.Tests.Base;
using LinQToSqlBuilder.DataAccessLayer.Tests.Entities;
using NUnit.Framework;

namespace LinQToSqlBuilder.DataAccessLayer.Tests;

[TestFixture]
public class LinQToSqlQueryTests : TestBase
{
    [Test]
    public void QueryCount()
    {
        var query = SqlBuilder.Count<User>(_ => _.Id)
                              .Where(_ => _.Id > 10);

        Assert.That(query.CommandText,
                    Is.EqualTo($"SELECT COUNT([dbo].[Users].[Id]) FROM [dbo].[Users] " +
                               $"WHERE [dbo].[Users].[Id] > @Param1"));

        query = SqlBuilder.Count<User>()
                          .Where(_ => _.Id > 10);

        Assert.That(query.CommandText,
                    Is.EqualTo($"SELECT COUNT(*) FROM [dbo].[Users] " +
                               $"WHERE [dbo].[Users].[Id] > @Param1"));
    }

    [Test]
    public void QueryWithPagination()
    {
        var query = SqlBuilder.Select<User>()
                              .OrderBy(_ => _.Id)
                              .Take(10);

        Assert.That(query.CommandText,
                    Is.EqualTo($"SELECT TOP(10) [dbo].[Users].* FROM [dbo].[Users] ORDER BY [dbo].[Users].[Id]"));
    }

    [Test]
    public void QueryFieldsWithPagination()
    {
        var query = SqlBuilder.Select<User, UserViewModel>(user => new UserViewModel
                               {
                                   Email     = user.Email,
                                   FirstName = user.FirstName,
                                   LastName  = user.LastName,
                                   Id        = user.Id
                               })
                              .Where(_ => !_.RecordDeleted)
                              .OrderBy(_ => _.Id)
                              .Take(10);

        Assert.That(query.CommandText,
                    Is.EqualTo($"SELECT TOP(10) [dbo].[Users].[Email], " +
                               $"[dbo].[Users].[FirstName], " +
                               $"[dbo].[Users].[LastName], " +
                               $"[dbo].[Users].[Id] " +
                               $"FROM [dbo].[Users] " +
                               $"WHERE NOT [dbo].[Users].[RecordDeleted] = @Param1 " +
                               $"ORDER BY [dbo].[Users].[Id]"));
    }

    [Test]
    public void QueryWithPagination2()
    {
        var query = SqlBuilder.Select<User>()
                              .Where(_ => _.ModifiedDate > DateTimeOffset.Now.Date.AddDays(-50))
                              .OrderBy(_ => _.Id)
                              .Take(10)
                              .Skip(1);


        Assert.That(query.CommandText,
                    Is.EqualTo($"SELECT [dbo].[Users].* " +
                               $"FROM [dbo].[Users] " +
                               $"WHERE [dbo].[Users].[ModifiedDate] > @Param1 " +
                               $"ORDER BY [dbo].[Users].[Id] " +
                               $"OFFSET 10 ROWS FETCH NEXT 10 ROWS ONLY"));
    }

    [Test]
    public void FindByFieldValue()
    {
        var userEmail = "user@domain1.com";

        var query = SqlBuilder.Select<User>()
                              .Where(user => user.Email == userEmail);

        Assert.That(query.CommandText,
                    Is.EqualTo("SELECT [dbo].[Users].* FROM [dbo].[Users] WHERE [dbo].[Users].[Email] = @Param1"));

        Assert.That(query.CommandParameters.First().Value, Is.EqualTo(userEmail));
    }

    [Test]
    public void FindByFieldValueAndGetOnlyOneResult()
    {
        var userEmail = "user@domain1.com";

        var query = SqlBuilder.SelectSingle<User>()
                              .Where(user => user.Email == userEmail);

        Assert.That(query.CommandText,
                    Is.EqualTo("SELECT TOP(1) [dbo].[Users].* FROM [dbo].[Users] WHERE [dbo].[Users].[Email] = @Param1"));

        Assert.That(query.CommandParameters.First().Value, Is.EqualTo(userEmail));
    }

    [Test]
    public void FindByFieldValueLike()
    {
        const string searchTerm = "domain.com";

        var query = SqlBuilder.Select<User>()
                              .Where(user => user.Email.Contains(searchTerm));

        Assert.That(query.CommandText,
                    Is.EqualTo("SELECT [dbo].[Users].* " +
                               "FROM [dbo].[Users] " +
                               "WHERE [dbo].[Users].[Email] LIKE @Param1"));
    }

    [Test]
    public void FindByJoinedEntityValue()
    {
        var email   = $"someemail@domain.com";
        var groupId = 3;
        var query = SqlBuilder.Select<User>()
                              .Join<UserUserGroup>((@user, @group) => user.Id == group.UserId)
                              .Join<UserGroup>((group,     g) => group.UserGroupId == g.Id)
                              .Where(group => group.Id == groupId);

        Assert.That(query.CommandText,
                    Is.EqualTo("SELECT [dbo].[Users].*, [dbo].[UsersUserGroup].*, [dbo].[UsersGroup].* " +
                               "FROM [dbo].[Users] " +
                               "JOIN [dbo].[UsersUserGroup] ON [dbo].[Users].[Id] = [dbo].[UsersUserGroup].[UserId] " +
                               "JOIN [dbo].[UsersGroup] ON [dbo].[UsersUserGroup].[UserGroupId] = [dbo].[UsersGroup].[Id] " +
                               "WHERE [dbo].[UsersGroup].[Id] = @Param1"));


        var query2 = SqlBuilder.Select<User>()
                               .Where(user => user.Email == email)
                               .Join<UserUserGroup>((@user, @group) => user.Id == group.UserId)
                               .Join<UserGroup>((group,     g) => group.UserGroupId == g.Id)
                               .Where(group => group.Id == groupId);

        Assert.That(query2.CommandText,
                    Is.EqualTo("SELECT [dbo].[Users].*, [dbo].[UsersUserGroup].*, [dbo].[UsersGroup].* " +
                               "FROM [dbo].[Users] " +
                               "JOIN [dbo].[UsersUserGroup] ON [dbo].[Users].[Id] = [dbo].[UsersUserGroup].[UserId] " +
                               "JOIN [dbo].[UsersGroup] ON [dbo].[UsersUserGroup].[UserGroupId] = [dbo].[UsersGroup].[Id] " +
                               "WHERE [dbo].[Users].[Email] = @Param1 " +
                               "AND [dbo].[UsersGroup].[Id] = @Param2"));

        //var result = new Dictionary<long, User>();
        //var results = Connection.Query<User, UserGroup, User>(query.CommandText,
        //                                                      (user, group) =>
        //                                                      {
        //                                                          if (!result.ContainsKey(user.Id))
        //                                                          {
        //                                                              user.Groups = new List<UserGroup>();
        //                                                              result.Add(user.Id, user);
        //                                                          }

        //                                                          result[user.Id].Groups.Add(group);
        //                                                          return user;
        //                                                      },
        //                                                      query.CommandParameters,
        //                                                      splitOn: "UserId,UserGroupId")
        //                        .ToList();
    }

    [Test]
    public void OrderEntitiesByField()
    {
        var query = SqlBuilder.Select<UserGroup>()
                              .OrderBy(_ => _.Name);

        Assert.That(query.CommandText,
                    Is.EqualTo("SELECT [dbo].[UsersGroup].* " +
                               "FROM [dbo].[UsersGroup] " +
                               "ORDER BY [dbo].[UsersGroup].[Name]"));
    }

    [Test]
    public void OrderEntitiesByFieldDescending()
    {
        var query = SqlBuilder.Select<UserGroup>()
                              .OrderByDescending(_ => _.Name);


        Assert.That(query.CommandText,
                    Is.EqualTo("SELECT [dbo].[UsersGroup].* " +
                               "FROM [dbo].[UsersGroup] " +
                               "ORDER BY [dbo].[UsersGroup].[Name] DESC"));
    }
}