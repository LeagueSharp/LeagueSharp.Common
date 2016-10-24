// <copyright file="DamageVelkozW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Velkoz W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Velkoz", SpellSlot.W)]
    public class DamageVelkozW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageVelkozW" /> class.
        /// </summary>
        public DamageVelkozW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 30, 50, 70, 90, 110 }[level] + new double[] { 45, 75, 105, 135, 165 }[level] + (0.4 * source.TotalMagicalDamage);
        }

        #endregion
    }
}