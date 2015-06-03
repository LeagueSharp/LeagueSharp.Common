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
using System.Resources;
using System.Web.Script.Serialization;
using LeagueSharp.Common.Properties;

#endregion

namespace LeagueSharp.Common
{
    public static class MultiLanguage
    {
        private static Dictionary<string, string> Translations = new Dictionary<string, string>();

        static MultiLanguage()
        {
            LoadLanguage(Config.SelectedLanguage);
        }

        public static string _(string textToTranslate)
        {
            var textToTranslateToLower = textToTranslate.ToLower();
            return Translations.ContainsKey(textToTranslateToLower) ? Translations[textToTranslateToLower] : textToTranslate;
        }
      
        public static bool LoadLanguage(string languageName)
        {
            try
            {
                var languageStrings = new ResourceManager("LeagueSharp.Common.Properties.Resources", typeof(Resources).Assembly).GetString(languageName + "Json");
                
                if (String.IsNullOrEmpty(languageStrings))
                {
                    return false;
                }

                Translations = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(languageStrings);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
