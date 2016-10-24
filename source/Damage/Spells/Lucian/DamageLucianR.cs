// <copyright file="DamageLucianR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Lucian R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Lucian", SpellSlot.R)]
    public class DamageLucianR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageLucianR" /> class.
        /// </summary>
        public DamageLucianR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 40, 50, 60 }[level] + (0.1 * source.TotalMagicalDamage) + (0.25 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}