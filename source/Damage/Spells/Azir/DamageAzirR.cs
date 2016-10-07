// <copyright file="DamageAzirR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Azir R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Azir", SpellSlot.R)]
    public class DamageAzirR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageAzirR" /> class.
        /// </summary>
        public DamageAzirR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 150, 225, 300 }[level] + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}