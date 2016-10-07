// <copyright file="DamageVelkozR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Velkoz R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Velkoz", SpellSlot.R)]
    public class DamageVelkozR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageVelkozR" /> class.
        /// </summary>
        public DamageVelkozR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.True;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return target.HasBuff("velkozresearchedstack")
                       ? new double[] { 500, 725, 950 }[level] + (1 * source.TotalMagicalDamage)
                       : source.CalcDamage(
                           target,
                           Common.Damage.DamageType.Magical,
                           new double[] { 500, 725, 950 }[level] + (1 * source.TotalMagicalDamage));
        }

        #endregion
    }
}