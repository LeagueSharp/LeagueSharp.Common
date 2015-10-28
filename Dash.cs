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
    /// <summary>
    /// Gets information about dashes, and provides events.
    /// </summary>
    public static class Dash
    {
        /// <summary>
        /// The detected dashes
        /// </summary>
        private static readonly Dictionary<int, DashItem> DetectedDashes = new Dictionary<int, DashItem>();

        /// <summary>
        /// Initializes static members of the <see cref="Dash"/> class. 
        /// </summary>
        static Dash()
        {
            Obj_AI_Hero.OnNewPath += ObjAiHeroOnOnNewPath;
        }

        /// <summary>
        /// Fired when a unit changes paths.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectNewPathEventArgs"/> instance containing the event data.</param>
        private static void ObjAiHeroOnOnNewPath(Obj_AI_Base sender, GameObjectNewPathEventArgs args)
        {
            if (sender.IsValid<Obj_AI_Hero>())
            {
                if (!DetectedDashes.ContainsKey(sender.NetworkId))
                {
                    DetectedDashes.Add(sender.NetworkId, new DashItem());
                }

                if (args.IsDash)
                {
                    var path = new List<Vector2> { sender.ServerPosition.To2D() };
                    path.AddRange(args.Path.ToList().To2D());

                    DetectedDashes[sender.NetworkId].StartTick = Utils.TickCount;
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
                else
                {
                    DetectedDashes[sender.NetworkId].EndTick = 0;
                }
            }
        }


        /// <summary>
        /// Determines whether this instance is dashing.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static bool IsDashing(this Obj_AI_Base unit)
        {
            if (DetectedDashes.ContainsKey(unit.NetworkId) && unit.Path.Length != 0)
            {
                return DetectedDashes[unit.NetworkId].EndTick != 0;
            }
            return false;
        }


        /// <summary>
        /// Gets the dash information.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static DashItem GetDashInfo(this Obj_AI_Base unit)
        {
            return DetectedDashes.ContainsKey(unit.NetworkId) ? DetectedDashes[unit.NetworkId] : new DashItem();
        }

        /// <summary>
        /// Represents a dash.
        /// </summary>
        public class DashItem
        {
            /// <summary>
            /// The duration
            /// </summary>
            public int Duration;

            /// <summary>
            /// The end position
            /// </summary>
            public Vector2 EndPos;

            /// <summary>
            /// The end tick
            /// </summary>
            public int EndTick;

            /// <summary>
            /// The path
            /// </summary>
            public List<Vector2> Path;

            /// <summary>
            /// The speed
            /// </summary>
            public float Speed;

            /// <summary>
            /// The start position
            /// </summary>
            public Vector2 StartPos;

            /// <summary>
            /// The start tick
            /// </summary>
            public int StartTick;

            /// <summary>
            /// The unit
            /// </summary>
            public Obj_AI_Base Unit;

            /// <summary>
            /// <c>true</c> if the dash was a blink, else <c>false</c>
            /// </summary>
            public bool IsBlink;
        }
    }
}
