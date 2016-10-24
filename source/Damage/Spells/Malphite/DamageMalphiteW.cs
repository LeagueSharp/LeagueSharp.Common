// <copyright file="DamageMalphiteW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Malphite W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Malphite", SpellSlot.W)]
    public class DamageMalphiteW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageMalphiteW" /> class.
        /// </summary>
        public DamageMalphiteW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 30, 38, 46, 54, 62 }[level] / 100 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}