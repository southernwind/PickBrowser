using System.Globalization;
using System.Web;
using System.Windows.Data;

namespace PickBrowser.Converters;
public class DecodeUrlConverter : IValueConverter {
	public object? Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		if (value is string str) {
			return HttpUtility.UrlDecode(str);
		}
		return null;
	}

	public object? ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture) {
		throw new InvalidOperationException();
	}
}
