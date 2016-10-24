// <copyright file="DamageRenektonW1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Renekton W (Stage 1).
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Renekton", SpellSlot.W, 1)]
    public class DamageRenektonW1 : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageRenektonW1" /> class.
        /// </summary>
        public DamageRenektonW1()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Physical;
            this.Stage = 1;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return (new double[] { 10, 30, 50, 70, 90 }[level] + (1.5 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod))) * 1.5;
        }

        #endregion
    }
}