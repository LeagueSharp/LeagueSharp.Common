// <copyright file="DamageGangPlankR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, GangPlank R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("GangPlank", SpellSlot.R)]
    public class DamageGangPlankR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageGangPlankR" /> class.
        /// </summary>
        public DamageGangPlankR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 70, 90 }[level] + (0.1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}