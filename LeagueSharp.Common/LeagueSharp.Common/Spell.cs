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
using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    ///     This class allows you to handle the spells easily.
    /// </summary>
    public class Spell
    {
        public enum CastStates
        {
            SuccessfullyCasted,
            NotReady,
            NotCasted,
            OutOfRange,
            Collision,
            NotEnoughTargets,
            LowHitChance,
        }

        public int ChargeDuration;
        public string ChargedBuffName;
        public int ChargedMaxRange;
        public int ChargedMinRange;
        public string ChargedSpellName;

        public bool Collision;
        public float Delay;
        public bool IsChargedSpell;
        public bool IsSkillshot;
        public int LastCastAttemptT = 0;
        public HitChance MinHitChance = HitChance.High;
        public SpellSlot Slot;
        public float Speed;
        public SkillshotType Type;
        public float Width;

        private int _chargedCastedT;
        private int _chargedReqSentT;
        private Vector3 _from;
        private float _range;
        private Vector3 _rangeCheckFrom;

        public Spell(SpellSlot slot, float range = float.MaxValue)
        {
            Slot = slot;
            Range = range;
        }

        public SpellDataInst Instance
        {
            get { return ObjectManager.Player.Spellbook.GetSpell(Slot); }
        }

        public float Range
        {
            get
            {
                if (IsChargedSpell)
                {
                    if (IsCharging)
                    {
                        return ChargedMinRange +
                               Math.Min(
                                   ChargedMaxRange - ChargedMinRange,
                                   (Environment.TickCount - _chargedCastedT) * (ChargedMaxRange - ChargedMinRange) /
                                   ChargeDuration - 150);
                    }

                    return ChargedMaxRange;
                }

                return _range;
            }
            set { _range = value; }
        }

        public bool IsCharging
        {
            get
            {
                return ObjectManager.Player.HasBuff(ChargedBuffName, true) ||
                       Environment.TickCount - _chargedCastedT < 300 + Game.Ping;
            }
        }

        public int Level
        {
            get { return ObjectManager.Player.Spellbook.GetSpell(Slot).Level; }
        }

        public Vector3 From
        {
            get
            {
                if (!_from.To2D().IsValid())
                {
                    return ObjectManager.Player.ServerPosition;
                }
                return _from;
            }
            set { _from = value; }
        }

        public Vector3 RangeCheckFrom
        {
            get { return !_rangeCheckFrom.To2D().IsValid() ? ObjectManager.Player.ServerPosition : _rangeCheckFrom; }
            set { _rangeCheckFrom = value; }
        }

        public void SetTargetted(float delay,
            float speed,
            Vector3 from = new Vector3(),
            Vector3 rangeCheckFrom = new Vector3())
        {
            Delay = delay;
            Speed = speed;
            From = from;
            RangeCheckFrom = rangeCheckFrom;
            IsSkillshot = false;
        }

        public void SetSkillshot(float delay,
            float width,
            float speed,
            bool collision,
            SkillshotType type,
            Vector3 from = new Vector3(),
            Vector3 rangeCheckFrom = new Vector3())
        {
            Delay = delay;
            Width = width;
            Speed = speed;
            From = from;
            Collision = collision;
            Type = type;
            RangeCheckFrom = rangeCheckFrom;
            IsSkillshot = true;
        }

        public void SetCharged(string spellName, string buffName, int minRange, int maxRange, float deltaT)
        {
            IsChargedSpell = true;
            ChargedSpellName = spellName;
            ChargedBuffName = buffName;
            ChargedMinRange = minRange;
            ChargedMaxRange = maxRange;
            ChargeDuration = (int) (deltaT * 1000);
            _chargedCastedT = 0;

            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
            Game.OnGameSendPacket += Game_OnGameSendPacket;
        }

        /// <summary>
        ///     Start charging the spell if its not charging.
        /// </summary>
        public void StartCharging()
        {
            if (!IsCharging && Environment.TickCount - _chargedReqSentT > 400 + Game.Ping)
            {
                ObjectManager.Player.Spellbook.CastSpell(Slot);
                _chargedReqSentT = Environment.TickCount;
            }
        }

        /// <summary>
        ///     Start charging the spell if its not charging.
        /// </summary>
        public void StartCharging(Vector3 position)
        {
            if (!IsCharging && Environment.TickCount - _chargedReqSentT > 400 + Game.Ping)
            {
                ObjectManager.Player.Spellbook.CastSpell(Slot, position);
                _chargedReqSentT = Environment.TickCount;
            }
        }

        private void Game_OnGameSendPacket(GamePacketEventArgs args)
        {
            if (args.PacketData[0] == Packet.C2S.ChargedCast.Header && Environment.TickCount - _chargedReqSentT < 3000)
            {
                var decoded = Packet.C2S.ChargedCast.Decoded(args.PacketData);
                if (decoded.SourceNetworkId != ObjectManager.Player.NetworkId)
                {
                    return;
                }
                Console.WriteLine((byte) decoded.Slot);
                args.Process = false;
            }

            if (args.PacketData[0] == Packet.C2S.Cast.Header)
            {
                var decoded = Packet.C2S.Cast.Decoded(args.PacketData);
                if (decoded.Slot != Slot)
                {
                    return;
                }
                if ((Environment.TickCount - _chargedReqSentT > 500))
                {
                    if (IsCharging)
                    {
                        Cast(new Vector2(decoded.ToX, decoded.ToY));
                    }
                }
            }
        }

        private void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == ChargedSpellName)
            {
                _chargedCastedT = Environment.TickCount;
            }
        }

        public void UpdateSourcePosition(Vector3 from = new Vector3(), Vector3 rangeCheckFrom = new Vector3())
        {
            From = from;
            RangeCheckFrom = rangeCheckFrom;
        }

        public PredictionOutput GetPrediction(Obj_AI_Base unit, bool aoe = false, float overrideRange = -1)
        {
            return
                Prediction.GetPrediction(
                    new PredictionInput
                    {
                        Unit = unit,
                        Delay = Delay,
                        Radius = Width,
                        Speed = Speed,
                        From = From,
                        Range = (overrideRange > 0) ? overrideRange : Range,
                        Collision = Collision,
                        Type = Type,
                        RangeCheckFrom = RangeCheckFrom,
                        Aoe = aoe,
                    });
        }

        public List<Obj_AI_Base> GetCollision(Vector2 from, List<Vector2> to, float delayOverride = -1)
        {
            return Common.Collision.GetCollision(
                to.Select(h => h.To3D()).ToList(),
                new PredictionInput
                {
                    From = from.To3D(),
                    Type = Type,
                    Radius = Width,
                    Delay = delayOverride > 0 ? delayOverride : Delay,
                    Speed = Speed,
                });
        }

        public float GetHitCount(HitChance hitChance = HitChance.High)
        {
            return
                (from hero in ObjectManager.Get<Obj_AI_Hero>() where hero.IsValidTarget() select GetPrediction(hero))
                    .Count(prediction => prediction.Hitchance >= hitChance);
        }

        private CastStates _cast(Obj_AI_Base unit,
            bool packetCast = false,
            bool aoe = false,
            bool exactHitChance = false,
            int minTargets = -1)
        {
            //Spell not ready.
            if (!IsReady())
            {
                return CastStates.NotReady;
            }

            if (minTargets != -1)
            {
                aoe = true;
            }

            //Targetted spell.
            if (!IsSkillshot)
            {
                //Target out of range
                if (RangeCheckFrom.Distance(unit.ServerPosition, true) > Range * Range)
                {
                    return CastStates.OutOfRange;
                }

                LastCastAttemptT = Environment.TickCount;

                if (packetCast)
                {
                    Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(unit.NetworkId, Slot)).Send();
                }
                else
                {
                    //Cant cast the Spell.
                    if (!ObjectManager.Player.Spellbook.CastSpell(Slot, unit))
                    {
                        return CastStates.NotCasted;
                    }
                }


                return CastStates.SuccessfullyCasted;
            }

            //Get the best position to cast the spell.
            var prediction = GetPrediction(unit, aoe);

            if (minTargets != -1 && prediction.AoeTargetsHitCount <= minTargets)
            {
                return CastStates.NotEnoughTargets;
            }

            //Skillshot collides.
            if (prediction.CollisionObjects.Count > 0)
            {
                return CastStates.Collision;
            }

            //Target out of range.
            if (RangeCheckFrom.Distance(prediction.CastPosition, true) > Range * Range)
            {
                return CastStates.OutOfRange;
            }

            //The hitchance is too low.
            if (prediction.Hitchance < MinHitChance || (exactHitChance && prediction.Hitchance != MinHitChance))
            {
                return CastStates.LowHitChance;
            }

            LastCastAttemptT = Environment.TickCount;

            if (IsChargedSpell)
            {
                if (IsCharging)
                {
                    Packet.C2S.ChargedCast.Encoded(
                        new Packet.C2S.ChargedCast.Struct(
                            (SpellSlot) ((byte) Slot), prediction.CastPosition.X, ObjectManager.Player.ServerPosition.Z,
                            prediction.CastPosition.Y)).Send();
                }
                else
                {
                    StartCharging();
                }
            }
            else if (packetCast)
            {
                Packet.C2S.Cast.Encoded(
                    new Packet.C2S.Cast.Struct(
                        0, Slot, -1, prediction.CastPosition.X, prediction.CastPosition.Y, prediction.CastPosition.X,
                        prediction.CastPosition.Y)).Send();
            }
            else
            {
                //Cant cast the spell (actually should not happen).
                if (!ObjectManager.Player.Spellbook.CastSpell(Slot, prediction.CastPosition))
                {
                    return CastStates.NotCasted;
                }
            }

            return CastStates.SuccessfullyCasted;
        }

        /// <summary>
        ///     Self-casts the spell.
        /// </summary>
        public bool Cast()
        {
            return IsReady() && ObjectManager.Player.Spellbook.CastSpell(Slot, ObjectManager.Player);
        }

        /// <summary>
        ///     Casts the targetted spell on the unit.
        /// </summary>
        public void CastOnUnit(Obj_AI_Base unit, bool packetCast = false)
        {
            if (!IsReady() || From.Distance(unit.ServerPosition) > Range)
            {
                return;
            }

            LastCastAttemptT = Environment.TickCount;

            if (packetCast)
            {
                Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(unit.NetworkId, Slot)).Send();
            }
            else
            {
                ObjectManager.Player.Spellbook.CastSpell(Slot, unit);
            }
        }

        /// <summary>
        ///     Casts the spell to the unit using the prediction if its an skillshot.
        /// </summary>
        public CastStates Cast(Obj_AI_Base unit, bool packetCast = false, bool aoe = false)
        {
            return _cast(unit, packetCast, aoe);
        }

        /// <summary>
        ///     Casts the spell (selfcast).
        /// </summary>
        public void Cast(bool packetCast = false)
        {
            if(!packetCast)
            {
                Cast();
            }
            else
            {
                Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(ObjectManager.Player.NetworkId, Slot)).Send();
            }
        }

        /// <summary>
        ///     Casts the spell to the position.
        /// </summary>
        public void Cast(Vector2 position, bool packetCast = false)
        {
            Cast(position.To3D(), packetCast);
        }

        /// <summary>
        ///     Casts the spell to the position.
        /// </summary>
        public void Cast(Vector3 position, bool packetCast = false)
        {
            if (!IsReady())
            {
                return;
            }

            LastCastAttemptT = Environment.TickCount;

            if (IsChargedSpell)
            {
                if (IsCharging)
                {
                    Packet.C2S.ChargedCast.Encoded(
                        new Packet.C2S.ChargedCast.Struct((SpellSlot) ((byte) Slot), position.X, position.Z, position.Y))
                        .Send();
                }
                else
                {
                    StartCharging();
                }
            }
            else if (packetCast)
            {
                Packet.C2S.Cast.Encoded(
                    new Packet.C2S.Cast.Struct(0, Slot, -1, position.X, position.Y, position.X, position.Y)).Send();
            }
            else
            {
                ObjectManager.Player.Spellbook.CastSpell(Slot, position);
            }
        }

        /// <summary>
        ///     Casts the spell if the hitchance equals the set hitchance.
        /// </summary>
        public bool CastIfHitchanceEquals(Obj_AI_Base unit, HitChance hitChance, bool packetCast = false)
        {
            var currentHitchance = MinHitChance;
            MinHitChance = hitChance;
            var castResult = _cast(unit, packetCast, false, false);
            MinHitChance = currentHitchance;
            return castResult == CastStates.SuccessfullyCasted;
        }

        /// <summary>
        ///     Casts the spell if it will hit the set targets.
        /// </summary>
        public bool CastIfWillHit(Obj_AI_Base unit, int minTargets = 5, bool packetCast = false)
        {
            var castResult = _cast(unit, packetCast, true, false, minTargets);
            return castResult == CastStates.SuccessfullyCasted;
        }

        /// <summary>
        ///     Returns if the spell is ready to use.
        /// </summary>
        public bool IsReady(int t = 0)
        {
            if (t == 0 && ObjectManager.Player.Spellbook.CanUseSpell(Slot) != SpellState.Ready)
            {
                return false;
            }

            return t == 0 ||
                   (ObjectManager.Player.Spellbook.CanUseSpell(Slot) == SpellState.Cooldown &&
                    (ObjectManager.Player.Spellbook.GetSpell(Slot).CooldownExpires - Game.Time) <= t / 1000f);
        }

        /// <summary>
        ///     Returns the unit health when the spell hits the unit.
        /// </summary>
        public float GetHealthPrediction(Obj_AI_Base unit)
        {
            var time = (int) (Delay * 1000 + From.Distance(unit.ServerPosition) / Speed - 100);
            return HealthPrediction.GetHealthPrediction(unit, time);
        }

        public MinionManager.FarmLocation GetCircularFarmLocation(List<Obj_AI_Base> minionPositions,
            float overrideWidth = float.MaxValue)
        {
            var positions = MinionManager.GetMinionsPredictedPositions(
                minionPositions, Delay, Width, Speed, From, Range, false, SkillshotType.SkillshotCircle);

            return GetCircularFarmLocation(positions, overrideWidth);
        }

        public MinionManager.FarmLocation GetCircularFarmLocation(List<Vector2> minionPositions,
            float overrideWidth = -1)
        {
            return MinionManager.GetBestCircularFarmLocation(
                minionPositions, overrideWidth >= 0 ? overrideWidth : Width, Range);
        }

        public MinionManager.FarmLocation GetLineFarmLocation(List<Obj_AI_Base> minionPositions,
            float overrideWidth = -1)
        {
            var positions = MinionManager.GetMinionsPredictedPositions(
                minionPositions, Delay, Width, Speed, From, Range, false, SkillshotType.SkillshotLine);

            return GetLineFarmLocation(positions, overrideWidth >= 0 ? overrideWidth : Width);
        }

        public MinionManager.FarmLocation GetLineFarmLocation(List<Vector2> minionPositions, float overrideWidth = -1)
        {
            return MinionManager.GetBestLineFarmLocation(
                minionPositions, overrideWidth >= 0 ? overrideWidth : Width, Range);
        }

        public int CountHits(List<Obj_AI_Base> units, Vector3 castPosition)
        {
            var points = units.Select(unit => GetPrediction(unit).UnitPosition).ToList();
            return CountHits(points, castPosition);
        }

        public int CountHits(List<Vector3> points, Vector3 castPosition)
        {
            return points.Count(point => WillHit(point, castPosition));
        }

        /// <summary>
        ///     Gets the damage that the skillshot will deal to the target using the damage lib.
        /// </summary>
        public float GetDamage(Obj_AI_Base target, int stage = 0)
        {
            return (float) ObjectManager.Player.GetSpellDamage(target, Slot, stage);
        }

        /// <summary>
        ///     Gets the damage that the skillshot will deal to the target using the damage lib and returns if the target is
        ///     killable or not.
        /// </summary>
        public bool IsKillable(Obj_AI_Base target, int stage = 0)
        {
            return ObjectManager.Player.GetSpellDamage(target, Slot, stage) > target.Health;
        }

        /// <summary>
        ///     Returns if the spell will hit the unit when casted on castPosition.
        /// </summary>
        public bool WillHit(Obj_AI_Base unit,
            Vector3 castPosition,
            int extraWidth = 0,
            HitChance minHitChance = HitChance.High)
        {
            var unitPosition = GetPrediction(unit);
            if (unitPosition.Hitchance >= minHitChance)
            {
                return WillHit(unitPosition.UnitPosition, castPosition, extraWidth);
            }

            return false;
        }

        /// <summary>
        ///     Returns if the spell will hit the point when casted on castPosition.
        /// </summary>
        public bool WillHit(Vector3 point, Vector3 castPosition, int extraWidth = 0)
        {
            switch (Type)
            {
                case SkillshotType.SkillshotCircle:
                    if (point.To2D().Distance(castPosition) < Width)
                    {
                        return true;
                    }
                    break;

                case SkillshotType.SkillshotLine:
                    if (point.To2D().Distance(castPosition.To2D(), From.To2D(), true) < Width + extraWidth)
                    {
                        return true;
                    }
                    break;
                case SkillshotType.SkillshotCone:
                    var edge1 = (castPosition.To2D() - From.To2D()).Rotated(-Width / 2);
                    var edge2 = edge1.Rotated(Width);
                    var v = point.To2D() - From.To2D();
                    if (point.To2D().Distance(From) < Range && edge1.CrossProduct(v) > 0 && v.CrossProduct(edge2) > 0)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        /// <summary>
        ///     Returns if the point is in range of the spell.
        /// </summary>
        public bool InRange(Vector3 point)
        {
            return RangeCheckFrom.Distance(point, true) < Range * Range;
        }
    }
}