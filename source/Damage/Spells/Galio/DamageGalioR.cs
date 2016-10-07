// <copyright file="DamageGalioR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Galio R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Galio", SpellSlot.R)]
    public class DamageGalioR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageGalioR" /> class.
        /// </summary>
        public DamageGalioR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 360, 540, 720 }[level] + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}