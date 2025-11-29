using DotNetBrightener.LinQToSqlBuilder.Npgsql.Tests.Base;
using DotNetBrightener.LinQToSqlBuilder.Npgsql.Tests.Entities;
using Shouldly;
using System.Linq.Expressions;
using Xunit;

namespace DotNetBrightener.LinQToSqlBuilder.Npgsql.Tests.UnitTests;

[Collection("PostgreSQL")]
public class LinQToSqlDeleteCommandTests
{
    [Fact]
    public void DeleteByFieldValue()
    {
        var testCases = new List<TestCase>()
        {
            new TestCase()
            {
                Expression = g => g.IsDeleted,
                ExpectedQueryString =
                    "DELETE FROM \"public\".\"CloneUserGroup\" WHERE \"public\".\"CloneUserGroup\".\"IsDeleted\" = @Param1",
                ExpectedParamValues = [true]
            },
            new TestCase()
            {
                Expression          = g => g.Id == 1,
                ExpectedQueryString = "DELETE FROM \"public\".\"CloneUserGroup\" WHERE \"public\".\"CloneUserGroup\".\"Id\" = @Param1",
                ExpectedParamValues = [1]
            },
            new TestCase()
            {
                Expression = g => g.Name == "Test",
                ExpectedQueryString =
                    "DELETE FROM \"public\".\"CloneUserGroup\" WHERE \"public\".\"CloneUserGroup\".\"Name\" = @Param1",
                ExpectedParamValues = ["Test"]
            },
            new TestCase()
            {
                Expression = g => g.Name == "Test" && g.IsDeleted,
                ExpectedQueryString =
                    "DELETE FROM \"public\".\"CloneUserGroup\" WHERE (\"public\".\"CloneUserGroup\".\"Name\" = @Param1 AND \"public\".\"CloneUserGroup\".\"IsDeleted\" = @Param2)",
                ExpectedParamValues = ["Test", true]
            },
            new TestCase()
            {
                Expression = g => g.Name == "Test" && g.IsDeleted && g.Id == 1,
                ExpectedQueryString =
                    "DELETE FROM \"public\".\"CloneUserGroup\" WHERE ((\"public\".\"CloneUserGroup\".\"Name\" = @Param1 AND \"public\".\"CloneUserGroup\".\"IsDeleted\" = @Param2) AND \"public\".\"CloneUserGroup\".\"Id\" = @Param3)",
                ExpectedParamValues = ["Test", true, 1]
            }
        };

        foreach (var testCase in testCases)
        {
            var query = SqlBuilder.Delete<CloneUserGroup>(testCase.Expression);

            query.CommandText
                 .ShouldBe(testCase.ExpectedQueryString);

            for (var i = 0; i < testCase.ExpectedParamValues.Count; i++)
            {
                query.CommandParameters.Values.ElementAt(i)
                     .ShouldBe(testCase.ExpectedParamValues[i]);
            }
        }
    }
}

internal class TestCase
{
    public Expression<Func<CloneUserGroup, bool>> Expression;
    public string                                 ExpectedQueryString;
    public List<object>                           ExpectedParamValues = [];
}