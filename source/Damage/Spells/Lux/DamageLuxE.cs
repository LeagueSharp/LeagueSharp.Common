// <copyright file="DamageLuxE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Lux E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Lux", SpellSlot.E)]
    public class DamageLuxE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageLuxE" /> class.
        /// </summary>
        public DamageLuxE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 105, 150, 195, 240 }[level] + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}