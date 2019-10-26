namespace Leettle.Data.Test.Client
{
    class SQLiteTestClient : AbstractTestClient
    {
        public override string StringDataType { get { return "varchar"; } }
        public override string ShortDataType { get { return "smallint"; } }
        public override string IntDataType { get { return "int"; } }
        public override string LongDataType { get { return "bigint"; } }
        public override string DoubleDataType { get { return "decimal(15, 9)"; } }
        public override string DecimalDataType { get { return "decimal(28, 7)"; } }
        public override string DateTimeDataType { get { return "datetime"; } }
        public override string BlobDataType { get { return "blob"; } }
        public override string LongTextDataType { get { return "text"; } }
        public SQLiteTestClient(string connectionString) : base(connectionString, typeof(System.Data.SQLite.SQLiteConnection)) { }
    }
}
