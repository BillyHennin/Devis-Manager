using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MANAGER.Classes
{
	public class Transharp
	{
		public enum LangsEnum
		{
			En,
			Fr,
			Es,
			De
		}

		private static LangsEnum _currentLanguage = LangsEnum.En; //Default
		private const String LangsFolder = "language"; // langs folder
		private const String LangFileExt = ".lang"; // File extension
		private const char Separator = '=';
		private const String Placeholder = "%x";

		public static void SetCurrentLanguage(LangsEnum lang)
		{
			_currentLanguage = lang;
		}

		public static String GetTranslation(String key)
		{
			return GetTranslation(key, _currentLanguage);
		}

		public static String GetTranslation(String key, params Object[] values)
		{
			return GetTranslation(key, _currentLanguage, values);
		}

		public static String GetTranslation(String key, LangsEnum lang, params Object[] values)
		{
			String strToFormat = GetTranslation(key, lang);
			if (strToFormat != null)
			{
				int index = 0;
				//Replacing <Placeholder> by {0}, {1} etc
				strToFormat = Regex.Replace(strToFormat, @Placeholder, delegate { return "{" + index++ + "}"; });
				return String.Format(strToFormat, values); // Format and return the translation
			}
			return null; // Translation not found for the given key and lang
		}

		public static String GetTranslation(String key, LangsEnum lang)
		{
			String filePath = GetLangFilePath(lang);
			const Int32 bufferSize = 1024;
			using (var fileStream = File.OpenRead(filePath))
			{
				using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, bufferSize))
				{
					String line;
					while ((line = streamReader.ReadLine()) != null)
					{
						String[] parts = line.Split(new[] {Separator}, 2); // Split only on 1st occurence (we want 2 parts max)
						String fileKey = parts[0]; // Left part of the separator
						if (!key.Equals(fileKey)) continue; //Not the correct key, we go to next loop
						return parts[1]; // Right part of the separator
					}
				}
			}
			return null; // Translation not found for the given key and lang
		}

		private static String GetLangFilePath(LangsEnum lang)
		{
			StringBuilder str = new StringBuilder();
			str.Append(LangsFolder).Append("/").Append(lang.ToString().ToLower()).Append(LangFileExt);
			return str.ToString();
		}
	}
}