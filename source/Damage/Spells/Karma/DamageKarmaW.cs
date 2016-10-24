// <copyright file="DamageKarmaW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Karma W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Karma", SpellSlot.W)]
    public class DamageKarmaW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKarmaW" /> class.
        /// </summary>
        public DamageKarmaW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 110, 160, 210, 260 }[level] + (0.9 * source.TotalMagicalDamage);
        }

        #endregion
    }
}