// <copyright file="DamageNautilusW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Nautilus W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Nautilus", SpellSlot.W)]
    public class DamageNautilusW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageNautilusW" /> class.
        /// </summary>
        public DamageNautilusW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 30, 40, 50, 60, 70 }[level] + (0.4 * source.TotalMagicalDamage);
        }

        #endregion
    }
}