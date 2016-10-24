// <copyright file="DamageRyzeQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Ryze Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Ryze", SpellSlot.Q)]
    public class DamageRyzeQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageRyzeQ" /> class.
        /// </summary>
        public DamageRyzeQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            var objAiHero = source as Obj_AI_Hero;
            if (objAiHero != null)
            {
                return (new double[] { 60, 85, 110, 135, 160, 185 }[level] + (0.45 * source.TotalMagicalDamage)
                        + (0.03 * (source.MaxMana - 392.4 - (52 * objAiHero.Level))))
                       * (1
                          + (target.HasBuff("RyzeE")
                                 ? new double[] { 40, 55, 70, 85, 100 }[
                                       ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).Level - 1] / 100
                                 : 0));
            }

            return 0d;
        }

        #endregion
    }
}