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
    /// This class allows you to handle the spells easily.
    /// </summary>
    public class Spell
    {
        /// <summary>
        /// The state of the spell after being cases.
        /// </summary>
        public enum CastStates
        {
            /// <summary>
            /// The spell was successfully casted
            /// </summary>
            SuccessfullyCasted,

            /// <summary>
            /// The spell was not ready
            /// </summary>
            NotReady,

            /// <summary>
            /// The spell was not casted
            /// </summary>
            NotCasted,

            /// <summary>
            /// The spell was out of range
            /// </summary>
            OutOfRange,

            /// <summary>
            /// There is a collision.
            /// </summary>
            Collision,

            /// <summary>
            /// There is not enough targets
            /// </summary>
            NotEnoughTargets,

            /// <summary>
            /// The spell has a low hit chance
            /// </summary>
            LowHitChance,
        }

        /// <summary>
        /// The tick the charged spell was casted at.
        /// </summary>
        private int _chargedCastedT;

        /// <summary>
        /// The tick the charged request was sent at.
        /// </summary>
        private int _chargedReqSentT;

        /// <summary>
        /// The from position.
        /// </summary>
        private Vector3 _from;

        /// <summary>
        /// The range of the spell
        /// </summary>
        private float _range;

        /// <summary>
        /// The position to check the range from.
        /// </summary>
        private Vector3 _rangeCheckFrom;

        /// <summary>
        /// The width
        /// </summary>
        private float _width;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spell"/> class.
        /// </summary>
        /// <param name="slot">The slot.</param>
        /// <param name="range">The range.</param>
        /// <param name="damageType">Type of the damage.</param>
        public Spell(SpellSlot slot,
            float range = float.MaxValue,
            TargetSelector.DamageType damageType = TargetSelector.DamageType.Physical)
        {
            Slot = slot;
            Range = range;
            DamageType = damageType;

            // Default values
            MinHitChance = HitChance.VeryHigh;
        }

        /// <summary>
        /// Gets or sets the name of the charged buff.
        /// </summary>
        /// <value>The name of the charged buff.</value>
        public string ChargedBuffName { get; set; }

        /// <summary>
        /// Gets or sets the charged maximum range.
        /// </summary>
        /// <value>The charged maximum range.</value>
        public int ChargedMaxRange { get; set; }

        /// <summary>
        /// Gets or sets the charged minimum range.
        /// </summary>
        /// <value>The charged minimum range.</value>
        public int ChargedMinRange { get; set; }

        /// <summary>
        /// Gets or sets the name of the charged spell.
        /// </summary>
        /// <value>The name of the charged spell.</value>
        public string ChargedSpellName { get; set; }

        /// <summary>
        /// Gets or sets the duration of the charge.
        /// </summary>
        /// <value>The duration of the charge.</value>
        public int ChargeDuration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Spell"/> has collision.
        /// </summary>
        /// <value><c>true</c> if the spell has collision; otherwise, <c>false</c>.</value>
        public bool Collision { get; set; }

        /// <summary>
        /// Gets or sets the delay.
        /// </summary>
        /// <value>The delay.</value>
        public float Delay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is charged spell.
        /// </summary>
        /// <value><c>true</c> if this instance is charged spell; otherwise, <c>false</c>.</value>
        public bool IsChargedSpell { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is skillshot.
        /// </summary>
        /// <value><c>true</c> if this instance is skillshot; otherwise, <c>false</c>.</value>
        public bool IsSkillshot { get; set; }

        /// <summary>
        /// Gets or sets the last cast attempt tick.
        /// </summary>
        /// <value>The last cast attempt tick.</value>
        public int LastCastAttemptT { get; set; }

        /// <summary>
        /// Gets or sets the minimum hit chance.
        /// </summary>
        /// <value>The minimum hit chance.</value>
        public HitChance MinHitChance { get; set; }

        /// <summary>
        /// Gets or sets the spell slot.
        /// </summary>
        /// <value>The slot.</value>
        public SpellSlot Slot { get; set; }

        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>The speed.</value>
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets the type of skillshot.
        /// </summary>
        /// <value>The type of skillshot.</value>
        public SkillshotType Type { get; set; }

        /// <summary>
        /// Gets or sets the type of the damage.
        /// </summary>
        /// <value>The type of the damage.</value>
        public TargetSelector.DamageType DamageType { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public float Width
        {
            get { return _width; }
            set
            {
                _width = value;
                WidthSqr = value*value;
            }
        }

        /// <summary>
        /// Gets the width squared.
        /// </summary>
        /// <value>The width squared.</value>
        public float WidthSqr { get; private set; }

        /// <summary>
        /// Gets the spell data instance.
        /// </summary>
        /// <value>The spell data instance.</value>
        public SpellDataInst Instance
        {
            get { return ObjectManager.Player.Spellbook.GetSpell(Slot); }
        }

        /// <summary>
        /// Gets or sets the range.
        /// </summary>
        /// <value>The range.</value>
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
                               (Utils.TickCount - _chargedCastedT)*(ChargedMaxRange - ChargedMinRange)/
                               ChargeDuration - 150);
                }

                return ChargedMaxRange;
            }
            set { _range = value; }
        }

        /// <summary>
        /// Gets the range squared.
        /// </summary>
        /// <value>The range squared.</value>
        public float RangeSqr
        {
            get { return Range*Range; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is charging a charged spell.
        /// </summary>
        /// <value><c>true</c> if this instance is charging  a charged spell; otherwise, <c>false</c>.</value>
        public bool IsCharging
        {
            get
            {
                if (!Slot.IsReady())
                    return false;

                return ObjectManager.Player.HasBuff(ChargedBuffName) ||
                       Utils.TickCount - _chargedCastedT < 300 + Game.Ping;
            }
        }

        /// <summary>
        /// Gets the level of the spell.
        /// </summary>
        /// <value>The level of the spell.</value>
        public int Level
        {
            get { return ObjectManager.Player.Spellbook.GetSpell(Slot).Level; }
        }

        /// <summary>
        /// Gets or sets from position.
        /// </summary>
        /// <value>From position.</value>
        public Vector3 From
        {
            get { return !_from.To2D().IsValid() ? ObjectManager.Player.ServerPosition : _from; }
            set { _from = value; }
        }

        /// <summary>
        /// Gets or sets the position to check the range from.
        /// </summary>
        /// <value>Yhe position to check the range from.</value>
        public Vector3 RangeCheckFrom
        {
            get { return !_rangeCheckFrom.To2D().IsValid() ? ObjectManager.Player.ServerPosition : _rangeCheckFrom; }
            set { _rangeCheckFrom = value; }
        }


        /// <summary>
        /// Sets spell the be a targetted spell.
        /// </summary>
        /// <param name="delay">The delay.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="from">The from position.</param>
        /// <param name="rangeCheckFrom">The position to check the range from.</param>
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

        /// <summary>
        /// Sets the spell to be a skillshot.
        /// </summary>
        /// <param name="delay">The delay.</param>
        /// <param name="width">The width.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="collision">if set to <c>true</c>, the spell has collision.</param>
        /// <param name="type">The type.</param>
        /// <param name="from">From.</param>
        /// <param name="rangeCheckFrom">The range check from.</param>
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

        /// <summary>
        /// Sets the spell to be a charged spell.
        /// </summary>
        /// <param name="spellName">Name of the spell.</param>
        /// <param name="buffName">Name of the buff.</param>
        /// <param name="minRange">The minimum range.</param>
        /// <param name="maxRange">The maximum range.</param>
        /// <param name="deltaT">The delta time.</param>
        public void SetCharged(string spellName, string buffName, int minRange, int maxRange, float deltaT)
        {
            IsChargedSpell = true;
            ChargedSpellName = spellName;
            ChargedBuffName = buffName;
            ChargedMinRange = minRange;
            ChargedMaxRange = maxRange;
            ChargeDuration = (int) (deltaT*1000);
            _chargedCastedT = 0;

            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
            Spellbook.OnUpdateChargedSpell += Spellbook_OnUpdateChargedSpell;
            Spellbook.OnCastSpell += SpellbookOnCastSpell;
        }

        /// <summary>
        /// Start charging the spell if its not charging.
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
        /// Start charging the spell if its not charging.
        /// </summary>
        /// <param name="position">The position.</param>
        public void StartCharging(Vector3 position)
        {
            if (!IsCharging && Utils.TickCount - _chargedReqSentT > 400 + Game.Ping)
            {
                ObjectManager.Player.Spellbook.CastSpell(Slot, position);
                _chargedReqSentT = Utils.TickCount;
            }
        }

        /// <summary>
        /// Fired when the charged spell is updated.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="SpellbookUpdateChargedSpellEventArgs"/> instance containing the event data.</param>
        void Spellbook_OnUpdateChargedSpell(Spellbook sender, SpellbookUpdateChargedSpellEventArgs args)
        {
            if (sender.Owner.IsMe && Utils.TickCount - _chargedReqSentT < 3000 && args.ReleaseCast)
            {
                args.Process = false;
            }
        }

        /// <summary>
        /// Fired when the spellbook casts a spell.
        /// </summary>
        /// <param name="spellbook">The spellbook.</param>
        /// <param name="args">The <see cref="SpellbookCastSpellEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Fired when the game processes a spell cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        private void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == ChargedSpellName)
            {
                _chargedCastedT = Utils.TickCount;
            }
        }

        /// <summary>
        /// Updates the source position.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="rangeCheckFrom">The range check from.</param>
        public void UpdateSourcePosition(Vector3 from = new Vector3(), Vector3 rangeCheckFrom = new Vector3())
        {
            From = from;
            RangeCheckFrom = rangeCheckFrom;
        }

        /// <summary>
        /// Gets the prediction.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="aoe">if set to <c>true</c>, the prediction will try to hit as many enemies.</param>
        /// <param name="overrideRange">The override range.</param>
        /// <param name="collisionable">The collisionable.</param>
        /// <returns>PredictionOutput.</returns>
        public PredictionOutput GetPrediction(Obj_AI_Base unit, bool aoe = false, float overrideRange = -1,
            CollisionableObjects[] collisionable = null)
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
                            collisionable ?? new[] {CollisionableObjects.Heroes, CollisionableObjects.Minions}
                    });
        }

        /// <summary>
        /// Gets the collision.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="delayOverride">The delay override.</param>
        /// <returns>List&lt;Obj_AI_Base&gt;.</returns>
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

        /// <summary>
        /// Gets the hit count.
        /// </summary>
        /// <param name="hitChance">The hit chance.</param>
        /// <returns>System.Single.</returns>
        public float GetHitCount(HitChance hitChance = HitChance.High)
        {
            return HeroManager.Enemies.Select(e => GetPrediction(e)).Count(p => p.Hitchance >= hitChance);
        }

        /// <summary>
        /// Casts the spell at the specified unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="packetCast">if set to <c>true</c> [packet cast].</param>
        /// <param name="aoe">if set to <c>true</c> [aoe].</param>
        /// <param name="exactHitChance">if set to <c>true</c> [exact hit chance].</param>
        /// <param name="minTargets">The minimum targets.</param>
        /// <returns>CastStates.</returns>
        private CastStates _cast(Obj_AI_Base unit,
            bool packetCast = false,
            bool aoe = false,
            bool exactHitChance = false,
            int minTargets = -1)
        {
            if (unit == null || MenuGUI.IsShopOpen || MenuGUI.IsChatOpen)
            {
                return CastStates.NotCasted;
            }

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
        /// Casts the targetted spell on the unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="packetCast">if set to <c>true</c> [packet cast].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CastOnUnit(Obj_AI_Base unit, bool packetCast = false)
        {
            if (!Slot.IsReady() || From.Distance(unit.ServerPosition, true) > RangeSqr)
            {
                return false;
            }

            if (MenuGUI.IsShopOpen || MenuGUI.IsChatOpen)
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
        /// Casts the spell to the unit using the prediction if its an skillshot.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="packetCast">if set to <c>true</c>, uses packets to cast the spell.</param>
        /// <param name="aoe">if set to <c>true</c>, the prediction will try to hit as many enemies as possible.</param>
        /// <returns>CastStates.</returns>
        public CastStates Cast(Obj_AI_Base unit, bool packetCast = false, bool aoe = false)
        {
            return _cast(unit, packetCast, aoe);
        }

        /// <summary>
        /// Casts the spell on the player.
        /// </summary>
        /// <param name="packetCast">if set to <c>true</c> uses packets to cast the spell.</param>
        /// <returns><c>true</c> if the spell was casted sucessfully, <c>false</c> otherwise.</returns>
        public bool Cast(bool packetCast = false)
        {
            return CastOnUnit(ObjectManager.Player, packetCast);
        }

        /// <summary>
        /// Casts the spell from a position to a position.
        /// </summary>
        /// <param name="fromPosition">From position.</param>
        /// <param name="toPosition">To position.</param>
        /// <returns><c>true</c> if the spell was sucessfully casted, <c>false</c> otherwise.</returns>
        public bool Cast(Vector2 fromPosition, Vector2 toPosition)
        {
            return Cast(fromPosition.To3D(), toPosition.To3D());
        }

        /// <summary>
        /// Casts the spell from a position to a position.
        /// </summary>
        /// <param name="fromPosition">From position.</param>
        /// <param name="toPosition">To position.</param>
        /// <returns><c>true</c> if the spell was sucessfully casted, <c>false</c> otherwise.</returns>
        public bool Cast(Vector3 fromPosition, Vector3 toPosition)
        {
            if (MenuGUI.IsShopOpen || MenuGUI.IsChatOpen)
            {
                return false;
            }
            return Slot.IsReady() && ObjectManager.Player.Spellbook.CastSpell(Slot, fromPosition, toPosition);
        }

        /// <summary>
        /// Casts the spell to the position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="packetCast">if set to <c>true</c> uses packets to cast the spell</param>
        /// <returns><c>true</c> if the spell was casted successfully, <c>false</c> otherwise.</returns>
        public bool Cast(Vector2 position, bool packetCast = false)
        {
            if (MenuGUI.IsShopOpen || MenuGUI.IsChatOpen)
            {
                return false;
            }
            return Cast(position.To3D(), packetCast);
        }

        /// <summary>
        /// Casts the spell to the position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="packetCast">if set to <c>true</c> uses packets to cast the spell.</param>
        /// <returns><c>true</c> if the spell was casted sucessfully, <c>false</c> otherwise.</returns>
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

            else if (IsChannelTypeSpell)
            {
                if (TargetSpellCancel)
                {
                    CastCancelSpell(position);
                }
                else
                {
                    CastCancelSpell();
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

        /// <summary>
        /// Shoots the charged spell.
        /// </summary>
        /// <param name="slot">The slot.</param>
        /// <param name="position">The position.</param>
        /// <param name="releaseCast">if set to <c>true</c> [release cast].</param>
        private static void ShootChargedSpell(SpellSlot slot, Vector3 position, bool releaseCast = true)
        {
            position.Z = NavMesh.GetHeightForPosition(position.X, position.Y);
            ObjectManager.Player.Spellbook.UpdateChargedSpell(slot, position, releaseCast, false);
            ObjectManager.Player.Spellbook.CastSpell(slot, position, false);
        }

        /// <summary>
        /// Casts the spell if the hitchance equals the set hitchance.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="hitChance">The hit chance.</param>
        /// <param name="packetCast">if set to <c>true</c> [packet cast].</param>
        /// <returns><c>true</c> if the spell was successfully casted, <c>false</c> otherwise.</returns>
        public bool CastIfHitchanceEquals(Obj_AI_Base unit, HitChance hitChance, bool packetCast = false)
        {
            var currentHitchance = MinHitChance;
            MinHitChance = hitChance;
            var castResult = _cast(unit, packetCast, false, false);
            MinHitChance = currentHitchance;
            return castResult == CastStates.SuccessfullyCasted;
        }

        /// <summary>
        /// Casts the spell if it will hit the set targets.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="minTargets">The minimum targets.</param>
        /// <param name="packetCast">if set to <c>true</c> [packet cast].</param>
        /// <returns><c>true</c> if the spell was successfully casted, <c>false</c> otherwise.</returns>
        public bool CastIfWillHit(Obj_AI_Base unit, int minTargets = 5, bool packetCast = false)
        {
            var castResult = _cast(unit, packetCast, true, false, minTargets);
            return castResult == CastStates.SuccessfullyCasted;
        }

        /// <summary>
        /// Returns the unit health when the spell hits the unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns>System.Single.</returns>
        public float GetHealthPrediction(Obj_AI_Base unit)
        {
            var time = (int)(Delay * 1000 + From.Distance(unit.ServerPosition) / Speed - 100);
            return HealthPrediction.GetHealthPrediction(unit, time);
        }

        /// <summary>
        /// Gets the circular farm location.
        /// </summary>
        /// <param name="minionPositions">The minion positions.</param>
        /// <param name="overrideWidth">Width of the override.</param>
        /// <returns>MinionManager.FarmLocation.</returns>
        public MinionManager.FarmLocation GetCircularFarmLocation(List<Obj_AI_Base> minionPositions,
            float overrideWidth = -1)
        {
            var positions = MinionManager.GetMinionsPredictedPositions(
                minionPositions, Delay, Width, Speed, From, Range, false, SkillshotType.SkillshotCircle);

            return GetCircularFarmLocation(positions, overrideWidth);
        }

        /// <summary>
        /// Gets the circular farm location.
        /// </summary>
        /// <param name="minionPositions">The minion positions.</param>
        /// <param name="overrideWidth">Width of the override.</param>
        /// <returns>MinionManager.FarmLocation.</returns>
        public MinionManager.FarmLocation GetCircularFarmLocation(List<Vector2> minionPositions,
            float overrideWidth = -1)
        {
            return MinionManager.GetBestCircularFarmLocation(
                minionPositions, overrideWidth >= 0 ? overrideWidth : Width, Range);
        }

        /// <summary>
        /// Gets the line farm location.
        /// </summary>
        /// <param name="minionPositions">The minion positions.</param>
        /// <param name="overrideWidth">Width of the override.</param>
        /// <returns>MinionManager.FarmLocation.</returns>
        public MinionManager.FarmLocation GetLineFarmLocation(List<Obj_AI_Base> minionPositions,
            float overrideWidth = -1)
        {
            var positions = MinionManager.GetMinionsPredictedPositions(
                minionPositions, Delay, Width, Speed, From, Range, false, SkillshotType.SkillshotLine);

            return GetLineFarmLocation(positions, overrideWidth >= 0 ? overrideWidth : Width);
        }

        /// <summary>
        /// Gets the line farm location.
        /// </summary>
        /// <param name="minionPositions">The minion positions.</param>
        /// <param name="overrideWidth">Width of the override.</param>
        /// <returns>MinionManager.FarmLocation.</returns>
        public MinionManager.FarmLocation GetLineFarmLocation(List<Vector2> minionPositions, float overrideWidth = -1)
        {
            return MinionManager.GetBestLineFarmLocation(
                minionPositions, overrideWidth >= 0 ? overrideWidth : Width, Range);
        }

        /// <summary>
        /// Counts the hits.
        /// </summary>
        /// <param name="units">The units.</param>
        /// <param name="castPosition">The cast position.</param>
        /// <returns>System.Int32.</returns>
        public int CountHits(List<Obj_AI_Base> units, Vector3 castPosition)
        {
            var points = units.Select(unit => GetPrediction(unit).UnitPosition).ToList();
            return CountHits(points, castPosition);
        }

        /// <summary>
        /// Counts the hits.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="castPosition">The cast position.</param>
        /// <returns>System.Int32.</returns>
        public int CountHits(List<Vector3> points, Vector3 castPosition)
        {
            return points.Count(point => WillHit(point, castPosition));
        }

        /// <summary>
        /// Gets the damage that the skillshot will deal to the target using the damage library.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="stage">The stage.</param>
        /// <returns>System.Single.</returns>
        public float GetDamage(Obj_AI_Base target, int stage = 0)
        {
            return (float) ObjectManager.Player.GetSpellDamage(target, Slot, stage);
        }

        /// <summary>
        /// Returns the amount of mana the spell costs to cast
        /// </summary>
        /// <value>The mana cost.</value>
        public float ManaCost
        {
            get { return ObjectManager.Player.Spellbook.GetSpell(Slot).ManaCost; }
        }

        /// <summary>
        /// Returns the cooldown of the spell.
        /// </summary>
        /// <value>The cooldown.</value>
        public float Cooldown
        {
            get
            {
                var coolDown = ObjectManager.Player.Spellbook.GetSpell(Slot).CooldownExpires;
                return Game.Time < coolDown ? coolDown - Game.Time : 0;
            }
        }

        /// <summary>
        /// Gets the damage that the skillshot will deal to the target using the damage lib and returns if the target is
        /// killable or not.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="stage">The stage.</param>
        /// <returns><c>true</c> if the specified target is killable; otherwise, <c>false</c>.</returns>
        public bool IsKillable(Obj_AI_Base target, int stage = 0)
        {
            return ObjectManager.Player.GetSpellDamage(target, Slot, stage) > target.Health;
        }

        /// <summary>
        /// Returns if the spell will hit the unit when casted on castPosition.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="castPosition">The cast position.</param>
        /// <param name="extraWidth">Added width to the spell.</param>
        /// <param name="minHitChance">The minimum hit chance.</param>
        /// <returns><c>true</c> if the spell will hit the unit, <c>false</c> otherwise.</returns>
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
        /// Returns if the spell will hit the point when casted on castPosition.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="castPosition">The cast position.</param>
        /// <param name="extraWidth">Width of the extra.</param>
        /// <returns><c>true</c> if the spell will hit the location, <c>false</c> otherwise.</returns>
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
                    var edge1 = (castPosition.To2D() - From.To2D()).Rotated(-Width/2);
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
        /// Returns if a spell can be cast and the target is in range.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns><c>true</c> if this instance can cast on the specified unit; otherwise, <c>false</c>.</returns>
        public bool CanCast(Obj_AI_Base unit)
        {
            return Slot.IsReady() && unit.IsValidTarget(Range);
        }

        /// <summary>
        /// Returns if the GameObject is in range of the spell.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="range">The range.</param>
        /// <returns><c>true</c> if the spell is in range of the unit; otherwise, <c>false</c>.</returns>
        public bool IsInRange(GameObject obj, float range = -1)
        {
            return IsInRange(
                obj is Obj_AI_Base ? (obj as Obj_AI_Base).ServerPosition.To2D() : obj.Position.To2D(), range);
        }

        /// <summary>
        /// Returns if the position is in range of the spell.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="range">The range.</param>
        /// <returns><c>true</c> if the specified location is in range of the spell; otherwise, <c>false</c>.</returns>
        public bool IsInRange(Vector3 point, float range = -1)
        {
            return IsInRange(point.To2D(), range);
        }

        /// <summary>
        /// Returns if the Vector2 is in range of the spell.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="range">The range.</param>
        /// <returns><c>true</c> if the specified location is in range of the spell; otherwise, <c>false</c>.</returns>
        public bool IsInRange(Vector2 point, float range = -1)
        {
            return RangeCheckFrom.To2D().Distance(point, true) < (range < 0 ? RangeSqr : range*range);
        }

        /// <summary>
        /// Returns the best target found using the current TargetSelector mode.
        /// Please make sure to set the Spell.DamageType Property to the type of damage this spell does (if not done on
        /// initialization).
        /// </summary>
        /// <param name="extraRange">The extra range.</param>
        /// <param name="champsToIgnore">The champs to ignore.</param>
        /// <returns>Obj_AI_Hero.</returns>
        public Obj_AI_Hero GetTarget(float extraRange = 0, IEnumerable<Obj_AI_Hero> champsToIgnore = null)
        {
            return TargetSelector.GetTarget(Range + extraRange, DamageType, true, champsToIgnore, From);
        }

        /// <summary>
        /// Spell will be casted on the best target found with the Spell.GetTarget method.
        /// </summary>
        /// <param name="extraRange">The extra range.</param>
        /// <param name="packetCast">if set to <c>true</c>, casts the spell with packets..</param>
        /// <param name="aoe">if set to <c>true</c>, the prediction will try to hit as many enemies as possible.</param>
        /// <returns>CastStates.</returns>
        public CastStates CastOnBestTarget(float extraRange = 0, bool packetCast = false, bool aoe = false)
        {
            if (MenuGUI.IsShopOpen || MenuGUI.IsChatOpen)
            {
                return CastStates.NotCasted;
            }

            var target = GetTarget(extraRange);
            return target != null ? Cast(target, packetCast, aoe) : CastStates.NotCasted;
        }


        /// <summary>
        /// Allow user to cancel channeling
        /// </summary>
        public bool CanBeCanceledByUser { get; set; }

        /// <summary>
        /// check if the spell is being channeled
        /// </summary>
        public bool IsChanneling = false;

        /// <summary>
        /// Is spell type channel
        /// </summary>
        public bool IsChannelTypeSpell { get; set; }

        /// <summary>
        /// Is spell targettable
        /// </summary>
        public bool TargetSpellCancel { get; set; }

        /// <summary>
        /// Should the spell  be interuptable by casting other spells
        /// </summary>
        public bool LetSpellcancel { get; set; }

        /// <summary>
        /// Last time casting has been issued
        /// </summary>
        private int _cancelSpellIssue;
        

        /// <summary>
        /// Spell setings
        /// </summary>
        /// <param name="letUserCancel"></param>
        /// <param name="targetted"></param>
        /// <param name="letSpellCancel"></param>
        public void Setinterruptible(bool letUserCancel, bool targetted,
            bool letSpellCancel = false)
        {
            CanBeCanceledByUser = letUserCancel;
            TargetSpellCancel = targetted;
            IsChanneling = false;
            LetSpellcancel = letSpellCancel;

            Obj_AI_Base.OnDoCast += OnDoCast;
            GameObject.OnDelete += OnDelete;
            Game.OnWndProc += OnWndProc;
            Obj_AI_Base.OnIssueOrder += OnOrder;
            Spellbook.OnCastSpell += OnCastSpell;

            if (ObjectManager.Player.ChampionName == "Fiddlesticks")
            {
                GameObject.OnCreate += OnCreate;
            }
        }

        /// <summary>
        /// Diffrenet spell process names
        /// </summary>
        private readonly string[] _processName =
        {
            "DrainChannel", "KatarinaR", "Crowstorm",
            "GalioIdolOfDurand", "AlZaharNetherGrasp",
            "ReapTheWhirlwind"
        };

        /// <summary>
        /// Diffrenet object names
        /// </summary>
        private readonly string[] _deleteObject =
        {
            "Fiddlesticks_Base_Drain.troy", "katarina_deathLotus_tar.troy",
            "Galio_Base_R_explo.troy", "Malzahar_Base_R_Beam.troy",
            "ReapTheWhirlwind_green_cas.troy",
        };


        private void OnCreate(GameObject sender, EventArgs args)
        {
            if (sender.Name == "Fiddlesticks_Base_Base_Crowstorm_green_cas.troy")
            {
                IsChanneling = false;
            }
        }

        /// <summary>
        /// Check when the skill object has been casted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;

            if (_processName.Contains(args.SData.Name))
            {
                IsChanneling = true;
            }
        }

        /// <summary>
        /// Check when an object has been deleted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDelete(GameObject sender, EventArgs args)
        {
            if (_deleteObject.Contains(sender.Name))
            {
                IsChanneling = false;
            }
        }

        private void OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {

            if (LetSpellcancel) return;

            args.Process = !IsChanneling;
        }

        public void CastCancelSpell()
        {
            if (!IsChanneling && Utils.TickCount - _cancelSpellIssue > 400 + Game.Ping)
            {
                ObjectManager.Player.Spellbook.CastSpell(Slot);
                _cancelSpellIssue = Utils.TickCount;
            }
        }

        public void CastCancelSpell(Vector3 position)
        {
            if (!IsChanneling && Utils.TickCount - _cancelSpellIssue > 400 + Game.Ping)
            {
                ObjectManager.Player.Spellbook.CastSpell(Slot, position);
                _cancelSpellIssue = Utils.TickCount;
            }
        }


        /// <summary>
        /// Check when a spell has been casted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnOrder(Obj_AI_Base sender, GameObjectIssueOrderEventArgs args)
        {
            if (!sender.IsMe) return;
            
            if (!IsChanneling) return;  

            if (args.Order == GameObjectOrder.MoveTo || args.Order == GameObjectOrder.AttackTo ||
                args.Order == GameObjectOrder.AttackUnit || args.Order == GameObjectOrder.AutoAttack)
            {
                args.Process = false;
            }
        }

        /// <summary>
        /// When player sends a key command
        /// </summary>
        /// <param name="args"></param>
        private void OnWndProc(WndEventArgs args)
        {

            if (!CanBeCanceledByUser) return;
            
            if (args.Msg == 517)
            {
                IsChanneling = false;
            }
        }
    }
}
