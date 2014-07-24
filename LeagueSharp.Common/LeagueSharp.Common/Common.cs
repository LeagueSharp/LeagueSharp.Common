using System;
using System.ComponentModel;

namespace LeagueSharp.Common
{
    internal static class Common
    {
        internal static bool isInitialized;
        private const int localversion = 16;

        internal static void InitializeCommonLib()
        {
            isInitialized = true;
            UpdateCheck();
        }

        private static void UpdateCheck()
        {
            Game.PrintChat("<font color='#33FFFF'>>>LeagueSharp.Common loaded <<");
            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += bgw_DoWork;
            bgw.RunWorkerAsync();
        }

        private static void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            Updater myUpdater = new Updater("https://github.com/Andyi19/LeagueSharp/raw/master/Versions/LeagueSharp.Common.txt", "https://github.com/Andyi19/LeagueSharp/raw/master/LibAssemblies/LeagueSharp.Common.dll", localversion);
            if (myUpdater.NeedUpdate)
            {
                Game.PrintChat("<font color='#33FFFF'>LeagueSharp.Common: Updating ...");
                if (myUpdater.Update())
                {
                    Game.PrintChat("<font color='#33FFFF'>LeagueSharp.Common: Update complete, reload please.");
                }
            }
            else
            {
                Game.PrintChat("<font color='#33FFFF'>>>LeagueSharp.Common: Most recent version ({0}) loaded!", localversion);
            }
        }
    }

    internal class Updater
    {
        private readonly string _updatelink;
        public bool NeedUpdate = false;

        readonly System.Net.WebClient _wc = new System.Net.WebClient() { Proxy = null };
        public Updater(string versionlink, string updatelink, int localversion)
        {
            _updatelink = updatelink;

            NeedUpdate = Convert.ToInt32(_wc.DownloadString(versionlink)) > localversion;
        }

        public bool Update()
        {
            try
            {
                if (System.IO.File.Exists(System.IO.Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location) + ".bak"))
                {
                    System.IO.File.Delete(System.IO.Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location) + ".bak");
                }
                System.IO.File.Move(System.Reflection.Assembly.GetExecutingAssembly().Location, System.IO.Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location) + ".bak");
                _wc.DownloadFile(_updatelink, System.IO.Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location));
                return true;
            }
            catch (Exception ex)
            {
                Game.PrintChat("<font color='#33FFFF'>LeagueSharp.Common Updater: " + ex.Message);
                return false;
            }
        }
    }
}
