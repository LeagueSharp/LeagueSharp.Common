// <copyright file="DamageDravenE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Draven E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Draven", SpellSlot.E)]
    public class DamageDravenE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageDravenE" /> class.
        /// </summary>
        public DamageDravenE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 105, 140, 175, 210 }[level] + (0.5 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}