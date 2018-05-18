using System;
using StoreKit;

namespace InAppPurchasesApp.iOS
{
    internal class IOSTransactionObserver : SKPaymentTransactionObserver
    {
        public event Action<SKPaymentTransaction, bool> TransactionCompleted;

        public override void UpdatedTransactions(SKPaymentQueue queue, SKPaymentTransaction[] transactions)
        {
            foreach (var transaction in transactions)
            {
                if (transaction?.TransactionState == null) break;

                switch (transaction.TransactionState)
                {
                    case SKPaymentTransactionState.Restored:
                    case SKPaymentTransactionState.Purchased:
                        this.TransactionCompleted?.Invoke(transaction, true);
                        SKPaymentQueue.DefaultQueue.FinishTransaction(transaction);
                        break;

                    case SKPaymentTransactionState.Failed:
                        this.TransactionCompleted?.Invoke(transaction, false);
                        SKPaymentQueue.DefaultQueue.FinishTransaction(transaction);
                        break;

                    default: break;
                }
            }
        }
    }
}