// <copyright file="DamageTwistedFateW2.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, TwistedFate W (Stage 2).
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("TwistedFate", SpellSlot.W, 2)]
    public class DamageTwistedFateW2 : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTwistedFateW2" /> class.
        /// </summary>
        public DamageTwistedFateW2()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
            this.Stage = 2;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new[] { 15, 22.5, 30, 37.5, 45 }[level] + (1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)) + (0.5 * source.TotalMagicalDamage);
        }

        #endregion
    }
}