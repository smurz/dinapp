using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DINAPP.Libs.Client;
using DINAPP.Libs.Client.Interfaces;
using DINAPP.Libs.Client.UWP;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace SampleClient.UWP
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IInAppService _inAppService;
        private InAppPurchaseDialog _inAppDialog;

        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Create service builder instance with specified ethereum network.
            //Use Main Net for your products and rinkeby or ropsten for test purposes.
            var inAppServiceBuilder = new InAppSeviceBuilder(EthereumNetwork.Rinkeby);

            _inAppService = inAppServiceBuilder.Build();


            //we have some test user addresses with active subscriptions and some consumables bought
            //use this settings to see how in-app purchase dialog behaves with active subscription.
            //rinkeby test user: "0x3d082697579c08ed9da629ad19c736a9b26b64e6"
            //ropsten test user: "0x20f8383693209be170c2042b9291c43fe85f585b"

            var settings = new AppSettingsInterop();
            settings.SaveUserWalletAddress("");
            //settings.SaveUserWalletAddress("0x3d082697579c08ed9da629ad19c736a9b26b64e6");
        }

        //we have some in-app contracts already deployed in test networks for test purposes

        private async void BuyPermanent_Click(object sender, RoutedEventArgs e)
        {
            // InAppPurchaseDialog inherits ContentDialog so it needs to be hidden first.
            _inAppDialog?.Hide();

            //show In-App purchase dialog for permanent subscription
            //rinkeby address: "0xa136a40c7e713542960d910ae9c81240b37b4145"
            //ropsten address: "0x70fe8d22d4bb4cefcbeaec88076169ecf6e93d46"
            _inAppDialog = new InAppPurchaseDialog("0xa136a40c7e713542960d910ae9c81240b37b4145", _inAppService);
            await _inAppDialog.ShowAsync();

        }

        private async void BuySubscription_Click(object sender, RoutedEventArgs e)
        {
            // InAppPurchaseDialog inherits ContentDialog so it needs to be hidden first.
            _inAppDialog?.Hide();

            //show In-App purchase dialog for time-limited subscription
            //rinkeby address: "0xa9589de3fab16dac92ed7f30000bcf087561335e"
            //ropsten address: "0xcf4709a4b4e3ae9333dab13ed44b8d06ece6da9e"
            _inAppDialog = new InAppPurchaseDialog("0xa9589de3fab16dac92ed7f30000bcf087561335e", _inAppService);
            await _inAppDialog.ShowAsync();
        }

        private async void BuyConsumable_Click(object sender, RoutedEventArgs e)
        {
            // InAppPurchaseDialog inherits ContentDialog so it needs to be hidden first.
            _inAppDialog?.Hide();

            //show In-App purchase dialog for consumable in-app product
            //rinkeby address: "0x7ea5c56768718fbc22718f45b1995ddf5e2d545a"
            //ropsten address: "0x6e1ac83fab32a0fafa51e400c594569c7de07fa1"
            _inAppDialog = new InAppPurchaseDialog("0x7ea5c56768718fbc22718f45b1995ddf5e2d545a", _inAppService);
            await _inAppDialog.ShowAsync();
        }

    }
}
