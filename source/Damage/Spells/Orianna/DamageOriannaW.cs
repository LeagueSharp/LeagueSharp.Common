// <copyright file="DamageOriannaW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Orianna W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Orianna", SpellSlot.W)]
    public class DamageOriannaW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageOriannaW" /> class.
        /// </summary>
        public DamageOriannaW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 115, 160, 205, 250 }[level] + (0.7 * source.TotalMagicalDamage);
        }

        #endregion
    }
}