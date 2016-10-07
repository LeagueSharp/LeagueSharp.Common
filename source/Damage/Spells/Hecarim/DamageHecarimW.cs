// <copyright file="DamageHecarimW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Hecarim W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Hecarim", SpellSlot.W)]
    public class DamageHecarimW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageHecarimW" /> class.
        /// </summary>
        public DamageHecarimW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 20, 30, 40, 50, 60 }[level] + (0.2 * source.TotalMagicalDamage);
        }

        #endregion
    }
}