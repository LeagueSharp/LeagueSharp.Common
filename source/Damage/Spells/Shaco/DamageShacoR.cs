// <copyright file="DamageShacoR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Shaco R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Shaco", SpellSlot.R)]
    public class DamageShacoR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageShacoR" /> class.
        /// </summary>
        public DamageShacoR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 300, 450, 600 }[level] + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}