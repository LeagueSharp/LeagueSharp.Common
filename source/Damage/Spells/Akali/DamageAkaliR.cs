// <copyright file="DamageAkaliR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Akali R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Akali", SpellSlot.R)]
    public class DamageAkaliR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageAkaliR" /> class.
        /// </summary>
        public DamageAkaliR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 100, 175, 250 }[level] + (0.5 * source.TotalMagicalDamage);
        }

        #endregion
    }
}