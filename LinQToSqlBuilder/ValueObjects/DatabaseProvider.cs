namespace DotNetBrightener.LinQToSqlBuilder.ValueObjects
{
    /// <summary>
    ///     An enumeration of the available providers for database accessing.
    ///     It is used to set the backing database for db specific SQL syntax
    /// </summary>
    public enum DatabaseProvider
    {
        /// <summary>
        ///     Default provider for SQL Server
        /// </summary>
        SqlServer,

        /// <summary>
        ///     Provider for PostgreSQL
        /// </summary>
        PostgreSql
    }
}
