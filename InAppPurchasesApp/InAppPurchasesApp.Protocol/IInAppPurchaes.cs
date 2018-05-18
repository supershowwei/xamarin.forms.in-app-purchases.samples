using System.Threading.Tasks;
using InAppPurchasesApp.Protocol.Model;

namespace InAppPurchasesApp.Protocol
{
    public interface IInAppPurchaes
    {
        Task<PaymentTransaction> PurchaseAsync(string productId);
    }
}