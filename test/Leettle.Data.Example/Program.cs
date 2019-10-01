using System;
using System.Collections.Generic;

namespace Leettle.Data.Example
{
    class Program
    {
#pragma warning disable IDE0060
        static void Main(string[] args)
#pragma warning restore IDE0060
        {
            // 00. initializing leettledb instance thread-safe
            var leettleDb = new LeettleDbBuilder()
                .WithConnectionType(typeof(System.Data.SQLite.SQLiteConnection))
                .WithConnectionString("Data Source=:memory:;Version=3;New=True;")
                .WithBindStrategy(new CamelObjectSnakeDbBindStrategy(':'))
                .Build();

#pragma warning disable IDE0063
            using (var con = leettleDb.OpenConnection())
#pragma warning restore IDE0063
            {
                // 01.
                Console.WriteLine("01. creating account table");
                con.NewCommand(
                    "create table ACCOUNT (" +
                    "    ID        integer primary key," +
                    "    NAME      varchar(128)," +
                    "    CREATE_DT datetime" +
                    ")").Execute();

                // 02.
                Console.WriteLine("02. inserting two accounts in transaction");
                var initialAccounts = new List<Account>(new Account[] { NewAccount(1, "foo"), NewAccount(2, "bar") });
                con.RunInTransaction(txCon =>
                {
                    foreach (var account in initialAccounts)
                    {
                        con.NewCommand("insert into ACCOUNT (ID, NAME, CREATE_DT) VALUES (:ID, :NAME, :CREATE_DT)")
                            .BindParam(account)
                            .Execute();
                    }
                });

                // 03.
                Console.WriteLine("03. selecting account foo");
                var foo = con.NewDataset("SELECT * FROM ACCOUNT WHERE ID = :ID")
                    .SetParam("ID", 1)
                    .OpenAndFetch<Account>();
                Console.WriteLine("    - " + foo);

                // 04.
                Console.WriteLine("04. selecting all account");
                var accounts = con.NewDataset("SELECT * FROM ACCOUNT ORDER BY ID")
                    .OpenAndFetchList<Account>();
                foreach (var account in accounts)
                {
                    Console.WriteLine("    + " + account);
                }

                // 05.
                Console.WriteLine("05. selecting scalar data : database current time");
                string nowOnDb = con.NewDataset("SELECT datetime('now','localtime')")
                    .OpenAndFetchScalar<string>();
                Console.WriteLine("    - " + nowOnDb);

                // 06.
                Console.WriteLine("06. selecting scalar data list : account id list");
                var ids = con.NewDataset("SELECT ID FROM ACCOUNT ORDER BY ID")
                    .OpenAndFetchScalarList<string>();
                foreach (var id in ids)
                {
                    Console.WriteLine("    + " + id);
                }
            }
        }

        static Account NewAccount(int id, string name)
        {
            return new Account()
            {
                Id = id,
                Name = name,
                CreateDT = DateTime.Now
            };
        }
    }
}
