#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 MultiLanguage.cs is part of LeagueSharp.Common.
 
 LeagueSharp.Common is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 LeagueSharp.Common is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with LeagueSharp.Common. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

#endregion

namespace LeagueSharp.Common
{
    public static class MultiLanguage
    {
        public static Dictionary<string, string> Translations = new Dictionary<string, string>();

       /* public static readonly XmlSerializer Serializer = new XmlSerializer(
            typeof(TranslatedEntry[]), new XmlRootAttribute { ElementName = "entries" });
        */
        static MultiLanguage()
        {
            //LoadLanguage(Config.SelectedLanguage);
        }

        public static string _(string textToTranslate)
        {
            return Translations.ContainsKey(textToTranslate) ? Translations[textToTranslate] : textToTranslate;
        }
        /*
        public static bool LoadLanguage(string name)
        {
            var filePath = Path.Combine(Config.LeagueSharpDirectory, "translations", name + ".xml");

            if (!File.Exists(filePath))
            {
                return false;
            }

            try
            {
                Translations =
                    ((TranslatedEntry[]) Serializer.Deserialize(File.OpenRead(filePath))).ToDictionary(
                        i => i.TextToTranslate, i => i.TranslatedText);
                return true;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString());
                return false;
            }
        }*/

        public class TranslatedEntry
        {
            [XmlAttribute] public string TextToTranslate;
            [XmlAttribute] public string TranslatedText;
        }
    }
}