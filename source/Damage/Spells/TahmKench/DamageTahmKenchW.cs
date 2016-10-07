// <copyright file="DamageTahmKenchW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, TahmKench W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("TahmKench", SpellSlot.W)]
    public class DamageTahmKenchW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTahmKenchW" /> class.
        /// </summary>
        public DamageTahmKenchW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return target is Obj_AI_Minion ? new double[] { 400, 450, 500, 550, 600 }[level] : (new[] { 0.20, 0.23, 0.26, 0.29, 0.32 }[level] + (0.02 * source.TotalMagicalDamage / 100)) * target.MaxHealth;
        }

        #endregion
    }
}