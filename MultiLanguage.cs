namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Resources;
    using System.Web.Script.Serialization;

    using LeagueSharp.Common.Properties;

    /// <summary>
    ///     Provides multi-lingual strings.
    /// </summary>
    public static class MultiLanguage
    {
        #region Static Fields

        /// <summary>
        ///     The translations
        /// </summary>
        private static Dictionary<string, string> Translations = new Dictionary<string, string>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="MultiLanguage" /> class.
        /// </summary>
        static MultiLanguage()
        {
            LoadLanguage(Config.SelectedLanguage);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Translates the text into the loaded language.
        /// </summary>
        /// <param name="textToTranslate">The text to translate.</param>
        /// <returns>System.String.</returns>
        public static string _(string textToTranslate)
        {
            var textToTranslateToLower = textToTranslate.ToLower();
            return Translations.ContainsKey(textToTranslateToLower)
                       ? Translations[textToTranslateToLower]
                       : textToTranslate;
        }

        /// <summary>
        ///     Loads the language.
        /// </summary>
        /// <param name="languageName">Name of the language.</param>
        /// <returns><c>true</c> if the operation succeeded, <c>false</c> otherwise false.</returns>
        public static bool LoadLanguage(string languageName)
        {
            try
            {
                var languageStrings =
                    new ResourceManager("LeagueSharp.Common.Properties.Resources", typeof(Resources).Assembly).GetString
                        (languageName + "Json");

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

        #endregion
    }
}