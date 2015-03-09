#region LICENSE
/*
 Copyright 2014 - 2014 LeagueSharp
 Dash.cs is part of LeagueSharp.Common.
 
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
using System.Collections.Generic;
using System.Linq;
using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    public static class Dash
    {
        private static readonly Dictionary<int, DashItem> DetectedDashes = new Dictionary<int, DashItem>();

        static Dash()
        {
            Obj_AI_Hero.OnNewPath += ObjAiHeroOnOnNewPath;
        }

        private static void ObjAiHeroOnOnNewPath(Obj_AI_Base sender, GameObjectNewPathEventArgs args)
        {
            if (sender.IsValid<Obj_AI_Hero>() && args.IsDash)
            {
                if (!DetectedDashes.ContainsKey(sender.NetworkId))
                {
                    DetectedDashes.Add(sender.NetworkId, new DashItem());
                }
                var path = new List<Vector2> { sender.ServerPosition.To2D() };
                path.AddRange(args.Path.ToList().To2D());

                DetectedDashes[sender.NetworkId].StartTick = Utils.TickCount - Game.Ping / 2;
                DetectedDashes[sender.NetworkId].Speed = args.Speed;
                DetectedDashes[sender.NetworkId].StartPos = sender.ServerPosition.To2D();
                DetectedDashes[sender.NetworkId].Unit = sender;
                DetectedDashes[sender.NetworkId].Path = path;
                DetectedDashes[sender.NetworkId].EndPos = DetectedDashes[sender.NetworkId].Path.Last();
                DetectedDashes[sender.NetworkId].EndTick = DetectedDashes[sender.NetworkId].StartTick +
                                                       (int)
                                                           (1000 *
                                                            (DetectedDashes[sender.NetworkId].EndPos.Distance(
                                                                DetectedDashes[sender.NetworkId].StartPos) / DetectedDashes[sender.NetworkId].Speed));
                DetectedDashes[sender.NetworkId].Duration = DetectedDashes[sender.NetworkId].EndTick - DetectedDashes[sender.NetworkId].StartTick;

                CustomEvents.Unit.TriggerOnDash(DetectedDashes[sender.NetworkId].Unit, DetectedDashes[sender.NetworkId]);
            }
        }

        /// <summary>
        /// Returns true if the unit is dashing.
        /// </summary>
        public static bool IsDashing(this Obj_AI_Base unit)
        {
            if (DetectedDashes.ContainsKey(unit.NetworkId))
            {
                return DetectedDashes[unit.NetworkId].EndTick > Utils.TickCount;
            }
            return false;
        }

        /// <summary>
        /// Gets the speed of the dashing unit if it is dashing.
        /// </summary>
        public static DashItem GetDashInfo(this Obj_AI_Base unit)
        {
            return DetectedDashes.ContainsKey(unit.NetworkId) ? DetectedDashes[unit.NetworkId] : new DashItem();
        }

        public class DashItem
        {
            public int Duration;
            public Vector2 EndPos;
            public int EndTick;
            public List<Vector2> Path;
            public float Speed;
            public Vector2 StartPos;
            public int StartTick;
            public Obj_AI_Base Unit;
            public bool IsBlink;
        }
    }
}
