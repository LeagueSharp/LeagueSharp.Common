// <copyright file="DamageSyndraR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Syndra R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Syndra", SpellSlot.R)]
    public class DamageSyndraR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageSyndraR" /> class.
        /// </summary>
        public DamageSyndraR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 270, 405, 540 }[level] + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}