#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 Selected.cs is part of SFXTargetSelector.

 SFXTargetSelector is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 SFXTargetSelector is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with SFXTargetSelector. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion License

#region

using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

#endregion

namespace SFXTargetSelector
{
    public static partial class TargetSelector
    {
        public static partial class Selected
        {
            static Selected()
            {
                ClickBuffer = 100f;
                Game.OnWndProc += OnGameWndProc;
            }

            public static float ClickBuffer { get; set; }
            public static Obj_AI_Hero Target { get; set; }

            public static Obj_AI_Hero GetTarget(float range, DamageType damageType, bool ignoreShields, Vector3 from)
            {
                if (Target != null &&
                    Utils.IsValidTarget(
                        Target, Focus.Enabled && Focus.Force ? float.MaxValue : range, damageType, ignoreShields, from))
                {
                    return Target;
                }
                return null;
            }

            private static void OnGameWndProc(WndEventArgs args)
            {
                if (args.Msg != (ulong) WindowsMessages.WM_LBUTTONDOWN)
                {
                    return;
                }

                Target =
                    Targets.Items.Select(t => t.Hero)
                        .Where(h => h.IsValidTarget() && h.Distance(Game.CursorPos) < h.BoundingRadius + ClickBuffer)
                        .OrderBy(h => h.Distance(Game.CursorPos))
                        .FirstOrDefault();
            }
        }
    }
}