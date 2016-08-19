namespace LeagueSharp.Common
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Security.Permissions;
    using System.Threading;

    using LeagueSharp.Sandbox;

    /// <summary>
    ///     Gets information about the L# system.
    /// </summary>
    public static class Config
    {
        #region Static Fields

        /// <summary>
        ///     The app data directory
        /// </summary>
        private static string _appDataDirectory;

        /// <summary>
        ///     The league sharp directory
        /// </summary>
        private static string _leagueSharpDirectory;

        /// <summary>
        ///     The selected language
        /// </summary>
        private static string _selectedLanguage;

        /// <summary>
        ///     The show menu hotkey
        /// </summary>
        private static byte _showMenuHotkey;

        /// <summary>
        ///     The show menu toggle hotkey
        /// </summary>
        private static byte _showMenuToggleHotkey;

        #endregion

        #region Constructors and Destructors

        [PermissionSet(SecurityAction.Assert, Unrestricted = true)]
        static Config()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the application data directory.
        /// </summary>
        /// <value>
        ///     The application data directory.
        /// </value>
        public static string AppDataDirectory
        {
            get
            {
                if (_appDataDirectory == null)
                {
                    _appDataDirectory =
                        Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                            "LS" + Environment.UserName.GetHashCode().ToString("X"));
                }

                return _appDataDirectory;
            }
        }

        /// <summary>
        ///     Gets the selected language.
        /// </summary>
        /// <value>
        ///     The selected language.
        /// </value>
        public static string SelectedLanguage
        {
            get
            {
                if (_selectedLanguage == null)
                {
                    try
                    {
                        _selectedLanguage = SandboxConfig.SelectedLanguage;
                        if (_selectedLanguage == "Traditional-Chinese")
                        {
                            _selectedLanguage = "Chinese";
                        }

                        if (_selectedLanguage.StartsWith("zh"))
                        {
                            _selectedLanguage = "Chinese";
                        }
                    }
                    catch (Exception)
                    {
                        _selectedLanguage = "";
                        Console.WriteLine(@"Could not get the menu language");
                    }
                }

                return _selectedLanguage;
            }
        }

        /// <summary>
        ///     Gets the show menu press key.
        /// </summary>
        /// <value>
        ///     The show menu press key.
        /// </value>
        public static byte ShowMenuPressKey
        {
            get
            {
                if (_showMenuHotkey == 0)
                {
                    try
                    {
                        _showMenuHotkey = (byte)SandboxConfig.MenuKey;
                        _showMenuHotkey = _showMenuHotkey == 0 ? (byte)16 : _showMenuHotkey;
                        _showMenuHotkey = Utils.FixVirtualKey(_showMenuHotkey);
                        Console.WriteLine(@"Menu press key set to {0}", _showMenuHotkey);
                    }
                    catch
                    {
                        _showMenuHotkey = 16;
                        Console.WriteLine(@"Could not get the menu press key");
                    }
                }

                return _showMenuHotkey;
            }
        }

        /// <summary>
        ///     Gets the show menu toggle key.
        /// </summary>
        /// <value>
        ///     The show menu toggle key.
        /// </value>
        public static byte ShowMenuToggleKey
        {
            get
            {
                if (_showMenuToggleHotkey == 0)
                {
                    try
                    {
                        _showMenuToggleHotkey = (byte)SandboxConfig.MenuToggleKey;
                        _showMenuToggleHotkey = _showMenuToggleHotkey == 0 ? (byte)120 : _showMenuToggleHotkey;
                        _showMenuToggleHotkey = Utils.FixVirtualKey(_showMenuToggleHotkey);
                        Console.WriteLine(@"Menu toggle key set to {0}", _showMenuToggleHotkey);
                    }
                    catch
                    {
                        _showMenuToggleHotkey = 120;
                        Console.WriteLine(@"Could not get the menu toggle key");
                    }
                }

                return _showMenuToggleHotkey;
            }
        }

        #endregion
    }
}