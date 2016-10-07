// <copyright file="DamageZyraR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Zyra R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Zyra", SpellSlot.R)]
    public class DamageZyraR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageZyraR" /> class.
        /// </summary>
        public DamageZyraR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 180, 265, 350 }[level] + (0.7 * source.TotalMagicalDamage);
        }

        #endregion
    }
}