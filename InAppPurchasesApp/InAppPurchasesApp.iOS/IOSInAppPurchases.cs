using System;
using System.Threading.Tasks;
using Foundation;
using InAppPurchasesApp.iOS;
using InAppPurchasesApp.Protocol;
using InAppPurchasesApp.Protocol.Model;
using StoreKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(IOSInAppPurchases))]

namespace InAppPurchasesApp.iOS
{
    public class IOSInAppPurchases : IInAppPurchases, IDisposable
    {
        private IOSTransactionObserver transactionObserver;

        public IOSInAppPurchases()
        {
            this.transactionObserver = new IOSTransactionObserver();

            // 添加交易結果的 Observer
            SKPaymentQueue.DefaultQueue.AddTransactionObserver(this.transactionObserver);
        }

        public async Task<PaymentTransaction> PurchaseAsync(string productId)
        {
            var paymentTrans = await this.Purchase(productId);

            var reference = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            var purchase = new PaymentTransaction();
            purchase.TransactionUtcDate = reference.AddSeconds(paymentTrans.TransactionDate.SecondsSinceReferenceDate);
            purchase.Id = paymentTrans.TransactionIdentifier;
            purchase.ProductId = paymentTrans.Payment?.ProductIdentifier ?? string.Empty;
            purchase.State = paymentTrans.TransactionState.ToString();
            purchase.PurchaseToken =
                paymentTrans.TransactionReceipt?.GetBase64EncodedString(NSDataBase64EncodingOptions.None)
                ?? string.Empty;

            return purchase;
        }

        public void Dispose()
        {
            if (this.transactionObserver != null)
            {
                // 移除交易結果的 Observer
                SKPaymentQueue.DefaultQueue.RemoveTransactionObserver(this.transactionObserver);
                this.transactionObserver.Dispose();
                this.transactionObserver = null;
            }
        }

        private Task<SKPaymentTransaction> Purchase(string productId)
        {
            var tcsTransaction = new TaskCompletionSource<SKPaymentTransaction>();

            void Handler(SKPaymentTransaction trans, bool result)
            {
                if (trans?.Payment == null) return;

                if (productId != trans.Payment.ProductIdentifier) return;

                this.transactionObserver.TransactionCompleted -= Handler;

                if (result)
                {
                    tcsTransaction.TrySetResult(trans);
                    return;
                }

                var errorCode = trans.Error?.Code ?? -1;
                var description = trans.Error?.LocalizedDescription ?? string.Empty;

                tcsTransaction.TrySetException(new Exception($"交易失敗（errorCode: {errorCode}）\r\n{description}"));
            }

            this.transactionObserver.TransactionCompleted += Handler;

            // 購買
            SKPaymentQueue.DefaultQueue.AddPayment(SKPayment.CreateFrom(productId));

            return tcsTransaction.Task;
        }
    }
}