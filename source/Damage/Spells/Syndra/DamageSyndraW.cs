// <copyright file="DamageSyndraW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Syndra W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Syndra", SpellSlot.W)]
    public class DamageSyndraW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageSyndraW" /> class.
        /// </summary>
        public DamageSyndraW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 80, 120, 160, 200, 240 }[level] + (0.8 * source.TotalMagicalDamage);
        }

        #endregion
    }
}