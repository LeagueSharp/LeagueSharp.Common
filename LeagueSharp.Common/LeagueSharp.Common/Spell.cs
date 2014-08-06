#region

using System;
using System.Collections.Generic;
using System.ServiceModel;
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
        public Prediction.HitChance MinHitChange = Prediction.HitChance.HighHitchance;
        public SpellSlot Slot;
        public float Speed;
        public Prediction.SkillshotType Type;
        public float Width;
        private int _chargedCastedT;
        private int _chargedReqSentT;
        private Vector3 _from;
        private float _range;
        private Vector3 _rangeCheckFrom;

        public Spell(SpellSlot slot, float range)
        {
            Slot = slot;
            Range = range;
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
                               Math.Min(ChargedMaxRange - ChargedMinRange,
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
                return ObjectManager.Player.HasBuff(ChargedBuffName, true) || Environment.TickCount - _chargedCastedT < 300;
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

        public void SetTargetted(float delay, float speed, Vector3 from = new Vector3(),
            Vector3 rangeCheckFrom = new Vector3())
        {
            Delay = delay;
            Speed = speed;
            From = from;
            RangeCheckFrom = rangeCheckFrom;
            IsSkillshot = false;
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
            IsSkillshot = true;
        }


        public void SetCharged(string spellName, string buffName, int minRange, int maxRange, float deltaT)
        {
            IsChargedSpell = true;
            ChargedSpellName = spellName;
            ChargedBuffName = buffName;
            ChargedMinRange = minRange;
            ChargedMaxRange = maxRange;
            ChargeDuration = (int)(deltaT * 1000);
            _chargedCastedT = 0;

            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
            Game.OnGameSendPacket += Game_OnGameSendPacket;
        }

        /// <summary>
        /// Start charging the spell if its not charging.
        /// </summary>
        public void StartCharging()
        {
            if (!IsCharging && Environment.TickCount - _chargedReqSentT > 300 + Game.Ping)
            {
                Cast();
                _chargedReqSentT = Environment.TickCount;
            }
        }

        private void Game_OnGameSendPacket(GamePacketEventArgs args)
        {
            if (args.PacketData[0] == Packet.C2S.ChargedCast.Header && Environment.TickCount - _chargedReqSentT < 3000)
            {
                var decoded = Packet.C2S.ChargedCast.Decoded(args.PacketData);
                if (decoded.SourceNetworkId != ObjectManager.Player.NetworkId) return;
                args.Process = false;
            }

            if (args.PacketData[0] == Packet.C2S.Cast.Header)
            {
                var decoded = Packet.C2S.Cast.Decoded(args.PacketData);
                if (decoded.Slot != Slot) return;
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
            if (!IsSkillshot)
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

            if (IsChargedSpell)
            {
                if (IsCharging)
                {
                    Packet.C2S.ChargedCast.Encoded(new Packet.C2S.ChargedCast.Struct((SpellSlot)(0x80 + (byte)Slot),
                        prediction.CastPosition.X, prediction.CastPosition.Z, prediction.CastPosition.Y)).Send();
                }
                else
                {
                    StartCharging();
                }
            }
            else if (packetCast)
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

        /// <summary>
        /// Self-casts the spell.
        /// </summary>
        public bool Cast()
        {
            if(IsReady())
                return ObjectManager.Player.Spellbook.CastSpell(Slot);
            else
                return false;
        }

        /// <summary>
        /// Casts the targetted spell on the unit.
        /// </summary>
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

        /// <summary>
        /// Casts the spell to the unit using the prediction if its an skillshot.
        /// </summary>
        public CastStates Cast(Obj_AI_Base unit, bool packetCast = false, bool aoe = false)
        {
            return _cast(unit, packetCast, aoe);
        }

        /// <summary>
        /// Casts the spell to the position.
        /// </summary>
        public void Cast(Vector2 position, bool packetCast = false)
        {
            Cast(position.To3D(), packetCast);
        }

        /// <summary>
        /// Casts the spell to the position.
        /// </summary>
        public void Cast(Vector3 position, bool packetCast = false)
        {
            LastCastAttemptT = Environment.TickCount;

            if (IsChargedSpell)
            {
                if (IsCharging)
                    Packet.C2S.ChargedCast.Encoded(new Packet.C2S.ChargedCast.Struct((SpellSlot)(0x80 + (byte)Slot),
                        position.X, position.Z, position.Y)).Send();
                else

                    StartCharging();
            }
            else if (packetCast)
            {
                Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(0, Slot, -1, position.X,
                    position.Y, position.X, position.Y)).Send();
            }
            else
            {
                ObjectManager.Player.Spellbook.CastSpell(Slot, position);
            }
        }

        /// <summary>
        /// Casts the spell if the hitchance equals the set hitchance.
        /// </summary>
        public bool CastIfHitchanceEquals(Obj_AI_Base unit, Prediction.HitChance hitChance, bool packetCast = false)
        {
            var currentHitchance = MinHitChange;
            MinHitChange = hitChance;
            var castResult = _cast(unit, packetCast, false, true);
            MinHitChange = currentHitchance;
            return castResult == CastStates.SuccessfullyCasted;
        }

        /// <summary>
        /// Casts the spell if it will hit the set targets.
        /// </summary>
        public bool CastIfWillHit(Obj_AI_Base unit, int minTargets = 5, bool packetCast = false)
        {
            var castResult = _cast(unit, packetCast, true, false, minTargets);
            return castResult == CastStates.SuccessfullyCasted;
        }

        /// <summary>
        /// Returns if the spell is ready to use.
        /// </summary>
        public bool IsReady(int t = 0)
        {
            if (t == 0 || ObjectManager.Player.Spellbook.CanUseSpell(Slot) == SpellState.Ready)
                return (t == 0) ? ObjectManager.Player.Spellbook.CanUseSpell(Slot) == SpellState.Ready : true;

            return ObjectManager.Player.Spellbook.CanUseSpell(Slot) == SpellState.Cooldown &&
                   (ObjectManager.Player.Spellbook.GetSpell(Slot).CooldownExpires - Game.Time) <= t / 1000f;
        }

        /// <summary>
        /// Returns the unit health when the spell hits the unit.
        /// </summary>
        public float GetHealthPrediction(Obj_AI_Base unit)
        {
            var time = (int)(Delay * 1000 + From.Distance(unit.ServerPosition) / Speed - 100);
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

        internal int CountHits(List<Obj_AI_Base> units, Vector3 castPosition)
        {
            var points = new List<Vector3>();
            foreach (var unit in units)
                points.Add(GetPrediction(unit).Position);
            return CountHits(points, castPosition);
        }

        internal int CountHits(List<Vector3> points, Vector3 castPosition)
        {
            var hits = 0;
            foreach (var point in points)
                if (WillHit(point, castPosition, 0))
                    hits++;

            return hits;
        }

        /// <summary>
        /// Gets the damage that the skillshot will deal to the target using the damage lib.
        /// </summary>
        public float GetDamage(Obj_AI_Base target, DamageLib.StageType stagetype = DamageLib.StageType.Default)
        {
            var type = DamageLib.SpellType.Q;
            switch (Slot)
            {
                case SpellSlot.Q: type = DamageLib.SpellType.Q; break;
                case SpellSlot.W: type = DamageLib.SpellType.W; break;
                case SpellSlot.E: type = DamageLib.SpellType.E; break;
                case SpellSlot.R: type = DamageLib.SpellType.R; break;
            }
            return (float)DamageLib.getDmg(target, type, stagetype);
        }

        /// <summary>
        /// Returns if the spell will hit the unit when casted on castPosition.
        /// </summary>
        public bool WillHit(Obj_AI_Base unit, Vector3 castPosition, int extraWidth = 0,
            Prediction.HitChance minHitChance = Prediction.HitChance.HighHitchance)
        {
            var unitPosition = GetPrediction(unit);
            if (unitPosition.HitChance >= minHitChance)
                return WillHit(unitPosition.Position, castPosition, extraWidth);

            return false;
        }

        /// <summary>
        /// Returns if the spell will hit the point when casted on castPosition.
        /// </summary>
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
                case Prediction.SkillshotType.SkillshotCone:
                    var edge1 = (castPosition.To2D() - From.To2D()).Rotated(-Width / 2);
                    var edge2 = edge1.Rotated(Width);
                    var v = point.To2D() - From.To2D();
                    if (point.To2D().Distance(From) < Range && edge1.CrossProduct(v) > 0 && v.CrossProduct(edge2) > 0)
                        return true;
                    break;
            }

            return false;
        }

        /// <summary>
        /// Returns if the point is in range of the spell.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool InRange(Vector3 point)
        {
            return RangeCheckFrom.Distance(point) < Range;
        }
    }
}