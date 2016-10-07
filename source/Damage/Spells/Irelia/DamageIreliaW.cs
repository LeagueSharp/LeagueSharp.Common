// <copyright file="DamageIreliaW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Irelia W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Irelia", SpellSlot.W)]
    public class DamageIreliaW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageIreliaW" /> class.
        /// </summary>
        public DamageIreliaW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.True;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 15, 30, 45, 60, 75 }[level];
        }

        #endregion
    }
}