// <copyright file="DamageTaricW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Taric W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Taric", SpellSlot.W)]
    public class DamageTaricW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTaricW" /> class.
        /// </summary>
        public DamageTaricW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 40, 80, 120, 160, 200 }[level] + (0.2 * source.Armor);
        }

        #endregion
    }
}