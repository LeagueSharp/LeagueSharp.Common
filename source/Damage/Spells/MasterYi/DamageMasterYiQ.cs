// <copyright file="DamageMasterYiQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, MasterYi Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("MasterYi", SpellSlot.Q)]
    public class DamageMasterYiQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageMasterYiQ" /> class.
        /// </summary>
        public DamageMasterYiQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 25, 60, 95, 130, 165 }[level] + (1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)) + (0.6 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod));
        }

        #endregion
    }
}