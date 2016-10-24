// <copyright file="DamageLeeSinR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, LeeSin R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("LeeSin", SpellSlot.R)]
    public class DamageLeeSinR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageLeeSinR" /> class.
        /// </summary>
        public DamageLeeSinR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 200, 400, 600 }[level] + (2 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}