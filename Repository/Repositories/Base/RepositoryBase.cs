using Dapper;
using Microsoft.Extensions.Logging;
using Repository.Context;
using Repository.Interfaces.Base;
using System.Data;
using System.Text;

namespace Repository.Repositories.Base;

public abstract class RepositoryBase
{
    protected readonly DbContext _context;
    protected readonly ILogger<RepositoryBase> _logger;
    protected readonly IUnitOfWork _unitOfWork;
    protected IDbConnection Connection => _context.Connection;

    public RepositoryBase(IUnitOfWork unitOfWork,
    DbContext context,
    ILogger<RepositoryBase> logger)
    {
        _unitOfWork = unitOfWork;
        _context = context;
        _logger = logger;
    }
    #region Transaction
    public void BeginTransaction(string key) => _unitOfWork.BeginTransaction(key);
    public void EndTransaction(string key) => _unitOfWork.Commit(key);
    public void RollBackTransaction(string key) => _unitOfWork.RollBack(key);
    public string CreateNewTransactionGuid() => _unitOfWork.CreateNewTransactionGuid();
    #endregion

    #region **** EXECUTE ****

    protected int DapperExecute(string sql, Dictionary<string, object> parameters, int commandTimeout = 60)
    {
        string logComplemento = null;
        try
        {
            var affectedRows = Connection.Execute(sql, parameters, commandTimeout: commandTimeout, commandType: CommandType.StoredProcedure);
            logComplemento = $" AFFECTED ROWS: {affectedRows}";

            return affectedRows;
        }
        finally
        {
            AddDapperSQLToLog(sql, parameters, logComplemento);
        }
    }

    #endregion

    #region **** SCALAR ****
    protected TResult DapperExecuteScalar<TResult>(string sql, Dictionary<string, object>? parameters = null)
    {
        string logComplemento = null;

        try
        {
            var ret = Connection.ExecuteScalar<TResult>(sql, dinamicParameters(parameters), commandType: CommandType.StoredProcedure);
            logComplemento = $" Result {ret}";

            return ret;
        }
        finally
        {
            AddDapperSQLToLog(sql, parameters, logComplemento);
        }
    }
    #endregion

    #region **** SINGLEORDEFAULT ****
    protected TResult DapperSingleOrDefault<TResult>(string sql, Dictionary<string, object> parameters)
    {
        AddDapperSQLToLog(sql, PadronizarEnum(parameters));
        return Connection.QuerySingleOrDefault<TResult>(sql, dinamicParameters(PadronizarEnum(parameters)), commandType: CommandType.StoredProcedure);
    }
    #endregion

    #region **** QUERY ****
    protected IEnumerable<TResult> DapperQuery<TResult>(string sql, Dictionary<string, object>? parameters = null)
    {
        AddDapperSQLToLog(sql, dinamicParameters(parameters));
        return Connection.Query<TResult>(sql, dinamicParameters(parameters), commandType: CommandType.StoredProcedure);
    }
    #endregion

    #region **** LOG ****
    private bool EnableDataLogging => true;
    private bool EnableSensitiveDataLogging => true;

    private void AddParametersToLog(StringBuilder sb, DynamicParameters parameters)
    {
        if (!EnableSensitiveDataLogging || parameters is null || !parameters.ParameterNames.Any())
            return;

        sb.AppendLine("PARAMETERS: ");

        foreach (var name in parameters.ParameterNames)
        {
            var pValue = parameters.Get<dynamic>(name);
            sb.AppendFormat("{0}={1} , ", name, pValue is null ? "null" : pValue.ToString());
        }
    }

    private void AddDapperSQLToLog(string sql, DynamicParameters parameters, string complemento = null)
    {
        if (!EnableDataLogging)
            return;

        var sb = new StringBuilder();

        sb.AppendLine($"EXECUTE SQL: {sql} ");

        AddParametersToLog(sb, parameters);

        if (!string.IsNullOrEmpty(complemento))
            sb.AppendLine(complemento);

        _logger.LogInformation(sb.ToString());
    }

    private void AddParametersToLog(StringBuilder sb, Dictionary<string, object> parameters)
    {
        if (!EnableSensitiveDataLogging || parameters is null || !parameters.Any())
            return;

        sb.AppendLine("PARAMETERS: ");

        foreach (var item in parameters)
        {
            var pValue = parameters[item.Key];
            sb.AppendFormat("{0}={1} , ", item.Key, pValue is null ? "null" : pValue.ToString());
        }
    }

    private void AddDapperSQLToLog(string sql, Dictionary<string, object> parameters, string complemento = null)
    {
        if (!EnableDataLogging)
            return;

        var sb = new StringBuilder();

        sb.AppendLine($"EXECUTE SQL: {sql} ");

        AddParametersToLog(sb, parameters);

        if (!string.IsNullOrEmpty(complemento))
            sb.AppendLine(complemento);

        _logger.LogInformation(sb.ToString());
    }
    #endregion

    #region Private Methods
    private Dictionary<string, object> PadronizarEnum(Dictionary<string, object>? parametros = null)
    {
        if (parametros == null)
            return new Dictionary<string, object>();

        if (parametros.Any(x => x.Value is Enum))
        {
            foreach (var item in parametros.Where(x => x.Value is Enum))
            {
                parametros[item.Key] = (int)item.Value;
            }
        }
        return parametros;
    }

    private DynamicParameters dinamicParameters(Dictionary<string, object>? parametros = null)
    {
        var dyParameters = new DynamicParameters();

        if (parametros == null)
            return dyParameters;

        foreach (var kvp in PadronizarEnum(parametros))
        {
            dyParameters.Add(kvp.Key, kvp.Value);
        }
        return dyParameters;
    }
    #endregion
}
