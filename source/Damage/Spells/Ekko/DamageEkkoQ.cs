// <copyright file="DamageEkkoQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Ekko Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Ekko", SpellSlot.Q)]
    public class DamageEkkoQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageEkkoQ" /> class.
        /// </summary>
        public DamageEkkoQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 75, 90, 105, 120 }[level] + (0.1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}