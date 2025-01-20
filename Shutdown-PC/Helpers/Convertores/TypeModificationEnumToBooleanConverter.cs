using ShutdownPC.Models.Enums;
using System.Globalization;
using System.Windows.Data;

namespace ShutdownPC.Helpers.Convertores
{
	/// <summary>
	/// Konvertní funkce překontroluje, jestli TypeModification jsou stejný. 
	/// Podle výsledku vrátí True/False.
	/// </summary>
	public class TypeModificationEnumToBooleanConverter : IValueConverter
	{
		/// <summary>
		/// Metoda Convert porovnává hodnotu typu eTypeModification s parametrem.
		/// Pokud jsou stejné, vrátí True, jinak False.
		/// </summary>
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