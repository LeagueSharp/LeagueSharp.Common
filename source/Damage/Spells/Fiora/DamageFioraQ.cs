// <copyright file="DamageFioraQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Fiora Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Fiora", SpellSlot.Q)]
    public class DamageFioraQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageFioraQ" /> class.
        /// </summary>
        public DamageFioraQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 65, 75, 85, 95, 105 }[level] + (new[] { 0.95, 1, 1.05, 1.1, 1.15 }[level] * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}