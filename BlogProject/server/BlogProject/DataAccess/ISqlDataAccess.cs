using System.Data;

namespace BlogProject.DataAccess
{
    public interface ISqlDataAccess
    {
        IDbTransaction BeginTransaction(string connectionId = "DbConnString");

        Task<IEnumerable<T>> LoadDataRefCursor<T, U>( string storedProcedure,U parameters,string connectionId ="DbConnString",
    CommandType commandType = CommandType.Text);

        Task<U> ExecuteScalarAsync<T, U>
    (string storedProcedure, T parameters, string connectionId = "DbConnString", CommandType commandType = CommandType.Text, IDbTransaction? transaction = null);
    }
}
