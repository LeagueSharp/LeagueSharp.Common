// <copyright file="DamageVelkozE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Velkoz E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Velkoz", SpellSlot.E)]
    public class DamageVelkozE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageVelkozE" /> class.
        /// </summary>
        public DamageVelkozE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 100, 130, 160, 190 }[level] + (0.3 * source.TotalMagicalDamage);
        }

        #endregion
    }
}