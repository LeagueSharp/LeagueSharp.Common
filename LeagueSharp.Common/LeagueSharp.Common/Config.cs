using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        _leagueSharpDirectory = Process.GetCurrentProcess().Modules.Cast<ProcessModule>().First(p => Path.GetFileName(p.ModuleName) == "Leaguesharp.Core.dll").ModuleName;
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
    }
}
