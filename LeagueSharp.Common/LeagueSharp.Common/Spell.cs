#region

using System;
using System.Collections.Generic;
using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    /// This class allows you to handle the spells easily.
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

        public bool Collision;
        public float Delay;

        public Prediction.HitChance MinHitChange = Prediction.HitChance.HighHitchance;

        public float Range;


        public bool Skillshot = false;
        public SpellSlot Slot;
        public float Speed;
        public Prediction.SkillshotType Type;
        public float Width;
        private Vector3 _from;
        private Vector3 _rangeCheckFrom;


        public Spell(SpellSlot slot, float range)
        {
            Slot = slot;
            Range = range;
        }

        public int LastCastAttemptT = 0;

        public int Level
        {
            get { return ObjectManager.Player.Spellbook.GetSpell(Slot).Level; }
        }

        public Vector3 From
        {
            get
            {
                if (!_from.To2D().IsValid()) return ObjectManager.Player.ServerPosition;
                return _from;
            }
            set { _from = value; }
        }

        public Vector3 RangeCheckFrom
        {
            get
            {
                if (!_rangeCheckFrom.To2D().IsValid()) return ObjectManager.Player.ServerPosition;
                return _rangeCheckFrom;
            }
            set { _rangeCheckFrom = value; }
        }

        public void SetSkillshot(float delay, float width, float speed, bool collision,
            Prediction.SkillshotType type, Vector3 from = new Vector3(), Vector3 rangeCheckFrom = new Vector3())
        {
            Delay = delay;
            Width = width;
            Speed = speed;
            From = from;
            Collision = collision;
            Type = type;
            RangeCheckFrom = rangeCheckFrom;
            Skillshot = true;
        }

        public void UpdateSourcePosition(Vector3 from = new Vector3(), Vector3 rangeCheckFrom = new Vector3())
        {
            From = from;
            RangeCheckFrom = rangeCheckFrom;
        }

        public Prediction.PredictionOutput GetPrediction(Obj_AI_Base unit, bool aoe = false)
        {
            return aoe
                ? Prediction.GetBestAOEPosition(unit, Delay, Width, Speed, From, Range, Collision, Type,
                    RangeCheckFrom)
                : Prediction.GetBestPosition(unit, Delay, Width, Speed, From, Range, Collision, Type,
                    RangeCheckFrom);
        }

        private CastStates _cast(Obj_AI_Base unit, bool packetCast = false, bool aoe = false,
            bool exactHitChance = false, int minTargets = -1)
        {
            //Spell not ready.
            if (ObjectManager.Player.Spellbook.CanUseSpell(Slot) != SpellState.Ready && !packetCast)
                return CastStates.NotReady;

            if (minTargets != -1) aoe = true;

            //Targetted spell.
            if (!Skillshot)
            {
                //Target out of range
                if (ObjectManager.Player.Distance(unit) > Range)
                    return CastStates.OutOfRange;

                LastCastAttemptT = Environment.TickCount;

                if (packetCast)
                {
                    Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(unit.NetworkId, Slot)).Send();
                }
                else
                {
                    //Cant cast the Spell.
                    if (!ObjectManager.Player.Spellbook.CastSpell(Slot, unit))
                        return CastStates.NotCasted;
                }


                return CastStates.SuccessfullyCasted;
            }

            //Get the best position to cast the spell.
            var prediction = GetPrediction(unit, aoe);

            if (minTargets != -1 && prediction.TargetsHit < minTargets)
                return CastStates.NotEnoughTargets;

            //Skillshot collides.
            if (prediction.CollisionUnitsList.Count > 0)
                return CastStates.Collision;

            //Target out of range.
            if (ObjectManager.Player.ServerPosition.Distance(prediction.CastPosition) > Range)
                return CastStates.OutOfRange;

            //The hitchance is too low.
            if (prediction.HitChance < MinHitChange || (exactHitChance && prediction.HitChance != MinHitChange))
                return CastStates.LowHitChance;

            LastCastAttemptT = Environment.TickCount;

            if (packetCast)
            {
                Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(0, Slot, -1, prediction.CastPosition.X,
                    prediction.CastPosition.Y, prediction.CastPosition.X, prediction.CastPosition.Y)).Send();
            }
            else
            {
                //Cant cast the spell (actually should not happen).
                if (!ObjectManager.Player.Spellbook.CastSpell(Slot, prediction.CastPosition))
                    return CastStates.NotCasted;
            }

            return CastStates.SuccessfullyCasted;
        }

        public CastStates Cast(Obj_AI_Base unit, bool packetCast = false, bool aoe = false)
        {
            return _cast(unit, packetCast, aoe);
        }

        public void CastOnUnit(Obj_AI_Base unit, bool packetCast = false)
        {
            if (From.Distance(unit.ServerPosition) > Range) return;

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

        public bool CastIfHitchanceEquals(Obj_AI_Base unit, Prediction.HitChance hitChance, bool packetCast = false)
        {
            var currentHitchance = MinHitChange;
            MinHitChange = hitChance;
            var castResult = _cast(unit, packetCast, false, true);
            MinHitChange = currentHitchance;
            return castResult == CastStates.SuccessfullyCasted;
        }

        public bool CastIfWillHit(Obj_AI_Base unit, int minTargets = 5, bool packetCast = false)
        {
            var castResult = _cast(unit, packetCast, true, false, minTargets);
            return castResult == CastStates.SuccessfullyCasted;
        }

        public void Cast(Vector2 position, bool packetCast = false)
        {
            Cast(position.To3D(), packetCast);
        }

        public void Cast(Vector3 position, bool packetCast = false)
        {
            LastCastAttemptT = Environment.TickCount;

            if (packetCast)
            {
                Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(0, Slot, -1, position.X,
                    position.Y, position.X, position.Y)).Send();
            }
            else
            {
                ObjectManager.Player.Spellbook.CastSpell(Slot, position);
            }
        }

        public bool IsReady()
        {
            return ObjectManager.Player.Spellbook.CanUseSpell(Slot) == SpellState.Ready;
        }

        public float GetHealthPrediction(Obj_AI_Base unit)
        {
            var time = (int)(Delay * 1000 +
                             From.Distance(
                                 unit.ServerPosition) / Speed - 100);
            return HealthPrediction.GetHealthPrediction(unit, time);
        }

        public MinionManager.FarmLocation GetCircularFarmLocation(List<Obj_AI_Base> minionPositions,
            float overrideWidth = float.MaxValue)
        {
            var positions = MinionManager.GetMinionsPredictedPositions(minionPositions, Delay, Width, Speed,
                From, Range, false, Prediction.SkillshotType.SkillshotCircle);

            return GetCircularFarmLocation(positions, overrideWidth);
        }

        public MinionManager.FarmLocation GetCircularFarmLocation(List<Vector2> minionPositions,
            float overrideWidth = float.MaxValue)
        {
            return MinionManager.GetBestCircularFarmLocation(minionPositions,
                overrideWidth != -1 ? overrideWidth : Width, Range);
        }

        public MinionManager.FarmLocation GetLineFarmLocation(List<Obj_AI_Base> minionPositions,
            float overrideWidth = float.MaxValue)
        {
            var positions = MinionManager.GetMinionsPredictedPositions(minionPositions, Delay, Width, Speed,
                From, Range, false, Prediction.SkillshotType.SkillshotCircle);

            return GetCircularFarmLocation(positions, overrideWidth);
        }

        public MinionManager.FarmLocation GetLineFarmLocation(List<Vector2> minionPositions,
            float overrideWidth = float.MaxValue)
        {
            return MinionManager.GetBestLineFarmLocation(minionPositions, overrideWidth != -1 ? overrideWidth : Width,
                Range);
        }


        public int CountHits(List<Obj_AI_Base> units, Vector3 castPosition)
        {
            var points = new List<Vector3>();
            foreach (var unit in units)
            {
                points.Add(GetPrediction(unit).Position);
            }
            return CountHits(points, castPosition);
        }

        public int CountHits(List<Vector3> points, Vector3 castPosition)
        {
            var hits = 0;
            foreach (var point in points)
            {
                if (WillHit(point, castPosition, 0))
                    hits++;
            }

            return hits;
        }


        public bool WillHit(Obj_AI_Base unit, Vector3 castPosition, int extraWidth = 0)
        {
            var unitPosition = GetPrediction(unit);
            if (unitPosition.HitChance >= Prediction.HitChance.HighHitchance)
            {
                return WillHit(unitPosition.Position, castPosition, extraWidth);
            }
            return false;
        }

        public bool WillHit(Vector3 point, Vector3 castPosition, int extraWidth = 0)
        {
            switch (Type)
            {
                case Prediction.SkillshotType.SkillshotCircle:
                    if (point.To2D().Distance(castPosition) < Width)
                        return true;
                    break;

                case Prediction.SkillshotType.SkillshotLine:
                    if (point.To2D().Distance(castPosition.To2D(), From.To2D(), true) < Width + extraWidth)
                        return true;
                    break;
            }

            return false;
        }
    }
}