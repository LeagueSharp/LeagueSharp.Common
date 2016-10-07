// <copyright file="DamageShyvanaE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Shyvana E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Shyvana", SpellSlot.E)]
    public class DamageShyvanaE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageShyvanaE" /> class.
        /// </summary>
        public DamageShyvanaE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 100, 140, 180, 220 }[level] + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}