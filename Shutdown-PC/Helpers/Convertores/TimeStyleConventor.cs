using ShutdownPC.Models.Enums;
using System.Globalization;
using System.Windows.Data;

namespace ShutdownPC.Helpers.Convertores
{
	public class TimeStyleConventor : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is int)
			{
				var intValue = (int)value;
				return (intValue < 10 ? "0" : "") + value.ToString();
			}
			else
			{
				return "";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (eTypeModification)parameter;
		}
	}
}