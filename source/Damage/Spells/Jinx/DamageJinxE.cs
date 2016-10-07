// <copyright file="DamageJinxE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Jinx E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Jinx", SpellSlot.E)]
    public class DamageJinxE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageJinxE" /> class.
        /// </summary>
        public DamageJinxE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 80, 135, 190, 245, 300 }[level] + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}