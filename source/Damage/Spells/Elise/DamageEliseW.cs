// <copyright file="DamageEliseW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Elise W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Elise", SpellSlot.W)]
    public class DamageEliseW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageEliseW" /> class.
        /// </summary>
        public DamageEliseW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 75, 125, 175, 225, 275 }[level] + (0.8 * source.TotalMagicalDamage);
        }

        #endregion
    }
}