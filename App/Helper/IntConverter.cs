using System;
using Xamarin.Forms;
using System.Globalization;

namespace App
{
	public class IntConverter : IValueConverter
	{
		public object Convert (
			object value,
			Type targetType,
			object parameter,
			CultureInfo culture)
		{
			int theInt = (int)value;
			return theInt.ToString ();
		}

		public object ConvertBack (
			object value,
			Type targetType,
			object parameter,
			CultureInfo culture)
		{
			string strValue = value as string;
			if (string.IsNullOrEmpty (strValue))
				return 0;
			int resultInt;
			if (int.TryParse (strValue, out resultInt)) {
				return resultInt;
			}
			return 0;
		}
	}
}
