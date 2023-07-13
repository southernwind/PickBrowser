using System.Windows.Data;

namespace PickBrowser.Converters; 
public class DoubleToPercentageConverter : IValueConverter {
	public object? Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
		if (value is not double num) {
			return "e";
		}

		return $"{num*100:F3}%";
	}

	public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
		throw new NotImplementedException();
	}

}
