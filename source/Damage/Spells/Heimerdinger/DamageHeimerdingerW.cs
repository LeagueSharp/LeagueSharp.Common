// <copyright file="DamageHeimerdingerW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Heimerdinger W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Heimerdinger", SpellSlot.W)]
    public class DamageHeimerdingerW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageHeimerdingerW" /> class.
        /// </summary>
        public DamageHeimerdingerW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 90, 120, 150, 180 }[level] + (0.45 * source.TotalMagicalDamage);
        }

        #endregion
    }
}