// <copyright file="DamageMasterYiE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, MasterYi E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("MasterYi", SpellSlot.E)]
    public class DamageMasterYiE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageMasterYiE" /> class.
        /// </summary>
        public DamageMasterYiE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.True;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return (new[] { 10, 12.5, 15, 17.5, 20 }[level] / 100 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)) + new double[] { 10, 15, 20, 25, 30 }[level];
        }

        #endregion
    }
}