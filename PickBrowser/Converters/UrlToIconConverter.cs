using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Data;

using MahApps.Metro.IconPacks;

namespace PickBrowser.Converters;
public class UrlToIconConverter : IValueConverter {
	public object? Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		if (value is not string url) {
			return null;
		}
		url = Regex.Replace(url, @"(\?|#).*$", "");
		if (url.StartsWith("data:image/")) {
			return PackIconOcticonsKind.FileMedia;
		}
		var extension = Path.GetExtension(url);
		switch (extension) {
			case "":
			case ".htm":
			case ".html":
				return null;//PackIconOcticonsKind.FileCode;
			case ".css":
				return null;//PackIconOcticonsKind.FileCode;
			case ".js":
				return null;//PackIconOcticonsKind.FileCode;
			case ".png":
			case ".jpg":
			case ".jpeg":
			case ".gif":
			case ".bmp":
			case ".mp4":
			case ".m4u8":
			case ".ts":
				return PackIconOcticonsKind.FileMedia;
			case ".pdf":
				return PackIconOcticonsKind.FilePdf;
		}
		return null;
	}

	public object? ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture) {
		throw new InvalidOperationException();
	}
}
