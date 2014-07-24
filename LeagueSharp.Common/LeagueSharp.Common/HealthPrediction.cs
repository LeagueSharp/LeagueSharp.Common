#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    ///     This class allows you to calculate the health of units after a set time. Only works on minions and only taking into account the auto-attack damage.
    /// </summary>
    public class HealthPrediction
    {
        private const bool OnlyMinionDamage = true;
        private static readonly Dictionary<int, PredictedDamage> ActiveAttacks = new Dictionary<int, PredictedDamage>();
        private static int LastTick;

        static HealthPrediction()
        {
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
            Game.OnGameProcessPacket += OnProcessPacket;
            Game.OnGameUpdate += OnTick;
        }

        private static void OnTick(EventArgs args)
        {
            if (Environment.TickCount - LastTick > 10 * 1000)
            {
                ActiveAttacks.ToList()
                    .Where(pair => pair.Value.StartTick < Environment.TickCount - 10000)
                    .ToList()
                    .ForEach(pair => ActiveAttacks.Remove(pair.Key));
                LastTick = Environment.TickCount;
            }
        }

        private static void OnProcessPacket(GamePacketEventArgs args)
        {
            if (args.PacketData[0] == 0x33)
            {
                var stream = new MemoryStream(args.PacketData);
                var b = new BinaryReader(stream);
                b.BaseStream.Position = b.BaseStream.Position + 1;
                var Nid = BitConverter.ToInt32(b.ReadBytes(4), 0);
                if (args.PacketData[9] == 17)
                {
                    if (ActiveAttacks.ContainsKey(Nid))
                    {
                        ActiveAttacks.Remove(Nid);
                    }
                }
            }
        }

        private static Obj_AI_Base GetAttackTarget(Vector3 Position)
        {
            Obj_AI_Base result = null;
            foreach (var Candidate in ObjectManager.Get<Obj_AI_Base>())
            {
                if (Candidate.IsValidTarget(float.MaxValue, false) &&
                    Vector2.DistanceSquared(Candidate.ServerPosition.To2D(), Position.To2D()) <=
                    Candidate.PathfindingCollisionRadius * Candidate.PathfindingCollisionRadius)
                {
                    result = Candidate;
                }
            }
            return result;
        }

        private static void OnProcessSpell(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs Spell)
        {
            if (unit.Team == ObjectManager.Player.Team && !(unit is Obj_AI_Hero) &&
                Vector2.DistanceSquared(unit.ServerPosition.To2D(),
                    ObjectManager.Player.ServerPosition.To2D()) <= 3000 * 3000)
            {
                /*Only auto-attacks for now*/
                if (Orbwalking.IsAutoAttack(Spell.SData.Name))
                {
                    var target = GetAttackTarget(Spell.End);
                    if (target != null)
                    {
                        ActiveAttacks.Remove(unit.NetworkId);
                        var Damage = CalcMinionToMinionDmg(unit, target);

                        var AttackData = new PredictedDamage(unit, target, Environment.TickCount - Game.Ping / 2,
                            unit.AttackCastDelay * 1000,
                            unit.AttackDelay * 1000,
                            (Spell.SData.MissileSpeed == 0) ? int.MaxValue : (int)Spell.SData.MissileSpeed,
                            (float)Damage);
                        ActiveAttacks.Add(unit.NetworkId, AttackData);
                    }
                }
            }
        }

        private static double CalcMinionToMinionDmg(Obj_AI_Base attackminion, Obj_AI_Base shotminion,
            double ExtraDamage = 0)
        {
            double armorPenPercent = attackminion.PercentArmorPenetrationMod;
            double armorPen = attackminion.FlatArmorPenetrationMod;

            if (attackminion is Obj_AI_Minion)
            {
                armorPen = 0;
                armorPenPercent = 1;
            }

            if (attackminion is Obj_AI_Turret)
            {
                armorPen = 0;
                armorPenPercent = 0.7f;
            }

            var newarmor = shotminion.Armor * armorPenPercent;
            var dmgreduction = 100 / (100 + Math.Max(newarmor - armorPen, 0));

            if ((attackminion is Obj_AI_Turret) &&
                (shotminion.BaseSkinName == "Red_Minion_MechCannon" ||
                 shotminion.BaseSkinName == "Blue_Minion_MechCannon"))
            {
                dmgreduction = 0.8 * dmgreduction;
            }

            if (attackminion is Obj_AI_Turret &&
                (shotminion.BaseSkinName == "Red_Minion_Wizard" || shotminion.BaseSkinName == "Blue_Minion_Wizard" ||
                 shotminion.BaseSkinName == "Red_Minion_Basic" || shotminion.BaseSkinName == "Blue_Minion_Basic"))
                dmgreduction = (1 / 0.875) * dmgreduction;

            if (attackminion is Obj_AI_Turret)
            {
                dmgreduction = 1.05 * dmgreduction;
            }

            return (((attackminion.BaseAttackDamage + attackminion.FlatPhysicalDamageMod + ExtraDamage) * dmgreduction));
        }

        /// <summary>
        /// Returns the unit health after a set time milliseconds. 
        /// </summary>
        public static float GetHealthPrediction(Obj_AI_Base unit, int time, int delay = 70)
        {
            var PredictedDamage = 0f;

            foreach (var attack in ActiveAttacks.Values)
            {
                var AttackDamage = 0f;
                if (attack.Attacked.NetworkId == unit.NetworkId &&
                    attack.Attacker.IsValidTarget(float.MaxValue, false))
                {
                    var d = Vector2.Distance(unit.ServerPosition.To2D(),
                        attack.Attacker.ServerPosition.To2D());
                    if (attack.FromEdgeToEdge)
                    {
                        d -= attack.Attacker.BoundingRadius - attack.Attacked.BoundingRadius;
                        d = Math.Max(d, 0);
                    }

                    var landTime = attack.StartTick + attack.Delay + 1000 * d / attack.ProjectileSpeed + delay;

                    if (Environment.TickCount < landTime - delay && landTime < Environment.TickCount + time)
                    {
                        AttackDamage = attack.Damage;
                    }
                }

                PredictedDamage += AttackDamage;
            }

            return unit.Health - PredictedDamage;
        }

        /// <summary>
        /// Returns the unit health after time milliseconds assuming that the past auto-attacks are periodic. 
        /// </summary>
        public static float LaneClearHealthPrediction(Obj_AI_Base unit, int time, int delay = 70)
        {
            var PredictedDamage = 0f;

            foreach (var attack in ActiveAttacks.Values)
            {
                var n = 0;
                if (Environment.TickCount - 100 <= attack.StartTick + attack.AnimationTime &&
                    attack.Attacked.IsValidTarget(float.MaxValue, false) &&
                    attack.Attacked.NetworkId == unit.NetworkId &&
                    attack.Attacker.IsValidTarget(float.MaxValue, false))
                {
                    var FromT = attack.StartTick;
                    var ToT = Environment.TickCount + time;

                    var d = Vector2.Distance(unit.ServerPosition.To2D(),
                        attack.Attacker.ServerPosition.To2D());
                    if (attack.FromEdgeToEdge)
                    {
                        d -= attack.Attacker.BoundingRadius - attack.Attacked.BoundingRadius;
                        d = Math.Max(d, 0);
                    }

                    while (FromT < ToT)
                    {
                        if (FromT >= Environment.TickCount && (FromT + attack.Delay + d / attack.ProjectileSpeed < ToT))
                        {
                            n++;
                        }
                        FromT += (int)attack.AnimationTime;
                    }
                }
                PredictedDamage += n * attack.Damage;
            }

            return unit.Health - PredictedDamage;
        }

        private class PredictedDamage
        {
            public readonly float AnimationTime;
            public readonly Obj_AI_Base Attacked;
            public readonly Obj_AI_Base Attacker;

            public readonly float Damage;
            public readonly float Delay;
            public readonly bool FromEdgeToEdge;
            public readonly int ProjectileSpeed;
            public readonly int StartTick;

            public PredictedDamage(Obj_AI_Base Attacker, Obj_AI_Base Attacked, int StartTick, float Delay,
                float AnimationTime, int ProjectileSpeed, float Damage, bool FromEdgeToEdge = true)
            {
                this.Attacked = Attacked;
                this.Attacker = Attacker;
                this.StartTick = StartTick;
                this.Delay = Delay;
                this.ProjectileSpeed = ProjectileSpeed;
                this.Damage = Damage;
                this.FromEdgeToEdge = false;
                this.AnimationTime = AnimationTime;
            }
        }
    }
}