// <copyright file="DamageTwistedFateW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, TwistedFate W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("TwistedFate", SpellSlot.W)]
    public class DamageTwistedFateW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTwistedFateW" /> class.
        /// </summary>
        public DamageTwistedFateW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 40, 60, 80, 100, 120 }[level] + (1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)) + (0.5 * source.TotalMagicalDamage);
        }

        #endregion
    }
}