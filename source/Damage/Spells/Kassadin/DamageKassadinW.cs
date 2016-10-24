// <copyright file="DamageKassadinW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Kassadin W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Kassadin", SpellSlot.W)]
    public class DamageKassadinW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKassadinW" /> class.
        /// </summary>
        public DamageKassadinW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 40, 65, 90, 115, 140 }[level] + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}