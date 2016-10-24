// <copyright file="DamageTwistedFateW1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, TwistedFate W (Stage 1).
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("TwistedFate", SpellSlot.W, 1)]
    public class DamageTwistedFateW1 : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTwistedFateW1" /> class.
        /// </summary>
        public DamageTwistedFateW1()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
            this.Stage = 1;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 30, 45, 60, 75, 90 }[level] + (1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)) + (0.5 * source.TotalMagicalDamage);
        }

        #endregion
    }
}