using System;
using System.Linq;
using DotNetBrightener.LinQToSqlBuilder;
using DotNetBrightener.LinQToSqlBuilder.ValueObjects;
using LinQToSqlBuilder.TestHelpers.Base;
using LinQToSqlBuilder.TestHelpers.Entities;
using NUnit.Framework;

namespace LinQToSqlBuilder.DataAccessLayer.Npgsql.Tests
{
    [TestFixture]
    public class LinQToSqlQueryTests : TestBase
    {
        public override void Init()
        {
            SqlBuilder.SetDefaultProvider(DatabaseProvider.PostgreSql);
        }

        [Test]
        public void QueryCount()
        {
            var query = SqlBuilder.Count<User>(_ => _.Id)
                                  .Where(_ => _.Id > 10);

            Assert.AreEqual($"SELECT COUNT(\"public\".\"Users\".\"Id\") FROM \"public\".\"Users\" " +
                            $"WHERE \"public\".\"Users\".\"Id\" > @Param1",
                            query.CommandText);

            query = SqlBuilder.Count<User>()
                              .Where(_ => _.Id > 10);

            Assert.AreEqual($"SELECT COUNT(*) FROM \"public\".\"Users\" " +
                            $"WHERE \"public\".\"Users\".\"Id\" > @Param1",
                            query.CommandText);
        }

        [Test]
        public void QueryForUserHasBirthdayGreaterThanCertainDate()
        {
            var dateToCheck = new DateTime(1990, 1, 1);

            var query = SqlBuilder.Select<User>()
                                  .Where(_ => _.Birthday > dateToCheck);
            
            Assert.AreEqual($"SELECT \"public\".\"Users\".* " +
                            $"FROM \"public\".\"Users\" " +
                            $"WHERE \"public\".\"Users\".\"Birthday\" > @Param1",
                            query.CommandText);
        }

        [Test]
        public void QueryWithPagination()
        {
            var query = SqlBuilder.Select<User>()
                                  .OrderBy(_ => _.Id)
                                  .Take(10);

            Assert.AreEqual($"SELECT TOP(10) \"public\".\"Users\".* FROM \"public\".\"Users\" ORDER BY \"public\".\"Users\".\"Id\"",
                            query.CommandText);
        }

        [Test]
        public void QueryFieldsWithPagination()
        {
            var query = SqlBuilder.Select<User, object>(user => new
                                   {
                                       Email     = user.Email,
                                       FirstName = user.FirstName,
                                       LastName  = user.LastName,
                                       Id        = user.Id
                                   })
                                  .Where(_ => !_.RecordDeleted)
                                  .OrderBy(_ => _.Id)
                                  .Take(10);

            Assert.AreEqual($"SELECT TOP(10) \"public\".\"Users\".\"Email\", " +
                            $"\"public\".\"Users\".\"FirstName\", " +
                            $"\"public\".\"Users\".\"LastName\", " +
                            $"\"public\".\"Users\".\"Id\" " +
                            $"FROM \"public\".\"Users\" " +
                            $"WHERE NOT \"public\".\"Users\".\"RecordDeleted\" = @Param1 " +
                            $"ORDER BY \"public\".\"Users\".\"Id\"",
                            query.CommandText);
        }

        [Test]
        public void QueryWithPagination2()
        {
            var query = SqlBuilder.Select<User>()
                                  .Where(_ => _.ModifiedDate > DateTimeOffset.Now.Date.AddDays(-50))
                                  .OrderBy(_ => _.Id)
                                  .Take(10)
                                  .Skip(1);


            Assert.AreEqual($"SELECT \"public\".\"Users\".* " +
                            $"FROM \"public\".\"Users\" " +
                            $"WHERE \"public\".\"Users\".\"ModifiedDate\" > @Param1 " +
                            $"ORDER BY \"public\".\"Users\".\"Id\" " +
                            $"OFFSET 10 ROWS FETCH NEXT 10 ROWS ONLY",
                            query.CommandText);
        }

        [Test]
        public void FindByFieldValue()
        {
            var userEmail = "user@domain1.com";

            var query = SqlBuilder.Select<User>()
                                  .Where(user => user.Email == userEmail);

            Assert.AreEqual("SELECT \"public\".\"Users\".* FROM \"public\".\"Users\" WHERE \"public\".\"Users\".\"Email\" = @Param1", 
                            query.CommandText);

            Assert.AreEqual(userEmail,
                            query.CommandParameters.First().Value);
        }

        [Test]
        public void FindByFieldValueAndGetOnlyOneResult()
        {
            var userEmail = "user@domain1.com";

            var query = SqlBuilder.SelectSingle<User>()
                                  .Where(user => user.Email == userEmail);

            Assert.AreEqual("SELECT TOP(1) \"public\".\"Users\".* FROM \"public\".\"Users\" WHERE \"public\".\"Users\".\"Email\" = @Param1",
                            query.CommandText);

            Assert.AreEqual(userEmail,
                            query.CommandParameters.First().Value);
        }

        [Test]
        public void FindByFieldValueLike()
        {
            const string searchTerm = "domain.com";

            var query = SqlBuilder.Select<User>()
                                  .Where(user => user.Email.Contains(searchTerm));

            Assert.AreEqual("SELECT \"public\".\"Users\".* " +
                            "FROM \"public\".\"Users\" " +
                            "WHERE \"public\".\"Users\".\"Email\" LIKE @Param1",
                            query.CommandText);
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

            Assert.AreEqual("SELECT \"public\".\"Users\".*, \"public\".\"UsersUserGroup\".*, \"public\".\"UsersGroup\".* " +
                            "FROM \"public\".\"Users\" " +
                            "JOIN \"public\".\"UsersUserGroup\" ON \"public\".\"Users\".\"Id\" = \"public\".\"UsersUserGroup\".\"UserId\" " +
                            "JOIN \"public\".\"UsersGroup\" ON \"public\".\"UsersUserGroup\".\"UserGroupId\" = \"public\".\"UsersGroup\".\"Id\" " +
                            "WHERE \"public\".\"UsersGroup\".\"Id\" = @Param1", 
                            query.CommandText);


            var query2 = SqlBuilder.Select<User>()
                                   .Where(user => user.Email == email)
                                   .Join<UserUserGroup>((@user, @group) => user.Id == group.UserId)
                                   .Join<UserGroup>((group,     g) => group.UserGroupId == g.Id)
                                   .Where(group => group.Id == groupId);

            Assert.AreEqual("SELECT \"public\".\"Users\".*, \"public\".\"UsersUserGroup\".*, \"public\".\"UsersGroup\".* " +
                            "FROM \"public\".\"Users\" " +
                            "JOIN \"public\".\"UsersUserGroup\" ON \"public\".\"Users\".\"Id\" = \"public\".\"UsersUserGroup\".\"UserId\" " +
                            "JOIN \"public\".\"UsersGroup\" ON \"public\".\"UsersUserGroup\".\"UserGroupId\" = \"public\".\"UsersGroup\".\"Id\" " +
                            "WHERE \"public\".\"Users\".\"Email\" = @Param1 " +
                            "AND \"public\".\"UsersGroup\".\"Id\" = @Param2",
                            query2.CommandText);

            //var result = new Dictionary<long, User>();
            //var results = Connection.Query<User, UserGroup, User>(query.CommandText,
            //                                                      (user, group) =>
            //                                                      {
            //                                                          if (!result.ContainsKey(user.Id))
            //                                                          {
            //                                                              user.Groups = new List<UserGroup>();
            //                                                              result.Add(user.Id, user);
            //                                                          }

            //                                                          result\"user.Id\".Groups.Add(group);
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

            Assert.AreEqual("SELECT \"public\".\"UsersGroup\".* " +
                            "FROM \"public\".\"UsersGroup\" " +
                            "ORDER BY \"public\".\"UsersGroup\".\"Name\"",
                            query.CommandText);
        }

        [Test]
        public void OrderEntitiesByFieldDescending()
        {
            var query = SqlBuilder.Select<UserGroup>()
                                  .OrderByDescending(_ => _.Name);


            Assert.AreEqual("SELECT \"public\".\"UsersGroup\".* " +
                            "FROM \"public\".\"UsersGroup\" " +
                            "ORDER BY \"public\".\"UsersGroup\".\"Name\" DESC",
                            query.CommandText);
        }
    }
}