using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DINAPP.Libs.Client.Interfaces;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DINAPP.Libs.Client.UWP
{
    public sealed partial class InAppPurchaseDialog : ContentDialog
    {
        #region ui strings
        private const string SubscriptionStatusTitle = "Your subscription status is:";
        private const string ConsumableStatusTitle = "Number of products owned:";

        private const string SubscriptionActiveString = "Active";
        private const string NotSubscribedString = "Not subscribed";
        #endregion

        //external dependecies
        private readonly string _inAppAddress;
        private readonly IInAppService _inAppService;

        //internal dependencies
        private readonly ContractAddressChecker _contractAddressChecker;
        private readonly QrCodeCanvasBuilder _canvasBuilder;
        private readonly AppSettingsInterop _appSettings;

        private IInAppInfo _inAppInfo;
        private IStorageFile _keyStorageFile;

        public event EventHandler<Exception> ExceptionThrown;
        public event EventHandler<string> DialogSceenNavigated;

        /// <summary>
        /// Displays the in-app purchase dialog for published in-app contract
        /// </summary>
        /// <param name="inAppContractAddress">In-app purchase contract address</param>
        /// <param name="inAppService">In-App service</param>
        public InAppPurchaseDialog(string inAppContractAddress, IInAppService inAppService)
        {
            _contractAddressChecker = new ContractAddressChecker();
            if (!_contractAddressChecker.CheckAddress(inAppContractAddress)) throw new ArgumentException("In-App contract address is not valid", nameof(inAppContractAddress));

            _inAppAddress = inAppContractAddress;
            _inAppService = inAppService ?? throw new ArgumentNullException(nameof(inAppService));

            _canvasBuilder = new QrCodeCanvasBuilder();
            _appSettings = new AppSettingsInterop();

            //component init
            this.InitializeComponent();
            Loaded += InAppPurchaseDialog_Loaded;
        }

        public string UserWalletAddress { get; set; }

        private async void InAppPurchaseDialog_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowLoadingScreen();

                //get in-app info from service async
                _inAppInfo = await _inAppService.InAppInfoReader.GetProductInfoAsync(_inAppAddress);
                DialogLayout.DataContext = _inAppInfo;

                //get qr code from In-App service
                var matrix = _inAppService.Payment.GetQrCodeMatrix(_inAppInfo);

                //render qr
                var canvas = _canvasBuilder.BuildQrCodeCanvas(matrix, QrCodeCanvasSlot.Width, QrCodeCanvasSlot.Height);
                QrCodeCanvasSlot.Children.Add(canvas);

                //get user wallet from settings
                UserWalletAddress = _appSettings.GetStoredUserWalletKey();

                if (_contractAddressChecker.CheckAddress(UserWalletAddress))
                {
                    UserWalletPublicKeyTextBox.Text = UserWalletAddress;

                    //check in app status for this address async
                    //if in-app purchase type is Permanent = 0 or Subscription = 1 then we should check its status
                    //before we show all other info.
                    if (_inAppInfo.InAppType < 2)
                    {
                        var subscriptionStatus =
                            await _inAppService.LicenseChecker.GetSubscriptionStatusAsync(_inAppAddress, UserWalletAddress);

                        //show the license status screen (which is the last screen) if the subscription is active.
                        if (subscriptionStatus.IsActive)
                        {
                            ShowSubscriptionStatusScreen(true);
                            return;
                        }
                    }
                }

                //show first screen
                ShowInAppInfoScreen();
            }
            catch (Exception ex)
            {
                OnExceptionThrown(ex);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            //show second screen
            ShowUserWalletScreen();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //validate walletaddress
                if (_contractAddressChecker.CheckAddress(UserWalletPublicKeyTextBox.Text))
                {
                    //save user wallet address for further use in parent application
                    UserWalletAddress = UserWalletPublicKeyTextBox.Text;

                    //switch page to payment
                    ShowPaymentScreen();

                    //save user wallet address to application settings to have access to it after this session ends
                    _appSettings.SaveUserWalletAddress(UserWalletAddress);
                }
                //todo: show 'ivalid address' error message


            }
            catch (Exception ex)
            {
                OnExceptionThrown(ex);
            }
        }

        private async void PayByUri_click(object sender, RoutedEventArgs e)
        {
            try
            {
                //get ethereum uri scheme for this in-app product
                var ethUriScheme = _inAppService.Payment.GetPaymentUriScheme(_inAppInfo);
                var uri = new Uri(ethUriScheme);

                //launch uri. It will be handeled if user has any app that supports this uri scheme.
                var launchResult =
                    await Launcher.LaunchUriAsync(uri, new LauncherOptions() { DisplayApplicationPicker = true });
            }
            catch (Exception ex)
            {
                OnExceptionThrown(ex);
            }
        }

        private void PayDirectly_click(object sender, RoutedEventArgs e)
        {
            ShowDirectPaymentScreen();
        }

        private async void BrowseKeyStorage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filePicker = new FileOpenPicker();
                filePicker.FileTypeFilter.Add("*");
                _keyStorageFile = await filePicker.PickSingleFileAsync();

                if (_keyStorageFile != null)
                {
                    KeyStoragePathTextBox.Text = _keyStorageFile.Path;
                }
            }
            catch (Exception ex)
            {
                OnExceptionThrown(ex);
            }
        }

        private async void SendEther_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(PasswordBox.Password)) return;
                if (_keyStorageFile == null) return;

                //authorized payment algorithm

                //read user password
                var password = PasswordBox.Password;

                //read user keystore file
                var keystore = await FileIO.ReadTextAsync(_keyStorageFile);

                if (string.IsNullOrEmpty(keystore)) return;

                //create new instance of KeyStorageInfo
                var keyStorageInfo = new KeyStorageInfo(password, keystore);

                //gas price = 20 Gwei. set it to what ever price is apropriate.
                var gasPrice = new BigInteger(20000000000);

                //handle payment
                //PayAuthorizedAsync method return true if payment was successfull
                var result = await _inAppService.Payment.PayAuthorizedAsync(_inAppAddress, UserWalletAddress, keyStorageInfo, gasPrice);

                if (result)
                {
                    //check in app purchase status and show the last screen of the dialog
                    await CheckInAppStatus();
                }
            }
            catch (Exception ex)
            {
                OnExceptionThrown(ex);
            }
        }


        private async void CheckStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await CheckInAppStatus();
            }
            catch (Exception ex)
            {
                OnExceptionThrown(ex);
            }
        }

        private void PublicKeyBack_Click(object sender, RoutedEventArgs e)
        {
            ShowInAppInfoScreen();
        }


        private void PaymentBack_Click(object sender, RoutedEventArgs e)
        {
            ShowUserWalletScreen();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ShowPaymentScreen();
        }

        #region dialog navigation

        private void ShowInAppInfoScreen()
        {
            HideAllScreens();
            InAppInfoScreen.Visibility = Visibility.Visible;
            OnDialogNavigated("InAppInfoScreen");
        }

        private void ShowUserWalletScreen()
        {
            HideAllScreens();
            EnterUserPublicKeyScreen.Visibility = Visibility.Visible;
            OnDialogNavigated("UserWalletScreen");
        }

        private void ShowPaymentScreen()
        {
            HideAllScreens();
            PaymentMethodsScreen.Visibility = Visibility.Visible;
            OnDialogNavigated("PaymentScreen");
        }

        private void ShowSubscriptionStatusScreen(bool isSubscriptionActive)
        {
            HideAllScreens();

            InAppStatusTitle.Text = SubscriptionStatusTitle;
            InAppStatusValue.Text = isSubscriptionActive ? SubscriptionActiveString : NotSubscribedString;

            InAppSubscriptionStatusScreen.Visibility = Visibility.Visible;
            OnDialogNavigated("SubscriptionStatusScreen");
        }

        private void ShowConsumableStatusScreen(BigInteger number)
        {
            HideAllScreens();

            InAppStatusTitle.Text = ConsumableStatusTitle;
            InAppStatusValue.Text = number.ToString();

            InAppSubscriptionStatusScreen.Visibility = Visibility.Visible;

            OnDialogNavigated("ConsumableStatusScreen");
        }

        private void ShowDirectPaymentScreen()
        {
            HideAllScreens();
            DirectPaymentScreen.Visibility = Visibility.Visible;

            OnDialogNavigated("DirectPaymentScreen");
        }

        private void ShowLoadingScreen()
        {
            HideAllScreens();
            LoadingScreen.Visibility = Visibility.Visible;
        }

        private void HideAllScreens()
        {
            LoadingScreen.Visibility = Visibility.Collapsed;
            InAppInfoScreen.Visibility = Visibility.Collapsed;
            EnterUserPublicKeyScreen.Visibility = Visibility.Collapsed;
            PaymentMethodsScreen.Visibility = Visibility.Collapsed;
            InAppSubscriptionStatusScreen.Visibility = Visibility.Collapsed;
            DirectPaymentScreen.Visibility = Visibility.Collapsed;
        }

        #endregion

        private async Task CheckInAppStatus()
        {
            try
            {
                ShowLoadingScreen();

                if (_contractAddressChecker.CheckAddress(UserWalletAddress))
                {
                    //check in app status for this address async
                    if (_inAppInfo.InAppType < 2)
                    {
                        var subscriptionStatus =
                            await _inAppService.LicenseChecker.GetSubscriptionStatusAsync(_inAppAddress, UserWalletAddress);

                        ShowSubscriptionStatusScreen(subscriptionStatus.IsActive);
                        return;
                    }
                    else
                    {
                        var consumableStatus =
                            await _inAppService.LicenseChecker.GetConsumableStatusAsync(_inAppAddress, UserWalletAddress);

                        ShowConsumableStatusScreen(consumableStatus.Count);
                        return;
                    }
                }

                ShowPaymentScreen();
            }
            catch (Exception ex)
            {
                OnExceptionThrown(ex);
                ShowPaymentScreen();
            }
        }

        private void OnExceptionThrown(Exception ex)
        {
            ExceptionThrown?.Invoke(this, ex);
            Hide();
        }

        private void OnDialogNavigated(string screenName)
        {
            DialogSceenNavigated?.Invoke(this, screenName);
        }

    }
}
