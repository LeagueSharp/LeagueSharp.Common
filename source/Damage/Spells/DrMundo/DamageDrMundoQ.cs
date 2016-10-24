// <copyright file="DamageDrMundoQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, DrMundo Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("DrMundo", SpellSlot.Q)]
    public class DamageDrMundoQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageDrMundoQ" /> class.
        /// </summary>
        public DamageDrMundoQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            if (target is Obj_AI_Minion)
            {
                return Math.Min(
                    new double[] { 300, 350, 400, 450, 500 }[level],
                    Math.Max(
                        new double[] { 80, 130, 180, 230, 280 }[level],
                        new[] { 15, 17.5, 20, 22.5, 25 }[level] / 100 * target.Health));
            }

            return Math.Max(
                new double[] { 80, 130, 180, 230, 280 }[level],
                new[] { 15, 17.5, 20, 22.5, 25 }[level] / 100 * target.Health);
        }

        #endregion
    }
}