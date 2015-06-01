#region LICENSE

/*
 Copyright 2014 - 2015 LeagueSharp
 PermaShow.cs is part of LeagueSharp.Common.
 
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

using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Direct3D9;
using Font = SharpDX.Direct3D9.Font;

namespace LeagueSharp.Common
{

    /// <summary>
    ///     The PermaShow class allows you to add important things to permashow easily.
    /// </summary>
    /// 

    public static class PermaShow
    {

        public static List<PermaShowItem> PermaShowItems = new List<PermaShowItem>();

        public class PermaShowItem
        {
            public String DisplayName { get; set; }
            public MenuItem Item { get; set; }
            internal MenuValueType ItemType { get; set; }
            public SharpDX.Color Color { get; set; }
        }

        public static Font Text;

        static PermaShow()
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        static void OnLoad(EventArgs args)
        {
            PrepareDrawing();
            Drawing.OnPreReset += DrawingOnOnPreReset;
            Drawing.OnPostReset += DrawingOnOnPostReset;
            Drawing.OnDraw += Drawing_OnEndScene;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomainOnDomainUnload;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnDomainUnload;
        }



        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (Drawing.Direct3DDevice == null || Drawing.Direct3DDevice.IsDisposed)
            {
                return;
            }

            foreach (var permaitem in PermaShowItems)
            {
                var index = PermaShowItems.IndexOf(permaitem);

                switch (permaitem.Item.ValueType)
                {
                    case MenuValueType.Boolean:
                        Text.DrawText(null, permaitem.DisplayName + ": " + permaitem.Item.GetValue<bool>(), 200, 16 * index, permaitem.Color);
                        break;
                    case MenuValueType.Slider:
                        Text.DrawText(null, permaitem.DisplayName + ": " + permaitem.Item.GetValue<Slider>().Value, 200, 16 * index, permaitem.Color);
                        break;
                    case MenuValueType.KeyBind:
                        Text.DrawText(null, permaitem.DisplayName + ": ( " + permaitem.Item.GetValue<KeyBind>().Active + " ) - " + (permaitem.Item.GetValue<KeyBind>().Type), 200, 16 * index, permaitem.Color);
                        break;
                    case MenuValueType.StringList:
                        Text.DrawText(null, permaitem.DisplayName + ": " + permaitem.Item.GetValue<StringList>().SelectedValue, permaitem.Item.DisplayName.Length, 16 * index, permaitem.Color);
                        break;
                    case MenuValueType.Color:
                        Text.DrawText(null, permaitem.DisplayName + ": " + permaitem.Item.GetValue<System.Drawing.Color>(), permaitem.Item.DisplayName.Length, 16 * index, permaitem.Color);
                        break;
                    case MenuValueType.Integer:
                        Text.DrawText(null, permaitem.DisplayName + ": " + permaitem.Item.GetValue<int>(), permaitem.Item.DisplayName.Length, 16 * index, permaitem.Color);
                        break;
                    case MenuValueType.None:
                        break;
                    default:
                        break;
                }
            }
        }

        public static void Permashow(this MenuItem item, String customdisplayname = null, bool enabled = true, SharpDX.Color? col = null)
        {

            if (enabled)
            {
                String DispName = customdisplayname != null ? customdisplayname : item.DisplayName;
                SharpDX.Color? color = col != null ? col : new ColorBGRA(255, 255, 255, 255);

                PermaShowItems.Add(new PermaShowItem { DisplayName = DispName, Item = item, ItemType = item.ValueType, Color = (SharpDX.Color)color });
            }

            else
            {
                var itemtoremove = PermaShowItems.FirstOrDefault(x => x.Item == item);
                if (itemtoremove != null)
                {
                    PermaShowItems.Remove(itemtoremove);
                }
            }
        }

        private static void CurrentDomainOnDomainUnload(object sender, EventArgs eventArgs)
        {
            Text.Dispose();
        }

        private static void DrawingOnOnPostReset(EventArgs args)
        {
            Text.OnResetDevice();
        }

        private static void DrawingOnOnPreReset(EventArgs args)
        {
            Text.OnLostDevice();
        }

        public static void PrepareDrawing()
        {
            Text = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Calibri",
                    Height = 16,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.Default,
                });
        }


    }
}
