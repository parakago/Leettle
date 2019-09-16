# LeettleDB

## Introduction
LeettleDB is a C# [ADO.NET](https://docs.microsoft.com/dotnet/framework/data/adonet) wrapper library to provide same api for different types of database providers and useful method that maps fetched result to a POCO or dynamic object.

## Example
* Creating leettleDB object
```csharp
var leettleDb = new LeettleDbBuilder()
    .WithConnectionType(typeof(System.Data.SQLite.SQLiteConnection))
    .WithConnectionString("Data Source=:memory:;Version=3;New=True;")
    .Build();
```

* Basic raw API fetching data
```csharp
using (var con = leettleDb.OpenConnection())
{
    string sql = "select * from Author where name like :name";
    
    using (var dataset = con.NewRawDataset(sql)) {
        dataset.SetParam("name", "%phen%").Open();
        
        while (dataset.Next())
        {
            var authorId = dataset.GetInt("id");
            var authorName = dataset.GetString("name");
        }
    }
}
```

* Fetching and mapping to POCO List
```csharp
using (var con = leettleDb.OpenConnection())
{
    string sql = "select * from Author where name like :name";
    // codes disposing resource doesn't need.
    List<Author> authors = con.NewDataset(sql)
        .SetParam("name", "%phen%")
        .OpenAndFetchList<Author>();
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
  * Oracle
  * MySql
  * PostgreSQL
  * SQLite
  * SQL Server