using Entity.DTOs.BaseDTO;
using Microsoft.Extensions.Logging;
using Repository.Context;
using Repository.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Base;

public class Repository<T> : RepositoryBase, IRepository<T> where T : BaseDto, new()
{
    public Repository(IUnitOfWork unitOfWork,
    DbContext context,
    ILogger<RepositoryBase> logger) : base(unitOfWork, context, logger)
    {
    }

    #region *** Personal SQL ***

    public int CommandExecute(string sqlToExecute, Dictionary<string, object> parameters, int commandTimeout = 0)
        => DapperExecute(sqlToExecute, parameters, commandTimeout);

    public IEnumerable<TResult> CommandQuery<TResult>(string query, Dictionary<string, object>? parameters = null)
        => DapperQuery<TResult>(query, parameters);
    public TResult CommandQuerySingleOrDefault<TResult>(string query, Dictionary<string, object> parameters)
        => DapperSingleOrDefault<TResult>(query, parameters);

    public TResult CommandScalar<TResult>(string query, Dictionary<string, object> parameters)
        => DapperExecuteScalar<TResult>(query, parameters);

    #endregion
}
