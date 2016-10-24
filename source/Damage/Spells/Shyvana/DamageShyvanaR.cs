// <copyright file="DamageShyvanaR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Shyvana R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Shyvana", SpellSlot.R)]
    public class DamageShyvanaR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageShyvanaR" /> class.
        /// </summary>
        public DamageShyvanaR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 175, 300, 425 }[level] + (0.7 * source.TotalMagicalDamage);
        }

        #endregion
    }
}