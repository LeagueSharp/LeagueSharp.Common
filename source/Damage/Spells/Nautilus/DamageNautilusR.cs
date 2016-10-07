// <copyright file="DamageNautilusR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Nautilus R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Nautilus", SpellSlot.R)]
    public class DamageNautilusR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageNautilusR" /> class.
        /// </summary>
        public DamageNautilusR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 200, 325, 450 }[level] + (0.8 * source.TotalMagicalDamage);
        }

        #endregion
    }
}