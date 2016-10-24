// <copyright file="DamageDariusR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Darius R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Darius", SpellSlot.R)]
    public class DamageDariusR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageDariusR" /> class.
        /// </summary>
        public DamageDariusR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.True;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 100, 200, 300 }[level] + (0.75 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}