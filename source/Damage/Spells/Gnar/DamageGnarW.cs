// <copyright file="DamageGnarW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Gnar W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Gnar", SpellSlot.W)]
    public class DamageGnarW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageGnarW" /> class.
        /// </summary>
        public DamageGnarW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 15, 25, 35, 45, 55 }[level] + (1 * source.TotalMagicalDamage) + (new double[] { 6, 8, 10, 12, 14 }[level] / 100 * target.MaxHealth);
        }

        #endregion
    }
}