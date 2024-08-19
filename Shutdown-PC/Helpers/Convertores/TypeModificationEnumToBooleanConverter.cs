using ShutdownPC.Models.Enums;
using System.Globalization;
using System.Windows.Data;

namespace ShutdownPC.Helpers.Convertores
{
	public class TypeModificationEnumToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (eTypeModification)value == (eTypeModification)parameter;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (eTypeModification)parameter;
		}
	}
}