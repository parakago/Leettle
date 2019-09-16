namespace Leettle.Data
{
    public interface LeettleDb
    {
        IConnection OpenConnection();
    }
}
