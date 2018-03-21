using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace DINAPP.Libs.Client.UWP
{
    /// <summary>
    /// Simplifies prices readability, which are in Wei 
    /// </summary>
    internal class WeiToReadablePriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is BigInteger bgInteger)) return "0 Eth";

            var dValue = (double)bgInteger;
            var digits = Math.Ceiling(Math.Log10(dValue));

            if (digits < 4) return value + " Wei";

            if (digits < 7)
            {
                var kwei = dValue / Math.Pow(10, 3);
                return kwei.ToString("F3") + " Kwei";
            }

            if (digits < 10)
            {
                var mwei = dValue / Math.Pow(10, 6);
                return mwei.ToString("F3") + " Mwei";
            }

            if (digits < 13)
            {
                var gwei = dValue / Math.Pow(10, 9);
                return gwei.ToString("F3") + " Gwei";
            }

            if (digits < 16)
            {
                var szabo = dValue / Math.Pow(10, 12);
                return szabo.ToString("F3") + " Szabo";
            }

            if (digits < 19)
            {
                var finney = dValue / Math.Pow(10, 15);
                return finney.ToString("F3") + " Finney";
            }

            if (digits >= 19)
            {
                var eth = dValue / Math.Pow(10, 18);
                return eth.ToString("F3") + " Eth";
            }

            return value + " Wei";
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
