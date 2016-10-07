// <copyright file="DamageQuinnR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Quinn R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Quinn", SpellSlot.R)]
    public class DamageQuinnR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageQuinnR" /> class.
        /// </summary>
        public DamageQuinnR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return source.TotalAttackDamage;
        }

        #endregion
    }
}