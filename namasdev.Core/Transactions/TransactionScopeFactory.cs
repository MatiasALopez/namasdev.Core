using System.Transactions;

namespace namasdev.Core.Transactions
{
    public class TransactionScopeFactory
    {
        public static TransactionScope Crear(
            TransactionScopeOption transaccionAUsar = TransactionScopeOption.Required, 
            IsolationLevel nivelAislamiento = IsolationLevel.ReadCommitted)
        {
            return new TransactionScope(transaccionAUsar, new TransactionOptions { IsolationLevel = nivelAislamiento });
        }
    }
}
