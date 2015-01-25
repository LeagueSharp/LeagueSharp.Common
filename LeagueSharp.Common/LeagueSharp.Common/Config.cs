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
        private static byte _showMenuHotkey;
        private static byte _showMenuToggleHotkey;

        public static string LeagueSharpDirectory
        {
            get
            {
                if (_leagueSharpDirectory == null)
                {
                    try
                    {
                        _leagueSharpDirectory =
                            Process.GetCurrentProcess()
                                .Modules.Cast<ProcessModule>()
                                .First(p => Path.GetFileName(p.ModuleName) == "Leaguesharp.Core.dll")
                                .FileName;
                        _leagueSharpDirectory =
                            Directory.GetParent(Path.GetDirectoryName(_leagueSharpDirectory)).FullName;
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(@"Could not resolve LeagueSharp directory: " + ee);
                        _leagueSharpDirectory = Directory.GetCurrentDirectory();
                    }
                }

                return _leagueSharpDirectory;
            }
        }

        public static string SelectedLanguage
        {
            get
            {
                var configFile = Path.Combine(LeagueSharpDirectory, "config.xml");
                try
                {
                    if (File.Exists(configFile))
                    {
                        var config = new XmlDocument();
                        config.Load(configFile);

                        if (config.DocumentElement != null && config.DocumentElement.SelectSingleNode("/Config/SelectedLanguage") != null)
                        {
                            return config.DocumentElement.SelectSingleNode("/Config/SelectedLanguage").InnerText;
                        }
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.ToString());
                }

                return "";
            }
        }

        public static byte ShowMenuPressKey
        {
            get
            {
                if (_showMenuHotkey == 0)
                {
                    _showMenuHotkey = GetHotkey("ShowMenuPress", 16);
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
                    _showMenuToggleHotkey = GetHotkey("ShowMenuToggle", 120);
                }

                return _showMenuToggleHotkey;
            }
        }

        public static byte GetHotkey(string name, byte defaultValue)
        {
            var configFile = Path.Combine(LeagueSharpDirectory, "config.xml");
            try
            {
                if (File.Exists(configFile))
                {
                    var config = new XmlDocument();
                    config.Load(configFile);
                    var node = config.DocumentElement.SelectSingleNode("/Config/Hotkeys/SelectedHotkeys");
                    foreach (var b in from XmlElement element in node.ChildNodes
                        where element.ChildNodes.Cast<XmlElement>().Any(e => e.Name == "Name" && e.InnerText == name)
                        select element.ChildNodes.Cast<XmlElement>().FirstOrDefault(e => e.Name == "HotkeyInt")
                        into b
                        where b != null
                        select b)
                    {
                        return byte.Parse(b.InnerText);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString());
            }

            return defaultValue;
        }
    }
}