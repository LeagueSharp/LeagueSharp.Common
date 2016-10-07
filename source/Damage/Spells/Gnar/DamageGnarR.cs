// <copyright file="DamageGnarR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Gnar R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Gnar", SpellSlot.R)]
    public class DamageGnarR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageGnarR" /> class.
        /// </summary>
        public DamageGnarR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 200, 300, 400 }[level] + (0.5 * source.TotalMagicalDamage) + (0.2 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}