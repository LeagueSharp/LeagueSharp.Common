// <copyright file="DamageShenE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Shen E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Shen", SpellSlot.E)]
    public class DamageShenE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageShenE" /> class.
        /// </summary>
        public DamageShenE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 85, 120, 155, 190 }[level] + (0.5 * source.TotalMagicalDamage);
        }

        #endregion
    }
}