// <copyright file="DamagePantheonE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Pantheon E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Pantheon", SpellSlot.E)]
    public class DamagePantheonE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamagePantheonE" /> class.
        /// </summary>
        public DamagePantheonE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return (new double[] { 13, 23, 33, 43, 53 }[level] + (0.6 * source.FlatPhysicalDamageMod)) * ((target is Obj_AI_Hero) ? 2 : 1);
        }

        #endregion
    }
}