// <copyright file="DamageLissandraE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Lissandra E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Lissandra", SpellSlot.E)]
    public class DamageLissandraE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageLissandraE" /> class.
        /// </summary>
        public DamageLissandraE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 115, 160, 205, 250 }[level] + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}