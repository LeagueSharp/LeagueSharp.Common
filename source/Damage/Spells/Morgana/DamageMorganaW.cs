// <copyright file="DamageMorganaW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Morgana W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Morgana", SpellSlot.W)]
    public class DamageMorganaW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageMorganaW" /> class.
        /// </summary>
        public DamageMorganaW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 8, 16, 24, 32, 40 }[level] + (0.11 * source.TotalMagicalDamage);
        }

        #endregion
    }
}