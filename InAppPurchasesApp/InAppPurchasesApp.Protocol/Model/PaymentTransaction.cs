using System;

namespace InAppPurchasesApp.Protocol.Model
{
    public class PaymentTransaction
    {
        public string Id { get; set; }

        public string ProductId { get; set; }

        public DateTime UtcDate { get; set; }

        public string State { get; set; }

        public string PurchaseToken { get; set; }
    }
}