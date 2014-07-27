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
            if (Common.isInitialized == false)
            {
                Common.InitializeCommonLib();
            }

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

                if (unit.IsValid && unit.Type == GameObjectType.obj_AI_Hero)
                {
                    if (!DetectedDashes.ContainsKey(unit.NetworkId))
                    {
                        var detectedDash = new DashItem();
                        DetectedDashes.Add(unit.NetworkId, detectedDash);
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
                    DetectedDashes[dashItem.Key].Path = dashItem.Value.Unit.GetWaypoints();
                    DetectedDashes[dashItem.Key].EndPos = dashItem.Value.Path[dashItem.Value.Path.Count - 1];
                    DetectedDashes[dashItem.Key].EndTick = dashItem.Value.StartTick +
                                                           (int)(1000 *
                                                                 (dashItem.Value.EndPos.Distance(
                                                                     dashItem.Value.StartPos) /
                                                                  dashItem.Value.Speed));
                    DetectedDashes[dashItem.Key].Duration = dashItem.Value.EndTick - dashItem.Value.StartTick;
                    DetectedDashes[dashItem.Key].Processed = true;

                    CustomEvents.Unit.TriggerOnDash(dashItem.Value.Unit, DetectedDashes[dashItem.Key]);
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
        }
    }
}