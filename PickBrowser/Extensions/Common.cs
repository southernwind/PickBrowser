using System.IO;

namespace PickBrowser.Extensions;
public static class Common {
	public static string FilePathEscape(this string source) {
		return string.Concat(source.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c));
	}
}
