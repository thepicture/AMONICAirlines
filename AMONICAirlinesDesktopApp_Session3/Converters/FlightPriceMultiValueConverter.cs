using AMONICAirlinesDesktopApp_Session3.Models.Entities;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AMONICAirlinesDesktopApp_Session3.Converters
{
    public class FlightPriceMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            if (values.Length == 0)
            {
                return null;
            }
            if (!(values[0] is decimal economyPrice))
            {
                return 0;
            }
            if (!(values[1] is CabinTypes cabinType))
            {
                return 0;
            }
            switch (cabinType.Name)
            {
                case "Economy":
                    return economyPrice;
                case "Business":
                    return economyPrice * (decimal)1.35;
                case "First Class":
                    return economyPrice
                           * (decimal)1.35
                           * (decimal)1.30;
                default:
                    return economyPrice;
            }
        }

        public object[] ConvertBack(object value,
                                    Type[] targetTypes,
                                    object parameter,
                                    CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
