using System;
using System.Threading.Tasks;
using InAppPurchasesApp.Protocol;
using Xamarin.Forms;

namespace InAppPurchasesApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Purchase(object sender, EventArgs e)
        {
            var inAppPurchases = DependencyService.Get<IInAppPurchaes>();

            try
            {
                var paymentTransaction = await inAppPurchases.PurchaseAsync("iaptest.product.1week");

                //var paymentTransaction = await Task.Run(() =>
                //    {
                //        var purchaseTask = inAppPurchases.PurchaseAsync("iaptest.product.1week");

                //        purchaseTask.Wait();

                //        return purchaseTask.Result;
                //    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}