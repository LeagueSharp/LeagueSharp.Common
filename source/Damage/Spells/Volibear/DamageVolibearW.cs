// <copyright file="DamageVolibearW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Volibear W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Volibear", SpellSlot.W)]
    public class DamageVolibearW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageVolibearW" /> class.
        /// </summary>
        public DamageVolibearW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 110, 160, 210, 260 }[level] * (((target.MaxHealth - target.Health) / target.MaxHealth) + 1);
        }

        #endregion
    }
}