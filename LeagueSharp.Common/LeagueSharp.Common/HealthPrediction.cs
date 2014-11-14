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
using System.Linq;

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    ///     This class allows you to calculate the health of units after a set time. Only works on minions and only taking into account the auto-attack damage.
    /// </summary>
    public class HealthPrediction
    {
        private static readonly Dictionary<int, PredictedDamage> ActiveAttacks = new Dictionary<int, PredictedDamage>();
        private static int LastTick;

        static HealthPrediction()
        {
            Obj_AI_Base.OnProcessSpellCast += ObjAiBaseOnOnProcessSpellCast;
            Game.OnGameProcessPacket += Game_OnGameProcessPacket;
            Game.OnGameUpdate += Game_OnGameUpdate;
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Environment.TickCount - LastTick <= 60 * 1000)
            {
                return;
            }
            ActiveAttacks.ToList()
                .Where(pair => pair.Value.StartTick < Environment.TickCount - 60000)
                .ToList()
                .ForEach(pair => ActiveAttacks.Remove(pair.Key));
            LastTick = Environment.TickCount;
        }

        private static void Game_OnGameProcessPacket(GamePacketEventArgs args)
        {
            if (args.PacketData[0] != 0x34 || new GamePacket(args).ReadInteger(1) != ObjectManager.Player.NetworkId ||
                (args.PacketData[5] != 0x11 && args.PacketData[5] != 0x91))
            {
                return;
            }

            var networkId = new GamePacket(args).ReadInteger(1);

            if (ActiveAttacks.ContainsKey(networkId))
            {
                ActiveAttacks.Remove(networkId);
            }
        }

        private static void ObjAiBaseOnOnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsValidTarget(3000, false) || sender.Team != ObjectManager.Player.Team || sender is Obj_AI_Hero ||
                !Orbwalking.IsAutoAttack(args.SData.Name) || !(args.Target is Obj_AI_Base))
            {
                return;
            }

            var target = (Obj_AI_Base) args.Target;
            ActiveAttacks.Remove(sender.NetworkId);

            var attackData = new PredictedDamage(
                sender, target, Environment.TickCount - Game.Ping / 2, sender.AttackCastDelay * 1000,
                sender.AttackDelay * 1000 - (sender is Obj_AI_Turret ? 70 : 0),
                sender.IsMelee() ? int.MaxValue : (int) args.SData.MissileSpeed,
                (float) sender.GetAutoAttackDamage(target, true));
            ActiveAttacks.Add(sender.NetworkId, attackData);
        }

        /// <summary>
        /// Returns the unit health after a set time milliseconds. 
        /// </summary>
        public static float GetHealthPrediction(Obj_AI_Base unit, int time, int delay = 70)
        {
            var predictedDamage = 0f;

            foreach (var attack in ActiveAttacks.Values)
            {
                var attackDamage = 0f;
                if (attack.Source.IsValidTarget(float.MaxValue, false) &&
                    attack.Target.IsValidTarget(float.MaxValue, false) && attack.Target.NetworkId == unit.NetworkId)
                {
                    var landTime = attack.StartTick + attack.Delay +
                                   1000 * unit.Distance(attack.Source) / attack.ProjectileSpeed + delay;

                    if (Environment.TickCount < landTime - delay && landTime < Environment.TickCount + time)
                    {
                        attackDamage = attack.Damage;
                    }
                }

                predictedDamage += attackDamage;
            }

            return unit.Health - predictedDamage;
        }

        /// <summary>
        /// Returns the unit health after time milliseconds assuming that the past auto-attacks are periodic. 
        /// </summary>
        public static float LaneClearHealthPrediction(Obj_AI_Base unit, int time, int delay = 70)
        {
            var predictedDamage = 0f;

            foreach (var attack in ActiveAttacks.Values)
            {
                var n = 0;
                if (Environment.TickCount - 100 <= attack.StartTick + attack.AnimationTime &&
                    attack.Target.IsValidTarget(float.MaxValue, false) &&
                    attack.Source.IsValidTarget(float.MaxValue, false) && attack.Target.NetworkId == unit.NetworkId)
                {
                    var fromT = attack.StartTick;
                    var toT = Environment.TickCount + time;

                    while (fromT < toT)
                    {
                        if (fromT >= Environment.TickCount &&
                            (fromT + attack.Delay + unit.Distance(attack.Source) / attack.ProjectileSpeed < toT))
                        {
                            n++;
                        }
                        fromT += (int) attack.AnimationTime;
                    }
                }
                predictedDamage += n * attack.Damage;
            }

            return unit.Health - predictedDamage;
        }

        private class PredictedDamage
        {
            public readonly float AnimationTime;

            public readonly float Damage;
            public readonly float Delay;
            public readonly int ProjectileSpeed;
            public readonly Obj_AI_Base Source;
            public readonly int StartTick;
            public readonly Obj_AI_Base Target;

            public PredictedDamage(Obj_AI_Base source,
                Obj_AI_Base target,
                int startTick,
                float delay,
                float animationTime,
                int projectileSpeed,
                float damage)
            {
                Source = source;
                Target = target;
                StartTick = startTick;
                Delay = delay;
                ProjectileSpeed = projectileSpeed;
                Damage = damage;
                AnimationTime = animationTime;
            }
        }
    }
}