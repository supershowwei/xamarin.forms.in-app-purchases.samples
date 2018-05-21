using System.Threading.Tasks;
using InAppPurchasesApp.Protocol.Model;

namespace InAppPurchasesApp.Protocol
{
    public interface IInAppPurchases
    {
        Task<PaymentTransaction> PurchaseAsync(string productId);
    }
}