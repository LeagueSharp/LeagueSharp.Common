namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp.Common.Data;
    using LeagueSharp.Data.Enumerations;

    using SharpDX;

    /// <summary>
    ///     This class allows you to handle the spells easily.
    /// </summary>
    public class Spell
    {
        #region Fields

        /// <summary>
        ///     check if the spell is being channeled
        /// </summary>
        public bool IsChanneling = false;

        /// <summary>
        ///     Diffrenet object names
        /// </summary>
        private readonly string[] _deleteObject =
            {
                "Fiddlesticks_Base_Drain.troy", "katarina_deathLotus_tar.troy",
                "Galio_Base_R_explo.troy", "Malzahar_Base_R_Beam.troy",
                "ReapTheWhirlwind_green_cas.troy",
            };

        /// <summary>
        ///     Diffrenet spell process names
        /// </summary>
        private readonly string[] _processName =
            {
                "DrainChannel", "KatarinaR", "Crowstorm", "GalioIdolOfDurand",
                "AlZaharNetherGrasp", "ReapTheWhirlwind"
            };

        /// <summary>
        ///     Last time casting has been issued
        /// </summary>
        private int _cancelSpellIssue;

        /// <summary>
        ///     The tick the charged spell was casted at.
        /// </summary>
        private int _chargedCastedT;

        /// <summary>
        ///     The tick the charged request was sent at.
        /// </summary>
        private int _chargedReqSentT;

        /// <summary>
        ///     The from position.
        /// </summary>
        private Vector3 _from;

        /// <summary>
        ///     The range of the spell
        /// </summary>
        private float _range;

        /// <summary>
        ///     The position to check the range from.
        /// </summary>
        private Vector3 _rangeCheckFrom;

        /// <summary>
        ///     The width
        /// </summary>
        private float _width;

        #endregion

        #region Constructors and Destructors

        public Spell()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Spell" /> class.
        /// </summary>
        /// <param name="slot">The slot.</param>
        /// <param name="range">The range.</param>
        /// <param name="damageType">Type of the damage.</param>
        public Spell(
            SpellSlot slot,
            float range = float.MaxValue,
            TargetSelector.DamageType damageType = TargetSelector.DamageType.Physical)
        {
            this.Slot = slot;
            this.Range = range;
            this.DamageType = damageType;

            // Default values
            this.MinHitChance = HitChance.VeryHigh;
        }

        /// <summary>
        ///     Initializes a spell using SpellDb defined values
        /// </summary>
        /// <param name="slot">The SpellSlot</param>
        /// <param name="useSpellDbValues">
        ///     Doesn't matter if it's true or false, using this override will automatically use SpellDb
        ///     Values.
        /// </param>
        public Spell(SpellSlot slot, bool useSpellDbValues)
        {
            this.Slot = slot;
            var spellData = SpellDatabase.GetBySpellSlot(slot, ObjectManager.Player.CharData.BaseSkinName);
            // Charged Spell:
            if (spellData.ChargedSpellName != "")
            {
                this.ChargedBuffName = spellData.ChargedBuffName;
                this.ChargedMaxRange = spellData.ChargedMaxRange;
                this.ChargedMinRange = spellData.ChargedMinRange;
                this.ChargedSpellName = spellData.ChargedSpellName;
                this.ChargeDuration = spellData.ChargeDuration;
                this.Delay = spellData.Delay;
                this.Range = spellData.Range;
                this.Width = spellData.Radius > 0 && spellData.Radius < 30000
                                 ? spellData.Radius
                                 : ((spellData.Width > 0 && spellData.Width < 30000) ? spellData.Width : 30000);
                this.Collision = (spellData.CollisionObjects != null
                                  && spellData.CollisionObjects.Any(
                                      obj => obj == LeagueSharp.Data.Enumerations.CollisionableObjects.Minions));
                this.Speed = spellData.MissileSpeed;
                this.IsChargedSpell = true;
                this.Type = SpellDatabase.GetSkillshotTypeFromSpellType(spellData.SpellType);
                return;
            }
            // Skillshot:
            if (spellData.CastType.Any(type => type == CastType.Position || type == CastType.Direction))
            {
                this.Delay = spellData.Delay;
                this.Range = spellData.Range;
                this.Width = spellData.Radius > 0 && spellData.Radius < 30000
                                 ? spellData.Radius
                                 : ((spellData.Width > 0 && spellData.Width < 30000) ? spellData.Width : 30000);
                this.Collision = (spellData.CollisionObjects != null
                                  && spellData.CollisionObjects.Any(
                                      obj => obj == LeagueSharp.Data.Enumerations.CollisionableObjects.Minions));
                this.Speed = spellData.MissileSpeed;
                this.IsSkillshot = true;
                this.Type = SpellDatabase.GetSkillshotTypeFromSpellType(spellData.SpellType);
                return;
            }
            // Targeted:
            this.Range = spellData.Range;
            this.Delay = spellData.Delay;
            this.Speed = spellData.MissileSpeed;
            this.IsSkillshot = false;
        }

        #endregion

        #region Enums

        /// <summary>
        ///     The state of the spell after being cases.
        /// </summary>
        public enum CastStates
        {
            /// <summary>
            ///     The spell was successfully casted
            /// </summary>
            SuccessfullyCasted,

            /// <summary>
            ///     The spell was not ready
            /// </summary>
            NotReady,

            /// <summary>
            ///     The spell was not casted
            /// </summary>
            NotCasted,

            /// <summary>
            ///     The spell was out of range
            /// </summary>
            OutOfRange,

            /// <summary>
            ///     There is a collision.
            /// </summary>
            Collision,

            /// <summary>
            ///     There is not enough targets
            /// </summary>
            NotEnoughTargets,

            /// <summary>
            ///     The spell has a low hit chance
            /// </summary>
            LowHitChance,
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Adds the Enemies hitbox to the range value
        /// </summary>
        public bool AddEnemyHitboxToRange { get; set; }

        /// <summary>
        ///     Adds the Players hitbox to the range value
        /// </summary>
        public bool AddSelfHitboxToRange { get; set; }

        /// <summary>
        ///     Allow user to cancel channeling
        /// </summary>
        public bool CanBeCanceledByUser { get; set; }

        /// <summary>
        ///     Gets or sets the name of the charged buff.
        /// </summary>
        /// <value>The name of the charged buff.</value>
        public string ChargedBuffName { get; set; }

        /// <summary>
        ///     Gets or sets the charged maximum range.
        /// </summary>
        /// <value>The charged maximum range.</value>
        public int ChargedMaxRange { get; set; }

        /// <summary>
        ///     Gets or sets the charged minimum range.
        /// </summary>
        /// <value>The charged minimum range.</value>
        public int ChargedMinRange { get; set; }

        /// <summary>
        ///     Gets or sets the name of the charged spell.
        /// </summary>
        /// <value>The name of the charged spell.</value>
        public string ChargedSpellName { get; set; }

        /// <summary>
        ///     Gets or sets the duration of the charge.
        /// </summary>
        /// <value>The duration of the charge.</value>
        public int ChargeDuration { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="Spell" /> has collision.
        /// </summary>
        /// <value><c>true</c> if the spell has collision; otherwise, <c>false</c>.</value>
        public bool Collision { get; set; }

        /// <summary>
        ///     Returns the cooldown of the spell.
        /// </summary>
        /// <value>The cooldown.</value>
        public float Cooldown
        {
            get
            {
                var coolDown = ObjectManager.Player.Spellbook.GetSpell(this.Slot).CooldownExpires;
                return Game.Time < coolDown ? coolDown - Game.Time : 0;
            }
        }

        /// <summary>
        ///     Gets or sets the type of the damage.
        /// </summary>
        /// <value>The type of the damage.</value>
        public TargetSelector.DamageType DamageType { get; set; }

        /// <summary>
        ///     Gets or sets the delay.
        /// </summary>
        /// <value>The delay.</value>
        public float Delay { get; set; }

        /// <summary>
        ///     Gets or sets from position.
        /// </summary>
        /// <value>From position.</value>
        public Vector3 From
        {
            get
            {
                return !this._from.To2D().IsValid() ? ObjectManager.Player.ServerPosition : this._from;
            }
            set
            {
                this._from = value;
            }
        }

        /// <summary>
        ///     Gets the spell data instance.
        /// </summary>
        /// <value>The spell data instance.</value>
        public SpellDataInst Instance
        {
            get
            {
                return ObjectManager.Player.Spellbook.GetSpell(this.Slot);
            }
        }

        /// <summary>
        ///     Is spell type channel
        /// </summary>
        public bool IsChannelTypeSpell { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is charged spell.
        /// </summary>
        /// <value><c>true</c> if this instance is charged spell; otherwise, <c>false</c>.</value>
        public bool IsChargedSpell { get; set; }

        /// <summary>
        ///     Gets a value indicating whether this instance is charging a charged spell.
        /// </summary>
        /// <value><c>true</c> if this instance is charging  a charged spell; otherwise, <c>false</c>.</value>
        public bool IsCharging
        {
            get
            {
                if (!this.Slot.IsReady()) return false;

                return ObjectManager.Player.HasBuff(this.ChargedBuffName)
                       || Utils.TickCount - this._chargedCastedT < 300 + Game.Ping;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is skillshot.
        /// </summary>
        /// <value><c>true</c> if this instance is skillshot; otherwise, <c>false</c>.</value>
        public bool IsSkillshot { get; set; }

        /// <summary>
        ///     Gets or sets the last cast attempt tick.
        /// </summary>
        /// <value>The last cast attempt tick.</value>
        public int LastCastAttemptT { get; set; }

        /// <summary>
        ///     Should the spell  be interuptable by casting other spells
        /// </summary>
        public bool LetSpellcancel { get; set; }

        /// <summary>
        ///     Gets the level of the spell.
        /// </summary>
        /// <value>The level of the spell.</value>
        public int Level
        {
            get
            {
                return ObjectManager.Player.Spellbook.GetSpell(this.Slot).Level;
            }
        }

        /// <summary>
        ///     Returns the amount of mana the spell costs to cast
        /// </summary>
        /// <value>The mana cost.</value>
        public float ManaCost
        {
            get
            {
                return ObjectManager.Player.Spellbook.GetSpell(this.Slot).ManaCost;
            }
        }

        /// <summary>
        ///     Gets or sets the minimum hit chance.
        /// </summary>
        /// <value>The minimum hit chance.</value>
        public HitChance MinHitChance { get; set; }

        /// <summary>
        ///     Gets or sets the range.
        /// </summary>
        /// <value>The range.</value>
        public float Range
        {
            get
            {
                var baseRange = this._range;

                if (this.AddSelfHitboxToRange)
                {
                    baseRange += ObjectManager.Player.BoundingRadius;
                }

                if (!this.IsChargedSpell)
                {
                    return baseRange;
                }

                if (this.IsCharging)
                {
                    return this.ChargedMinRange
                           + Math.Min(
                               this.ChargedMaxRange - this.ChargedMinRange,
                               (Utils.TickCount - this._chargedCastedT) * (this.ChargedMaxRange - this.ChargedMinRange)
                               / this.ChargeDuration - 150);
                }

                return this.ChargedMaxRange;
            }

            set
            {
                this._range = value;
            }
        }

        /// <summary>
        ///     Gets or sets the position to check the range from.
        /// </summary>
        /// <value>Yhe position to check the range from.</value>
        public Vector3 RangeCheckFrom
        {
            get
            {
                return !this._rangeCheckFrom.To2D().IsValid()
                           ? ObjectManager.Player.ServerPosition
                           : this._rangeCheckFrom;
            }
            set
            {
                this._rangeCheckFrom = value;
            }
        }

        /// <summary>
        ///     Gets the range squared.
        /// </summary>
        /// <value>The range squared.</value>
        public float RangeSqr
        {
            get
            {
                return this.Range * this.Range;
            }
        }

        /// <summary>
        ///     Gets or sets the spell slot.
        /// </summary>
        /// <value>The slot.</value>
        public SpellSlot Slot { get; set; }

        /// <summary>
        ///     Gets or sets the speed.
        /// </summary>
        /// <value>The speed.</value>
        public float Speed { get; set; }

        /// <summary>
        ///     Is spell targettable
        /// </summary>
        public bool TargetSpellCancel { get; set; }

        /// <summary>
        ///     Gets or sets the type of skillshot.
        /// </summary>
        /// <value>The type of skillshot.</value>
        public SkillshotType Type { get; set; }

        /// <summary>
        ///     Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public float Width
        {
            get
            {
                return this._width;
            }
            set
            {
                this._width = value;
                this.WidthSqr = value * value;
            }
        }

        /// <summary>
        ///     Gets the width squared.
        /// </summary>
        /// <value>The width squared.</value>
        public float WidthSqr { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Returns if a spell can be cast and the target is in range.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns><c>true</c> if this instance can cast on the specified unit; otherwise, <c>false</c>.</returns>
        public bool CanCast(Obj_AI_Base unit)
        {
            return this.Slot.IsReady() && unit.IsValidTarget(this.Range);
        }

        /// <summary>
        ///     Casts the spell to the unit using the prediction if its an skillshot.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="packetCast">if set to <c>true</c>, uses packets to cast the spell.</param>
        /// <param name="aoe">if set to <c>true</c>, the prediction will try to hit as many enemies as possible.</param>
        /// <returns>CastStates.</returns>
        public CastStates Cast(Obj_AI_Base unit, bool packetCast = false, bool aoe = false)
        {
            return this._cast(unit, packetCast, aoe);
        }

        /// <summary>
        ///     Casts the spell on the player.
        /// </summary>
        /// <param name="packetCast">if set to <c>true</c> uses packets to cast the spell.</param>
        /// <returns><c>true</c> if the spell was casted sucessfully, <c>false</c> otherwise.</returns>
        public bool Cast(bool packetCast = false)
        {
            return this.CastOnUnit(ObjectManager.Player, packetCast);
        }

        /// <summary>
        ///     Casts the spell from a position to a position.
        /// </summary>
        /// <param name="fromPosition">From position.</param>
        /// <param name="toPosition">To position.</param>
        /// <returns><c>true</c> if the spell was sucessfully casted, <c>false</c> otherwise.</returns>
        public bool Cast(Vector2 fromPosition, Vector2 toPosition)
        {
            return this.Cast(fromPosition.To3D(), toPosition.To3D());
        }

        /// <summary>
        ///     Casts the spell from a position to a position.
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
            return this.Slot.IsReady() && ObjectManager.Player.Spellbook.CastSpell(this.Slot, fromPosition, toPosition);
        }

        /// <summary>
        ///     Casts the spell to the position.
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
            return this.Cast(position.To3D(), packetCast);
        }

        /// <summary>
        ///     Casts the spell to the position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="packetCast">if set to <c>true</c> uses packets to cast the spell.</param>
        /// <returns><c>true</c> if the spell was casted sucessfully, <c>false</c> otherwise.</returns>
        public bool Cast(Vector3 position, bool packetCast = false)
        {
            if (!this.Slot.IsReady())
            {
                return false;
            }

            this.LastCastAttemptT = Utils.TickCount;

            if (this.IsChargedSpell)
            {
                if (this.IsCharging)
                {
                    ShootChargedSpell(this.Slot, position);
                }
                else
                {
                    this.StartCharging();
                }
            }

            else if (this.IsChannelTypeSpell)
            {
                if (this.TargetSpellCancel)
                {
                    this.CastCancelSpell(position);
                }
                else
                {
                    this.CastCancelSpell();
                }
            }

            else if (packetCast)
            {
                return ObjectManager.Player.Spellbook.CastSpell(this.Slot, position, false);
            }
            else
            {
                return ObjectManager.Player.Spellbook.CastSpell(this.Slot, position);
            }
            return false;
        }

        public void CastCancelSpell()
        {
            if (!this.IsChanneling && Utils.TickCount - this._cancelSpellIssue > 400 + Game.Ping)
            {
                ObjectManager.Player.Spellbook.CastSpell(this.Slot);
                this._cancelSpellIssue = Utils.TickCount;
            }
        }

        public void CastCancelSpell(Vector3 position)
        {
            if (!this.IsChanneling && Utils.TickCount - this._cancelSpellIssue > 400 + Game.Ping)
            {
                ObjectManager.Player.Spellbook.CastSpell(this.Slot, position);
                this._cancelSpellIssue = Utils.TickCount;
            }
        }

        /// <summary>
        ///     Casts the spell if the hitchance equals the set hitchance.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="hitChance">The hit chance.</param>
        /// <param name="packetCast">if set to <c>true</c> [packet cast].</param>
        /// <returns><c>true</c> if the spell was successfully casted, <c>false</c> otherwise.</returns>
        public bool CastIfHitchanceEquals(Obj_AI_Base unit, HitChance hitChance, bool packetCast = false)
        {
            var currentHitchance = this.MinHitChance;
            this.MinHitChance = hitChance;
            var castResult = this._cast(unit, packetCast, false, false);
            this.MinHitChance = currentHitchance;
            return castResult == CastStates.SuccessfullyCasted;
        }

        /// <summary>
        ///     Casts the spell if it will hit the set targets.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="minTargets">The minimum targets.</param>
        /// <param name="packetCast">if set to <c>true</c> [packet cast].</param>
        /// <returns><c>true</c> if the spell was successfully casted, <c>false</c> otherwise.</returns>
        public bool CastIfWillHit(Obj_AI_Base unit, int minTargets = 5, bool packetCast = false)
        {
            var castResult = this._cast(unit, packetCast, true, false, minTargets);
            return castResult == CastStates.SuccessfullyCasted;
        }

        /// <summary>
        ///     Spell will be casted on the best target found with the Spell.GetTarget method.
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

            var target = this.GetTarget(extraRange);
            return target != null ? this.Cast(target, packetCast, aoe) : CastStates.NotCasted;
        }

        /// <summary>
        ///     Casts the targetted spell on the unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="packetCast">if set to <c>true</c> [packet cast].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CastOnUnit(Obj_AI_Base unit, bool packetCast = false)
        {
            if (!this.Slot.IsReady() || this.From.Distance(unit.ServerPosition, true) > this.GetRangeSqr(unit))
            {
                return false;
            }

            if (MenuGUI.IsShopOpen || MenuGUI.IsChatOpen)
            {
                return false;
            }

            this.LastCastAttemptT = Utils.TickCount;

            if (packetCast)
            {
                return ObjectManager.Player.Spellbook.CastSpell(this.Slot, unit, false);
            }
            else
            {
                return ObjectManager.Player.Spellbook.CastSpell(this.Slot, unit);
            }
        }

        /// <summary>
        ///     Counts the hits.
        /// </summary>
        /// <param name="units">The units.</param>
        /// <param name="castPosition">The cast position.</param>
        /// <returns>System.Int32.</returns>
        public int CountHits(List<Obj_AI_Base> units, Vector3 castPosition)
        {
            var points = units.Select(unit => this.GetPrediction(unit).UnitPosition).ToList();
            return this.CountHits(points, castPosition);
        }

        /// <summary>
        ///     Counts the hits.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="castPosition">The cast position.</param>
        /// <returns>System.Int32.</returns>
        public int CountHits(List<Vector3> points, Vector3 castPosition)
        {
            return points.Count(point => this.WillHit(point, castPosition));
        }

        /// <summary>
        ///     Gets the circular farm location.
        /// </summary>
        /// <param name="minionPositions">The minion positions.</param>
        /// <param name="overrideWidth">Width of the override.</param>
        /// <returns>MinionManager.FarmLocation.</returns>
        public MinionManager.FarmLocation GetCircularFarmLocation(
            List<Obj_AI_Base> minionPositions,
            float overrideWidth = -1)
        {
            var positions = MinionManager.GetMinionsPredictedPositions(
                minionPositions,
                this.Delay,
                this.Width,
                this.Speed,
                this.From,
                this.Range,
                false,
                SkillshotType.SkillshotCircle);

            return this.GetCircularFarmLocation(positions, overrideWidth);
        }

        /// <summary>
        ///     Gets the circular farm location.
        /// </summary>
        /// <param name="minionPositions">The minion positions.</param>
        /// <param name="overrideWidth">Width of the override.</param>
        /// <returns>MinionManager.FarmLocation.</returns>
        public MinionManager.FarmLocation GetCircularFarmLocation(
            List<Vector2> minionPositions,
            float overrideWidth = -1)
        {
            return MinionManager.GetBestCircularFarmLocation(
                minionPositions,
                overrideWidth >= 0 ? overrideWidth : this.Width,
                this.Range);
        }

        /// <summary>
        ///     Gets the collision.
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
                        From = from.To3D(), Type = this.Type, Radius = this.Width,
                        Delay = delayOverride > 0 ? delayOverride : this.Delay, Speed = this.Speed
                    });
        }

        /// <summary>
        ///     Gets the damage that the skillshot will deal to the target using the damage library.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="stage">The stage.</param>
        /// <returns>System.Single.</returns>
        public float GetDamage(Obj_AI_Base target, int stage = 0)
        {
            return (float)ObjectManager.Player.GetSpellDamage(target, this.Slot, stage);
        }

        /// <summary>
        ///     Returns the unit health when the spell hits the unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns>System.Single.</returns>
        public float GetHealthPrediction(Obj_AI_Base unit)
        {
            var time = (int)(this.Delay * 1000 + this.From.Distance(unit.ServerPosition) / this.Speed - 100);
            return HealthPrediction.GetHealthPrediction(unit, time);
        }

        /// <summary>
        ///     Gets the hit count.
        /// </summary>
        /// <param name="hitChance">The hit chance.</param>
        /// <returns>System.Single.</returns>
        public float GetHitCount(HitChance hitChance = HitChance.High)
        {
            return HeroManager.Enemies.Select(e => this.GetPrediction(e)).Count(p => p.Hitchance >= hitChance);
        }

        /// <summary>
        ///     Gets the line farm location.
        /// </summary>
        /// <param name="minionPositions">The minion positions.</param>
        /// <param name="overrideWidth">Width of the override.</param>
        /// <returns>MinionManager.FarmLocation.</returns>
        public MinionManager.FarmLocation GetLineFarmLocation(
            List<Obj_AI_Base> minionPositions,
            float overrideWidth = -1)
        {
            var positions = MinionManager.GetMinionsPredictedPositions(
                minionPositions,
                this.Delay,
                this.Width,
                this.Speed,
                this.From,
                this.Range,
                false,
                SkillshotType.SkillshotLine);

            return this.GetLineFarmLocation(positions, overrideWidth >= 0 ? overrideWidth : this.Width);
        }

        /// <summary>
        ///     Gets the line farm location.
        /// </summary>
        /// <param name="minionPositions">The minion positions.</param>
        /// <param name="overrideWidth">Width of the override.</param>
        /// <returns>MinionManager.FarmLocation.</returns>
        public MinionManager.FarmLocation GetLineFarmLocation(List<Vector2> minionPositions, float overrideWidth = -1)
        {
            return MinionManager.GetBestLineFarmLocation(
                minionPositions,
                overrideWidth >= 0 ? overrideWidth : this.Width,
                this.Range);
        }

        /// <summary>
        ///     Gets the prediction.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="aoe">if set to <c>true</c>, the prediction will try to hit as many enemies.</param>
        /// <param name="overrideRange">The override range.</param>
        /// <param name="collisionable">The collisionable.</param>
        /// <returns>PredictionOutput.</returns>
        public PredictionOutput GetPrediction(
            Obj_AI_Base unit,
            bool aoe = false,
            float overrideRange = -1,
            CollisionableObjects[] collisionable = null)
        {
            return
                Prediction.GetPrediction(
                    new PredictionInput
                        {
                            Unit = unit, Delay = this.Delay, Radius = this.Width, Speed = this.Speed, From = this.From,
                            Range = (overrideRange > 0) ? overrideRange : this.Range, Collision = this.Collision,
                            Type = this.Type, RangeCheckFrom = this.RangeCheckFrom, Aoe = aoe,
                            CollisionObjects =
                                collisionable ?? new[] { CollisionableObjects.Heroes, CollisionableObjects.Minions }
                        });
        }

        /// <summary>
        ///     Gets the range the spell has when casted to target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public float GetRange(Obj_AI_Base target)
        {
            var result = this.Range;

            if (this.AddEnemyHitboxToRange && target != null)
            {
                result += target.BoundingRadius;
            }

            return result;
        }

        /// <summary>
        ///     Gets the range sqared the spell has when casted to target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public float GetRangeSqr(Obj_AI_Base target)
        {
            var result = this.GetRange(target);
            return result * result;
        }

        /// <summary>
        ///     Returns the best target found using the current TargetSelector mode.
        ///     Please make sure to set the Spell.DamageType Property to the type of damage this spell does (if not done on
        ///     initialization).
        /// </summary>
        /// <param name="extraRange">The extra range.</param>
        /// <param name="champsToIgnore">The champs to ignore.</param>
        /// <returns>Obj_AI_Hero.</returns>
        public Obj_AI_Hero GetTarget(float extraRange = 0, IEnumerable<Obj_AI_Hero> champsToIgnore = null)
        {
            return TargetSelector.GetTarget(this.Range + extraRange, this.DamageType, true, champsToIgnore, this.From);
        }

        /// <summary>
        ///     Returns if the GameObject is in range of the spell.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="range">The range.</param>
        /// <returns><c>true</c> if the spell is in range of the unit; otherwise, <c>false</c>.</returns>
        public bool IsInRange(GameObject obj, float range = -1)
        {
            return this.IsInRange(
                obj is Obj_AI_Base ? (obj as Obj_AI_Base).ServerPosition.To2D() : obj.Position.To2D(),
                range);
        }

        /// <summary>
        ///     Returns if the position is in range of the spell.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="range">The range.</param>
        /// <returns><c>true</c> if the specified location is in range of the spell; otherwise, <c>false</c>.</returns>
        public bool IsInRange(Vector3 point, float range = -1)
        {
            return this.IsInRange(point.To2D(), range);
        }

        /// <summary>
        ///     Returns if the Vector2 is in range of the spell.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="range">The range.</param>
        /// <returns><c>true</c> if the specified location is in range of the spell; otherwise, <c>false</c>.</returns>
        public bool IsInRange(Vector2 point, float range = -1)
        {
            return this.RangeCheckFrom.To2D().Distance(point, true) < (range < 0 ? this.RangeSqr : range * range);
        }

        /// <summary>
        ///     Gets the damage that the skillshot will deal to the target using the damage lib and returns if the target is
        ///     killable or not.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="stage">The stage.</param>
        /// <returns><c>true</c> if the specified target is killable; otherwise, <c>false</c>.</returns>
        public bool IsKillable(Obj_AI_Base target, int stage = 0)
        {
            return ObjectManager.Player.GetSpellDamage(target, this.Slot, stage) > target.Health;
        }

        /// <summary>
        ///     Sets the spell to be a charged spell.
        /// </summary>
        /// <param name="spellName">Name of the spell.</param>
        /// <param name="buffName">Name of the buff.</param>
        /// <param name="minRange">The minimum range.</param>
        /// <param name="maxRange">The maximum range.</param>
        /// <param name="deltaT">The delta time.</param>
        public void SetCharged(string spellName, string buffName, int minRange, int maxRange, float deltaT)
        {
            this.IsChargedSpell = true;
            this.ChargedSpellName = spellName;
            this.ChargedBuffName = buffName;
            this.ChargedMinRange = minRange;
            this.ChargedMaxRange = maxRange;
            this.ChargeDuration = (int)(deltaT * 1000);
            this._chargedCastedT = 0;

            Obj_AI_Base.OnProcessSpellCast += this.Obj_AI_Hero_OnProcessSpellCast;
            Spellbook.OnUpdateChargedSpell += this.Spellbook_OnUpdateChargedSpell;
            Spellbook.OnCastSpell += this.SpellbookOnCastSpell;
        }

        /// <summary>
        ///     Spell setings
        /// </summary>
        /// <param name="letUserCancel"></param>
        /// <param name="targetted"></param>
        /// <param name="letSpellCancel"></param>
        public void Setinterruptible(bool letUserCancel, bool targetted, bool letSpellCancel = false)
        {
            this.CanBeCanceledByUser = letUserCancel;
            this.TargetSpellCancel = targetted;
            this.IsChanneling = false;
            this.LetSpellcancel = letSpellCancel;

            Obj_AI_Base.OnDoCast += this.OnDoCast;
            GameObject.OnDelete += this.OnDelete;
            Game.OnWndProc += this.OnWndProc;
            Obj_AI_Base.OnIssueOrder += this.OnOrder;
            Spellbook.OnCastSpell += this.OnCastSpell;

            if (ObjectManager.Player.ChampionName == "Fiddlesticks")
            {
                GameObject.OnCreate += this.OnCreate;
            }
        }

        /// <summary>
        ///     Sets the spell to be a skillshot.
        /// </summary>
        /// <param name="delay">The delay.</param>
        /// <param name="width">The width.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="collision">if set to <c>true</c>, the spell has collision.</param>
        /// <param name="type">The type.</param>
        /// <param name="from">From.</param>
        /// <param name="rangeCheckFrom">The range check from.</param>
        public void SetSkillshot(
            float delay,
            float width,
            float speed,
            bool collision,
            SkillshotType type,
            Vector3 from = new Vector3(),
            Vector3 rangeCheckFrom = new Vector3())
        {
            this.Delay = delay;
            this.Width = width;
            this.Speed = speed;
            this.From = from;
            this.Collision = collision;
            this.Type = type;
            this.RangeCheckFrom = rangeCheckFrom;
            this.IsSkillshot = true;
        }

        /// <summary>
        ///     Sets spell the be a targetted spell.
        /// </summary>
        /// <param name="delay">The delay.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="from">The from position.</param>
        /// <param name="rangeCheckFrom">The position to check the range from.</param>
        public void SetTargetted(
            float delay,
            float speed,
            Vector3 from = new Vector3(),
            Vector3 rangeCheckFrom = new Vector3())
        {
            this.Delay = delay;
            this.Speed = speed;
            this.From = from;
            this.RangeCheckFrom = rangeCheckFrom;
            this.IsSkillshot = false;
        }

        /// <summary>
        ///     Start charging the spell if its not charging.
        /// </summary>
        public void StartCharging()
        {
            if (!this.IsCharging && Utils.TickCount - this._chargedReqSentT > 400 + Game.Ping)
            {
                ObjectManager.Player.Spellbook.CastSpell(this.Slot);
                this._chargedReqSentT = Utils.TickCount;
            }
        }

        /// <summary>
        ///     Start charging the spell if its not charging.
        /// </summary>
        /// <param name="position">The position.</param>
        public void StartCharging(Vector3 position)
        {
            if (!this.IsCharging && Utils.TickCount - this._chargedReqSentT > 400 + Game.Ping)
            {
                ObjectManager.Player.Spellbook.CastSpell(this.Slot, position);
                this._chargedReqSentT = Utils.TickCount;
            }
        }

        /// <summary>
        ///     Updates the source position.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="rangeCheckFrom">The range check from.</param>
        public void UpdateSourcePosition(Vector3 from = new Vector3(), Vector3 rangeCheckFrom = new Vector3())
        {
            this.From = from;
            this.RangeCheckFrom = rangeCheckFrom;
        }

        /// <summary>
        ///     Returns if the spell will hit the unit when casted on castPosition.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="castPosition">The cast position.</param>
        /// <param name="extraWidth">Added width to the spell.</param>
        /// <param name="minHitChance">The minimum hit chance.</param>
        /// <returns><c>true</c> if the spell will hit the unit, <c>false</c> otherwise.</returns>
        public bool WillHit(
            Obj_AI_Base unit,
            Vector3 castPosition,
            int extraWidth = 0,
            HitChance minHitChance = HitChance.High)
        {
            var unitPosition = this.GetPrediction(unit);
            return unitPosition.Hitchance >= minHitChance
                   && this.WillHit(unitPosition.UnitPosition, castPosition, extraWidth);
        }

        /// <summary>
        ///     Returns if the spell will hit the point when casted on castPosition.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="castPosition">The cast position.</param>
        /// <param name="extraWidth">Width of the extra.</param>
        /// <returns><c>true</c> if the spell will hit the location, <c>false</c> otherwise.</returns>
        public bool WillHit(Vector3 point, Vector3 castPosition, int extraWidth = 0)
        {
            switch (this.Type)
            {
                case SkillshotType.SkillshotCircle:
                    if (point.To2D().Distance(castPosition, true) < this.WidthSqr)
                    {
                        return true;
                    }
                    break;

                case SkillshotType.SkillshotLine:
                    if (point.To2D().Distance(castPosition.To2D(), this.From.To2D(), true, true)
                        < Math.Pow(this.Width + extraWidth, 2))
                    {
                        return true;
                    }
                    break;
                case SkillshotType.SkillshotCone:
                    var edge1 = (castPosition.To2D() - this.From.To2D()).Rotated(-this.Width / 2);
                    var edge2 = edge1.Rotated(this.Width);
                    var v = point.To2D() - this.From.To2D();
                    if (point.To2D().Distance(this.From, true) < this.RangeSqr && edge1.CrossProduct(v) > 0
                        && v.CrossProduct(edge2) > 0)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Shoots the charged spell.
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
        ///     Casts the spell at the specified unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="packetCast">if set to <c>true</c> [packet cast].</param>
        /// <param name="aoe">if set to <c>true</c> [aoe].</param>
        /// <param name="exactHitChance">if set to <c>true</c> [exact hit chance].</param>
        /// <param name="minTargets">The minimum targets.</param>
        /// <returns>CastStates.</returns>
        private CastStates _cast(
            Obj_AI_Base unit,
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
            if (!this.Slot.IsReady())
            {
                return CastStates.NotReady;
            }

            if (minTargets != -1)
            {
                aoe = true;
            }

            //Targetted spell.
            if (!this.IsSkillshot)
            {
                //Target out of range
                if (this.RangeCheckFrom.Distance(unit.ServerPosition, true) > this.GetRangeSqr(unit))
                {
                    return CastStates.OutOfRange;
                }

                this.LastCastAttemptT = Utils.TickCount;

                if (packetCast)
                {
                    if (!ObjectManager.Player.Spellbook.CastSpell(this.Slot, unit, false))
                    {
                        return CastStates.NotCasted;
                    }
                }
                else
                {
                    //Cant cast the Spell.
                    if (!ObjectManager.Player.Spellbook.CastSpell(this.Slot, unit))
                    {
                        return CastStates.NotCasted;
                    }
                }

                return CastStates.SuccessfullyCasted;
            }

            //Get the best position to cast the spell.
            var prediction = this.GetPrediction(unit, aoe);

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
            if (this.RangeCheckFrom.Distance(prediction.CastPosition, true) > this.RangeSqr)
            {
                return CastStates.OutOfRange;
            }

            //The hitchance is too low.
            if (prediction.Hitchance < this.MinHitChance
                || (exactHitChance && prediction.Hitchance != this.MinHitChance))
            {
                return CastStates.LowHitChance;
            }

            this.LastCastAttemptT = Utils.TickCount;

            if (this.IsChargedSpell)
            {
                if (this.IsCharging)
                {
                    ShootChargedSpell(this.Slot, prediction.CastPosition);
                }
                else
                {
                    this.StartCharging();
                }
            }
            else if (packetCast)
            {
                ObjectManager.Player.Spellbook.CastSpell(this.Slot, prediction.CastPosition, false);
            }
            else
            {
                //Cant cast the spell (actually should not happen).
                if (!ObjectManager.Player.Spellbook.CastSpell(this.Slot, prediction.CastPosition))
                {
                    return CastStates.NotCasted;
                }
            }

            return CastStates.SuccessfullyCasted;
        }

        /// <summary>
        ///     Fired when the game processes a spell cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs" /> instance containing the event data.</param>
        private void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == this.ChargedSpellName)
            {
                this._chargedCastedT = Utils.TickCount;
            }
        }

        private void OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (this.LetSpellcancel) return;

            args.Process = !this.IsChanneling;
        }

        private void OnCreate(GameObject sender, EventArgs args)
        {
            if (sender.Name == "Fiddlesticks_Base_Base_Crowstorm_green_cas.troy")
            {
                this.IsChanneling = false;
            }
        }

        /// <summary>
        ///     Check when an object has been deleted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDelete(GameObject sender, EventArgs args)
        {
            if (this._deleteObject.Contains(sender.Name))
            {
                this.IsChanneling = false;
            }
        }

        /// <summary>
        ///     Check when the skill object has been casted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;

            if (this._processName.Contains(args.SData.Name))
            {
                this.IsChanneling = true;
            }
        }

        /// <summary>
        ///     Check when a spell has been casted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnOrder(Obj_AI_Base sender, GameObjectIssueOrderEventArgs args)
        {
            if (!sender.IsMe) return;

            if (!this.IsChanneling) return;

            if (args.Order == GameObjectOrder.MoveTo || args.Order == GameObjectOrder.AttackTo
                || args.Order == GameObjectOrder.AttackUnit || args.Order == GameObjectOrder.AutoAttack)
            {
                args.Process = false;
            }
        }

        /// <summary>
        ///     When player sends a key command
        /// </summary>
        /// <param name="args"></param>
        private void OnWndProc(WndEventArgs args)
        {
            if (!this.CanBeCanceledByUser) return;

            if (args.Msg == 517)
            {
                this.IsChanneling = false;
            }
        }

        /// <summary>
        ///     Fired when the charged spell is updated.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="SpellbookUpdateChargedSpellEventArgs" /> instance containing the event data.</param>
        void Spellbook_OnUpdateChargedSpell(Spellbook sender, SpellbookUpdateChargedSpellEventArgs args)
        {
            if (sender.Owner.IsMe && Utils.TickCount - this._chargedReqSentT < 3000 && args.ReleaseCast)
            {
                args.Process = false;
            }
        }

        /// <summary>
        ///     Fired when the spellbook casts a spell.
        /// </summary>
        /// <param name="spellbook">The spellbook.</param>
        /// <param name="args">The <see cref="SpellbookCastSpellEventArgs" /> instance containing the event data.</param>
        private void SpellbookOnCastSpell(Spellbook spellbook, SpellbookCastSpellEventArgs args)
        {
            if (args.Slot != this.Slot)
            {
                return;
            }

            if ((Utils.TickCount - this._chargedReqSentT > 500))
            {
                if (this.IsCharging)
                {
                    this.Cast(args.StartPosition.To2D());
                }
            }
        }

        #endregion
    }
}