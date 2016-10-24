// <copyright file="DamageTeemoE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Teemo E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Teemo", SpellSlot.E)]
    public class DamageTeemoE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTeemoE" /> class.
        /// </summary>
        public DamageTeemoE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 34, 68, 102, 136, 170 }[level] + (0.7 * source.TotalMagicalDamage);
        }

        #endregion
    }
}