using ShutdownPC.Models.Enums;
using System.Globalization;
using System.Windows.Data;

namespace ShutdownPC.Helpers.Convertores
{
	/// <summary>
	/// Konvertní funkce, která přidá ke vstupní hodnotě 0 pokud je hodnota menší než 10. 
	/// Převede číslo na text.
	/// </summary>
	public class TimeStyleConventor : IValueConverter
	{
		/// <summary>
		/// Metoda převádí číselnou hodnotu na textový řetězec, přidává nulu před jednociferné číslo.
		/// </summary>
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