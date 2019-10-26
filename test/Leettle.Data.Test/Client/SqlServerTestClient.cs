namespace Leettle.Data.Test.Client
{
    class SqlServerTestClient : AbstractTestClient
    {
        public override string StringDataType { get { return "varchar"; } }
        public override string ShortDataType { get { return "smallint"; } }
        public override string IntDataType { get { return "int"; } }
        public override string LongDataType { get { return "bigint"; } }
        public override string DoubleDataType { get { return "numeric(15, 9)"; } }
        public override string DecimalDataType { get { return "numeric(28, 7)"; } }
        public override string DateTimeDataType { get { return "datetime"; } }
        public override string BlobDataType { get { return "varbinary(max)"; } }
        public override string LongTextDataType { get { return "varchar(MAX)"; } }
        public SqlServerTestClient(string connectionString) : base(connectionString, typeof(System.Data.SqlClient.SqlConnection), '@'){ }
    }
}
