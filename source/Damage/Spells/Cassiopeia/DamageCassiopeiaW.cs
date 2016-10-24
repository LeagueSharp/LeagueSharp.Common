// <copyright file="DamageCassiopeiaW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Cassiopeia W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Cassiopeia", SpellSlot.W)]
    public class DamageCassiopeiaW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageCassiopeiaW" /> class.
        /// </summary>
        public DamageCassiopeiaW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 20, 35, 50, 65, 80 }[level] + (0.15 * source.TotalMagicalDamage);
        }

        #endregion
    }
}