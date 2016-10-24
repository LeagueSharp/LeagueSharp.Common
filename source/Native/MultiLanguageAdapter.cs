// <copyright file="MultiLanguageAdapter.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    /// <summary>
    ///     Provides multi language support.
    /// </summary>
    public partial class MultiLanguage
    {
        #region Properties

        /// <summary>
        ///     Gets the translations.
        /// </summary>
        private static IDictionary<string, MultiLanguage> Translations { get; } =
            new Dictionary<string, MultiLanguage>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Translates the text into the selected language.
        /// </summary>
        /// <param name="text">
        ///     The text.
        /// </param>
        /// <param name="language">
        ///     The language to translate to, null for default.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string _(string text, [CanBeNull] string language = null)
        {
            if (string.IsNullOrEmpty(language))
            {
                return Translations.Values.FirstOrDefault()?.Translate(text) ?? text;
            }

            MultiLanguage value;
            return Translations.TryGetValue(language, out value) ? value.Translate(text) : null;
        }

        /// <summary>
        ///     Loads the language.
        /// </summary>
        /// <param name="languageName">
        ///     The language name.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool LoadLanguage(string languageName)
        {
            var translation = new MultiLanguage(languageName);
            if (translation.IsValid)
            {
                Translations.Add(languageName, translation);
                return true;
            }

            return false;
        }

        #endregion
    }
}