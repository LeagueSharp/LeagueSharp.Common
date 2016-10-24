// <copyright file="DamageGragasR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Gragas R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Gragas", SpellSlot.R)]
    public class DamageGragasR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageGragasR" /> class.
        /// </summary>
        public DamageGragasR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 200, 300, 400 }[level] + (0.7 * source.TotalMagicalDamage);
        }

        #endregion
    }
}