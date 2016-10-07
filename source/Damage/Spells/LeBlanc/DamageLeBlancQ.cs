// <copyright file="DamageLeBlancQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, LeBlanc Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("LeBlanc", SpellSlot.Q)]
    public class DamageLeBlancQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageLeBlancQ" /> class.
        /// </summary>
        public DamageLeBlancQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 55, 80, 105, 130, 155 }[level] + (0.4 * source.TotalMagicalDamage);
        }

        #endregion
    }
}