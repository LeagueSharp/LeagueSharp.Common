// <copyright file="MultiLanguage.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Resources;
    using System.Web.Script.Serialization;

    using LeagueSharp.Common.Properties;

    using log4net;

    using PlaySharp.Toolkit.Logging;

    /// <summary>
    ///     Provides multi-lingual strings.
    /// </summary>
    public partial class MultiLanguage
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="MultiLanguage" /> class.
        /// </summary>
        static MultiLanguage()
        {
            LoadLanguage(Config.SelectedLanguage);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MultiLanguage" /> class.
        /// </summary>
        /// <param name="language">
        ///     The language.
        /// </param>
        public MultiLanguage(string language)
        {
            var lang =
                new ResourceManager("LeagueSharp.Common.Properties.Resources", typeof(Resources).Assembly).GetString(
                    language + "Json");
            if (string.IsNullOrEmpty(lang))
            {
                return;
            }

            try
            {
                this.LanguageTranslations = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(lang);
            }
            catch (Exception)
            {
                this.Log.Error($"Unable to load language {language}.");
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether the multi language translation is valid.
        /// </summary>
        public bool IsValid => this.LanguageTranslations != null;

        #endregion

        #region Properties

        private IDictionary<string, string> LanguageTranslations { get; }

        private ILog Log { get; } = AssemblyLogs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Translates the text.
        /// </summary>
        /// <param name="text">
        ///     The text.
        /// </param>
        /// <returns>
        ///     The translated text, if available.
        /// </returns>
        public string Translate(string text)
        {
            string value;
            return this.LanguageTranslations.TryGetValue(text.ToLower(), out value) ? value : text;
        }

        #endregion
    }
}