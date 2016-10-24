// <copyright file="DamageZiggsW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Ziggs W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Ziggs", SpellSlot.W)]
    public class DamageZiggsW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageZiggsW" /> class.
        /// </summary>
        public DamageZiggsW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 105, 140, 175, 210 }[level] + (0.35 * source.TotalMagicalDamage);
        }

        #endregion
    }
}