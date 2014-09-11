#region LICENSE
/*
 Copyright 2014 - 2014 LeagueSharp
 Orbwalking.cs is part of LeagueSharp.Common.
 
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

using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    public static class Dash
    {
        private static readonly Dictionary<int, DashItem> DetectedDashes = new Dictionary<int, DashItem>();

        static Dash()
        {
            Game.OnGameUpdate += GameOnOnGameUpdate;
            Game.OnGameProcessPacket += GameOnOnGameProcessPacket;
        }

        private static void GameOnOnGameProcessPacket(GamePacketEventArgs args)
        {
            if (args.PacketData[0] == Packet.S2C.Dash.Header)
            {
                var decodedPacket = Packet.S2C.Dash.Decoded(args.PacketData);

                var networkId = decodedPacket.UnitNetworkId;
                var speed = decodedPacket.Speed;
                var unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(networkId);

                if (unit != null && unit.IsValid && unit.Type == GameObjectType.obj_AI_Hero)
                {
                    if (!DetectedDashes.ContainsKey(unit.NetworkId))
                    {
                        DetectedDashes.Add(unit.NetworkId, new DashItem());
                    }

                    DetectedDashes[unit.NetworkId].StartTick = Environment.TickCount - Game.Ping / 2;
                    DetectedDashes[unit.NetworkId].Speed = speed;
                    DetectedDashes[unit.NetworkId].StartPos = unit.ServerPosition.To2D();
                    DetectedDashes[unit.NetworkId].Processed = false;
                    DetectedDashes[unit.NetworkId].Unit = unit;
                }
            }
        }

        private static void GameOnOnGameUpdate(EventArgs args)
        {
            foreach (var dashItem in DetectedDashes)
            {
                if (!dashItem.Value.Processed)
                {
                    if (dashItem.Value.Unit.IsValid)
                    {
                        DetectedDashes[dashItem.Key].Path = dashItem.Value.Unit.GetWaypoints();
                        DetectedDashes[dashItem.Key].EndPos = dashItem.Value.Path[dashItem.Value.Path.Count - 1];
                        DetectedDashes[dashItem.Key].EndTick = dashItem.Value.StartTick +
                                                               (int)
                                                                   (1000 *
                                                                    (dashItem.Value.EndPos.Distance(
                                                                        dashItem.Value.StartPos) / dashItem.Value.Speed));
                        DetectedDashes[dashItem.Key].Duration = dashItem.Value.EndTick - dashItem.Value.StartTick;
                        DetectedDashes[dashItem.Key].Processed = true;

                        CustomEvents.Unit.TriggerOnDash(dashItem.Value.Unit, DetectedDashes[dashItem.Key]);
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if the unit is dashing.
        /// </summary>
        public static bool IsDashing(this Obj_AI_Base unit)
        {
            if (DetectedDashes.ContainsKey(unit.NetworkId))
            {
                return DetectedDashes[unit.NetworkId].EndTick > Environment.TickCount;
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
            public bool Processed = false;
            public float Speed;
            public Vector2 StartPos;
            public int StartTick;
            public Obj_AI_Base Unit;
            public bool IsBlink;
        }
    }
}