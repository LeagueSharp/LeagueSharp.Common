#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Spell.cs is part of LeagueSharp.Common.
 
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
            LowHitChance
        }

        private int _chargedCastedT;
        private int _chargedReqSentT;
        private Vector3 _from;
        private float _range;
        private Vector3 _rangeCheckFrom;
        private float _width;

        public Spell(SpellSlot slot,
            float range = float.MaxValue,
            TargetSelector.DamageType damageType = TargetSelector.DamageType.Physical)
        {
            Slot = slot;
            Range = range;
            DamageType = damageType;

            // Default values
            MinHitChance = HitChance.High;
        }

        public string ChargedBuffName { get; set; }
        public int ChargedMaxRange { get; set; }
        public int ChargedMinRange { get; set; }
        public string ChargedSpellName { get; set; }
        public int ChargeDuration { get; set; }
        public bool Collision { get; set; }
        public float Delay { get; set; }
        public bool IsChargedSpell { get; set; }
        public bool IsSkillshot { get; set; }
        public int LastCastAttemptT { get; set; }
        public HitChance MinHitChance { get; set; }
        public SpellSlot Slot { get; set; }
        public float Speed { get; set; }
        public SkillshotType Type { get; set; }
        public TargetSelector.DamageType DamageType { get; set; }

        public float Width
        {
            get { return _width; }
            set
            {
                _width = value;
                WidthSqr = value * value;
            }
        }

        public float WidthSqr { get; private set; }

        public SpellDataInst Instance
        {
            get { return ObjectManager.Player.Spellbook.GetSpell(Slot); }
        }

        public float Range
        {
            get
            {
                if (!IsChargedSpell)
                {
                    return _range;
                }

                if (IsCharging)
                {
                    return ChargedMinRange +
                           Math.Min(
                               ChargedMaxRange - ChargedMinRange,
                               (Utils.TickCount - _chargedCastedT) * (ChargedMaxRange - ChargedMinRange) /
                               ChargeDuration - 150);
                }

                return ChargedMaxRange;
            }
            set { _range = value; }
        }

        public float RangeSqr
        {
            get { return Range * Range; }
        }

        public bool IsCharging
        {
            get
            {
                return ObjectManager.Player.HasBuff(ChargedBuffName, true) ||
                       Utils.TickCount - _chargedCastedT < 300 + Game.Ping;
            }
        }

        public int Level
        {
            get { return ObjectManager.Player.Spellbook.GetSpell(Slot).Level; }
        }

        public Vector3 From
        {
            get { return !_from.To2D().IsValid() ? ObjectManager.Player.ServerPosition : _from; }
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
            Spellbook.OnUpdateChargedSpell += Spellbook_OnUpdateChargedSpell;
            Spellbook.OnCastSpell += SpellbookOnCastSpell;
        }

        /// <summary>
        ///     Start charging the spell if its not charging.
        /// </summary>
        public void StartCharging()
        {
            if (!IsCharging && Utils.TickCount - _chargedReqSentT > 400 + Game.Ping)
            {
                ObjectManager.Player.Spellbook.CastSpell(Slot);
                _chargedReqSentT = Utils.TickCount;
            }
        }

        /// <summary>
        ///     Start charging the spell if its not charging.
        /// </summary>
        public void StartCharging(Vector3 position)
        {
            if (!IsCharging && Utils.TickCount - _chargedReqSentT > 400 + Game.Ping)
            {
                ObjectManager.Player.Spellbook.CastSpell(Slot, position);
                _chargedReqSentT = Utils.TickCount;
            }
        }
        
        void Spellbook_OnUpdateChargedSpell(Spellbook sender, SpellbookUpdateChargedSpellEventArgs args)
        {
            if (sender.Owner.IsMe && Utils.TickCount - _chargedReqSentT < 3000)
            {
                args.Process = false;
            }
        }

        private void SpellbookOnCastSpell(Spellbook spellbook, SpellbookCastSpellEventArgs args)
        {
            if (args.Slot != Slot)
            {
                return;
            }

            if ((Utils.TickCount - _chargedReqSentT > 500))
            {
                if (IsCharging)
                {
                    Cast(args.StartPosition.To2D());
                }
            }
        }

        private void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == ChargedSpellName)
            {
                _chargedCastedT = Utils.TickCount;
            }
        }

        public void UpdateSourcePosition(Vector3 from = new Vector3(), Vector3 rangeCheckFrom = new Vector3())
        {
            From = from;
            RangeCheckFrom = rangeCheckFrom;
        }

        public PredictionOutput GetPrediction(Obj_AI_Base unit, bool aoe = false, float overrideRange = -1, CollisionableObjects[] collisionable = null)
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
                        CollisionObjects =
                            collisionable ?? new[] { CollisionableObjects.Heroes, CollisionableObjects.Minions }
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
                    Speed = Speed
                });
        }

        public float GetHitCount(HitChance hitChance = HitChance.High)
        {
            return HeroManager.Enemies.Select(e => GetPrediction(e)).Count(p => p.Hitchance >= hitChance);
        }

        private CastStates _cast(Obj_AI_Base unit,
            bool packetCast = false,
            bool aoe = false,
            bool exactHitChance = false,
            int minTargets = -1)
        {
            //Spell not ready.
            if (!Slot.IsReady())
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
                if (RangeCheckFrom.Distance(unit.ServerPosition, true) > RangeSqr)
                {
                    return CastStates.OutOfRange;
                }

                LastCastAttemptT = Utils.TickCount;

                if (packetCast)
                {
                    if (!ObjectManager.Player.Spellbook.CastSpell(Slot, unit, false))
                    {
                        return CastStates.NotCasted;
                    }
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

            if (minTargets != -1 && prediction.AoeTargetsHitCount < minTargets)
            {
                return CastStates.NotEnoughTargets;
            }

            //Skillshot collides.
            if (prediction.CollisionObjects.Count > 0)
            {
                return CastStates.Collision;
            }

            //Target out of range.
            if (RangeCheckFrom.Distance(prediction.CastPosition, true) > RangeSqr)
            {
                return CastStates.OutOfRange;
            }

            //The hitchance is too low.
            if (prediction.Hitchance < MinHitChance || (exactHitChance && prediction.Hitchance != MinHitChance))
            {
                return CastStates.LowHitChance;
            }

            LastCastAttemptT = Utils.TickCount;

            if (IsChargedSpell)
            {
                if (IsCharging)
                {
                    ShootChargedSpell(Slot, prediction.CastPosition);
                }
                else
                {
                    StartCharging();
                }
            }
            else if (packetCast)
            {
                ObjectManager.Player.Spellbook.CastSpell(Slot, prediction.CastPosition, false);
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
        ///     Casts the targetted spell on the unit.
        /// </summary>
        public bool CastOnUnit(Obj_AI_Base unit, bool packetCast = false)
        {
            if (!Slot.IsReady() || From.Distance(unit.ServerPosition, true) > RangeSqr)
            {
                return false;
            }

            LastCastAttemptT = Utils.TickCount;

            if (packetCast)
            {
                return ObjectManager.Player.Spellbook.CastSpell(Slot, unit, false);
            }
            else
            {
                return ObjectManager.Player.Spellbook.CastSpell(Slot, unit);
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
        public bool Cast(bool packetCast = false)
        {
            return CastOnUnit(ObjectManager.Player, packetCast);
        }

        public bool Cast(Vector2 fromPosition, Vector2 toPosition)
        {
            return Cast(fromPosition.To3D(), toPosition.To3D());
        }

        public bool Cast(Vector3 fromPosition, Vector3 toPosition)
        {
            return Slot.IsReady() && ObjectManager.Player.Spellbook.CastSpell(Slot, fromPosition, toPosition);
        }

        /// <summary>
        ///     Casts the spell to the position.
        /// </summary>
        public bool Cast(Vector2 position, bool packetCast = false)
        {
            return Cast(position.To3D(), packetCast);
        }

        /// <summary>
        ///     Casts the spell to the position.
        /// </summary>
        public bool Cast(Vector3 position, bool packetCast = false)
        {
            if (!Slot.IsReady())
            {
                return false;
            }

            LastCastAttemptT = Utils.TickCount;

            if (IsChargedSpell)
            {
                if (IsCharging)
                {
                    ShootChargedSpell(Slot, position);
                }
                else
                {
                    StartCharging();
                }
            }
            else if (packetCast)
            {
                return ObjectManager.Player.Spellbook.CastSpell(Slot, position, false);
            }
            else
            {
                return ObjectManager.Player.Spellbook.CastSpell(Slot, position);
            }
            return false;
        }

        private static void ShootChargedSpell(SpellSlot slot, Vector3 position, bool releaseCast = true)
        {
            ObjectManager.Player.Spellbook.CastSpell(slot, position, false);
            ObjectManager.Player.Spellbook.UpdateChargedSpell(slot, position, releaseCast, false);
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
        ///     Returns the unit health when the spell hits the unit.
        /// </summary>
        public float GetHealthPrediction(Obj_AI_Base unit)
        {
            var time = (int) (Delay * 1000 + From.Distance(unit.ServerPosition) / Speed - 100);
            return HealthPrediction.GetHealthPrediction(unit, time);
        }

        public MinionManager.FarmLocation GetCircularFarmLocation(List<Obj_AI_Base> minionPositions,
            float overrideWidth = -1)
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
            return unitPosition.Hitchance >= minHitChance &&
                   WillHit(unitPosition.UnitPosition, castPosition, extraWidth);
        }

        /// <summary>
        ///     Returns if the spell will hit the point when casted on castPosition.
        /// </summary>
        public bool WillHit(Vector3 point, Vector3 castPosition, int extraWidth = 0)
        {
            switch (Type)
            {
                case SkillshotType.SkillshotCircle:
                    if (point.To2D().Distance(castPosition, true) < WidthSqr)
                    {
                        return true;
                    }
                    break;

                case SkillshotType.SkillshotLine:
                    if (point.To2D().Distance(castPosition.To2D(), From.To2D(), true, true) <
                        Math.Pow(Width + extraWidth, 2))
                    {
                        return true;
                    }
                    break;
                case SkillshotType.SkillshotCone:
                    var edge1 = (castPosition.To2D() - From.To2D()).Rotated(-Width / 2);
                    var edge2 = edge1.Rotated(Width);
                    var v = point.To2D() - From.To2D();
                    if (point.To2D().Distance(From, true) < RangeSqr && edge1.CrossProduct(v) > 0 &&
                        v.CrossProduct(edge2) > 0)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        /// <summary>
        ///     Returns if a spell can be cast and the target is in range.
        /// </summary>
        public bool CanCast(Obj_AI_Base unit)
        {
            return Slot.IsReady() && unit.IsValidTarget(Range);
        }

        /// <summary>
        ///     Returns if the GameObject is in range of the spell.
        /// </summary>
        public bool IsInRange(GameObject obj, float range = -1)
        {
            return IsInRange(
                obj is Obj_AI_Base ? (obj as Obj_AI_Base).ServerPosition.To2D() : obj.Position.To2D(), range);
        }

        /// <summary>
        ///     Returns if the Vector3 is in range of the spell.
        /// </summary>
        public bool IsInRange(Vector3 point, float range = -1)
        {
            return IsInRange(point.To2D(), range);
        }

        /// <summary>
        ///     Returns if the Vector2 is in range of the spell.
        /// </summary>
        public bool IsInRange(Vector2 point, float range = -1)
        {
            return RangeCheckFrom.To2D().Distance(point, true) < (range < 0 ? RangeSqr : range * range);
        }

        /// <summary>
        ///     Returns the best target found using the current TargetSelector mode.
        ///     Please make sure to set the Spell.DamageType Property to the type of damage this spell does (if not done on
        ///     initialization).
        /// </summary>
        public Obj_AI_Hero GetTarget(float extraRange = 0, IEnumerable<Obj_AI_Hero> champsToIgnore = null)
        {
            return TargetSelector.GetTarget(Range + extraRange, DamageType, true, champsToIgnore, From);
        }

        /// <summary>
        ///     Spell will be casted on the best target found with the Spell.GetTarget method.
        /// </summary>
        public CastStates CastOnBestTarget(float extraRange = 0, bool packetCast = false, bool aoe = false)
        {
            var target = GetTarget(extraRange);
            return target != null ? Cast(target, packetCast, aoe) : CastStates.NotCasted;
        }
    }
}
