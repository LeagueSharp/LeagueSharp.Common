// <copyright file="DamageSpell.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    /// <summary>
    ///     Gets the damage done to a target.
    /// </summary>
    /// <param name="source">
    ///     The source.
    /// </param>
    /// <param name="target">
    ///     The target.
    /// </param>
    /// <param name="level">
    ///     The level.
    /// </param>
    /// <returns>
    ///     The <see cref="double" />.
    /// </returns>
    public delegate double SpellDamageDelegate(Obj_AI_Base source, Obj_AI_Base target, int level);

    /// <summary>
    ///     Spell damage information.
    /// </summary>
    public class DamageSpell : IDamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageSpell" /> class.
        /// </summary>
        public DamageSpell()
        {
            this.Damage = this.GetDamage;
        }

        #endregion

        #region Public Properties

        /// <inheritdoc />
        public double CalculatedDamage { get; set; }

        /// <inheritdoc />
        public SpellDamageDelegate Damage { get; set; }

        /// <inheritdoc />
        public Damage.DamageType DamageType { get; set; }

        /// <inheritdoc />
        public SpellSlot Slot { get; set; }

        /// <inheritdoc />
        public int Stage { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Calculates the spell damage.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <param name="level">
        ///     The level.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        protected virtual double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return 0d;
        }

        #endregion
    }
}