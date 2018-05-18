using System;
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}