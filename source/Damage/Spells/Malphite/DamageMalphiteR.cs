// <copyright file="DamageMalphiteR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Malphite R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Malphite", SpellSlot.R)]
    public class DamageMalphiteR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageMalphiteR" /> class.
        /// </summary>
        public DamageMalphiteR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 200, 300, 400 }[level] + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}