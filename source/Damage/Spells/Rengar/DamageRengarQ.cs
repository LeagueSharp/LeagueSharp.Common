// <copyright file="DamageRengarQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Rengar Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Rengar", SpellSlot.Q)]
    public class DamageRengarQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageRengarQ" /> class.
        /// </summary>
        public DamageRengarQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 30, 60, 90, 120, 150 }[level] + (((new double[] { 100, 105, 110, 115, 120 }[level] / 100) - 1) * (source.BaseAttackDamage + source.FlatPhysicalDamageMod));
        }

        #endregion
    }
}