// <copyright file="DamageTwitchE1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Twitch E (Stage 1).
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Twitch", SpellSlot.E, 1)]
    public class DamageTwitchE1 : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTwitchE1" /> class.
        /// </summary>
        public DamageTwitchE1()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
            this.Stage = 1;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 15, 20, 25, 30, 35 }[level] + (0.2 * source.TotalMagicalDamage) + (0.25 * source.FlatPhysicalDamageMod) + new double[] { 20, 35, 50, 65, 80 }[level];
        }

        #endregion
    }
}