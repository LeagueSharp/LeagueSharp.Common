// <copyright file="DamageCassiopeiaE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Cassiopeia E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Cassiopeia", SpellSlot.E)]
    public class DamageCassiopeiaE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageCassiopeiaE" /> class.
        /// </summary>
        public DamageCassiopeiaE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return (48 + (4 * ((Obj_AI_Hero)source).Level)) + (0.1 * source.TotalMagicalDamage) + (target.HasBuffOfType(BuffType.Poison) ? new double[] { 10, 40, 70, 100, 130 }[level] + (0.35 * source.TotalMagicalDamage) : 0);
        }

        #endregion
    }
}