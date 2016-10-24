// <copyright file="DamageShacoW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Shaco W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Shaco", SpellSlot.W)]
    public class DamageShacoW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageShacoW" /> class.
        /// </summary>
        public DamageShacoW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 35, 50, 65, 80, 95 }[level] + (0.2 * source.TotalMagicalDamage);
        }

        #endregion
    }
}