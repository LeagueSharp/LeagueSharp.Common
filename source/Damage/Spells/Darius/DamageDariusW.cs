// <copyright file="DamageDariusW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Darius W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Darius", SpellSlot.W)]
    public class DamageDariusW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageDariusW" /> class.
        /// </summary>
        public DamageDariusW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return source.TotalAttackDamage + (0.4 * source.TotalAttackDamage);
        }

        #endregion
    }
}