// <copyright file="DamageCassiopeiaQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Cassiopeia Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Cassiopeia", SpellSlot.Q)]
    public class DamageCassiopeiaQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageCassiopeiaQ" /> class.
        /// </summary>
        public DamageCassiopeiaQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 75, 120, 165, 210, 255 }[level] + (0.7 * source.TotalMagicalDamage);
        }

        #endregion
    }
}