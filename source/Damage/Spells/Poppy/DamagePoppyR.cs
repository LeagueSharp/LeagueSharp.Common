// <copyright file="DamagePoppyR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Poppy R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Poppy", SpellSlot.R)]
    public class DamagePoppyR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamagePoppyR" /> class.
        /// </summary>
        public DamagePoppyR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 200, 300, 400 }[level] + (0.9 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}