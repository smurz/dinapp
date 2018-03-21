using Windows.Foundation.Collections;
using Windows.Storage;

namespace DINAPP.Libs.Client.UWP
{
    public class AppSettingsInterop
    {
        private const string UserWalletSettingsKey = "DinappPurchaseDialog_userwalletaddress";
        private readonly IPropertySet _settings;

        public AppSettingsInterop()
        {
            _settings = ApplicationData.Current.LocalSettings.Values;
        }

        /// <summary>
        /// Saves user wallet address to application settings to use it in further instances of the dialog.
        /// </summary>
        /// <param name="userWalletAddress">User wallet address</param>
        public void SaveUserWalletAddress(string userWalletAddress)
        {
            _settings[UserWalletSettingsKey] = userWalletAddress;
        }

        /// <summary>
        /// Returns stored user wallet address
        /// </summary>
        /// <returns>User wallet address</returns>
        public string GetStoredUserWalletKey()
        {
            if (_settings.ContainsKey(UserWalletSettingsKey))
                return _settings[UserWalletSettingsKey] as string;

            return null;
        }
    }
}
