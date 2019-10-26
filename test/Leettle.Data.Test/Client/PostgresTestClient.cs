namespace Leettle.Data.Test.Client
{
    class PostgresTestClient : AbstractTestClient
    {
        public override string StringDataType { get { return "varchar"; } }
        public override string ShortDataType { get { return "smallint"; } }
        public override string IntDataType { get { return "integer"; } }
        public override string LongDataType { get { return "bigint"; } }
        public override string DoubleDataType { get { return "numeric(15, 9)"; } }
        public override string DecimalDataType { get { return "numeric(28, 7)"; } }
        public override string DateTimeDataType { get { return "timestamp"; } }
        public override string BlobDataType { get { return "bytea"; } }
        public override string LongTextDataType { get { return "text"; } }
        public PostgresTestClient(string connectionString) : base(connectionString, typeof(Npgsql.NpgsqlConnection)) { }
    }
}
