// <copyright file="DamageLissandraW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Lissandra W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Lissandra", SpellSlot.W)]
    public class DamageLissandraW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageLissandraW" /> class.
        /// </summary>
        public DamageLissandraW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 110, 150, 190, 230 }[level] + (0.4 * source.TotalMagicalDamage);
        }

        #endregion
    }
}