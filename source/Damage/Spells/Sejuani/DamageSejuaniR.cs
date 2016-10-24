// <copyright file="DamageSejuaniR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Sejuani R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Sejuani", SpellSlot.R)]
    public class DamageSejuaniR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageSejuaniR" /> class.
        /// </summary>
        public DamageSejuaniR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 150, 250, 350 }[level] + (0.8 * source.TotalMagicalDamage);
        }

        #endregion
    }
}