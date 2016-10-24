// <copyright file="DamageLeonaW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Leona W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Leona", SpellSlot.W)]
    public class DamageLeonaW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageLeonaW" /> class.
        /// </summary>
        public DamageLeonaW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 100, 140, 180, 220 }[level] + (0.4 * source.TotalMagicalDamage);
        }

        #endregion
    }
}