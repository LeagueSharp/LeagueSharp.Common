// <copyright file="DamageKatarinaW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Katarina W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Katarina", SpellSlot.W)]
    public class DamageKatarinaW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKatarinaW" /> class.
        /// </summary>
        public DamageKatarinaW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 40, 75, 110, 145, 180 }[level] + (0.6 * source.FlatPhysicalDamageMod) + (0.25 * source.TotalMagicalDamage);
        }

        #endregion
    }
}