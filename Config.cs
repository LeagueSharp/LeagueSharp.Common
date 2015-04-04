#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Config.cs is part of LeagueSharp.Common.
 
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

#endregion

namespace LeagueSharp.Common
{
    public static class Config
    {
        private static string _leagueSharpDirectory;
        private static string _appDataDirectory;
        private static byte _showMenuHotkey;
        private static byte _showMenuToggleHotkey;

        public static string AppDataDirectory
        {
            get
            {
                if (_appDataDirectory == null)
                {
                    _appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                        "LS" + Environment.UserName.GetHashCode().ToString("X"));
                }

                return _appDataDirectory;
            }
        }

        public static string SelectedLanguage
        {
            get
            {
                return "";
            }
        }

        public static byte ShowMenuPressKey
        {
            get
            {
                if (_showMenuHotkey == 0)
                {
                    try
                    {
                        _showMenuHotkey = (byte)Sandbox.SandboxConfig.MenuKey;
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

        public static byte ShowMenuToggleKey
        {
            get
            {
                if (_showMenuToggleHotkey == 0)
                {
                    try
                    {
                        _showMenuToggleHotkey = (byte) Sandbox.SandboxConfig.MenuToggleKey;
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
    }
}