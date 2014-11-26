using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows;
using System.Windows.Input;

namespace LeagueSharp.Common
{
    public static class Config
    {
        private static string _leagueSharpDirectory;
        public static string LeagueSharpDirectory
        {
            get
            {
                if(_leagueSharpDirectory == null)
                {
                    try
                    {
                        _leagueSharpDirectory = Process.GetCurrentProcess().Modules.Cast<ProcessModule>().First(p => Path.GetFileName(p.ModuleName) == "Leaguesharp.Core.dll").FileName;
                        _leagueSharpDirectory = Directory.GetParent(Path.GetDirectoryName(_leagueSharpDirectory)).FullName;
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
                        return config.DocumentElement.SelectSingleNode("/Config/SelectedLanguage").InnerText;
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.ToString());
                }
                
                return "";
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
                    foreach (var n in node.ChildNodes)
                    {
                        var element = (XmlElement)n;
                        if (element.ChildNodes.Cast<XmlElement>().Any(e => e.Name == "Name" && e.InnerText == name))
                        {
                            
                            var b = element.ChildNodes.Cast<XmlElement>()
                                        .FirstOrDefault(e => e.Name == "HotkeyInt");
                            if (b != null)
                            {
                                return byte.Parse(b.InnerText);
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString());
            }

            return defaultValue;
        }

        private static byte _showMenuHotkey = 0;
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

        private static byte _showMenuToggleHotkey = 0;
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
    }
}
