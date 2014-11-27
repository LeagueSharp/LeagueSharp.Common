using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace LeagueSharp.Common
{
    public static class MultiLanguage
    {
        public static Dictionary<string, string> Translations = new Dictionary<string, string>();

        public static XmlSerializer Serializer = new XmlSerializer(
            typeof(TranslatedEntry[]), new XmlRootAttribute { ElementName = "entries" });

        static MultiLanguage()
        {
            LoadLanguage(Config.SelectedLanguage);
        }

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
        }

        public static string _(string textToTranslate)
        {
            return Translations.ContainsKey(textToTranslate) ? Translations[textToTranslate] : textToTranslate;
        }

        public class TranslatedEntry
        {
            [XmlAttribute] public string TextToTranslate;
            [XmlAttribute] public string TranslatedText;
        }
    }
}