// <copyright file="DamageLissandraR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Lissandra R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Lissandra", SpellSlot.R)]
    public class DamageLissandraR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageLissandraR" /> class.
        /// </summary>
        public DamageLissandraR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 150, 250, 350 }[level] + (0.7 * source.TotalMagicalDamage);
        }

        #endregion
    }
}