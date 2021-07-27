<a name='assembly'></a>
# LinQToSqlBuilder

## Contents

- [DatabaseProvider](#T-DotNetBrightener-LinQToSqlBuilder-ValueObjects-DatabaseProvider 'DotNetBrightener.LinQToSqlBuilder.ValueObjects.DatabaseProvider')
  - [PostgreSql](#F-DotNetBrightener-LinQToSqlBuilder-ValueObjects-DatabaseProvider-PostgreSql 'DotNetBrightener.LinQToSqlBuilder.ValueObjects.DatabaseProvider.PostgreSql')
  - [SqlServer](#F-DotNetBrightener-LinQToSqlBuilder-ValueObjects-DatabaseProvider-SqlServer 'DotNetBrightener.LinQToSqlBuilder.ValueObjects.DatabaseProvider.SqlServer')
- [ISqlAdapter](#T-DotNetBrightener-LinQToSqlBuilder-Adapter-ISqlAdapter 'DotNetBrightener.LinQToSqlBuilder.Adapter.ISqlAdapter')
- [LambdaResolver](#T-DotNetBrightener-LinQToSqlBuilder-Resolver-LambdaResolver 'DotNetBrightener.LinQToSqlBuilder.Resolver.LambdaResolver')
  - [Insert\`\`1(expression)](#M-DotNetBrightener-LinQToSqlBuilder-Resolver-LambdaResolver-Insert``1-System-Linq-Expressions-Expression{System-Func{``0,``0}}- 'DotNetBrightener.LinQToSqlBuilder.Resolver.LambdaResolver.Insert``1(System.Linq.Expressions.Expression{System.Func{``0,``0}})')
  - [Insert\`\`1(expression)](#M-DotNetBrightener-LinQToSqlBuilder-Resolver-LambdaResolver-Insert``1-System-Linq-Expressions-Expression{System-Func{``0,System-Collections-Generic-IEnumerable{``0}}}- 'DotNetBrightener.LinQToSqlBuilder.Resolver.LambdaResolver.Insert``1(System.Linq.Expressions.Expression{System.Func{``0,System.Collections.Generic.IEnumerable{``0}}})')
  - [Insert\`\`2(expression)](#M-DotNetBrightener-LinQToSqlBuilder-Resolver-LambdaResolver-Insert``2-System-Linq-Expressions-Expression{System-Func{``0,``1}}- 'DotNetBrightener.LinQToSqlBuilder.Resolver.LambdaResolver.Insert``2(System.Linq.Expressions.Expression{System.Func{``0,``1}})')
  - [OutputInsertIdentity\`\`1()](#M-DotNetBrightener-LinQToSqlBuilder-Resolver-LambdaResolver-OutputInsertIdentity``1 'DotNetBrightener.LinQToSqlBuilder.Resolver.LambdaResolver.OutputInsertIdentity``1')
- [LikeMethod](#T-DotNetBrightener-LinQToSqlBuilder-ValueObjects-LikeMethod 'DotNetBrightener.LinQToSqlBuilder.ValueObjects.LikeMethod')
- [SelectFunction](#T-DotNetBrightener-LinQToSqlBuilder-ValueObjects-SelectFunction 'DotNetBrightener.LinQToSqlBuilder.ValueObjects.SelectFunction')
- [SqlAdapterBase](#T-DotNetBrightener-LinQToSqlBuilder-Adapter-SqlAdapterBase 'DotNetBrightener.LinQToSqlBuilder.Adapter.SqlAdapterBase')
- [SqlBuilder](#T-DotNetBrightener-LinQToSqlBuilder-SqlBuilder 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder')
  - [Count\`\`1(expression)](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-Count``1-System-Linq-Expressions-Expression{System-Func{``0,System-Boolean}}- 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder.Count``1(System.Linq.Expressions.Expression{System.Func{``0,System.Boolean}})')
  - [Count\`\`1(countExpression,expression)](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-Count``1-System-Linq-Expressions-Expression{System-Func{``0,System-Object}},System-Linq-Expressions-Expression{System-Func{``0,System-Boolean}}- 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder.Count``1(System.Linq.Expressions.Expression{System.Func{``0,System.Object}},System.Linq.Expressions.Expression{System.Func{``0,System.Boolean}})')
  - [Delete\`\`1(expression)](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-Delete``1-System-Linq-Expressions-Expression{System-Func{``0,System-Boolean}}- 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder.Delete``1(System.Linq.Expressions.Expression{System.Func{``0,System.Boolean}})')
  - [InsertFrom\`\`2(expression)](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-InsertFrom``2-System-Linq-Expressions-Expression{System-Func{``0,``1}}- 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder.InsertFrom``2(System.Linq.Expressions.Expression{System.Func{``0,``1}})')
  - [InsertMany\`\`1(expression)](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-InsertMany``1-System-Linq-Expressions-Expression{System-Func{``0,System-Collections-Generic-IEnumerable{``0}}}- 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder.InsertMany``1(System.Linq.Expressions.Expression{System.Func{``0,System.Collections.Generic.IEnumerable{``0}}})')
  - [Insert\`\`1(expression)](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-Insert``1-System-Linq-Expressions-Expression{System-Func{``0,``0}}- 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder.Insert``1(System.Linq.Expressions.Expression{System.Func{``0,``0}})')
  - [SelectSingle\`\`1(expressions)](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-SelectSingle``1-System-Linq-Expressions-Expression{System-Func{``0,System-Object}}[]- 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder.SelectSingle``1(System.Linq.Expressions.Expression{System.Func{``0,System.Object}}[])')
  - [Select\`\`1(expressions)](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-Select``1-System-Linq-Expressions-Expression{System-Func{``0,System-Object}}[]- 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder.Select``1(System.Linq.Expressions.Expression{System.Func{``0,System.Object}}[])')
  - [Select\`\`2(expression)](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-Select``2-System-Linq-Expressions-Expression{System-Func{``0,``1}}- 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder.Select``2(System.Linq.Expressions.Expression{System.Func{``0,``1}})')
  - [SetDefaultProvider(provider)](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-SetDefaultProvider-DotNetBrightener-LinQToSqlBuilder-ValueObjects-DatabaseProvider- 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder.SetDefaultProvider(DotNetBrightener.LinQToSqlBuilder.ValueObjects.DatabaseProvider)')
  - [Update\`\`1(expression)](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-Update``1-System-Linq-Expressions-Expression{System-Func{``0,System-Object}}- 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder.Update``1(System.Linq.Expressions.Expression{System.Func{``0,System.Object}})')
- [SqlBuilderBase](#T-DotNetBrightener-LinQToSqlBuilder-SqlBuilderBase 'DotNetBrightener.LinQToSqlBuilder.SqlBuilderBase')
- [SqlBuilder\`1](#T-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1')
  - [#ctor()](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1-#ctor 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1.#ctor')
  - [#ctor()](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1-#ctor-DotNetBrightener-LinQToSqlBuilder-ValueObjects-DatabaseProvider- 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1.#ctor(DotNetBrightener.LinQToSqlBuilder.ValueObjects.DatabaseProvider)')
  - [#ctor(adapter)](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1-#ctor-DotNetBrightener-LinQToSqlBuilder-Adapter-ISqlAdapter- 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1.#ctor(DotNetBrightener.LinQToSqlBuilder.Adapter.ISqlAdapter)')
  - [Insert(expression)](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1-Insert-System-Linq-Expressions-Expression{System-Func{`0,`0}}- 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1.Insert(System.Linq.Expressions.Expression{System.Func{`0,`0}})')
  - [Insert(expression)](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1-Insert-System-Linq-Expressions-Expression{System-Func{`0,System-Collections-Generic-IEnumerable{`0}}}- 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1.Insert(System.Linq.Expressions.Expression{System.Func{`0,System.Collections.Generic.IEnumerable{`0}}})')
  - [Insert\`\`1(expression)](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1-Insert``1-System-Linq-Expressions-Expression{System-Func{`0,``0}}- 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1.Insert``1(System.Linq.Expressions.Expression{System.Func{`0,``0}})')
  - [OutputIdentity()](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1-OutputIdentity 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1.OutputIdentity')
  - [Skip(pageIndex)](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1-Skip-System-Int32- 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1.Skip(System.Int32)')
- [SqlQueryBuilder](#T-DotNetBrightener-LinQToSqlBuilder-Builder-SqlQueryBuilder 'DotNetBrightener.LinQToSqlBuilder.Builder.SqlQueryBuilder')
  - [AssignInsertField(fieldName,value)](#M-DotNetBrightener-LinQToSqlBuilder-Builder-SqlQueryBuilder-AssignInsertField-System-String,System-Object- 'DotNetBrightener.LinQToSqlBuilder.Builder.SqlQueryBuilder.AssignInsertField(System.String,System.Object)')
  - [UpdateAssignField(fieldName,value)](#M-DotNetBrightener-LinQToSqlBuilder-Builder-SqlQueryBuilder-UpdateAssignField-System-String,System-Object- 'DotNetBrightener.LinQToSqlBuilder.Builder.SqlQueryBuilder.UpdateAssignField(System.String,System.Object)')
  - [UpdateFieldReplaceString(fieldName,findWhat,replaceWith)](#M-DotNetBrightener-LinQToSqlBuilder-Builder-SqlQueryBuilder-UpdateFieldReplaceString-System-String,System-Object,System-Object- 'DotNetBrightener.LinQToSqlBuilder.Builder.SqlQueryBuilder.UpdateFieldReplaceString(System.String,System.Object,System.Object)')
  - [UpdateFieldWithOperation(fieldName,operandValue,operation)](#M-DotNetBrightener-LinQToSqlBuilder-Builder-SqlQueryBuilder-UpdateFieldWithOperation-System-String,System-Object,System-String- 'DotNetBrightener.LinQToSqlBuilder.Builder.SqlQueryBuilder.UpdateFieldWithOperation(System.String,System.Object,System.String)')
- [SqlServerAdapterBase](#T-DotNetBrightener-LinQToSqlBuilder-Adapter-SqlServerAdapterBase 'DotNetBrightener.LinQToSqlBuilder.Adapter.SqlServerAdapterBase')

<a name='T-DotNetBrightener-LinQToSqlBuilder-ValueObjects-DatabaseProvider'></a>
## DatabaseProvider `type`

##### Namespace

DotNetBrightener.LinQToSqlBuilder.ValueObjects

##### Summary

An enumeration of the available providers for database accessing.
    It is used to set the backing database for db specific SQL syntax

<a name='F-DotNetBrightener-LinQToSqlBuilder-ValueObjects-DatabaseProvider-PostgreSql'></a>
### PostgreSql `constants`

##### Summary

Provider for PostgreSQL

<a name='F-DotNetBrightener-LinQToSqlBuilder-ValueObjects-DatabaseProvider-SqlServer'></a>
### SqlServer `constants`

##### Summary

Default provider for SQL Server

<a name='T-DotNetBrightener-LinQToSqlBuilder-Adapter-ISqlAdapter'></a>
## ISqlAdapter `type`

##### Namespace

DotNetBrightener.LinQToSqlBuilder.Adapter

##### Summary

SQL adapter provides db specific functionality related to db specific SQL syntax

<a name='T-DotNetBrightener-LinQToSqlBuilder-Resolver-LambdaResolver'></a>
## LambdaResolver `type`

##### Namespace

DotNetBrightener.LinQToSqlBuilder.Resolver

##### Summary

Provides methods to perform resolution to SQL expressions from given lambda expressions

<a name='M-DotNetBrightener-LinQToSqlBuilder-Resolver-LambdaResolver-Insert``1-System-Linq-Expressions-Expression{System-Func{``0,``0}}-'></a>
### Insert\`\`1(expression) `method`

##### Summary

Prepares an INSERT INTO method which expression to copy values from another table

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| expression | [System.Linq.Expressions.Expression{System.Func{\`\`0,\`\`0}}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Linq.Expressions.Expression 'System.Linq.Expressions.Expression{System.Func{``0,``0}}') | The expression that generates the record to insert |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The type of entity that associates to the table to insert record(s) to |

<a name='M-DotNetBrightener-LinQToSqlBuilder-Resolver-LambdaResolver-Insert``1-System-Linq-Expressions-Expression{System-Func{``0,System-Collections-Generic-IEnumerable{``0}}}-'></a>
### Insert\`\`1(expression) `method`

##### Summary

Prepares an INSERT INTO method which expression to copy values from another table

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| expression | [System.Linq.Expressions.Expression{System.Func{\`\`0,System.Collections.Generic.IEnumerable{\`\`0}}}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Linq.Expressions.Expression 'System.Linq.Expressions.Expression{System.Func{``0,System.Collections.Generic.IEnumerable{``0}}}') | The expression that generates the record(s) to insert |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The type of entity that associates to the table to insert record(s) to |

<a name='M-DotNetBrightener-LinQToSqlBuilder-Resolver-LambdaResolver-Insert``2-System-Linq-Expressions-Expression{System-Func{``0,``1}}-'></a>
### Insert\`\`2(expression) `method`

##### Summary

Performs an INSERT INTO method which expression to copy values from another table

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| expression | [System.Linq.Expressions.Expression{System.Func{\`\`0,\`\`1}}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Linq.Expressions.Expression 'System.Linq.Expressions.Expression{System.Func{``0,``1}}') | The expression of taking values from TFrom and assigning to TTo |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TFrom | The type of entity associated to the source table |
| TTo | The type of entity associated to the destination table |

<a name='M-DotNetBrightener-LinQToSqlBuilder-Resolver-LambdaResolver-OutputInsertIdentity``1'></a>
### OutputInsertIdentity\`\`1() `method`

##### Summary

Append OUTPUT to the insert statement to get the output identity of the inserted record.

##### Parameters

This method has no parameters.

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TEntity | The type of the inserting entity |

<a name='T-DotNetBrightener-LinQToSqlBuilder-ValueObjects-LikeMethod'></a>
## LikeMethod `type`

##### Namespace

DotNetBrightener.LinQToSqlBuilder.ValueObjects

##### Summary

An enumeration of the supported string methods for the SQL LIKE statement. The item names should match the related string methods.

<a name='T-DotNetBrightener-LinQToSqlBuilder-ValueObjects-SelectFunction'></a>
## SelectFunction `type`

##### Namespace

DotNetBrightener.LinQToSqlBuilder.ValueObjects

##### Summary

An enumeration of the supported aggregate SQL functions. The item names should match the related function names

<a name='T-DotNetBrightener-LinQToSqlBuilder-Adapter-SqlAdapterBase'></a>
## SqlAdapterBase `type`

##### Namespace

DotNetBrightener.LinQToSqlBuilder.Adapter

##### Summary

Generates the SQL queries that are compatible to all supported databases

<a name='T-DotNetBrightener-LinQToSqlBuilder-SqlBuilder'></a>
## SqlBuilder `type`

##### Namespace

DotNetBrightener.LinQToSqlBuilder

<a name='M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-Count``1-System-Linq-Expressions-Expression{System-Func{``0,System-Boolean}}-'></a>
### Count\`\`1(expression) `method`

##### Summary

Prepares a select count query to specified `T` from given expression

##### Returns

The instance of [SqlBuilder\`1](#T-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1') for chaining calls

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| expression | [System.Linq.Expressions.Expression{System.Func{\`\`0,System.Boolean}}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Linq.Expressions.Expression 'System.Linq.Expressions.Expression{System.Func{``0,System.Boolean}}') | The expression that describe how to filter the results |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The type of entity that associates to the table to prepare the query to |

<a name='M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-Count``1-System-Linq-Expressions-Expression{System-Func{``0,System-Object}},System-Linq-Expressions-Expression{System-Func{``0,System-Boolean}}-'></a>
### Count\`\`1(countExpression,expression) `method`

##### Summary

Prepares a select count query to specified `T` from given expression

##### Returns

The instance of [SqlBuilder\`1](#T-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1') for chaining calls

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| countExpression | [System.Linq.Expressions.Expression{System.Func{\`\`0,System.Object}}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Linq.Expressions.Expression 'System.Linq.Expressions.Expression{System.Func{``0,System.Object}}') | The expression that describe how to pick the fields for counting |
| expression | [System.Linq.Expressions.Expression{System.Func{\`\`0,System.Boolean}}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Linq.Expressions.Expression 'System.Linq.Expressions.Expression{System.Func{``0,System.Boolean}}') | The expression that describe how to filter the results |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The type of entity that associates to the table to prepare the query to |

<a name='M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-Delete``1-System-Linq-Expressions-Expression{System-Func{``0,System-Boolean}}-'></a>
### Delete\`\`1(expression) `method`

##### Summary

Prepares a delete command to specified `T`

##### Returns

The instance of [SqlBuilder\`1](#T-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1') for chaining calls

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| expression | [System.Linq.Expressions.Expression{System.Func{\`\`0,System.Boolean}}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Linq.Expressions.Expression 'System.Linq.Expressions.Expression{System.Func{``0,System.Boolean}}') | The expression that filters the records to be deleted |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The type of entity that associates to the table to performs the deletion |

<a name='M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-InsertFrom``2-System-Linq-Expressions-Expression{System-Func{``0,``1}}-'></a>
### InsertFrom\`\`2(expression) `method`

##### Summary

Prepares an insert command to copy record(s) from specific `T` table to the `TTo` destination table

##### Returns

The instance of [SqlBuilder\`1](#T-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1') for chaining calls

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| expression | [System.Linq.Expressions.Expression{System.Func{\`\`0,\`\`1}}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Linq.Expressions.Expression 'System.Linq.Expressions.Expression{System.Func{``0,``1}}') | The expression describes how to form the destination record |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The type of entity that associates to the source table to copy record(s) from |
| TTo | The type of entity that associates to the destination table to copy record(s) to |

<a name='M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-InsertMany``1-System-Linq-Expressions-Expression{System-Func{``0,System-Collections-Generic-IEnumerable{``0}}}-'></a>
### InsertMany\`\`1(expression) `method`

##### Summary

Prepares an insert command to do the insert operation for many records of specified `T`

##### Returns

The instance of [SqlBuilder\`1](#T-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1') for chaining calls

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| expression | [System.Linq.Expressions.Expression{System.Func{\`\`0,System.Collections.Generic.IEnumerable{\`\`0}}}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Linq.Expressions.Expression 'System.Linq.Expressions.Expression{System.Func{``0,System.Collections.Generic.IEnumerable{``0}}}') | The expression that generates the records to insert |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The type of entity that associates to the table to insert record(s) to |

<a name='M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-Insert``1-System-Linq-Expressions-Expression{System-Func{``0,``0}}-'></a>
### Insert\`\`1(expression) `method`

##### Summary

Prepares an insert command to do the insert operation for one record of specified `T`

##### Returns

The instance of [SqlBuilder\`1](#T-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1') for chaining calls

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| expression | [System.Linq.Expressions.Expression{System.Func{\`\`0,\`\`0}}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Linq.Expressions.Expression 'System.Linq.Expressions.Expression{System.Func{``0,``0}}') | The expression that generates the record to insert |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The type of entity that associates to the table to insert record(s) to |

<a name='M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-SelectSingle``1-System-Linq-Expressions-Expression{System-Func{``0,System-Object}}[]-'></a>
### SelectSingle\`\`1(expressions) `method`

##### Summary

Prepares a select query to retrieve a single record of specified type `T` satisfies given expression

##### Returns

The instance of [SqlBuilder\`1](#T-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1') for chaining calls

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| expressions | [System.Linq.Expressions.Expression{System.Func{\`\`0,System.Object}}[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Linq.Expressions.Expression 'System.Linq.Expressions.Expression{System.Func{``0,System.Object}}[]') | The expression that describes which fields of the `T` to return |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The type of entity that associates to the table to prepare the query to |

<a name='M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-Select``1-System-Linq-Expressions-Expression{System-Func{``0,System-Object}}[]-'></a>
### Select\`\`1(expressions) `method`

##### Summary

Prepares a select query to specified `T` from given expression

##### Returns

The instance of [SqlBuilder\`1](#T-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1') for chaining calls

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| expressions | [System.Linq.Expressions.Expression{System.Func{\`\`0,System.Object}}[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Linq.Expressions.Expression 'System.Linq.Expressions.Expression{System.Func{``0,System.Object}}[]') | The expressions that describes which fields of the `T` to return |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The type of entity that associates to the table to prepare the query to |

<a name='M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-Select``2-System-Linq-Expressions-Expression{System-Func{``0,``1}}-'></a>
### Select\`\`2(expression) `method`

##### Summary

Prepares a select query to specified `T` from given expression

##### Returns

The instance of [SqlBuilder\`1](#T-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1') for chaining calls

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| expression | [System.Linq.Expressions.Expression{System.Func{\`\`0,\`\`1}}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Linq.Expressions.Expression 'System.Linq.Expressions.Expression{System.Func{``0,``1}}') | The expression that describes which fields of the `T` to return |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The type of entity that associates to the table to prepare the query to |
| TResult |  |

<a name='M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-SetDefaultProvider-DotNetBrightener-LinQToSqlBuilder-ValueObjects-DatabaseProvider-'></a>
### SetDefaultProvider(provider) `method`

##### Summary

Set the default database provider

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| provider | [DotNetBrightener.LinQToSqlBuilder.ValueObjects.DatabaseProvider](#T-DotNetBrightener-LinQToSqlBuilder-ValueObjects-DatabaseProvider 'DotNetBrightener.LinQToSqlBuilder.ValueObjects.DatabaseProvider') | The database provider |

<a name='M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder-Update``1-System-Linq-Expressions-Expression{System-Func{``0,System-Object}}-'></a>
### Update\`\`1(expression) `method`

##### Summary

Prepares an update command to specified `T`

##### Returns

The instance of [SqlBuilder\`1](#T-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1') for chaining calls

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| expression | [System.Linq.Expressions.Expression{System.Func{\`\`0,System.Object}}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Linq.Expressions.Expression 'System.Linq.Expressions.Expression{System.Func{``0,System.Object}}') | The expression that describes how to update the record |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The type of entity that associates to the table to performs the update |

<a name='T-DotNetBrightener-LinQToSqlBuilder-SqlBuilderBase'></a>
## SqlBuilderBase `type`

##### Namespace

DotNetBrightener.LinQToSqlBuilder

##### Summary

Represents the basic operations / properties to generate the SQL queries

<a name='T-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1'></a>
## SqlBuilder\`1 `type`

##### Namespace

DotNetBrightener.LinQToSqlBuilder

##### Summary

Represents the service that will generate SQL commands from given lambda expression

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The type of entity that associates to the table, used to perform the table and field name resolution |

<a name='M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1-#ctor'></a>
### #ctor() `constructor`

##### Summary

Instantiates a new instance of [SqlBuilder\`1](#T-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1') with default adapter

##### Parameters

This constructor has no parameters.

##### Remarks

The [DefaultAdapter](#F-DotNetBrightener-LinQToSqlBuilder-SqlBuilderBase-DefaultAdapter 'DotNetBrightener.LinQToSqlBuilder.SqlBuilderBase.DefaultAdapter') can be configured by calling [SetAdapter](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilderBase-SetAdapter-DotNetBrightener-LinQToSqlBuilder-ValueObjects-DatabaseProvider- 'DotNetBrightener.LinQToSqlBuilder.SqlBuilderBase.SetAdapter(DotNetBrightener.LinQToSqlBuilder.ValueObjects.DatabaseProvider)') method

<a name='M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1-#ctor-DotNetBrightener-LinQToSqlBuilder-ValueObjects-DatabaseProvider-'></a>
### #ctor() `constructor`

##### Summary

Instantiates a new instance of [SqlBuilder\`1](#T-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1') with a given [DatabaseProvider](#T-DotNetBrightener-LinQToSqlBuilder-ValueObjects-DatabaseProvider 'DotNetBrightener.LinQToSqlBuilder.ValueObjects.DatabaseProvider')

##### Parameters

This constructor has no parameters.

<a name='M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1-#ctor-DotNetBrightener-LinQToSqlBuilder-Adapter-ISqlAdapter-'></a>
### #ctor(adapter) `constructor`

##### Summary

Instantiates a new instance of [SqlBuilder\`1](#T-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1') with a given [ISqlAdapter](#T-DotNetBrightener-LinQToSqlBuilder-Adapter-ISqlAdapter 'DotNetBrightener.LinQToSqlBuilder.Adapter.ISqlAdapter')

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| adapter | [DotNetBrightener.LinQToSqlBuilder.Adapter.ISqlAdapter](#T-DotNetBrightener-LinQToSqlBuilder-Adapter-ISqlAdapter 'DotNetBrightener.LinQToSqlBuilder.Adapter.ISqlAdapter') | The adapter that provides the functionalities to convert the LinQ expressions to specific database provider syntax |

<a name='M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1-Insert-System-Linq-Expressions-Expression{System-Func{`0,`0}}-'></a>
### Insert(expression) `method`

##### Summary

Performs insert a new record from the given expression

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| expression | [System.Linq.Expressions.Expression{System.Func{\`0,\`0}}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Linq.Expressions.Expression 'System.Linq.Expressions.Expression{System.Func{`0,`0}}') | The expression describes what to insert |

<a name='M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1-Insert-System-Linq-Expressions-Expression{System-Func{`0,System-Collections-Generic-IEnumerable{`0}}}-'></a>
### Insert(expression) `method`

##### Summary

Performs insert many records from the given expression

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| expression | [System.Linq.Expressions.Expression{System.Func{\`0,System.Collections.Generic.IEnumerable{\`0}}}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Linq.Expressions.Expression 'System.Linq.Expressions.Expression{System.Func{`0,System.Collections.Generic.IEnumerable{`0}}}') | The expression describes the entities to insert |

<a name='M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1-Insert``1-System-Linq-Expressions-Expression{System-Func{`0,``0}}-'></a>
### Insert\`\`1(expression) `method`

##### Summary

Performs insert to [](#!-TTo 'TTo') table using the values copied from the given expression

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| expression | [System.Linq.Expressions.Expression{System.Func{\`0,\`\`0}}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Linq.Expressions.Expression 'System.Linq.Expressions.Expression{System.Func{`0,``0}}') | The expression describes how to copy values from original table [](#!-T 'T') |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TTo | The destination table |

<a name='M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1-OutputIdentity'></a>
### OutputIdentity() `method`

##### Summary

Append OUTPUT to the insert statement to get the output identity of the inserted record.

##### Parameters

This method has no parameters.

<a name='M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1-Skip-System-Int32-'></a>
### Skip(pageIndex) `method`

##### Summary

Use with [Take](#M-DotNetBrightener-LinQToSqlBuilder-SqlBuilder`1-Take-System-Int32- 'DotNetBrightener.LinQToSqlBuilder.SqlBuilder`1.Take(System.Int32)')(), to skip specified pages of result

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| pageIndex | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Number of pages to skip |

<a name='T-DotNetBrightener-LinQToSqlBuilder-Builder-SqlQueryBuilder'></a>
## SqlQueryBuilder `type`

##### Namespace

DotNetBrightener.LinQToSqlBuilder.Builder

##### Summary

Provides methods to build up SQL query, adding up parameters and conditions to the query and generate the final SQL statement

<a name='M-DotNetBrightener-LinQToSqlBuilder-Builder-SqlQueryBuilder-AssignInsertField-System-String,System-Object-'></a>
### AssignInsertField(fieldName,value) `method`

##### Summary

Updates specified [](#!-fieldName 'fieldName') with assigning [](#!-value 'value')

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fieldName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| value | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') |  |

<a name='M-DotNetBrightener-LinQToSqlBuilder-Builder-SqlQueryBuilder-UpdateAssignField-System-String,System-Object-'></a>
### UpdateAssignField(fieldName,value) `method`

##### Summary

Updates specified [](#!-fieldName 'fieldName') with assigning [](#!-value 'value')

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fieldName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| value | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') |  |

<a name='M-DotNetBrightener-LinQToSqlBuilder-Builder-SqlQueryBuilder-UpdateFieldReplaceString-System-String,System-Object,System-Object-'></a>
### UpdateFieldReplaceString(fieldName,findWhat,replaceWith) `method`

##### Summary

Updates specified [](#!-fieldName 'fieldName') by replacing [](#!-findWhat 'findWhat') with [](#!-replaceWith 'replaceWith')

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fieldName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| findWhat | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') |  |
| replaceWith | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') |  |

<a name='M-DotNetBrightener-LinQToSqlBuilder-Builder-SqlQueryBuilder-UpdateFieldWithOperation-System-String,System-Object,System-String-'></a>
### UpdateFieldWithOperation(fieldName,operandValue,operation) `method`

##### Summary

Updates specified [](#!-fieldName 'fieldName') by performing the [](#!-operation 'operation') of the [](#!-operandValue 'operandValue') to current value

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fieldName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of field to update |
| operandValue | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | The other operand of the operation |
| operation | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The operation |

<a name='T-DotNetBrightener-LinQToSqlBuilder-Adapter-SqlServerAdapterBase'></a>
## SqlServerAdapterBase `type`

##### Namespace

DotNetBrightener.LinQToSqlBuilder.Adapter

##### Summary

Provides functionality common to all supported SQL Server versions
