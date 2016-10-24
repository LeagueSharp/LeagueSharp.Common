// <copyright file="DamageSpells.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

    using LeagueSharp.Common.Spells;

    /// <summary>
    ///     Damage calculations and data.
    /// </summary>
    public partial class Damage
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the spells.
        /// </summary>
        [ImportMany]
        public IEnumerable<Lazy<IDamageSpell, IDamageSpellMetadata>> SpellLazies { get; protected set; }

        #endregion

        #region Properties

        private IDictionary<string, IList<Lazy<IDamageSpell, IDamageSpellMetadata>>> SpellsDictionary { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the damage spell.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <param name="spellName">
        ///     The spell name.
        /// </param>
        /// <returns>
        ///     The <see cref="DamageSpell" />
        /// </returns>
        public DamageSpell GetDamageSpell(Obj_AI_Base source, Obj_AI_Base target, string spellName)
        {
            return null;
        }

        /// <summary>
        ///     Gets the damage spell.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <param name="slot">
        ///     The slot.
        /// </param>
        /// <param name="stage">
        ///     The stage.
        /// </param>
        /// <returns>
        ///     The <see cref="DamageSpell" />
        /// </returns>
        public DamageSpell GetDamageSpell(Obj_AI_Hero source, Obj_AI_Base target, SpellSlot slot, int stage = 0)
        {
            IList<Lazy<IDamageSpell, IDamageSpellMetadata>> value;
            if (this.SpellsDictionary.TryGetValue(source.ChampionName, out value))
            {
                var spell = value.FirstOrDefault(v => v.Metadata.SpellSlot == slot && v.Metadata.Stage == stage);
                if (spell != null)
                {
                    var level = Math.Max(0, Math.Min(source.Spellbook.GetSpell(slot).Level - 1, 5));
                    var d = spell.Value.Damage(source, target, level);

                    spell.Value.CalculatedDamage = this.CalculateDamage(source, target, spell.Value.DamageType, d);
                    return spell.Value as DamageSpell;
                }
            }

            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sorts the spells lazies into a dictionary for faster functionality.
        /// </summary>
        internal void SortSpells()
        {
            var dic = new Dictionary<string, IList<Lazy<IDamageSpell, IDamageSpellMetadata>>>();

            var champs = ObjectManager.Get<Obj_AI_Hero>().Select(s => s.ChampionName).ToArray();

            foreach (var lazy in this.SpellLazies)
            {
                if (!champs.Any(c => c.Equals(lazy.Metadata.ChampionName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    continue;
                }

                IList<Lazy<IDamageSpell, IDamageSpellMetadata>> value;
                if (dic.TryGetValue(lazy.Metadata.ChampionName, out value))
                {
                    value.Add(lazy);
                }
                else
                {
                    dic[lazy.Metadata.ChampionName] = new List<Lazy<IDamageSpell, IDamageSpellMetadata>> { lazy };
                }
            }

            this.SpellsDictionary = dic;
        }

        #endregion
    }
}