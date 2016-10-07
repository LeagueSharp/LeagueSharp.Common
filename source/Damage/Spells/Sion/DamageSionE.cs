// <copyright file="DamageSionE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Sion E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Sion", SpellSlot.E)]
    public class DamageSionE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageSionE" /> class.
        /// </summary>
        public DamageSionE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 105, 140, 175, 210 }[level] + (0.4 * source.TotalMagicalDamage);
        }

        #endregion
    }
}