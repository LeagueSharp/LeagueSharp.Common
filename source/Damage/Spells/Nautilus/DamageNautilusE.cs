// <copyright file="DamageNautilusE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Nautilus E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Nautilus", SpellSlot.E)]
    public class DamageNautilusE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageNautilusE" /> class.
        /// </summary>
        public DamageNautilusE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 100, 140, 180, 220 }[level] + (0.3 * source.TotalMagicalDamage);
        }

        #endregion
    }
}