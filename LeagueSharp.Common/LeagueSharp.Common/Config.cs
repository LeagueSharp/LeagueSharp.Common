using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

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
                        Console.WriteLine("Could not resolve LeagueSharp directory: " + ee);
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
            string configFile = Path.Combine(LeagueSharpDirectory, "config.xml");
            try
            {
                if (File.Exists(configFile))
                {
                    var config = new XmlDocument();
                    config.Load(configFile);
                    XmlNode node = config.DocumentElement.SelectSingleNode("/Config/Hotkeys/SelectedHotkeys");
                    foreach (XmlElement b in from XmlElement element in node.ChildNodes
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