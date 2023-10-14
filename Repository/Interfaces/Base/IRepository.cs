using Entity.DTOs.BaseDTO;

namespace Repository.Interfaces.Base;

public interface IRepository<T> where T : BaseDto, new()
{
    #region Transaction
    string CreateNewTransactionGuid();
    void BeginTransaction(string key);
    void EndTransaction(string key);
    void RollBackTransaction(string key);
    #endregion


    int CommandExecute(string sqlToExecute, Dictionary<string, object> parameters, int commandTimeout = 0);
    TResult CommandScalar<TResult>(string sql, Dictionary<string, object> parameters);
    TResult CommandQuerySingleOrDefault<TResult>(string query, Dictionary<string, object> parameters);
    IEnumerable<TResult> CommandQuery<TResult>(string query, Dictionary<string, object> parameters);
}
