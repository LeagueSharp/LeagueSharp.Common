// <copyright file="DamageGnarE1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Gnar E (Stage 1).
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Gnar", SpellSlot.E, 1)]
    public class DamageGnarE1 : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageGnarE1" /> class.
        /// </summary>
        public DamageGnarE1()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
            this.Stage = 1;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 20, 60, 100, 140, 180 }[level] + (source.MaxHealth * 0.06);
        }

        #endregion
    }
}