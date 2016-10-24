// <copyright file="DamageSyndraE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Syndra E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Syndra", SpellSlot.E)]
    public class DamageSyndraE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageSyndraE" /> class.
        /// </summary>
        public DamageSyndraE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 115, 160, 205, 250 }[level] + (0.5 * source.TotalMagicalDamage);
        }

        #endregion
    }
}