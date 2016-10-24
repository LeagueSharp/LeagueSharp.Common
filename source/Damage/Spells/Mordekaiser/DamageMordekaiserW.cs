// <copyright file="DamageMordekaiserW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Mordekaiser W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Mordekaiser", SpellSlot.W)]
    public class DamageMordekaiserW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageMordekaiserW" /> class.
        /// </summary>
        public DamageMordekaiserW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 24, 38, 52, 66, 80 }[level] + (0.2 * source.TotalMagicalDamage);
        }

        #endregion
    }
}