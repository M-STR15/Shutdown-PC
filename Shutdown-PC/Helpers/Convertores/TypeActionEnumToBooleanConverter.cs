using ShutdownPC.Models.Enums;
using System.Globalization;
using System.Windows.Data;

namespace ShutdownPC.Helpers.Convertores
{
	/// <summary>
	/// Konvertní funkce překontroluje, jestli TypeAction jsou stejný. 
	/// Podle výsledku vrátí True/False.
	/// </summary>
	public class TypeActionEnumToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (eTypeAction)value == (eTypeAction)parameter;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (eTypeAction)parameter;
		}
	}
}