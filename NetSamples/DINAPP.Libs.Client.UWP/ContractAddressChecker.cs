using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DINAPP.Libs.Client.UWP
{
    internal class ContractAddressChecker
    {
        private const string ValidAddressRegex = "^0x[0-9a-f]{40}$";

        /// <summary>
        /// Returns true if address is a valid ethereum address
        /// </summary>
        /// <param name="address">contract or wallet address</param>
        /// <returns></returns>
        public bool CheckAddress(string address)
        {
            if (string.IsNullOrEmpty(address)) return false;
            if (!Regex.IsMatch(address, ValidAddressRegex)) return false;

            return true;
        }
    }
}
