using Bogus;
using DotNetBrightener.LinQToSqlBuilder.Mssql.Tests.Entities;

namespace DotNetBrightener.LinQToSqlBuilder.Mssql.Tests.IntegrationTests;

/// <summary>
///     Provides Bogus-based test data generators for integration tests.
/// </summary>
public static class TestDataGenerators
{
    /// <summary>
    ///     Creates a Faker for generating User entities.
    /// </summary>
    public static Faker<User> CreateUserFaker()
    {
        return new Faker<User>()
              .RuleFor(u => u.FirstName, f => f.Name.FirstName())
              .RuleFor(u => u.LastName, f => f.Name.LastName())
              .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
              .RuleFor(u => u.RecordDeleted, f => false)
              .RuleFor(u => u.LastChangePassword, f => f.Date.PastOffset(1))
              .RuleFor(u => u.ModifiedDate, f => f.Date.RecentOffset(30))
              .RuleFor(u => u.FailedLogIns, f => f.Random.Int(0, 5));
    }

    /// <summary>
    ///     Creates a Faker for generating UserGroup entities.
    /// </summary>
    public static Faker<UserGroup> CreateUserGroupFaker()
    {
        return new Faker<UserGroup>()
              .RuleFor(g => g.Name, f => f.Commerce.Department())
              .RuleFor(g => g.Description, f => f.Lorem.Sentence())
              .RuleFor(g => g.IsUndeletable, f => f.Random.Bool(0.1f))
              .RuleFor(g => g.IsDeleted, f => false)
              .RuleFor(g => g.CreatedDate, f => f.Date.PastOffset(2))
              .RuleFor(g => g.ModifiedDate, f => f.Date.RecentOffset(30))
              .RuleFor(g => g.CreatedBy, f => f.Internet.UserName())
              .RuleFor(g => g.ModifiedBy, f => f.Internet.UserName());
    }

    /// <summary>
    ///     Creates a Faker for generating UserUserGroup entities.
    /// </summary>
    public static Faker<UserUserGroup> CreateUserUserGroupFaker(long userId, long userGroupId)
    {
        return new Faker<UserUserGroup>()
              .RuleFor(ug => ug.UserId, _ => userId)
              .RuleFor(ug => ug.UserGroupId, _ => userGroupId);
    }
}

