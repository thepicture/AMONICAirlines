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
            if (!(values[0] is Schedules schedule))
            {
                return 0;
            }
            if (!(values[1] is CabinTypes cabinType))
            {
                return 0;
            }
            switch (cabinType.Name)
            {
                case "Business":
                    return schedule.BusinessPrice;
                case "First Class":
                    return schedule.FirstClassPrice;
                case "Economy":
                default:
                    return schedule.EconomyPrice;
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
