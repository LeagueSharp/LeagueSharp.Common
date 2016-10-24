// <copyright file="DamagePantheonQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Pantheon Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Pantheon", SpellSlot.Q)]
    public class DamagePantheonQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamagePantheonQ" /> class.
        /// </summary>
        public DamagePantheonQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return (new double[] { 65, 105, 145, 185, 225 }[level] + (1.4 * source.FlatPhysicalDamageMod)) * ((target.Health / target.MaxHealth < 0.15) ? 2 : 1);
        }

        #endregion
    }
}