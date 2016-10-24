// <copyright file="DamageLeBlancW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, LeBlanc W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("LeBlanc", SpellSlot.W)]
    public class DamageLeBlancW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageLeBlancW" /> class.
        /// </summary>
        public DamageLeBlancW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 85, 125, 165, 205, 245 }[level] + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}