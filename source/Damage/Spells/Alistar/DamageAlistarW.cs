// <copyright file="DamageAlistarW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Alistar W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Alistar", SpellSlot.W)]
    public class DamageAlistarW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageAlistarW" /> class.
        /// </summary>
        public DamageAlistarW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 55, 110, 165, 220, 275 }[level] + (0.7 * source.TotalMagicalDamage);
        }

        #endregion
    }
}