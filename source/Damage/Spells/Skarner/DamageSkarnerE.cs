// <copyright file="DamageSkarnerE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Skarner E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Skarner", SpellSlot.E)]
    public class DamageSkarnerE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageSkarnerE" /> class.
        /// </summary>
        public DamageSkarnerE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 40, 75, 110, 145, 180 }[level] + (0.4 * source.TotalMagicalDamage);
        }

        #endregion
    }
}