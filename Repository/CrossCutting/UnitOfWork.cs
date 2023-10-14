using Microsoft.Extensions.Logging;
using Repository.Context;
using Repository.Interfaces.Base;
using System.Data;

namespace Repository.CrossCutting;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private readonly ILogger<UnitOfWork> _logger;
    private IDbTransaction _transaction;
    private bool disposedValue;
    private string _transactionGuid = null;


    public UnitOfWork(DbContext context,
        ILogger<UnitOfWork> logger)
    {
        _context = context;
        _logger = logger;
    }


    public string CreateNewTransactionGuid() => Guid.NewGuid().ToString();

    public void BeginTransaction(string transactionGuid)
    {
        if (string.IsNullOrEmpty(transactionGuid))
            throw new ArgumentException("transactionGuid não pode ser vazio ou nulo");

        if (!string.IsNullOrEmpty(_transactionGuid)) // Já tenho uma transação externa aberta
            return;

        _transaction = _context.Connection.BeginTransaction();
        _transactionGuid = transactionGuid;
    }


    public void Commit(string transactionGuid)
    {
        if (transactionGuid != _transactionGuid)
            return;

        if (string.IsNullOrEmpty(_transactionGuid))
            throw new ArgumentException("Nenhuma transação aberta.");

        _transaction.Commit();
        _transaction.Dispose();

        _transactionGuid = null;
        _transaction = null;
    }


    public void RollBack(string transactionGuid)
    {
        if (transactionGuid != _transactionGuid)
            return;

        if (string.IsNullOrEmpty(_transactionGuid))
            throw new ArgumentException("Nenhuma transação aberta.");

        _transaction.Rollback();
        _transaction.Dispose();

        _transactionGuid = null;
        _transaction = null;
    }


    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }

                if (_context != null)
                    _context.Dispose();
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
