namespace Leettle.Data.Test.Client
{
    class OracleTestClient : AbstractTestClient
    {
        public override string StringDataType { get { return "varchar2"; } }
        public override string ShortDataType { get { return "number(5)"; } }
        public override string IntDataType { get { return "number(10)"; } }
        public override string LongDataType { get { return "number(20)"; } }
        public override string DoubleDataType { get { return "number(15, 9)"; } }
        public override string DecimalDataType { get { return "number(28, 7)"; } }
        public override string DateTimeDataType { get { return "date"; } }
        public override string BlobDataType { get { return "blob"; } }
        public override string LongTextDataType { get { return "clob"; } }
        public OracleTestClient(string connectionString) : base(connectionString, typeof(Oracle.ManagedDataAccess.Client.OracleConnection)) { }
    }
}
