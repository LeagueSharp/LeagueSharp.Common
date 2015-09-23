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

namespace LeagueSharp.Common
{

	/// <summary>
	///     The PermaShow class allows you to add important items to permashow easily.
	/// </summary>


	public static class PermaShow
	{
		private static bool subbed;

		/// <summary>
		///    List of items for PermaShow
		/// </summary>

		private static readonly List<PermaShowItem> PermaShowItems = new List<PermaShowItem>();

		internal class PermaShowItem
		{
			internal string DisplayName { get; set; }
			internal MenuItem Item { get; set; }
			internal MenuValueType ItemType { get; set; }
			internal Color Color { get; set; }
		}


		/// <summary>
		///    Positioning
		/// </summary>

		private static Vector2 DefaultPosition = new Vector2(Drawing.Width - (0.79f * Drawing.Width),
			Drawing.Height - (0.98f * Drawing.Height));

		private static Vector2 BoxPosition;

		private static readonly float XFactor = Drawing.Width / 1366f;
		private static readonly float YFactor = Drawing.Height / 768f;


		private static readonly float DefaultPermaShowWidth = 270f;
		private static readonly float DefaultSmallBoxWidth = 45f;

		private static float PermaShowWidth
		{
			get { return ScaleValue(placetosave.Item("bwidth").GetValue<Slider>().Value, Direction.X); }
		}

		private static float SmallBoxWidth
		{
			get { return ScaleValue(placetosave.Item("swidth").GetValue<Slider>().Value, Direction.X); }
		}

		/// <summary>
		///   Drawing tools
		/// </summary>
		/// 
		private static Font Text;
		private static Line BoxLine;
		private static Sprite sprite;

		/// <summary>
		///    Menu to save position and bool for moving the menu
		/// </summary>

		private static Menu placetosave;
		private static bool Dragging;


		/// <summary>
		///    Constructor
		/// </summary>

		static PermaShow()
		{
			CreateMenu();
			BoxPosition = GetPosition();
			PrepareDrawing();
		}


		private static void Sub()
		{
			subbed = true;
			Drawing.OnPreReset += DrawingOnPreReset;
			Drawing.OnPostReset += DrawingOnOnPostReset;
			Drawing.OnDraw += Drawing_OnEndScene;
			AppDomain.CurrentDomain.DomainUnload += CurrentDomainOnDomainUnload;
			AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnDomainUnload;
			Game.OnWndProc += OnWndProc;
		}

		private static void Unsub()
		{
			subbed = false;
			Drawing.OnPreReset -= DrawingOnPreReset;
			Drawing.OnPostReset -= DrawingOnOnPostReset;
			Drawing.OnDraw -= Drawing_OnEndScene;
			AppDomain.CurrentDomain.DomainUnload -= CurrentDomainOnDomainUnload;
			AppDomain.CurrentDomain.ProcessExit -= CurrentDomainOnDomainUnload;
			Game.OnWndProc -= OnWndProc;
		}

		private static void Drawing_OnEndScene(EventArgs args)
		{
			if (Drawing.Direct3DDevice == null || Drawing.Direct3DDevice.IsDisposed)
			{
				return;
			}

			if (!placetosave.Item("enablepermashow").GetValue<bool>())
			{
				Unsub();
				return;
			}

			PermaArea();

			var halfwidth = 0.96f * (PermaShowWidth / 2);

			var baseposition = new Vector2(BoxPosition.X - halfwidth, BoxPosition.Y);

			var boxx = BoxPosition.X + (PermaShowWidth / 2) - (SmallBoxWidth/  2);

			foreach (var permaitem in PermaShowItems)
			{
				var index = PermaShowItems.IndexOf(permaitem);
				var boxpos = new Vector2(boxx,
					baseposition.Y + (Text.Description.Height * 1.2f * index));
				var endpos = new Vector2(BoxPosition.X + (PermaShowWidth / 2), baseposition.Y + (Text.Description.Height * 1.2f * index));
				var itempos = new Vector2(baseposition.X, baseposition.Y + (Text.Description.Height * 1.2f * index));

				int textpos = (int) (endpos.X - (SmallBoxWidth / 1.2f));

				switch (permaitem.Item.ValueType)
				{
					case MenuValueType.Boolean:
						DrawBox(boxpos, permaitem.Item.GetValue<bool>());
						Text.DrawText(null, permaitem.DisplayName + ":",
							(int)itempos.X, (int)itempos.Y, permaitem.Color);
						Text.DrawText(null, permaitem.Item.GetValue<bool>().ToString(),
							textpos, (int)itempos.Y, permaitem.Color);
						break;
					case MenuValueType.Slider:
						Text.DrawText(null, permaitem.DisplayName + ":",
							(int)itempos.X, (int)itempos.Y, permaitem.Color);
						Text.DrawText(null, permaitem.Item.GetValue<Slider>().Value.ToString(),
							textpos, (int)itempos.Y, permaitem.Color);
						break;
					case MenuValueType.KeyBind:
						DrawBox(boxpos, permaitem.Item.GetValue<KeyBind>().Active);
						Text.DrawText(null,
							permaitem.DisplayName + ":", (int)itempos.X, (int)itempos.Y,
							permaitem.Color);

						Text.DrawText(null,
							permaitem.Item.GetValue<KeyBind>().Active.ToString(), textpos, (int)(boxpos.Y),
							permaitem.Color);
						break;
					case MenuValueType.StringList:
						Text.DrawText(null,
							permaitem.DisplayName + ":",
							(int)itempos.X, (int)itempos.Y, permaitem.Color);
						var dimen = Text.MeasureText(sprite, permaitem.Item.GetValue<StringList>().SelectedValue);
						Text.DrawText(null, permaitem.Item.GetValue<StringList>().SelectedValue,
							(int) (textpos + dimen.Width < endpos.X ? textpos : endpos.X - dimen.Width), (int)itempos.Y, permaitem.Color);
						break;
					case MenuValueType.Integer:
						Text.DrawText(null, permaitem.DisplayName + ":",
							(int)itempos.X, (int)itempos.Y, permaitem.Color);
						Text.DrawText(null, permaitem.Item.GetValue<int>().ToString(),
							textpos, (int)itempos.Y, permaitem.Color);
						break;
					case MenuValueType.None:
						break;
					default:
						throw new Exception("[PermaShow] - MenuItem not supported");
				}
			}
		}

		/// <summary>
		///    Adds a menuitem to PermaShow, can be used without any arguements or with if you want to customize. The bool can be set to false to remove the item from permashow. 
		///    When removing, you can simply set the bool parameter to false and everything else can be null. The default color is White.
		/// </summary>

		public static void Permashow(this MenuItem item, bool enabled = true, string customdisplayname = null,
			Color? col = null)
		{
			if (!IsEnabled())
			{
				return;
			}
			if (enabled && PermaShowItems.All(x => x.Item != item))
			{
				if (!PermaShowItems.Any())
				{
					Sub();
				}
				string dispName = customdisplayname ?? item.DisplayName;
				Color? color = col ?? new ColorBGRA(255, 255, 255, 255);
				PermaShowItems.Add(new PermaShowItem
				{
					DisplayName = dispName,
					Item = item,
					ItemType = item.ValueType,
					Color = (Color)color
				});
			}

			else if (!enabled)
			{
				var itemtoremove = PermaShowItems.FirstOrDefault(x => x.Item == item);
				if (itemtoremove != null)
				{
					PermaShowItems.Remove(itemtoremove);
					if (!PermaShowItems.Any())
					{
						Unsub();
					}
				}
			}
		}

		/// <summary>
		///    Drawing Events
		/// </summary>
		private static void CurrentDomainOnDomainUnload(object sender, EventArgs eventArgs)
		{
			Text.Dispose();
			BoxLine.Dispose();
		}

		private static void DrawingOnOnPostReset(EventArgs args)
		{
			Text.OnResetDevice();
			BoxLine.OnResetDevice();
		}

		private static void DrawingOnPreReset(EventArgs args)
		{
			Text.OnLostDevice();
			BoxLine.OnLostDevice();
		}

		/// <summary>
		///    Dragging / Moving the menu
		/// </summary>

		private static void OnWndProc(WndEventArgs args)
		{
			if (MenuSettings.DrawMenu || !placetosave.Item("moveable").GetValue<bool>())
			{
				Dragging = false;
				return;
			}

			if (Dragging)
			{
				BoxPosition = Utils.GetCursorPos();
			}

			if (MouseOverArea() && args.Msg == (uint)WindowsMessages.WM_LBUTTONDOWN)
			{
				if (!Dragging)
				{
					Dragging = true;
				}
			}
			else if (Dragging && args.Msg == (uint)WindowsMessages.WM_LBUTTONUP)
			{
				Dragging = false;
				BoxPosition = Utils.GetCursorPos();
				SavePosition();
			}
		}

		private static bool MouseOverArea()
		{
			Vector2 pos = Utils.GetCursorPos();
			return ((pos.X >= BoxPosition.X - (PermaShowWidth / 2f)) && pos.X <= (BoxPosition.X + (PermaShowWidth / 2f)) &&
					pos.Y >= BoxPosition.Y && pos.Y <= (BoxPosition.Y + PermaShowItems.Count * (Text.Description.Height * 1.2f)));
		}

		/// <summary>
		///   Create the menu
		/// </summary>
		private static void CreateMenu()
		{
			placetosave = new Menu("PermaShow", "Permashow");
			MenuItem enablepermashow = new MenuItem("enablepermashow", "Enable PermaShow").SetValue(true);
			placetosave.AddItem(enablepermashow);
			var xvalue = new MenuItem("X", "X").SetValue(new Slider((int)DefaultPosition.X, 0, Drawing.Width));
			var yvalue = new MenuItem("Y", "Y").SetValue(new Slider((int)DefaultPosition.Y, 0, Drawing.Height));
			xvalue.ShowItem = false;
			yvalue.ShowItem = false;
			placetosave.AddItem(xvalue);
			placetosave.AddItem(yvalue);

			var bigwidth = new MenuItem("bwidth", "Width").SetValue(new Slider((int)DefaultPermaShowWidth, 100, 400));
			var smallwidth = new MenuItem("swidth", "Indicator Width").SetValue(new Slider((int)DefaultSmallBoxWidth, 30, 90));

			var moveable = new MenuItem("moveable", "Moveable").SetValue(true);

			placetosave.AddItem(moveable);
			placetosave.AddItem(bigwidth);
			placetosave.AddItem(smallwidth);

			
			var def = new MenuItem("defaults", "Default").SetValue(false);
			def.ValueChanged += (sender, args) =>
			{
				if (args.GetNewValue<bool>())
				{
					bigwidth.SetValue(new Slider((int) DefaultPermaShowWidth, 100, 400));
					smallwidth.SetValue(new Slider((int) DefaultSmallBoxWidth, 30, 90));
				}
			};

			placetosave.AddItem(def);

			CommonMenu.Config.AddSubMenu(placetosave);

			enablepermashow.ValueChanged += (sender, args) =>
			{
				if (args.GetNewValue<bool>() && !subbed)
				{
					Sub();
				}
				else if (!args.GetNewValue<bool>())
				{
					Unsub();
				}
			};
		}

		private static bool IsEnabled()
		{
			return placetosave.Item("enablepermashow").GetValue<bool>();
		}

		/// <summary>
		///   Save current position
		/// </summary>
		private static void SavePosition()
		{
			placetosave.Item("X")._valueSet = true;
			placetosave.Item("Y")._valueSet = true;
			placetosave.Item("X").SetValue(new Slider((int)BoxPosition.X, 0, Drawing.Width));
			placetosave.Item("Y").SetValue(new Slider((int)BoxPosition.Y, 0, Drawing.Height));
		}

		/// <summary>
		///    Return current positions from file
		/// </summary>
		private static Vector2 GetPosition()
		{
			return new Vector2(placetosave.Item("X").GetValue<Slider>().Value, placetosave.Item("Y").GetValue<Slider>().Value);
		}

		/// <summary>
		///     Draw area where text will be drawn
		/// </summary>
		private static void PermaArea()
		{

			BoxLine.Width = PermaShowWidth;

			BoxLine.Begin();

			var pos = BoxPosition;

			var positions = new[]
			{
				new Vector2(pos.X, pos.Y),
				new Vector2(pos.X, pos.Y + PermaShowItems.Count*(Text.Description.Height*1.2f))
			};

			var col = Color.Black;

			BoxLine.Draw(positions, new ColorBGRA(col.B, col.G, col.R, 0.4f));

			BoxLine.End();
		}

		/// <summary>
		///     Colored box to indicate booleans true or false with green and red respectively
		/// </summary>
		private static void DrawBox(Vector2 pos, bool ison)
		{

			BoxLine.Width = SmallBoxWidth;

			BoxLine.Begin();

			var positions = new[]
			{
				new Vector2(pos.X, pos.Y),
				new Vector2(pos.X, pos.Y + (Text.Description.Height*1.2f))
			};

			var col = ison ? Color.DarkGreen : Color.DarkRed;
			col.A = 150;
			BoxLine.Draw(positions, col);

			BoxLine.End();
		}


		private enum Direction
		{
			X,
			Y
		}

		private static float ScaleValue(float value, Direction direction)
		{
			var returnvalue = direction == Direction.X ? value * XFactor : value * YFactor;
			return returnvalue;
		}

		/// <summary>
		///    Initialize the Drawing tools
		/// </summary>
		private static void PrepareDrawing()
		{
		    Text = new Font(
		        Drawing.Direct3DDevice,
		        new FontDescription
		            {
		                FaceName = CommonMenu.Config.Item("FontName").GetValue<StringList>().SelectedValue,
		                Height = CommonMenu.Config.Item("FontSize").GetValue<Slider>().Value,
		                OutputPrecision = FontPrecision.Default,
		                Quality =
		                    (FontQuality)
		                    Enum.Parse(
		                        typeof(FontQuality),
		                        CommonMenu.Config.Item("FontQuality").GetValue<StringList>().SelectedValue)
		            });

			sprite = new Sprite(Drawing.Direct3DDevice);
			BoxLine = new Line(Drawing.Direct3DDevice) { Width = 1 };
		}
	}
}
