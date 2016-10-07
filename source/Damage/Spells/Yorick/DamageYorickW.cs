// <copyright file="DamageYorickW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Yorick W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Yorick", SpellSlot.W)]
    public class DamageYorickW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageYorickW" /> class.
        /// </summary>
        public DamageYorickW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 95, 130, 165, 200 }[level] + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}