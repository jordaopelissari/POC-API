namespace Repository.Interfaces.Base;

public interface IUnitOfWork : IDisposable
{
    string CreateNewTransactionGuid();
    void BeginTransaction(string transactionGuid);
    void Commit(string transactionGuid);
    void RollBack(string transactionGuid);
}
