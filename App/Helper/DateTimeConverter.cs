using System;
using Xamarin.Forms;
using System.Globalization;

namespace App
{
	public class DateTimeConverter : IValueConverter
	{
		public object Convert (
			object value,
			Type targetType,
			object parameter,
			CultureInfo culture)
		{
			DateTime theInt = (DateTime)value;
			return theInt.ToString();
		}

		public object ConvertBack (
			object value,
			Type targetType,
			object parameter,
			CultureInfo culture)
		{
			string strValue = value as string;
			if (string.IsNullOrEmpty (strValue))
				return DateTime.MinValue;
			DateTime result;
			if (DateTime.TryParse(strValue, out result)) {
				return result;
			}
			return DateTime.MinValue;
		}
	}
}
