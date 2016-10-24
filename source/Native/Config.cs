// <copyright file="Config.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.IO;
    using System.Reflection;
    using LeagueSharp.Sandbox;
    using log4net;
    using PlaySharp.Toolkit.Logging;

    /// <summary>
    ///     The platform config.
    /// </summary>
    public static class Config
    {
        #region Static Fields

        private static readonly ILog Log = AssemblyLogs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static string appDataDirectory;

        private static string selectedLanguage;

        private static byte showMenuPressKey;

        private static byte showMenuToggleKey;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the app data directory.
        /// </summary>
        public static string AppDataDirectory
            =>
            appDataDirectory
            ?? (appDataDirectory =
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "LS" + Environment.UserName.GetHashCode().ToString("X")));

        /// <summary>
        ///     Gets the selected language.
        /// </summary>
        public static string SelectedLanguage
        {
            get
            {
                if (selectedLanguage == null)
                {
                    try
                    {
                        selectedLanguage = SandboxConfig.SelectedLanguage;
                        if (selectedLanguage == "Traditional-Chinese")
                        {
                            selectedLanguage = "Chinese";
                        }

                        if (selectedLanguage.StartsWith("zh"))
                        {
                            selectedLanguage = "Chinese";
                        }
                    }
                    catch
                    {
                        selectedLanguage = string.Empty;
                        Log.Error("Unable to get the menu language.");
                    }
                }

                return selectedLanguage;
            }
        }

        /// <summary>
        ///     Gets the show menu press key.
        /// </summary>
        public static byte ShowMenuPressKey
        {
            get
            {
                if (showMenuPressKey == 0)
                {
                    try
                    {
                        showMenuPressKey = (byte)SandboxConfig.MenuKey;
                        showMenuPressKey = showMenuPressKey == 0 ? (byte)16 : showMenuPressKey;
                        showMenuPressKey = Utils.FixVirtualKey(showMenuPressKey);

                        Log.Info($"Menu press key set to {showMenuPressKey}");
                    }
                    catch
                    {
                        showMenuPressKey = 16;
                        Log.Error("Unable to get menu press key.");
                    }
                }

                return showMenuPressKey;
            }
        }

        /// <summary>
        ///     Gets the show menu toggle key.
        /// </summary>
        public static byte ShowMenuToggleKey
        {
            get
            {
                if (showMenuToggleKey == 0)
                {
                    try
                    {
                        showMenuToggleKey = (byte)SandboxConfig.MenuToggleKey;
                        showMenuToggleKey = showMenuToggleKey == 0 ? (byte)120 : showMenuToggleKey;
                        showMenuToggleKey = Utils.FixVirtualKey(showMenuToggleKey);

                        Log.Info($"Menu toggle key set to {showMenuToggleKey}");
                    }
                    catch
                    {
                        showMenuToggleKey = 120;
                        Log.Error("Unable to get menu toggle key.");
                    }
                }

                return showMenuToggleKey;
            }
        }

        #endregion
    }
}