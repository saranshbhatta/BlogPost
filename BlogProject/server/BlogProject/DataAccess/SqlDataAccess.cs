using Npgsql;
using System.Data;
using Dapper;



namespace BlogProject.DataAccess
{
    public class SqlDataAccess : ISqlDataAccess
    {

        private readonly IConfiguration _config;
        private IDbConnection _connection;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public IDbTransaction BeginTransaction(string connectionId = "DbConnString")
        {
            _connection = new NpgsqlConnection(_config.GetConnectionString(connectionId));
            _connection.Open();
            var transaction = _connection.BeginTransaction();
            return transaction;
        }

        public async Task<IEnumerable<T>> LoadDataRefCursor<T, U>(
    string storedProcedure,
    U parameters,
    string connectionId = "DbConnString",
    CommandType commandType = CommandType.Text)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            using IDbConnection connection = new NpgsqlConnection(_config.GetConnectionString(connectionId));
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();
            var resultsReference =
                (IDictionary<string, object>)
                connection.Query<dynamic>(storedProcedure, parameters,
                commandType: commandType, transaction: transaction).Single();
            string resultSetName = (string)resultsReference[resultsReference.Keys.First()];
            string resultSetReferenceCommand = string.Format(@"FETCH ALL IN ""{0}""", resultSetName);

            var result = await connection.QueryAsync<T>(resultSetReferenceCommand,
                null, commandType: CommandType.Text, transaction: transaction);

            transaction.Commit();
            return result;
        }



        public async Task<U> ExecuteScalarAsync<T, U>(
    string storedProcedure,
    T parameters,
    string connectionId = "DbConnString",
    CommandType commandType = CommandType.Text,
    IDbTransaction? transaction = null
)
        {
            //ToDo: Need to optimize the process
            U retVal;
            if (transaction == null)
            {
                using IDbConnection connection = new NpgsqlConnection(_config.GetConnectionString(connectionId));
                retVal = await connection.ExecuteScalarAsync<U>(storedProcedure, parameters,
                    commandType: commandType);
            }
            else
                retVal = await _connection.ExecuteScalarAsync<U>(storedProcedure, parameters,
                commandType: commandType, transaction: transaction);
            return retVal;
        }
    }

}

