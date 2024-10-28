namespace Application.Interface.Repository.Transaction;
using Domain.Entities.Transactions;
public interface ITransactionRepository
{
    Task<string> GenerateTransactionIdAsync();
    Task SaveChangesAsync(Transaction transaction);
    Task UpdateChangesAsync(Transaction transaction);
    Task<Transaction> GetTransactionByTransactionIdAsync(string transactionId);
}
