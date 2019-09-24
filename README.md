# LeettleDB

## Introduction
LeettleDB is a C# [ADO.NET](https://docs.microsoft.com/dotnet/framework/data/adonet) wrapper library to provide same(?) api for different types of database providers and useful method that maps fetched result to a POCO.

## Example
* Creating leettleDB object
```csharp
var leettleDb = new LeettleDbBuilder()
    .WithConnectionType(typeof(System.Data.SQLite.SQLiteConnection))
    .WithConnectionString("Data Source=:memory:;Version=3;New=True;")
	.WithBindStrategy(new CamelObjectSnakeDbBindStrategy())
    .Build();
```

* Fetching and mapping to POCO List
```csharp
using (var con = leettleDb.OpenConnection())
{
    // codes disposing resource doesn't need.
    List<Author> authors = con.NewDataset("select * from Author where name like :name")
        .SetParam("name", "%phen%")
        .OpenAndFetchList<Author>();
}
```

* Basic raw API fetching data
```csharp
using (var con = leettleDb.OpenConnection())
{
    con.NewDataset("select * from Author where name like :name")
        .SetParam("name", "%phen%")
		.Open(dr =>
    {
		while (dr.Next())
        {
            var authorId = dr.GetInt("id");
            var authorName = dr.GetString("name");
        }
    });
}
```

* Transaction
```csharp
using (var con = leettleDb.OpenConnection())
{
    con.RunInTransaction(() =>
    {
        con.NewCommand("insert into Author values (:id, :name)")
            .SetParam("id", 1)
            .SetParam("name", "dodo")
            .Execute();
        
        con.NewCommand("insert into AuthorBook values (:id, :title)")
            .SetParam("id", 1)
            .SetParam("title", "dodo's story")
            .Execute();
    });
}
```
## Installation
* install LeettleDB package through nuget.
* install ado.net data provider that what you want. (following data providers were tested)
  * Oracle (Oracle.ManagedDataAccess)
  * MySql (Mysql.Data)
  * PostgreSQL (Npgsql)
  * SQLite (System.Data.SQLite.Core)
  * SQL Server (System.Data.SqlClient)
