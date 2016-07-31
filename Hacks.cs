using System;

namespace LeagueSharp.Common
{
    /// <summary>
    /// Adds hacks to the menu.
    /// </summary>
    internal class Hacks
    {
        private static Menu menu;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            CustomEvents.Game.OnGameLoad += eventArgs =>
            {
                menu = new Menu("Hacks", "Hacks");

                var console = menu.AddItem(new MenuItem("ConsoleHack", "Console").SetValue(false));
                console.ValueChanged += (sender, args) => LeagueSharp.Hacks.Console = args.GetNewValue<bool>();

                var afk = menu.AddItem(new MenuItem("AfkHack", "Anti-AFK").SetValue(false));
                afk.ValueChanged += (sender, args) => LeagueSharp.Hacks.AntiAFK = args.GetNewValue<bool>();

                var draw = menu.AddItem(new MenuItem("DrawingHack", "Disable Drawing").SetValue(false));
                draw.ValueChanged += (sender, args) => LeagueSharp.Hacks.DisableDrawings = args.GetNewValue<bool>();

                var say = menu.AddItem(new MenuItem("SayHack", "Disable L# Send Chat").SetValue(false).SetTooltip("Block Game.Say from Assemblies"));
                say.ValueChanged += (sender, args) => LeagueSharp.Hacks.DisableSay = args.GetNewValue<bool>();

                var tower = menu.AddItem(new MenuItem("TowerHack", "Show Tower Ranges").SetValue(false));
                tower.ValueChanged += (sender, args) => LeagueSharp.Hacks.TowerRanges = args.GetNewValue<bool>();

                LeagueSharp.Hacks.Console = console.GetValue<bool>();
                LeagueSharp.Hacks.AntiAFK = afk.GetValue<bool>();
                LeagueSharp.Hacks.DisableDrawings = draw.GetValue<bool>();
                LeagueSharp.Hacks.DisableSay = say.GetValue<bool>();
                LeagueSharp.Hacks.TowerRanges = tower.GetValue<bool>();

                CommonMenu.Instance.AddSubMenu(menu);
            };
        }

        public static void Shutdown()
        {
            Menu.Remove(menu);
        }
    }
}