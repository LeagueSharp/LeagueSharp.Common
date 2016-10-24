// <copyright file="DamageGravesW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Graves W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Graves", SpellSlot.W)]
    public class DamageGravesW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageGravesW" /> class.
        /// </summary>
        public DamageGravesW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 110, 160, 210, 260 }[level] + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}