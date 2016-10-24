// <copyright file="DamagePoppyQ1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Poppy Q (Stage 1).
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Poppy", SpellSlot.Q, 1)]
    public class DamagePoppyQ1 : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamagePoppyQ1" /> class.
        /// </summary>
        public DamagePoppyQ1()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
            this.Stage = 1;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 110, 150, 190, 230 }[level] + (1.6 * source.FlatPhysicalDamageMod) + (0.14 * target.MaxHealth);
        }

        #endregion
    }
}