using System;

namespace LeagueSharp.Common
{
    /// <summary>
    /// Adds hacks to the menu.
    /// </summary>
    internal class Hacks
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            CustomEvents.Game.OnGameLoad += eventArgs =>
            {
                var menu = new Menu("Hacks", "Hacks");

                var draw = menu.AddItem(new MenuItem("DrawingHack", "Disable Drawing").SetValue(false));
                draw.SetValue(LeagueSharp.Hacks.DisableDrawings);
                draw.ValueChanged +=
                    delegate(object sender, OnValueChangeEventArgs args)
                    {
                        LeagueSharp.Hacks.DisableDrawings = args.GetNewValue<bool>();
                    };

                var say = menu.AddItem(new MenuItem("SayHack", "Disable L# Send Chat").SetValue(false)
                    .SetTooltip("Block Game.Say from Assemblies"));
                say.SetValue(LeagueSharp.Hacks.DisableSay);
                say.ValueChanged +=
                    delegate(object sender, OnValueChangeEventArgs args)
                    {
                        LeagueSharp.Hacks.DisableSay = args.GetNewValue<bool>();
                    };

                /*  var zoom = menu.AddItem(new MenuItem("ZoomHack", "Extended Zoom").SetValue(false));
                zoom.SetValue(LeagueSharp.Hacks.ZoomHack);
                zoom.ValueChanged +=
                    delegate (object sender, OnValueChangeEventArgs args)
                    {
                        LeagueSharp.Hacks.ZoomHack = args.GetNewValue<bool>();
                    };

                menu.AddItem(
                    new MenuItem("ZoomHackInfo", "Note: ZoomHack may be unsafe!", false, FontStyle.Regular, Color.Red));
                */

                var tower = menu.AddItem(new MenuItem("TowerHack", "Show Tower Ranges").SetValue(false));
                tower.SetValue(LeagueSharp.Hacks.TowerRanges);
                tower.ValueChanged +=
                    delegate(object sender, OnValueChangeEventArgs args)
                    {
                        LeagueSharp.Hacks.TowerRanges = args.GetNewValue<bool>();
                    };
                CommonMenu.Instance.AddSubMenu(menu);
            };
        }
    }
}