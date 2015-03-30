// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MANAGER.Classes
{
    public static class Transharp
    {
        public enum LangsEnum
        {
            Deutsch,
            English,
            French,
            Spanish
        }

        private const String LangsFolder = "Language"; // langs folder
        private const String LangFileExt = ".lang"; // File extension
        private const char Separator = '=';
        private const String Placeholder = "%x";
        private static LangsEnum _currentLanguage = LangsEnum.English; //Default

        public static void SetCurrentLanguage(LangsEnum lang)
        {
            _currentLanguage = lang;
        }

        public static string getCurrentLanguage()
        {
            return _currentLanguage.ToString();
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
            var strToFormat = GetTranslation(key, lang);
            if(strToFormat == null)
            {
                return null; // Translation not found for the given key and lang
            }
            var index = 0;
            //Replacing <Placeholder> by {0}, {1} etc
            strToFormat = Regex.Replace(strToFormat, @Placeholder, delegate { return "{" + index++ + "}"; });
            return String.Format(strToFormat, values); // Format and return the translation
        }

        public static String GetTranslation(String key, LangsEnum lang)
        {
            var filePath = GetLangFilePath(lang);
            const Int32 bufferSize = 1024;
            using(var fileStream = File.OpenRead(filePath))
            {
                using(var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, bufferSize))
                {
                    String line;
                    while((line = streamReader.ReadLine()) != null)
                    {
                        var parts = line.Split(new[] {Separator}, 2); // Split only on 1st occurence (we want 2 parts max)
                        var fileKey = parts[0]; // Left part of the separator
                        if(!key.Equals(fileKey))
                        {
                            continue; //Not the correct key, we go to next loop
                        }
                        return parts[1]; // Right part of the separator
                    }
                }
            }
            return null; // Translation not found for the given key and lang
        }

        private static String GetLangFilePath(LangsEnum lang)
        {
            var str = new StringBuilder();
            str.Append(LangsFolder).Append("/").Append(lang.ToString().ToLower()).Append(LangFileExt);
            return str.ToString();
        }
    }
}