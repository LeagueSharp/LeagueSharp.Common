#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Damage.cs is part of LeagueSharp.Common.
 
 LeagueSharp.Common is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 LeagueSharp.Common is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with LeagueSharp.Common. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

#region

using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp.Common.Data;
using Ferocity = LeagueSharp.Common.Data.MasteryData.Ferocity;
using Cunning = LeagueSharp.Common.Data.MasteryData.Cunning;
using Resolve = LeagueSharp.Common.Data.MasteryData.Resolve;


#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    /// Gets the damage done to a target.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="target">The target.</param>
    /// <param name="level">The level.</param>
    /// <returns></returns>
    public delegate double SpellDamageDelegate(Obj_AI_Base source, Obj_AI_Base target, int level);

    /// <summary>
    /// Represents a spell that deals damage.
    /// </summary>
    public class DamageSpell
    {
        /// <summary>
        /// The calculated damage
        /// </summary>
        public double CalculatedDamage;

        /// <summary>
        /// The damage delegate
        /// </summary>
        public SpellDamageDelegate Damage;

        /// <summary>
        /// The damage type
        /// </summary>
        public Damage.DamageType DamageType;

        /// <summary>
        /// The slot
        /// </summary>
        public SpellSlot Slot;

        /// <summary>
        /// The stage
        /// </summary>
        public int Stage;
    }

    /// <summary>
    /// Calculates damage to units.
    /// </summary>
    public static class Damage
    {
        /// <summary>
        /// Represents items that deal damage.
        /// </summary>
        public enum DamageItems
        {
            /// <summary>
            /// The hexgun
            /// </summary>
            Hexgun,

            /// <summary>
            /// The Dfg
            /// </summary>
            Dfg,

            /// <summary>
            /// The botrk
            /// </summary>
            Botrk,

            /// <summary>
            /// The bilgewater
            /// </summary>
            Bilgewater,

            /// <summary>
            /// The tiamat
            /// </summary>
            Tiamat,

            /// <summary>
            /// The hydra
            /// </summary>
            Hydra,

            /// <summary>
            /// The black fire torch
            /// </summary>
            BlackFireTorch,

            /// <summary>
            /// The oding veils
            /// </summary>
            OdingVeils,

            /// <summary>
            /// The frost queen claim
            /// </summary>
            FrostQueenClaim,

            /// <summary>
            /// The liandrys torment
            /// </summary>
            LiandrysTorment,
        }

        /// <summary>
        /// The type of damage.
        /// </summary>
        public enum DamageType
        {
            /// <summary>
            /// Physical damage. (AD)
            /// </summary>
            Physical,

            /// <summary>
            /// Magical damage. (AP)
            /// </summary>
            Magical,

            /// <summary>
            /// True damage
            /// </summary>
            True
        }

        /// <summary>
        /// Represnets summoner spells that deal damage.
        /// </summary>
        public enum SummonerSpell
        {
            /// <summary>
            /// The ignite spell.
            /// </summary>
            Ignite,

            /// <summary>
            /// The smite spell.
            /// </summary>
            Smite,
        }

        /// <summary>
        /// The spells
        /// </summary>
        public static Dictionary<string, List<DamageSpell>> Spells =
            new Dictionary<string, List<DamageSpell>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The attack passives
        /// </summary>
        private static readonly List<PassiveDamage> AttackPassives = new List<PassiveDamage>();

        //attack passives are handled in the orbwalker, it will be changed in the future :^)

        /// <summary>
        /// Initializes static members of the <see cref="Damage"/> class. 
        /// </summary>
        static Damage()
        {
            //Add the passive damages
            PassiveDamage p;

            #region PassiveDamages

            #region Aatrox

            p = new PassiveDamage
            {
                ChampionName = "Aatrox",
                IsActive =
                            (source, target) => (source.HasBuff("AatroxWPower") && source.HasBuff("AatroxWONHPowerBuff")),
                GetDamage = (source, target) => ((float)source.GetSpellDamage(target, SpellSlot.W)),
            };
            AttackPassives.Add(p);

            #endregion

            #region Alistar

            p = new PassiveDamage
            {
                ChampionName = "Alistar",
                IsActive = (source, target) => (source.HasBuff("alistartrample")),
                GetDamage =
                            (source, target) =>
                            ((float)
                             source.CalcDamage(
                                 target,
                                 DamageType.Magical,
                                 6d + source.Level + (0.1d * source.TotalMagicalDamage))),
            };
            AttackPassives.Add(p);

            #endregion

            #region Caitlyn

            p = new PassiveDamage
            {
                ChampionName = "Caitlyn", IsActive = (source, target) => (source.HasBuff("caitlynheadshot")),
                GetDamage =
                            (source, target) =>
                            ((float)
                             source.CalcDamage(
                                 target,
                                 DamageType.Physical,
                                 1.5d * (source.BaseAttackDamage + source.FlatPhysicalDamageMod))),
            };
            AttackPassives.Add(p);

            #endregion

            #region Draven

            p = new PassiveDamage
            {
                ChampionName = "Draven", IsActive = (source, target) => (source.HasBuff("DravenSpinning")),
                GetDamage =
                            (source, target) =>
                            ((float)
                             source.CalcDamage(
                                 target,
                                 DamageType.Physical,
                                 0.45d * (source.BaseAttackDamage + source.FlatPhysicalDamageMod))),
            };
            AttackPassives.Add(p);

            #endregion

            #region Corki

            p = new PassiveDamage
            {
                ChampionName = "Corki", IsActive = (source, target) => (source.HasBuff("rapidreload")),
                GetDamage =
                            (source, target) => ((float)0.1d * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)),
            };
            AttackPassives.Add(p);

            #endregion

            #region Ekko

            p = new PassiveDamage
            {
                ChampionName = "Ekko",
                IsActive = (source, target) => (target.GetBuffCount("EkkoStacks") == 2),
                GetDamage = (source, target) =>
                 (float)source.CalcDamage(target, DamageType.Magical, 10 + (source.Level * 10) + (source.TotalMagicalDamage * 0.8)),
            };
            AttackPassives.Add(p);
            
            p = new PassiveDamage
            {
                ChampionName = "Ekko",
                IsActive = (source, target) => (target.HealthPercent < 30),
                GetDamage = (source, target) =>
                {
                    float dmg = (float)source.CalcDamage(target, LeagueSharp.Common.Damage.DamageType.Magical, (target.MaxHealth - target.Health) * (5 + Math.Floor(source.TotalMagicalDamage / 100) * 2.2f) / 100);
                    if (!(target is Obj_AI_Hero) && dmg > 150f)
                        dmg = 150f;
                    return dmg;
                }
            };
            AttackPassives.Add(p);

            #endregion

            #region Graves

            p = new PassiveDamage
            {
                ChampionName = "Graves",
                IsActive = (source, target) => true,
                GetDamage =
                            (source, target) =>
                            (float) (((72 + 3 * source.Level) /100f) * source.CalcDamage(
                                 target,
                                 DamageType.Physical,
                                 source.TotalAttackDamage) - source.CalcDamage(
                                 target,
                                 DamageType.Physical,
                                 source.TotalAttackDamage)),
            };
            AttackPassives.Add(p);

            #endregion

            #region Gnar

            p = new PassiveDamage
            {
                ChampionName = "Gnar",
                IsActive = (source, target) => (target.GetBuffCount("gnarwproc") == 2),
                GetDamage = (source, target) => ((float)source.GetSpellDamage(target, SpellSlot.W)),
            };
            AttackPassives.Add(p);

            #endregion

            #region Jinx

            p = new PassiveDamage
            {
                ChampionName = "Jinx", IsActive = (source, target) => (source.HasBuff("JinxQ")),
                GetDamage =
                            (source, target) =>
                            ((float)
                             source.CalcDamage(
                                 target,
                                 DamageType.Physical,
                                 0.1d * (source.BaseAttackDamage + source.FlatPhysicalDamageMod))),
            };
            AttackPassives.Add(p);

            #endregion

            #region Katarina

            p = new PassiveDamage
            {
                ChampionName = "Katarina", IsActive = (source, target) => (target.HasBuff("katarinaqmark")),
                GetDamage = (source, target) => ((float)source.GetSpellDamage(target, SpellSlot.Q, 1)),
            };
            AttackPassives.Add(p);

            #endregion

            #region KogMaw

            p = new PassiveDamage
            {
                ChampionName = "KogMaw", IsActive = (source, target) => (source.HasBuff("KogMawBioArcaneBarrage")),
                GetDamage = (source, target) => ((float)source.GetSpellDamage(target, SpellSlot.W)),
            };
            AttackPassives.Add(p);

            #endregion

            #region Nasus

            p = new PassiveDamage
            {
                ChampionName = "Nasus", IsActive = (source, target) => (source.HasBuff("NasusQ")),
                GetDamage = (source, target) => ((float)source.GetSpellDamage(target, SpellSlot.Q)),
            };
            AttackPassives.Add(p);

            #endregion

            #region Orianna

            p = new PassiveDamage
            {
                ChampionName = "Orianna", IsActive = (source, target) => (source.HasBuff("orianaspellsword")),
                GetDamage =
                            (source, target) =>
                            (float)
                            source.CalcDamage(
                                target,
                                DamageType.Magical,
                                (float)0.15 * source.TotalMagicalDamage
                                + new float[] { 10, 10, 10, 18, 18, 18, 26, 26, 26, 34, 34, 34, 42, 42, 42, 50, 50, 50 }[
                                    source.Level - 1]),
            };
            AttackPassives.Add(p);

            #endregion

            #region Quinn

            p = new PassiveDamage
            {
                ChampionName = "Quinn",
                IsActive = (source, target) => (target.HasBuff("quinnw")),
                GetDamage =
                            (source, target) =>
                            ((float)
                             source.CalcDamage(
                                 target,
                                 DamageType.Physical,
                                 0.5d * source.TotalAttackDamage)),
            };
            AttackPassives.Add(p);

            #endregion
            
            #region Rengar

            p = new PassiveDamage
            {
                ChampionName = "Rengar",
                IsActive = (source, target) => source.HasBuff("rengarqbase"),
                GetDamage = (source, target) => (float)source.CalcDamage(target, LeagueSharp.Common.Damage.DamageType.Physical, new int[] { 30, 60, 90, 120, 150 }[source.GetSpell(SpellSlot.Q).Level - 1] + (source.BaseAttackDamage + source.FlatPhysicalDamageMod) * new int[] { 0, 5, 10, 15, 20 }[source.GetSpell(SpellSlot.Q).Level - 1] / 100f)
            };
            AttackPassives.Add(p);

			
			p = new PassiveDamage
            {
                ChampionName = "Rengar",
                IsActive = (source, target) => source.HasBuff("rengarqemp"),
                GetDamage = (source, target) => (float)source.CalcDamage(target, LeagueSharp.Common.Damage.DamageType.Physical, new int[] { 30, 45, 60, 75, 90, 105, 120, 135, 150, 160, 170, 180, 190, 200, 210, 220, 230, 240 }[source.Level - 1] + (source.BaseAttackDamage + source.FlatPhysicalDamageMod) * 0.5f)
            };
            AttackPassives.Add(p);
            #endregion
 
            #region Riven

            p = new PassiveDamage
            {
                ChampionName = "Riven",
                IsActive = (source, target) => source.GetBuffCount("rivenpassiveaaboost") > 0,
                GetDamage =
                            (source, target) =>
                            ((float)
                             source.CalcDamage(
                                 target,
                                 DamageType.Physical,
                                 new[]
                                     {
                                         0.2, 0.2, 0.25, 0.25, 0.25, 0.3, 0.3, 0.3, 0.35, 0.35, 0.35, 0.4, 0.4, 0.4, 0.45,
                                         0.45, 0.45, 0.5
                                     }[source.Level - 1]
                                 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod))),
            };
            AttackPassives.Add(p);

            #endregion

            #region Teemo

            p = new PassiveDamage
            {
                ChampionName = "Teemo", IsActive = (source, target) => (source.HasBuff("ToxicShot")),
                GetDamage =
                            (source, target) =>
                            ((float)
                             source.CalcDamage(
                                 target,
                                 DamageType.Magical,
                                 source.Spellbook.GetSpell(SpellSlot.E).Level * 10 + source.TotalMagicalDamage * 0.3)),
            };
            AttackPassives.Add(p);

            #endregion

            #region TwistedFate

            p = new PassiveDamage
            {
                ChampionName = "TwistedFate", IsActive = (source, target) => (source.HasBuff("bluecardpreattack")),
                GetDamage =
                            (source, target) =>
                            (float)source.GetSpellDamage(target, SpellSlot.W)
                            - (float)
                              source.CalcDamage(
                                  target,
                                  DamageType.Physical,
                                  (source.BaseAttackDamage + source.FlatPhysicalDamageMod)) - 10f,
            };
            AttackPassives.Add(p);
            
            p = new PassiveDamage
            {
                ChampionName = "TwistedFate", IsActive = (source, target) => (source.HasBuff("redcardpreattack")),
                GetDamage =
                            (source, target) =>
                            (float)source.GetSpellDamage(target, SpellSlot.W, 2)
                            - (float)
                              source.CalcDamage(
                                  target,
                                  DamageType.Physical,
                                  (source.BaseAttackDamage + source.FlatPhysicalDamageMod)) - 10f,
            };
            AttackPassives.Add(p);
            
            p = new PassiveDamage
            {
                ChampionName = "TwistedFate", IsActive = (source, target) => (source.HasBuff("goldcardpreattack")),
                GetDamage =
                            (source, target) =>
                            (float)source.GetSpellDamage(target, SpellSlot.W, 3)
                            - (float)
                              source.CalcDamage(
                                  target,
                                  DamageType.Physical,
                                  (source.BaseAttackDamage + source.FlatPhysicalDamageMod)) - 10f,
            };
            AttackPassives.Add(p);

            p = new PassiveDamage
            {
                ChampionName = "TwistedFate",
                IsActive = (source, target) => (source.HasBuff("cardmasterstackparticle")),
                GetDamage = (source, target) => (float)source.GetSpellDamage(target, SpellSlot.E),
            };
            AttackPassives.Add(p);

            #endregion

            #region Varus

            p = new PassiveDamage
            {
                ChampionName = "Varus", IsActive = (source, target) => (source.HasBuff("VarusW")),
                GetDamage = (source, target) => ((float)source.GetSpellDamage(target, SpellSlot.W)),
            };
            AttackPassives.Add(p);

            #endregion

            #region Vayne

            p = new PassiveDamage
            {
                ChampionName = "Vayne", IsActive = (source, target) => (source.HasBuff("vaynetumblebonus")),
                GetDamage = (source, target) => ((float)source.GetSpellDamage(target, SpellSlot.Q)),
            };
            AttackPassives.Add(p);

            p = new PassiveDamage
            {
                ChampionName = "Vayne",
                IsActive =
                            (source, target) =>
                            (from buff in target.Buffs where buff.Name == "vaynesilvereddebuff" select buff.Count)
                                .FirstOrDefault() == 2,
                GetDamage = (source, target) => ((float)source.GetSpellDamage(target, SpellSlot.W)),
            };
            AttackPassives.Add(p);

            #endregion

            #region Viktor

            p = new PassiveDamage
            {
                ChampionName = "Viktor", IsActive = (source, target) => (source.HasBuff("viktorpowertransferreturn")),
                GetDamage =
                            (source, target) =>
                            (float)
                            source.CalcDamage(
                                target,
                                DamageType.Magical,
                                (float)0.5d * source.TotalMagicalDamage
                                + new float[]
                                      { 20, 25, 30, 35, 40, 45, 50, 55, 60, 70, 80, 90, 110, 130, 150, 170, 190, 210 }[
                                          source.Level - 1]),
            };
            AttackPassives.Add(p);

            #endregion

            #region Ziggs

            p = new PassiveDamage
            {
                ChampionName = "Ziggs", IsActive = (source, target) => (source.HasBuff("ziggsshortfuse")),
                GetDamage =
                            (source, target) =>
                            (float)
                            source.CalcDamage(
                                target,
                                DamageType.Magical,
                                (float)0.25d * source.TotalMagicalDamage
                                + new float[]
                                      { 20, 24, 28, 32, 36, 40, 48, 56, 64, 72, 80, 88, 100, 112, 124, 136, 148, 160 }[
                                          source.Level - 1]),
            };
            AttackPassives.Add(p);

            #endregion

            #endregion

            //Synced on -[dte]- 18:53 with patch 4.16 data.

            #region Spells

            Spells.Add(
                "Aatrox",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 115, 160, 205, 250 }[level]
                                    + 0.6 * source.FlatPhysicalDamageMod
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 95, 130, 165, 200 }[level]
                                    + 1 * source.FlatPhysicalDamageMod
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 75, 110, 145, 180, 215 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                                    + 0.6 * source.FlatPhysicalDamageMod
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 200, 300, 400 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Ahri",
                new List<DamageSpell>
                    {
                        //Normal Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 65, 90, 115, 140 }[level]
                                    + 0.35 * source.TotalMagicalDamage
                            },
                        //Q Return
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.True,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 65, 90, 115, 140 }[level]
                                    + 0.35 * source.TotalMagicalDamage
                            },
                        //W => First FF to target
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 65, 90, 115, 140 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //W => Additional FF to already FF target
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 12, 19.5, 27, 34.5, 42 }[level]
                                    + 0.12 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 95, 130, 165, 200 }[level]
                                    + 0.50 * source.TotalMagicalDamage
                            },
                        //R, per dash
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 110, 150 }[level]
                                    + 0.3 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Akali",
                new List<DamageSpell>
                    {
                        //Q Initial
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 35, 55, 75, 95, 115 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //Q Detonation
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 45, 70, 95, 120, 145 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 55, 80, 105, 130 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                                    + 0.6 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 100, 175, 250 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Alistar",
                new List<DamageSpell>
                    {
                        //Q Initial
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 105, 150, 195, 240 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 55, 110, 165, 220, 275 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Amumu",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 130, 180, 230, 280 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                        //W - per second
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 8, 12, 16, 20, 24 }[level]
                                    + (new[] { 0.01, 0.015, 0.02, 0.025, 0.03 }[level]
                                       + 0.01 * source.TotalMagicalDamage / 100) * target.MaxHealth
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 75, 100, 125, 150, 175 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 250, 350 }[level]
                                    + 0.8 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Anivia",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 90, 120, 150, 180 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //Q - max
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 90, 120, 150, 180 }[level] * 2
                                    + 1 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 55, 85, 115, 145, 175 }[level]
                                    + 0.5 * source.TotalMagicalDamage) * (target.HasBuff("chilled") ? 2 : 1)
                            },
                        //R - per second
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 120, 160 }[level]
                                    + 0.25 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Annie",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 115, 150, 185, 220 }[level]
                                    + 0.8 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 115, 160, 205, 250 }[level]
                                    + 0.85 * source.TotalMagicalDamage
                            },
                        //R - total
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 210, 335, 460 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Ashe",
                new List<DamageSpell>
                    {
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 35, 50, 65, 80 }[level]
                                    + 1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //R - total
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 250, 425, 600 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                        //R - Min
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 250, 425, 600 }[level]
                                     + 1 * source.TotalMagicalDamage) / 2
                            },
                    });

            Spells.Add(
                "Azir",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 65, 85, 105, 125, 145 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //W - Soldier auto attacks
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 55, 60, 75, 80, 90 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 120, 160, 200, 240 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 225, 300 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Blitzcrank",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 135, 190, 245, 300 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 250, 375, 500 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Bard",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 125, 170, 215, 260 }[level]
                                    + 0.65 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Brand",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 120, 160, 200, 240 }[level]
                                    + 0.65 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 75, 120, 165, 210, 255 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 105, 140, 175, 210 }[level]
                                    + 0.55 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 250, 350 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Braum",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 115, 160, 205, 250 }[level]
                                    + 0.025 * source.MaxHealth
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 250, 350 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Caitlyn",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 70, 110, 150, 190 }[level]
                                    + new double[] { 1.3, 1.4, 1.5, 1.6, 1.7 }[level] * (source.TotalAttackDamage)
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 110, 150, 190, 230 }[level]
                                    + 0.8 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 250, 475, 700 }[level]
                                    + 2 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Cassiopeia",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 75, 115, 155, 195, 235 }[level]
                                    + 0.45 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 10, 15, 20, 25, 30 }[level]
                                    + 0.1 * source.TotalMagicalDamage
                            },
                        //E 
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 55, 80, 105, 130, 155 }[level]
                                    + 0.55 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 250, 350 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "ChoGath",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 135, 190, 245, 305 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 75, 125, 175, 225, 275 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 35, 50, 65, 80 }[level]
                                    + 0.3 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.True,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 300, 475, 650 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Corki",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 130, 180, 230, 280 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                                    + 0.5 * source.FlatPhysicalDamageMod
                            },
                        //W 
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 90, 120, 150, 180 }[level]
                                    + 0.4 * source.TotalMagicalDamage 
                            },

                        //W - Burn
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical, Stage = 1,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 45, 60, 75, 90 }[level] + (1.5 * source.FlatPhysicalDamageMod)
                                    + 0.2 * source.TotalMagicalDamage
                            },


                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 32, 44, 56, 68 }[level]
                                    + 0.4 * source.FlatPhysicalDamageMod
                            },
                        //R - Normal missile
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 100, 130, 160 }[level]
                                    + 0.3 * source.TotalMagicalDamage
                                    + new double[] { 20, 50, 80 }[level] / 100
                                    * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //R - Big missile
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 195, 240 }[level]
                                    + 0.45 * source.TotalMagicalDamage
                                    + new double[] { 30, 75, 120 }[level] / 100
                                    * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                    });

            Spells.Add(
                "Darius",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new[] { 40, 70, 100, 130, 160 }[level] +
                                   (new [] { 0.5, 1.1, 1.2, 1.3, 1.4 }[level] * source.TotalAttackDamage)
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                        source.TotalAttackDamage + (0.4 * source.TotalAttackDamage)
                            },
                        //R 
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.True,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 100, 200, 300 }[level] + 0.75 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Diana",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 95, 130, 165, 200 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 22, 34, 46, 58, 70 }[level]
                                    + 0.2 * source.TotalMagicalDamage
                            },
                        //R - total
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 100, 160, 220 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "DrMundo",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage = (source, target, level) =>
                                    {
                                        if (target is Obj_AI_Minion)
                                        {
                                            return
                                                Math.Min(
                                                    new double[] { 300, 350, 400, 450, 500 }[level],
                                                    Math.Max(
                                                        new double[] { 80, 130, 180, 230, 280 }[level],
                                                        new double[] { 15, 17.5, 20, 22.5, 25 }[level] / 100
                                                        * target.Health));
                                        }
                                        return Math.Max(
                                            new double[] { 80, 130, 180, 230, 280 }[level],
                                            new double[] { 15, 17.5, 20, 22.5, 25 }[level] / 100
                                            * target.Health);
                                    }
                            },
                        //W - per second
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 35, 50, 65, 80, 95 }[level]
                                    + 0.2 * source.TotalMagicalDamage
                            }
                    });

            Spells.Add(
                "Draven",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 45, 55, 65, 75, 85 }[level] / 100
                                    * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 105, 140, 175, 210 }[level]
                                    + 0.5 * source.FlatPhysicalDamageMod
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 175, 275, 375 }[level]
                                    + 1.1 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Ekko",
                new List<DamageSpell>
                    {
                        // Q - Outgoing
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 75, 90, 105, 120 }[level]
                                    + 0.1 * source.TotalMagicalDamage
                            },
                        // Q - Incoming
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 85, 110, 135, 160 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        // W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 195, 240, 285, 330 }[level]
                                    + 0.8 * source.TotalMagicalDamage
                            },
                        // E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 80, 110, 140, 170 }[level]
                                    + 0.2 * source.TotalMagicalDamage
                            },
                        // R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 200, 350, 500 }[level]
                                    + 1.3 * source.TotalMagicalDamage
                            }
                    });

            Spells.Add(
                "Elise",
                new List<DamageSpell>
                    {
                        //Q - Human
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 75, 110, 145, 180 }[level]
                                    + (0.08 + 0.03 / 100 * source.TotalMagicalDamage) * target.Health
                            },
                        //Q - Spider
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 100, 140, 180, 220 }[level]
                                    + (0.08 + 0.03 / 100 * source.TotalMagicalDamage)
                                    * (target.MaxHealth - target.Health)
                            },
                        //W - Human
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 75, 125, 175, 225, 275 }[level]
                                    + 0.8 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Evelynn",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 50, 60, 70, 80 }[level]
                                    + new double[] { 35, 40, 45, 50, 55 }[level] / 100
                                    * source.TotalMagicalDamage
                                    + new double[] { 50, 55, 60, 65, 70 }[level] / 100
                                    * source.FlatPhysicalDamageMod
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 110, 150, 190, 230 }[level]
                                    + 1 * source.TotalMagicalDamage + 1 * source.FlatPhysicalDamageMod
                            },
                        //R - total
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    (new[] { 0.15, 0.20, 0.25 }[level]
                                     + 0.01 / 100 * source.TotalMagicalDamage) * target.Health
                            },
                    });

            Spells.Add(
                "Ezreal",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 35, 55, 75, 95, 115 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                                    + 1.1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 115, 160, 205, 250 }[level]
                                    + 0.8 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 75, 125, 175, 225, 275 }[level]
                                    + 0.75 * source.TotalMagicalDamage
                                    + 0.5 * source.FlatPhysicalDamageMod
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 350, 500, 650 }[level]
                                    + 0.9 * source.TotalMagicalDamage + 1 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Fiddlesticks",
                new List<DamageSpell>
                    {
                        //W - Per second
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 90, 120, 150, 180 }[level]
                                    + 0.45 * source.TotalMagicalDamage
                            },
                        //E - Per bounce
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 65, 85, 105, 125, 145 }[level]
                                    + 0.45 * source.TotalMagicalDamage
                            },
                        //R - Per second
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 125, 225, 325 }[level]
                                    + 0.45 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Fiora",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 65, 75, 85, 95, 105 }[level]
                                    + new [] { .55, .70, .85, 1, 1.15 }[level] * source.FlatPhysicalDamageMod
                            },
                        //W 
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 90, 130, 170, 210, 250 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Fizz",
                new List<DamageSpell>
                    {
                        //Q - AA excluded.
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 10, 25, 40, 55, 70 }[level]
                                    + 0.35 * source.TotalMagicalDamage
                            },
                        //W - Per attack
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 10, 15, 20, 25, 30 }[level]
                                    + 0.3 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 120, 170, 220, 270 }[level]
                                    + 0.75 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 200, 325, 450 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Galio",
                new List<DamageSpell>
                    {
                        //Q 
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 135, 190, 245, 300 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 105, 150, 195, 240 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //R - max
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 360, 540, 720 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "GangPlank",
                new List<DamageSpell>
                    {
                        //Q 
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 45, 70, 95, 120 }[level]
                                    + 1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //R - per cannonball
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 70, 90 }[level]
                                    + 0.1 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Garen",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 55, 80, 105, 130 }[level]
                                    + 1.4 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //E 
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 45, 70, 95, 120 }[level]
                                    + new double[] { 70, 80, 90, 100, 110 }[level] / 100
                                    * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //R - Max damage
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 175, 350, 525 }[level]
                                    + new[] { 28.57, 33.33, 40 }[level] / 100
                                    * (target.MaxHealth - target.Health)
                            },
                    });

            Spells.Add(
                "Gnar",
                new List<DamageSpell>
                    {
                        //Q - mini
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 5, 35, 65, 95, 125 }[level]
                                    + 1.15 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //Q - big
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 5, 45, 85, 125, 165 }[level]
                                    + 1.2 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //W - mini
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 10, 20, 30, 40, 50 }[level]
                                    + 1 * source.TotalMagicalDamage
                                    + new double[] { 6, 8, 10, 12, 14 }[level] / 100 * target.MaxHealth
                            },
                        //W - big
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, Stage = 1, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 25, 45, 65, 85, 105 }[level]
                                    + 1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //E - mini
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 60, 100, 140, 180 }[level]
                                    + source.MaxHealth * 0.06
                            },
                        //E - big
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, Stage = 1, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 60, 100, 140, 180 }[level]
                                    + source.MaxHealth * 0.06
                            },
                        //R - Max damage
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 200, 300, 400 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                                    + 0.2 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Gragas",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 120, 160, 200, 240 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 50, 80, 110, 140 }[level]
                                    + 8 / 100 * target.MaxHealth
                                    + 0.3 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 130, 180, 230, 280 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 200, 300, 400 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Graves",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 55, 70, 85, 100, 115 }[level]
                                    + 0.75 * source.FlatPhysicalDamageMod
                            },
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 90, 155, 220, 285, 350 }[level]
                                    + new double[] { 0.4, 0.6, 0.8, 1, 1.2 }[level] * source.FlatPhysicalDamageMod
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 110, 160, 210, 260 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //R - Max damage
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 250, 400, 550 }[level]
                                    + 1.5 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Hecarim",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 95, 130, 165, 200 }[level]
                                    + 0.6 * source.FlatPhysicalDamageMod
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 30, 40, 50, 60 }[level]
                                    + 0.2 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 75, 110, 145, 180 }[level]
                                    + 0.5 * source.FlatPhysicalDamageMod
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 250, 350 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Heimerdinger",
                new List<DamageSpell>
                    {
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 90, 120, 150, 180 }[level]
                                    + 0.45 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 135, 180, 225 }[
                                        source.Spellbook.GetSpell(SpellSlot.R).Level - 1]
                                    + 0.45 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 100, 140, 180, 220 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 200, 250 }[
                                        source.Spellbook.GetSpell(SpellSlot.R).Level - 1]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Irelia",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 50, 80, 110, 140 }[level]
                                    + 1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.True,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 15, 30, 45, 60, 75 }[level]
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 120, 160, 200, 240 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //R - per blade
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 120, 160 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                                    + 0.6 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Janna",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 85, 110, 135, 160 }[level]
                                    + 0.35 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 115, 170, 225, 280 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "JarvanIV",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 115, 160, 205, 250 }[level]
                                    + 1.2 * source.FlatPhysicalDamageMod
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 105, 150, 195, 240 }[level]
                                    + 0.8 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 200, 325, 450 }[level]
                                    + 1.5 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Jax",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 110, 150, 190, 230 }[level]
                                    + 1 * source.FlatPhysicalDamageMod + 0.6 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 75, 110, 145, 180 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 75, 100, 125, 150 }[level]
                                    + 0.5 * source.FlatPhysicalDamageMod
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 100, 160, 220 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Jayce",
                new List<DamageSpell>
                    {
                        //Q 
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 120, 170, 220, 270, 320 }[level]
                                    + 1.2 * source.FlatPhysicalDamageMod
                            },
                        //Q - Melee
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 70, 110, 150, 190, 230 }[level]
                                    + 1 * source.FlatPhysicalDamageMod
                            },
                        //W - per second
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new[] { 25, 40, 55, 70, 85, 100 }[level]
                                    + 0.25 * source.TotalMagicalDamage
                            },

                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    (new[] { 8, 10.4, 12.8, 15.2, 17.6, 20 }[level] / 100)
                                    * target.MaxHealth + 1 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Jinx",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    0.1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 10, 60, 110, 160, 210 }[level]
                                    + 1.4 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 135, 190, 245, 300 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                        //R - Min
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 25, 35, 45 }[level]
                                    + new double[] { 25, 30, 35 }[level] / 100
                                    * (target.MaxHealth - target.Health)
                                    + 0.1 * source.FlatPhysicalDamageMod
                            },
                        //R - Max
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, Stage = 1, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 250, 350, 450 }[level]
                                    + new double[] { 25, 30, 35 }[level] / 100
                                    * (target.MaxHealth - target.Health)
                                    + 1 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Karma",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 125, 170, 215, 260 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //Q - mantra
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 125, 170, 215, 260 }[level]
                                    + new double[] { 25, 75, 125, 175 }[
                                        source.Spellbook.GetSpell(SpellSlot.R).Level - 1]
                                    + 0.9 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 110, 160, 210, 260 }[level]
                                    + 0.9 * source.TotalMagicalDamage
                            },
                        //W - mantra
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 110, 160, 210, 260 }[level]
                                    + 0.9 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Karthus",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 40, 60, 80, 100, 120 }[level]
                                     + 0.3 * source.TotalMagicalDamage) * 2
                            },
                        //Q - Multi-target
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 60, 80, 100, 120 }[level]
                                    + 0.3 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 50, 70, 90, 110 }[level]
                                    + 0.2 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 250, 400, 550 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Kassadin",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 95, 120, 145, 170 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 65, 90, 115, 140 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //W - pasive
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, Stage = 1, DamageType = DamageType.Magical,
                                Damage = (source, target, level) => 20 + 0.1 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 105, 130, 155, 180 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                        //R - Base
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 100, 120 }[level] + 0.02 * source.MaxMana
                            },
                        //R - Per Stack
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 50, 60 }[level] + 0.01 * source.MaxMana
                            },
                    });

            Spells.Add(
                "Katarina",
                new List<DamageSpell>
                    {
                        //Q - dagger
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 85, 110, 135, 160 }[level]
                                    + 0.45 * source.TotalMagicalDamage
                            },
                        //Q - mark
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 15, 30, 45, 60, 75 }[level]
                                    + 0.15 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 75, 110, 145, 180 }[level]
                                    + 0.6 * source.FlatPhysicalDamageMod
                                    + 0.25 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 70, 100, 130, 160 }[level]
                                    + 0.25 * source.TotalMagicalDamage
                            },
                        //R - per dagger
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 350, 550, 750 }[level]
                                     + 3.75 * source.FlatPhysicalDamageMod
                                     + 2.5 * source.TotalMagicalDamage) / 10
                            },
                        //R - max
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 350, 550, 750 }[level]
                                    + 3.75 * source.FlatPhysicalDamageMod
                                    + 2.5 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Kayle",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 110, 160, 210, 260 }[level]
                                    + 1 * source.FlatPhysicalDamageMod + 0.6 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 30, 40, 50, 60 }[level]
                                    + 0.25 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Kennen",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 75, 115, 155, 195, 235 }[level]
                                    + 0.75 * source.TotalMagicalDamage
                            },
                        //W - Passive
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 50, 60, 70, 80 }[level] / 100
                                    * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //W - Active
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 65, 95, 125, 155, 185 }[level]
                                    + 0.55 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 85, 125, 165, 205, 245 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 145, 210 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "KhaZix",
                new List<DamageSpell>
                    {
                        //Q - Normal target - UnEvolved
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 95, 120, 145, 170 }[level]
                                    + 1.2 * source.FlatPhysicalDamageMod
                            },
                        //Q - Isolated target - UnEvolved
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new[] { 91, 123.5, 156, 188.5, 221 }[level]
                                    + 1.56 * source.FlatPhysicalDamageMod
                            },
                        //Q - Normal target - Evolved
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 2, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 95, 120, 145, 170 }[level]
                                    + 2.24 * source.FlatPhysicalDamageMod
                                    + 10 * ((Obj_AI_Hero)source).Level
                            },
                        //Q - Isolated target - Evolved
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 3, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new[] { 91, 123.5, 156, 188.5, 221 }[level]
                                    + 2.6 * source.FlatPhysicalDamageMod
                                    + 10 * ((Obj_AI_Hero)source).Level
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 110, 140, 170, 200 }[level]
                                    + 1 * source.FlatPhysicalDamageMod
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 65, 100, 135, 170, 205 }[level]
                                    + 0.2 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "KogMaw",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 130, 180, 230, 280 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage = (source, target, level) =>
                                    {
                                        var dmg = (0.02 + (Math.Truncate(source.TotalMagicalDamage / 100) * 0.75))
                                                  * target.MaxHealth;

                                        if (target is Obj_AI_Minion && dmg > 100)
                                        {
                                            dmg = 100;
                                        }

                                        return dmg;
                                    }                
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 110, 160, 210, 260 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 70, 110, 150 }[level]
                                    + 0.65 * source.FlatPhysicalDamageMod
                                    + 0.25 * source.TotalMagicalDamage) * 
                                    (target.HealthPercent < 25 ? 3 : (target.HealthPercent < 50 ? 2 : 1))
                            },
                    });

            Spells.Add(
                "Kalista",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 10, 70, 130, 190, 250 }[level]
                                    + source.BaseAttackDamage + source.FlatPhysicalDamageMod
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 12, 14, 16, 18, 20 }[level] / 100)
                                    * target.MaxHealth
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage = (source, target, level) =>
                                    {
                                        var count = target.GetBuffCount("kalistaexpungemarker");
                                        if (count > 0)
                                        {
                                            return (new double[] { 20, 30, 40, 50, 60 }[level]
                                                    + 0.6
                                                    * (source.BaseAttackDamage
                                                       + source.FlatPhysicalDamageMod)) +
                                                   // Base damage of E
                                                   ((count - 1)
                                                    * (new double[] { 10, 14, 19, 25, 32 }[level]
                                                       + // Base damage per spear
                                                       new double[] { 0.2, 0.225, 0.25, 0.275, 0.3 }[
                                                           level]
                                                       * (source.BaseAttackDamage
                                                          + source.FlatPhysicalDamageMod)));
                                            // Damage multiplier per spear
                                        }
                                        return 0;
                                    }
                            },
                    });

            Spells.Add(
                "Kindred",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 90, 120, 150, 180}[level]
                                    + (source.BaseAttackDamage + source.FlatPhysicalDamageMod) * 0.2f
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 25, 30, 35, 40, 45 }[level]
                                    + (source.BaseAttackDamage + source.FlatPhysicalDamageMod) * 0.4f
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 110, 140, 170, 200 }[level]
                                    + (source.BaseAttackDamage + source.FlatPhysicalDamageMod) * 0.2f
                                    + target.MaxHealth * 0.05f
                            },
                    });

            Spells.Add(
                "LeBlanc",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 55, 80, 105, 130, 155 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //Q . explosion
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 55, 80, 105, 130, 155 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 85, 125, 165, 205, 245 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 65, 90, 115, 140 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "LeeSin",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 80, 110, 140, 170 }[level]
                                    + 0.9 * source.FlatPhysicalDamageMod
                            },
                        //Q - 2nd
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 80, 110, 140, 170 }[level]
                                    + 0.9 * source.FlatPhysicalDamageMod
                                    + 0.08 * (target.MaxHealth - target.Health)
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 95, 130, 165, 200 }[level]
                                    + 1 * source.FlatPhysicalDamageMod
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 200, 400, 600 }[level]
                                    + 2 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Leona",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 70, 100, 130, 160 }[level]
                                    + 0.3 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 110, 160, 210, 260 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 100, 140, 180, 220 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 250, 350 }[level]
                                    + 0.8 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Lissandra",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 100, 130, 160, 190 }[level]
                                    + 0.65 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 110, 150, 190, 230 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 115, 160, 205, 250 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 250, 350 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Lucian",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 110, 140, 170, 200 }[level]
                                    + new double[] { 60, 75, 90, 105, 120 }[level] / 100
                                    * source.FlatPhysicalDamageMod
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 100, 140, 180, 220 }[level]
                                    + 0.9 * source.TotalMagicalDamage
                            },
                        //R - per shot
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 50, 60 }[level] + 0.1 * source.TotalMagicalDamage
                                    + 0.25 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Lulu",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 125, 170, 215, 260 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 110, 140, 170, 200 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Lux",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 110, 160, 210, 260 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 105, 150, 195, 240 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 300, 400, 500 }[level]
                                    + 0.75 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Malphite",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 120, 170, 220, 270 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },

                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 38, 46, 54, 62 }[level] / 100
                                    * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 100, 140, 180, 220 }[level] + 0.3 * source.Armor
                                    + 0.2 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 200, 300, 400 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Malzahar",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 135, 190, 245, 300 }[level]
                                    + 0.8 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 4, 4.5, 5, 5.5, 6 }[level] / 100
                                     + 0.01 / 100 * source.TotalMagicalDamage) * target.MaxHealth
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 140, 200, 260, 320 }[level]
                                    + 0.8 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 250, 400, 550 }[level]
                                    + 1.3 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Maokai",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 115, 160, 205, 250 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 9, 10, 11, 12, 13 }[level] / 100
                                     + 0.03 / 100 * source.TotalMagicalDamage) * target.MaxHealth
                            },
                        //E - impact
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 60, 80, 100, 120 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //E - explosion
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 120, 160, 200, 240 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 100, 150, 200 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "MasterYi",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 25, 60, 95, 130, 165 }[level]
                                    + 1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                                    + 0.6 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.True,
                                Damage =
                                    (source, target, level) =>
                                    new[] { 10, 12.5, 15, 17.5, 20 }[level] / 100
                                    * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                                    + new double[] { 10, 15, 20, 25, 30 }[level]
                            },
                    });

            Spells.Add(
                "MissFortune",
                new List<DamageSpell>
                    {
                        //Q - First target
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 35, 50, 65, 80 }[level]
                                    + 0.35 * source.TotalMagicalDamage
                                    + 0.85 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //Q - Second target
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 70, 100, 130, 160 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                                    + 1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    0.06 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 115, 150, 185, 220 }[level]
                                    + 0.8 * source.TotalMagicalDamage
                            },
                        //R - per wave
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    0.75 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                                    + 0.2 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "MonkeyKing",
                new List<DamageSpell> //AKA wukong
                    {
                        //Q - bonus
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 60, 90, 120, 150 }[level]
                                    + 0.1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 115, 160, 205, 250 }[level] + 0.6 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 105, 150, 195, 240 }[level] + 0.8 * source.FlatPhysicalDamageMod
                            },
                        //R - per second
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 110, 200 }[level]
                                    + 1.1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                    });

            Spells.Add(
                "Mordekaiser",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 110, 140, 170, 200 }[level]
                                    + 1 * source.FlatPhysicalDamageMod + 0.4 * source.TotalMagicalDamage
                            },
                        //W - per second
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 24, 38, 52, 66, 80 }[level]
                                    + 0.2 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 115, 160, 205, 250 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //R - total
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 24, 29, 34 }[level] / 100
                                     + 0.04 / 100 * source.TotalMagicalDamage) * target.MaxHealth
                            },
                    });

            Spells.Add(
                "Morgana",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 135, 190, 245, 300 }[level]
                                    + 0.9 * source.TotalMagicalDamage
                            },
                        //W - per tick
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 8, 16, 24, 32, 40 }[level]
                                    + 0.11 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 225, 300 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Nami",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 75, 130, 185, 240, 295 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 110, 150, 190, 230 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 25, 40, 55, 70, 85 }[level]
                                    + 0.2 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 250, 350 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Nasus",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    (from buff in ObjectManager.Player.Buffs
                                     where buff.Name == "nasusqstacks"
                                     select buff.Count).FirstOrDefault()
                                    + new double[] { 30, 50, 70, 90, 110 }[level]
                            },
                        //E - Initial
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 55, 95, 135, 175, 215 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //E - per second
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 11, 19, 27, 35, 43 }[level]
                                    + 0.12 * source.TotalMagicalDamage
                            },
                        //R - per second
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 3, 4, 5 }[level] / 100
                                     + 0.01 / 100 * source.TotalMagicalDamage) * target.MaxHealth
                            },
                    });

            Spells.Add(
                "Nautilus",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 105, 150, 195, 240 }[level]
                                    + 0.75 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 40, 50, 60, 70 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 100, 140, 180, 220 }[level]
                                    + 0.3 * source.TotalMagicalDamage
                            },
                        //R - main target
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 200, 325, 450 }[level]
                                    + 0.8 * source.TotalMagicalDamage
                            },
                        //R - missile
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 125, 175, 225 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Nidalee",
                new List<DamageSpell>
                    {
                        //Q - human - min * 3 = max
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 77.5, 95, 112.5, 130 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //Q - cat
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) => {
                                     var dmg = (new double[] { 4, 20, 50, 90 }[
                                        source.Spellbook.GetSpell(SpellSlot.R).Level - 1]
                                     + 0.36 * source.TotalMagicalDamage
                                     + 0.75 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod))
                                    * ((target.MaxHealth - target.Health) / target.MaxHealth * 1.5 + 1);
                                        dmg *= target.HasBuff("nidaleepassivehunted") ? 1.33 : 1.0;
                                        return dmg;
                                    }
                            },
                        //W - human
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 80, 120, 160, 200 }[level]
                                    + 0.2 * source.TotalMagicalDamage
                            },
                        //W - cat
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 100, 150, 200 }[
                                        source.Spellbook.GetSpell(SpellSlot.R).Level - 1]
                                    + 0.3 * source.TotalMagicalDamage
                            },
                        //E - cat
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 130, 190, 250 }[
                                        source.Spellbook.GetSpell(SpellSlot.R).Level - 1]
                                    + 0.45 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Nocturne",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 105, 150, 195, 240 }[level]
                                    + 0.75 * source.FlatPhysicalDamageMod
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 120, 160, 200, 260 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 250, 350 }[level]
                                    + 1.2 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Nunu",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.True,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 400, 550, 700, 850, 1000 }[level]
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 85, 130, 175, 225, 275 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                        //R - Max Damage
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 625, 875, 1125 }[level]
                                    + 2.5 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Olaf",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 115, 160, 205, 250 }[level]
                                    + 1 * source.FlatPhysicalDamageMod
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.True,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 115, 160, 205, 250 }[level]
                                    + 0.4 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                    });

            Spells.Add(
                "Orianna",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 90, 120, 150, 180 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 115, 160, 205, 250 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 90, 120, 150, 180 }[level]
                                    + 0.3 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 225, 300 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Pantheon",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 65, 105, 145, 185, 225 }[level]
                                     + 1.4 * source.FlatPhysicalDamageMod)
                                    * ((target.Health / target.MaxHealth < 0.15) ? 2 : 1)
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 75, 100, 125, 150 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                        //E - per strike
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 13, 23, 33, 43, 53 }[level]
                                     + 0.6 * source.FlatPhysicalDamageMod)
                                    * ((target is Obj_AI_Hero) ? 2 : 1)
                            },
                        //R - max
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 400, 700, 1000 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                        //R - min
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 400, 700, 1000 }[level]
                                     + 1 * source.TotalMagicalDamage) * 0.5
                            },
                    });

            Spells.Add(
                "Poppy",
                new List<DamageSpell>
                    {
                        //Q - single hit
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] {40, 70, 100, 130, 160}[level]
                                    + 0.65 * source.FlatPhysicalDamageMod
                                    + 0.06 * target.MaxHealth
                            },
                        //Q - both hits
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] {80, 140, 200, 260, 320}[level]
                                    + 1.3 * source.FlatPhysicalDamageMod
                                    + 0.12 * target.MaxHealth)
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 110, 150, 190, 230 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                        //E - without colliding
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 70, 90, 110, 130 }[level]
                                    + 0.5 * source.FlatPhysicalDamageMod
                            },
                        //E - with colliding
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, Stage = 1, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 100, 140, 180, 220, 260 }[level]
                                    + 1 * source.FlatPhysicalDamageMod)
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 200, 300, 400 }[level]
                                    + 0.9 * source.FlatPhysicalDamageMod)
                            },
                    });

            Spells.Add(
                "Quinn",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) => {
                                        double damage = (new double[] { 20, 45, 70, 95, 120}[level] + (new double[] { 0.8, 0.9, 1.0, 1.1, 1.2 }[level] * source.TotalAttackDamage)
                                        + 0.35 * source.TotalMagicalDamage);
                                        damage += damage * ((100 - target.HealthPercent) / 100);
                                        return damage;
                                    }
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 70, 100, 130, 160 }[level]
                                    + 0.2 * source.FlatPhysicalDamageMod
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) => source.TotalAttackDamage
                            },
                    });

            Spells.Add(
                "Rammus",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 100, 150, 200, 250, 300 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 15, 25, 35, 45, 55 }[level] + 0.1 * source.Armor
                            },
                        //R - per second
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 65, 130, 195 }[level]
                                    + 0.3 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Renekton",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 90, 120, 150, 180 }[level]
                                    + 0.8 * source.FlatPhysicalDamageMod
                            },
                        //Q - empowered
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 60, 90, 120, 150, 180 }[level]
                                     + 0.8 * source.FlatPhysicalDamageMod) * 1.5
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 10, 30, 50, 70, 90 }[level]
                                    + 1.5 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //W - empowered
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, Stage = 1, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 10, 30, 50, 70, 90 }[level]
                                     + 1.5 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod))
                                    * 1.5
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 60, 90, 120, 150 }[level]
                                    + 0.9 * source.FlatPhysicalDamageMod
                            },
                        //E - empowered
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 30, 60, 90, 120, 150 }[level]
                                     + 0.9 * source.FlatPhysicalDamageMod) * 1.5
                            },
                        //R - per second
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 60, 120 }[level]
                                    + 0.1 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Rengar",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 60, 90, 120, 150 }[level]
                                    + new double[] { 0, 5, 10, 15, 20 }[level] / 100
                                    * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //Q - Extra
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 60, 90, 120, 150 }[level]
                                    + (new double[] { 100, 105, 110, 115, 120 }[level] / 100 - 1)
                                    * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 80, 110, 140, 170 }[level]
                                    + 0.8 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 100, 150, 200, 250 }[level]
                                    + 0.7 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Riven",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 10, 30, 50, 70, 90 }[level]
                                    + ((source.BaseAttackDamage + source.FlatPhysicalDamageMod) / 100)
                                    * new double[] { 40, 45, 50, 55, 60 }[level]
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 80, 110, 140, 170 }[level]
                                    + 1 * source.FlatPhysicalDamageMod
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 80, 120, 160 }[level]
                                     + 0.6 * source.FlatPhysicalDamageMod)
                                    * ((target.MaxHealth - target.Health) / target.MaxHealth * 2.67 + 1)
                            },
                    });

            Spells.Add(
                "Rumble",
                new List<DamageSpell>
                    {
                        //Q - total  damage
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 75, 135, 195, 255, 315 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                        //Q - Danger Zone total damage
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new[] { 112.5, 202.5, 292.5, 382.5, 472.5 }[level]
                                    + 1.5 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 45, 70, 95, 120, 145 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //E - Danger Zone
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new[] { 67.5, 105, 142.5, 180, 217.5 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //R - per second
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 130, 185, 240 }[level]
                                    + 0.3 * source.TotalMagicalDamage
                            },
                        //R - Total
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 650, 925, 1200 }[level]
                                    + 1.5 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Ryze",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 85, 110, 135, 160 }[level]
                                    + 0.55 * source.TotalMagicalDamage
                                    + new double[] { 2, 2.5, 3, 3.5, 4 }[level] / 100 * source.MaxMana
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 100, 120, 140, 160 }[level]
                                    + 0.4 * source.TotalMagicalDamage + 0.025 * source.MaxMana
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 36, 52, 68, 84, 100 }[level]
                                    + 0.2 * source.TotalMagicalDamage + 0.02 * source.MaxMana
                            },
                    });

            Spells.Add(
                "Sejuani",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 125, 170, 215, 260 }[level]
                            },

                        //W - AA  damage
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new[] { 4, 4.5, 5, 5.5, 6 }[level] / 100 * target.MaxHealth
                            },
                        //W - Aoe per second
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new[] { 10, 17.5, 25, 32.5, 40 }[level]
                                    + (new double[] { 4, 6, 8, 10, 12 }[level] / 100) * source.MaxHealth
                                    + 0.15 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 90, 120, 150, 180 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 250, 350 }[level]
                                    + 0.8 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Shaco",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 140, 160, 180, 200, 220 }[level] / 100
                                    * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //W - per attack
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 35, 50, 65, 80, 95 }[level]
                                    + 0.2 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 90, 130, 170, 210 }[level]
                                    + 1 * source.FlatPhysicalDamageMod + 1 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 300, 450, 600 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Shen",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 100, 140, 180, 220 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 85, 120, 155, 190 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Shyvana",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 85, 90, 95, 100 }[level] / 100
                                    * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //W - per second
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 35, 50, 65, 80 }[level]
                                    + 0.2 * source.FlatPhysicalDamageMod
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 100, 140, 180, 220 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 175, 300, 425 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Singed",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 22, 34, 46, 58, 70 }[level]
                                    + 0.3 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 65, 80, 95, 110 }[level]
                                    + 0.75 * source.TotalMagicalDamage
                                    + new double[] { 4, 5.5, 7, 8.5, 10 }[level] / 100
                                    * target.MaxHealth
                            },
                    });

            Spells.Add(
                "Sion",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 40, 60, 80, 100 }[level]
                                    + 0.6 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 120, 180, 240, 300 }[level]
                                    + 1.8 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 65, 90, 115, 140 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                                    + new double[] { 10, 11, 12, 13, 14 }[level] / 100
                                    * target.MaxHealth
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 105, 140, 175, 210 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 70, 105, 140, 175, 210 }[level]
                                     + 0.4 * source.TotalMagicalDamage) * 1.5
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 300, 450 }[level]
                                    + 0.4 * source.FlatPhysicalDamageMod
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, Stage = 1, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 150, 300, 450 }[level]
                                     + 0.4 * source.FlatPhysicalDamageMod) * 2
                            },
                    });

            Spells.Add(
                "Sivir",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 25, 45, 65, 85, 105 }[level]
                                    + new double[] { 70, 80, 90, 100, 110 }[level] / 100
                                    * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //W - bounce
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 65, 70, 75, 80 }[level] / 100
                                    * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                    });

            Spells.Add(
                "Skarner",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 30, 40, 50, 60 }[level]
                                    + 0.4 * source.FlatPhysicalDamageMod
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 75, 110, 145, 180 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //R - total
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 20, 60, 100 }[level]
                                     + 0.5 * source.TotalMagicalDamage) + (0.60 * source.TotalAttackDamage)
                            },
                    });

            Spells.Add(
                "Sona",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 80, 120, 160, 200 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 250, 350 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Soraka",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 110, 150, 190, 230 }[level]
                                    + 0.35 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 110, 150, 190, 230 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Swain",
                new List<DamageSpell>
                    {
                        //Q - per second
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 25, 40, 55, 70, 85 }[level]
                                    + 0.3 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 120, 160, 200, 240 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 75, 115, 155, 195, 235 }[level]
                                    + 0.8 * source.TotalMagicalDamage
                            },
                        //R - per draven
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 70, 90 }[level] + 0.2 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Syndra",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 50, 95, 140, 185, 230 }[level]
                                     + 0.6 * source.TotalMagicalDamage)
                                    * ((level == 5 && target is Obj_AI_Hero) ? 1.15 : 1)
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 120, 160, 200, 240 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 115, 160, 205, 250 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //R - min damage
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 270, 405, 540 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //R - per sphere
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 90, 135, 180 }[level]
                                    + 0.2 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Talon",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 60, 90, 120, 150 }[level]
                                    + 0.3 * source.FlatPhysicalDamageMod
                                    + ((target is Obj_AI_Hero)
                                           ? (new double[] { 10, 20, 30, 40, 50 }[level]
                                              + 1 * source.FlatPhysicalDamageMod)
                                           : 0)
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 55, 80, 105, 130 }[level]
                                    + 0.6 * source.FlatPhysicalDamageMod
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 120, 170, 220 }[level]
                                    + 0.75 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Taric",
                new List<DamageSpell>
                    {
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 80, 120, 160, 200 }[level] + 0.2 * source.Armor
                            },
                        //E - min damage
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 70, 100, 130, 160 }[level]
                                    + 0.2 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 250, 350 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "TahmKench",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 125, 170, 215, 260 }[level] 
                                    + 0.7 * source.TotalMagicalDamage
                            },
                        //W - Devour 
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) => target is Obj_AI_Minion ?  
                                    new double[] { 400, 450, 500, 550, 600 }[level] :
                                    (new double[] { 0.20, 0.23, 0.26, 0.29, 0.32 }[level]
                                    + 0.02 * source.TotalMagicalDamage / 100) * target.MaxHealth
                            },
                        //W - Regugitate
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 100, 150, 200, 250, 300 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                    });


            Spells.Add(
                "Teemo",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 125, 170, 215, 260 }[level]
                                    + 0.8 * source.TotalMagicalDamage
                            },
                        //E - total
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 34, 68, 102, 136, 170 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                        //E - onhit
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 10, 20, 30, 40, 50 }[level]
                                    + 0.3 * source.TotalMagicalDamage
                            },
                        //R - total
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 200, 325, 450 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Thresh",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 120, 160, 200, 240 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //E - Active
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 65, 95, 125, 155, 185 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 250, 400, 550 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Tristana",
                new List<DamageSpell>
                    {
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 110, 160, 210, 260 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //E - base damage
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 70, 80, 90, 100 }[level]
                                    + new double[] { 0.5, 0.65, 0.8, 0.95, 1.10 }[level]
                                    * source.FlatPhysicalDamageMod + 0.5 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 300, 400, 500 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Trundle",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 40, 60, 80, 100 }[level]
                                    + new[] { 0, 0.5, 0.1, 0.15, 0.2 }[level]
                                    * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //R - Total
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 20, 24, 28 }[level] / 100
                                     + 0.02 * source.TotalMagicalDamage / 100) * target.MaxHealth
                            },
                    });

            Spells.Add(
                "Tryndamere",
                new List<DamageSpell>
                    {
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 100, 130, 160, 190 }[level]
                                    + 1.2 * source.FlatPhysicalDamageMod + 1 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "TwistedFate",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 105, 150, 195, 240 }[level]
                                    + 0.65 * source.TotalMagicalDamage
                            },
                        //W - Blue
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 60, 80, 100, 120 }[level]
                                    + 1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //W - Red
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 45, 60, 75, 90 }[level]
                                    + 1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //W - Yellow
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, Stage = 2, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new[] { 15, 22.5, 30, 37.5, 45 }[level]
                                    + 1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 55, 80, 105, 130, 155 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Twitch",
                new List<DamageSpell>
                    {
                        //E - current stacks
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    (from buff in target.Buffs
                                     where buff.DisplayName.ToLower() == "twitchdeadlyvenom"
                                     select buff.Count).FirstOrDefault()
                                    * (new double[] { 15, 20, 25, 30, 35 }[level]
                                       + 0.2 * source.TotalMagicalDamage
                                       + 0.25 * source.FlatPhysicalDamageMod)
                                    + new double[] { 20, 35, 50, 65, 80 }[level]
                            },
                        //E - per stack
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, Stage = 1, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 15, 20, 25, 30, 35 }[level]
                                    + 0.2 * source.TotalMagicalDamage
                                    + 0.25 * source.FlatPhysicalDamageMod
                                    + new double[] { 20, 35, 50, 65, 80 }[level]
                            },
                    });

            Spells.Add(
                "Udyr",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 80, 130, 180, 230 }[level]
                                    + (new double[] { 120, 130, 140, 150, 160 }[level] / 100)
                                    * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //R - per wave
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 15, 25, 35, 45, 55 }[level]
                                    + 0.25 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Urgot",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 10, 40, 70, 100, 130 }[level]
                                    + 0.85 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 75, 130, 185, 240, 295 }[level]
                                    + 0.6 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Varus",
                new List<DamageSpell>
                    {
                        //Q - min
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 10, 47, 83, 120, 157 }[level]
                                    + 1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //Q - max
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 15, 70, 125, 180, 235 }[level]
                                    + +1.6 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //W - on hit
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 10, 14, 18, 22, 26 }[level]
                                    + 0.25 * source.TotalMagicalDamage
                            },
                        //W - per stack
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    (new[] { 2, 2.75, 3.5, 4.25, 5 }[level] / 100
                                     + 0.02 * source.TotalMagicalDamage / 100) * target.MaxHealth
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 65, 100, 135, 170, 205 }[level]
                                    + 0.6 * source.FlatPhysicalDamageMod
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 100, 175, 250 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Vayne",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 35, 40, 45, 50 }[level] / 100
                                    * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.True,
                                Damage =
                                    (source, target, level) => Math.Max(new double[] { 40, 60, 80, 100, 120 }[level], (new double[] { 6, 7.5, 9, 10.5, 12 }[level] / 100) * target.MaxHealth)
                            },
            
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 45, 80, 115, 150, 185 }[level]
                                    + 0.5 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Veigar",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 125, 170, 215, 260 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 120, 170, 220, 270, 320 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 250, 375, 500 }[level]
                                    + 0.8 * target.TotalMagicalDamage + 1.0 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Velkoz",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 120, 160, 200, 240 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //W - Max
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 50, 70, 90, 110 }[level]
                                    + new double[] { 45, 75, 105, 135, 165 }[level]
                                    + 0.625 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 100, 130, 160, 190 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //R - max
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 500, 700, 900 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Vi",
                new List<DamageSpell>
                    {
                        //Q - min
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 75, 100, 125, 150 }[level]
                                    + 0.8 * source.FlatPhysicalDamageMod
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    (new[] { 4, 5.5, 7, 8.5, 10 }[level] / 100
                                     + 0.01 * source.FlatPhysicalDamageMod / 35) * target.MaxHealth
                            },
                        //E - extra
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 5, 20, 35, 50, 65 }[level]
                                    + 1.15 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                                    + 0.7 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 300, 450 }[level]
                                    + 1.4 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Viktor",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 60, 80, 100, 120 }[level]
                                    + 0.2 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 115, 160, 205, 250 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 98, 161, 224, 287, 350 }[level]
                                    + 0.98 * source.TotalMagicalDamage
                            },
                        //R - summon damage
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 250, 350 }[level]
                                    + 0.55 * source.TotalMagicalDamage
                            },
                        //R - per bolt
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 15, 30, 45 }[level] + 0.1 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Vladimir",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 90, 125, 160, 195, 230 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //W - max
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 135, 190, 245, 300 }[level]
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 85, 110, 135, 160 }[level]
                                    + 0.45 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 250, 350 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Volibear",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 60, 90, 120, 150 }[level]
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 80, 125, 170, 215, 260 }[level])
                                    * ((target.MaxHealth - target.Health) / target.MaxHealth + 1)
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 105, 150, 195, 240 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //R - per bolt
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 75, 115, 155 }[level]
                                    + 0.3 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Warwick",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    Math.Max(
                                        new double[] { 75, 125, 175, 225, 275 }[level],
                                        new double[] { 8, 10, 12, 14, 16 }[level] / 100
                                        * target.MaxHealth) + 1 * source.TotalMagicalDamage
                            },
                        //R - max
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 250, 350 }[level]
                                    + 2 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Xerath",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 120, 160, 200, 240 }[level]
                                    + 0.75 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 90, 120, 150, 180 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 110, 140, 170, 200 }[level]
                                    + 0.45 * source.TotalMagicalDamage
                            },
                        //R - per charge
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 190, 245, 300 }[level]
                                    + 0.43 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "XinZhao",
                new List<DamageSpell>
                    {
                        //Q - per attack
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 15, 30, 45, 60, 75 }[level]
                                    + 0.2 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 110, 150, 190, 230 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 75, 175, 275 }[level]
                                    + 1 * source.FlatPhysicalDamageMod + 0.15 * target.Health
                            },
                    });

            Spells.Add(
                "Yasuo",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 40, 60, 80, 100 }[level]
                                    + 1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //E - min
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 90, 110, 130, 150 }[level]
                                    + 0.6 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 200, 300, 400 }[level]
                                    + 1.5 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Yorick",
                new List<DamageSpell>
                    {
                        //Q - extra
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 30, 60, 90, 120, 150 }[level]
                                    + 1.2 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 95, 130, 165, 200 }[level]
                                    + 1 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 55, 85, 115, 145, 175 }[level]
                                    + 1 * source.FlatPhysicalDamageMod
                            },
                    });

            Spells.Add(
                "Zac",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 110, 150, 190, 230 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 55, 70, 85, 100 }[level]
                                    + (new double[] { 4, 5, 6, 7, 8 }[level] / 100
                                       + 0.02 * source.TotalMagicalDamage / 100) * target.MaxHealth
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 130, 180, 230, 280 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                        //R - per bounce
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 140, 210, 280 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Zed",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 75, 115, 155, 195, 235 }[level]
                                    + 1 * source.FlatPhysicalDamageMod
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 90, 120, 150, 180 }[level]
                                    + 0.8 * source.FlatPhysicalDamageMod
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
                            },
                    });

            Spells.Add(
                "Ziggs",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 75, 120, 165, 210, 255 }[level]
                                    + 0.65 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 105, 140, 175, 210 }[level]
                                    + 0.35 * source.TotalMagicalDamage
                            },
                        //E - per mine
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 40, 65, 90, 115, 140 }[level]
                                    + 0.3 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 250, 375, 500 }[level]
                                    + 0.9 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Zilean",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 90, 145, 200, 260, 320 }[level]
                                    + 0.9 * source.TotalMagicalDamage
                            },
                    });

            Spells.Add(
                "Zyra",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 105, 140, 175, 210 }[level]
                                    + 0.65 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 95, 130, 165, 200 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 180, 265, 350 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                    });

            #endregion
        }

        /// <summary>
        /// Gets the summoner spell damage.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="summonerSpell">The summoner spell.</param>
        /// <returns></returns>
        public static double GetSummonerSpellDamage(
            this Obj_AI_Hero source,
            Obj_AI_Base target,
            SummonerSpell summonerSpell)
        {
            if (summonerSpell == SummonerSpell.Ignite)
            {
                return 50 + 20 * source.Level - (target.HPRegenRate / 5 * 3);
            }

            if (summonerSpell == SummonerSpell.Smite)
            {
                if (target is Obj_AI_Hero)
                {
                    var chillingSmite =
                        source.Spellbook.Spells.FirstOrDefault(h => h.Name.Equals("s5_summonersmiteplayerganker"));
                    var challengingSmite =
                        source.Spellbook.Spells.FirstOrDefault(h => h.Name.Equals("s5_summonersmiteduel"));

                    if (chillingSmite != null)
                    {
                        return 20 + 8 * source.Level;
                    }

                    if (challengingSmite != null)

                    {
                        return 54 + 6 * source.Level;
                    }
                }

                return
                    new double[]
                        { 390, 410, 430, 450, 480, 510, 540, 570, 600, 640, 680, 720, 760, 800, 850, 900, 950, 1000 }[
                            source.Level - 1];
            }

            return 0d;
        }

        /// <summary>
        /// Gets the item damage.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static double GetItemDamage(this Obj_AI_Hero source, Obj_AI_Base target, DamageItems item)
        {
            switch (item)
            {
                case DamageItems.Bilgewater:
                    return source.CalcDamage(target, DamageType.Magical, 100);
                case DamageItems.BlackFireTorch:
                    return source.CalcDamage(target, DamageType.Magical, target.MaxHealth * 0.2);
                case DamageItems.Botrk:
                    return source.CalcDamage(target, DamageType.Physical, target.MaxHealth * 0.1);
                case DamageItems.FrostQueenClaim:
                    return source.CalcDamage(target, DamageType.Magical, 50 + 5 * source.Level);
                case DamageItems.Hexgun:
                    return source.CalcDamage(target, DamageType.Magical, 150 + 0.4 * source.TotalMagicalDamage);
                case DamageItems.Hydra:
                    return source.CalcDamage(
                        target,
                        DamageType.Physical,
                        source.BaseAttackDamage + source.FlatPhysicalDamageMod);
                case DamageItems.OdingVeils:
                    return source.CalcDamage(target, DamageType.Magical, 200);
                case DamageItems.Tiamat:
                    return source.CalcDamage(
                        target,
                        DamageType.Physical,
                        source.BaseAttackDamage + source.FlatPhysicalDamageMod);
                case DamageItems.LiandrysTorment:
                    var d = target.Health * .2f * 3f;
                    return (target.CanMove || target.HasBuff("slow")) ? d : d * 2;
            }
            return 1d;
        }

        /// <summary>
        /// Gets the automatic attack damage.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="includePassive">if set to <c>true</c> [include passive].</param>
        /// <returns></returns>
        public static double GetAutoAttackDamage(
            this Obj_AI_Base source,
            Obj_AI_Base target,
            bool includePassive = false)
        {
            double result = source.TotalAttackDamage;
	        var k = source.CharData.BaseSkinName == "Kalista" ? 0.9d : 1d;
	    
            if (!includePassive)
            {
                return CalcPhysicalDamage(source, target, result * k);
            }

            
            var reduction = 0d;

            var hero = source as Obj_AI_Hero;
            if (hero != null)
            {
                // Spoils of War
                var minionTarget = target as Obj_AI_Minion;
                if (hero.IsMelee() && minionTarget != null && minionTarget.IsEnemy
                    && minionTarget.Team != GameObjectTeam.Neutral
                    && hero.Buffs.Any(buff => buff.Name == "talentreaperdisplay" && buff.Count > 0))
                {
                    if (
                        HeroManager.AllHeroes.Any(
                            h =>
                            h.NetworkId != source.NetworkId && h.Team == source.Team
                            && h.Distance(minionTarget.Position) < 1100))
                    {
                        var value = 0;

                        if (Items.HasItem(3302, hero))
                        {
                            value = 200; // Relic Shield
                        }
                        else if (Items.HasItem(3097, hero))
                        {
                            value = 240; // Targon's Brace
                        }
                        else if (Items.HasItem(3401, hero))
                        {
                            value = 400; // Face of the Mountain
                        }

                        return value + hero.TotalAttackDamage;
                    }
                }


                //Champions passive damages:
                result +=
                    AttackPassives.Where(
                        p =>
                            (p.ChampionName == "" || p.ChampionName == hero.ChampionName) &&
                            p.IsActive(hero, target)).Sum(passive => passive.GetDamage(hero, target));

                // BotRK
                if (Items.HasItem(3153, hero))
                {
                    var d = 0.06 * target.Health;
                    if (target is Obj_AI_Minion)
                    {
                        d = Math.Min(d, 60);
                    }

                    result += d;
                }
            }

            var targetHero = target as Obj_AI_Hero;
            if (targetHero != null)
            {
                // Ninja tabi
                if (Items.HasItem(3047, targetHero))
                {
                    k *= 0.9d;
                }

                // Nimble Fighter
                if (targetHero.ChampionName == "Fizz")
                {
                    var f = new int[] { 4, 6, 8, 10, 12, 14};
                    reduction += f[(targetHero.Level - 1) / 3];
                }
            }

            return CalcPhysicalDamage(source, target, (result - reduction) * k + PassiveFlatMod(source, target));
        }

        /// <summary>
        ///     Calculates the combo damage of the given spell combo on the given target.
        /// </summary>
        /// <param name="source">The source object</param>
        /// <param name="target">The target object</param>
        /// <param name="spellCombo">SpellType array containing the combo spells</param>
        /// <returns>Returns the calculated combo damage</returns>
        public static double GetComboDamage(
            this Obj_AI_Hero source,
            Obj_AI_Base target,
            IEnumerable<SpellSlot> spellCombo)
        {
            return source.GetComboDamage(target, spellCombo.Select(spell => Tuple.Create(spell, 0)).ToArray());
        }

        /// <summary>
        ///     Calculates the combo damage of the given spell combo on the given target respecting the stage type of each spell
        /// </summary>
        /// <param name="source">The source object</param>
        /// <param name="target">The target object</param>
        /// <param name="spellCombo">SpellType/StageType tuple containing the combo spells</param>
        /// <returns>Returns the calculated combo damage</returns>
        public static double GetComboDamage(
            this Obj_AI_Hero source,
            Obj_AI_Base target,
            IEnumerable<Tuple<SpellSlot, int>> spellCombo)
        {
            return spellCombo.Sum(spell => source.GetSpellDamage(target, spell.Item1, spell.Item2));
        }

        /// <summary>
        ///     Calculates the combo damage of the given spell combo on the given target and returns if that damage would kill the
        ///     target.
        /// </summary>
        /// <param name="source">The source object</param>
        /// <param name="target">The target object</param>
        /// <param name="spellCombo">SpellType array containing the combo spells</param>
        /// <returns>true if target is killable, false if not.</returns>
        public static bool IsKillable(
            this Obj_AI_Hero source,
            Obj_AI_Base target,
            IEnumerable<Tuple<SpellSlot, int>> spellCombo)
        {
            return GetComboDamage(source, target, spellCombo) > target.Health;
        }

        /// <summary>
        /// Gets the damage spell.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="spellName">Name of the spell.</param>
        /// <returns></returns>
        public static DamageSpell GetDamageSpell(this Obj_AI_Base source, Obj_AI_Base target, string spellName)
        {
            if (Orbwalking.IsAutoAttack(spellName))
            {
                return new DamageSpell
                           {
                               DamageType = DamageType.Physical,
                               CalculatedDamage = GetAutoAttackDamage(source, target, true),
                           };
            }

            var hero = source as Obj_AI_Hero;
            if (hero != null)
            {
                return (from spell in hero.Spellbook.Spells
                        where String.Equals(spell.Name, spellName, StringComparison.InvariantCultureIgnoreCase)
                        select GetDamageSpell(hero, target, spell.Slot)).FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Gets the damage spell.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="slot">The slot.</param>
        /// <param name="stage">The stage.</param>
        /// <returns></returns>
        public static DamageSpell GetDamageSpell(
            this Obj_AI_Hero source,
            Obj_AI_Base target,
            SpellSlot slot,
            int stage = 0)
        {
            if (Spells.ContainsKey(source.ChampionName))
            {
                var spell = Spells[source.ChampionName].FirstOrDefault(s => s.Slot == slot && stage == s.Stage)
                            ?? Spells[source.ChampionName].FirstOrDefault(s => s.Slot == slot);

                if (spell == null)
                {
                    return null;
                }

                var rawDamage = spell.Damage(
                    source,
                    target,
                    Math.Max(0, Math.Min(source.Spellbook.GetSpell(slot).Level - 1, 5)));
                spell.CalculatedDamage = CalcDamage(source, target, spell.DamageType, rawDamage);
                return spell;
            }

            //Spell not found.
            return null;
        }

        /// <summary>
        /// Gets the spell damage.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="spellName">Name of the spell.</param>
        /// <returns></returns>
        public static double GetSpellDamage(this Obj_AI_Base source, Obj_AI_Base target, string spellName)
        {
            var spell = GetDamageSpell(source, target, spellName);
            return spell != null ? spell.CalculatedDamage : 0d;
        }

        /// <summary>
        /// Gets the spell damage.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="slot">The slot.</param>
        /// <param name="stage">The stage.</param>
        /// <returns></returns>
        public static double GetSpellDamage(this Obj_AI_Hero source, Obj_AI_Base target, SpellSlot slot, int stage = 0)
        {
            var spell = GetDamageSpell(source, target, slot, stage);
            return spell != null ? spell.CalculatedDamage : 0d;
        }

        /// <summary>
        /// Calculates the damage.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="damageType">Type of the damage.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static double CalcDamage(
            this Obj_AI_Base source,
            Obj_AI_Base target,
            DamageType damageType,
            double amount)
        {
            var damage = 0d;
            switch (damageType)
            {
                case DamageType.Magical:
                    damage = CalcMagicDamage(source, target, amount);
                    break;
                case DamageType.Physical:
                    damage = CalcPhysicalDamage(source, target, amount);
                    break;
                case DamageType.True:
                    damage = amount;
                    break;
            }

            return Math.Max(damage, 0d);
        }

        /// <summary>
        /// Calculates the magic damage.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        private static double CalcMagicDamage(Obj_AI_Base source, Obj_AI_Base target, double amount)
        {
            var magicResist = target.SpellBlock;

            // Penetration can't reduce magic resist below 0.
            double value;

            if (magicResist < 0)
            {
                value = 2 - 100 / (100 - magicResist);
            }
            else if ((magicResist * source.PercentMagicPenetrationMod) - source.FlatMagicPenetrationMod < 0)
            {
                value = 1;
            }
            else
            {
                value = 100 / (100 + (magicResist * source.PercentMagicPenetrationMod) - source.FlatMagicPenetrationMod);
            }

            var damage = DamageReductionMod(
                source,
                target,
                PassivePercentMod(source, target, value) * amount,
                DamageType.Magical);

            return damage;
        }

        /// <summary>
        /// Calculates the physical damage.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        private static double CalcPhysicalDamage(Obj_AI_Base source, Obj_AI_Base target, double amount)
        {
            double armorPenetrationPercent = source.PercentArmorPenetrationMod;
            double armorPenetrationFlat = source.FlatArmorPenetrationMod;
            double bonusArmorPenetrationMod = source.PercentBonusArmorPenetrationMod;

            // Minions return wrong percent values.
            if (source is Obj_AI_Minion)
            {
                armorPenetrationFlat = 0d;
                armorPenetrationPercent = 1d;
                bonusArmorPenetrationMod = 1d;
            }

            // Turrets too.
            if (source is Obj_AI_Turret)
            {
                armorPenetrationFlat = 0d;
                armorPenetrationPercent = 1d;
                bonusArmorPenetrationMod = 1d;
            }

            

            if (source is Obj_AI_Turret)
            {
                if (target is Obj_AI_Minion)
                {
                    amount *= 1.25;
                    if (target.CharData.BaseSkinName.EndsWith("MinionSiege"))
                    {
                        amount *= 0.7;
                    }

                    return amount;
                }
            }

            // Penetration can't reduce armor below 0.
            var armor = target.Armor;
            var bonusArmor = target.Armor - target.CharData.Armor;


            double value;
            if (armor < 0)
            {
                value = 2 - 100 / (100 - armor);
            }
            else if ((armor * armorPenetrationPercent) - (bonusArmor * (1 - bonusArmorPenetrationMod)) - armorPenetrationFlat < 0)
            {
                value = 1;
            }
            else
            {
                value = 100 / (100 + (armor * armorPenetrationPercent) - (bonusArmor * (1 - bonusArmorPenetrationMod)) - armorPenetrationFlat);
            }

            var damage = DamageReductionMod(
                source,
                target,
                PassivePercentMod(source, target, value) * amount,
                DamageType.Physical);

            // Take into account the percent passives, flat passives and damage reduction.
            return damage;
        }

        /// <summary>
        /// Gets the damage reduction modifier.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="damageType">Type of the damage.</param>
        /// <returns></returns>
        private static double DamageReductionMod(
            Obj_AI_Base source,
            Obj_AI_Base target,
            double amount,
            DamageType damageType)
        {
            if (source is Obj_AI_Hero)
            {
                // Exhaust:
                // + Exhausts target enemy champion, reducing their Movement Speed and Attack Speed by 30%, their Armor and Magic Resist by 10, and their damage dealt by 40% for 2.5 seconds.
                if (source.HasBuff("Exhaust"))
                {
                    amount *= 0.6d;
                }

                // Lament
                // + Phantom Dancer reduces all damage dealt to attacker (if he's attack you) by 12%
                if (source.HasBuff("itemphantomdancerdebuff"))
                {
                    var caster = source.GetBuff("itemphantomdancerdebuff").Caster;
                    if (caster.NetworkId == target.NetworkId)
                    {
                        amount *= 0.88d;
                    }
                }
            }

            var targetHero = target as Obj_AI_Hero;
            if (targetHero != null)
            {

                //Damage Reduction Masteries

                //DAMAGE REDUCTION 2 %, increasing to 8 % when near at least one allied champion
                //IN THIS TOGETHER 8 % of the damage that the nearest allied champion would take is dealt to you instead.This can't bring you below 15% health.
                Mastery BondofStones = targetHero.GetMastery(Resolve.BondofStones);
                if (BondofStones != null && BondofStones.IsActive())
                {
                    bool closebyenemies = HeroManager.Enemies.Any(x => x.NetworkId != target.NetworkId && x.Distance(target) <= 500); //500 is not the real value
                    if (closebyenemies)
                    {
                        amount *= 0.92d;
                    }
                    else
                    {
                        amount *= 0.98d;
                    }
                }
                

                // Items:

                // Doran's Shield
                // + Blocks 8 damage from single target attacks and spells from champions.
                if (Items.HasItem(1054, targetHero))
                {
                    amount -= 8;
                }

                // Passives:

                // Unbreakable Will
                // + Alistar removes all crowd control effects from himself, then gains additional attack damage and takes 70% reduced physical and magic damage for 7 seconds.
                if (target.HasBuff("Ferocious Howl"))
                {
                    amount *= 0.3d;
                }

                // Tantrum
                // + Amumu takes reduced physical damage from basic attacks and abilities.
                if (target.HasBuff("Tantrum") && damageType == DamageType.Physical)
                {
                    amount -= new[] { 2, 4, 6, 8, 10 }[target.Spellbook.GetSpell(SpellSlot.E).Level - 1];
                }

                // Unbreakable
                // + Grants Braum 30% / 32.5% / 35% / 37.5% / 40% damage reduction from oncoming sources (excluding true damage and towers) for 3 / 3.25 / 3.5 / 3.75 / 4 seconds.
                // + The damage reduction is increased to 100% for the first source of champion damage that would be reduced.
                if (target.HasBuff("BraumShieldRaise"))
                {
                    amount -= amount
                              * new[] { 0.3d, 0.325d, 0.35d, 0.375d, 0.4d }[
                                  target.Spellbook.GetSpell(SpellSlot.E).Level - 1];
                }

                // Idol of Durand
                // + Galio becomes a statue and channels for 2 seconds, Taunt icon taunting nearby foes and reducing incoming physical and magic damage by 50%.
                if (target.HasBuff("GalioIdolOfDurand"))
                {
                    amount *= 0.5d;
                }

                // Courage
                // + Garen gains a defensive shield for a few seconds, reducing incoming damage by 30% and granting 30% crowd control reduction for the duration.
                if (target.HasBuff("GarenW"))
                {
                    amount *= 0.7d;
                }

                // Drunken Rage
                // + Gragas takes a long swig from his barrel, disabling his ability to cast or attack for 1 second and then receives 10% / 12% / 14% / 16% / 18% reduced damage for 3 seconds.
                if (target.HasBuff("GragasWSelf"))
                {
                    amount -= amount
                              * new[] { 0.1d, 0.12d, 0.14d, 0.16d, 0.18d }[
                                  target.Spellbook.GetSpell(SpellSlot.W).Level - 1];
                }

                // Void Stone
                // + Kassadin reduces all magic damage taken by 15%.
                if (target.HasBuff("VoidStone") && damageType == DamageType.Magical)
                {
                    amount *= 0.85d;
                }

                // Shunpo
                // + Katarina teleports to target unit and gains 15% damage reduction for 1.5 seconds. If the target is an enemy, the target takes magic damage.
                if (target.HasBuff("KatarinaEReduction"))
                {
                    amount *= 0.85d;
                }

                // Vengeful Maelstrom
                // + Maokai creates a magical vortex around himself, protecting him and allied champions by reducing damage from non-turret sources by 20% for a maximum of 10 seconds.
                if (target.HasBuff("MaokaiDrainDefense") && !(source is Obj_AI_Turret))
                {
                    amount *= 0.8d;
                }

                // Meditate
                // + Master Yi channels for up to 4 seconds, restoring health each second. This healing is increased by 1% for every 1% of his missing health. Meditate also resets the autoattack timer.
                // + While channeling, Master Yi reduces incoming damage (halved against turrets).
                if (target.HasBuff("Meditate"))
                {
                    amount -= amount
                              * new[] { 0.5d, 0.55d, 0.6d, 0.65d, 0.7d }[
                                  target.Spellbook.GetSpell(SpellSlot.W).Level - 1] / (source is Obj_AI_Turret ? 2 : 1);
                }

                // Shadow Dash
                // + Shen reduces all physical damage by 50% from taunted enemies.
                if (target.HasBuff("Shen Shadow Dash") && source.HasBuff("Taunt") && damageType == DamageType.Physical)
                {
                    amount *= 0.5d;
                }
            }
            return amount;
        }

        /// <summary>
        /// Gets the passive percent modifier.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        private static double PassivePercentMod(Obj_AI_Base source, Obj_AI_Base target, double amount)
        {
            var SiegeMinionList = new List<string> { "Red_Minion_MechCannon", "Blue_Minion_MechCannon" };
            var NormalMinionList = new List<string>
                                       {
                                           "Red_Minion_Wizard", "Blue_Minion_Wizard", "Red_Minion_Basic",
                                           "Blue_Minion_Basic"
                                       };

            //Minions and towers passives:
            if (source is Obj_AI_Turret)
            {
                //Siege minions receive 70% damage from turrets
                if (SiegeMinionList.Contains(target.CharData.BaseSkinName))
                {
                    amount *= 0.7d;
                }

                //Normal minions take 114% more damage from towers.
                else if (NormalMinionList.Contains(target.CharData.BaseSkinName))
                {
                    amount *= 1.14285714285714d;
                }
            }

            // Masteries:
            var hero = source as Obj_AI_Hero;
            var targetHero = target as Obj_AI_Hero;
            if (hero != null)
            {
                // Offensive masteries:

                //INCREASED DAMAGE FROM ABILITIES 0.4/0.8/1.2/1.6/2%
                /*
                Mastery sorcery = hero.GetMastery(Ferocity.Sorcery);
                if (sorcery != null && sorcery.IsActive())
                {
                    amount *= 1 + ((new double[] { 0.4, 0.8, 1.2, 1.6, 2.0 }[sorcery.Points]) / 100);
                } /*

                //MELEE Deal an additional 3 % damage, but receive an additional 1.5 % damage
                //RANGED Deal an additional 2 % damage, but receive an additional 2 % damage
                Mastery DoubleEdgedSword = hero.GetMastery(Ferocity.DoubleEdgedSword);
                if (DoubleEdgedSword != null && DoubleEdgedSword.IsActive())
                {
                    amount *= hero.IsMelee() ? 1.03 : 1.02;
                }

                /* Bounty Hunter: TAKING NAMES You gain a permanent 1 % damage increase for each unique enemy champion you kill
                Mastery BountyHunter = hero.GetMastery(Ferocity.BountyHunter);
                if (BountyHunter != null && BountyHunter.IsActive())
                {
                    //We need a hero.UniqueChampionsKilled or both the sender and the target for ChampionKilled OnNotify Event
                    // amount += amount * Math.Min(hero.ChampionsKilled, 5);
                } */

                //Opressor: KICK 'EM WHEN THEY'RE DOWN You deal 2.5% increased damage to targets with impaired movement (slows, stuns, taunts, etc)
                Mastery Opressor = hero.GetMastery(Ferocity.Oppresor);
                if (targetHero != null && Opressor != null && Opressor.IsActive() && targetHero.IsMovementImpaired())
                {
                    amount *= 1.025;
                }

                //Merciless DAMAGE AMPLIFICATION 1 / 2 / 3 / 4 / 5 % increased damage to champions below 40 % health
                if (targetHero != null)
                {
                    Mastery Merciless = hero.GetMastery(Cunning.Merciless);
                    if (Merciless != null && Merciless.IsActive() && targetHero.HealthPercent < 40)
                    {
                        amount *= 1 + Merciless.Points / 100f;
                    }
                }

                //Thunderlord's Decree: RIDE THE LIGHTNING Your 3rd ability or basic attack on an enemy champion shocks them, dealing 10 - 180(+0.2 bonus attack damage)(+0.1 ability power) magic damage in an area around them
                if (false) // Need a good way to check if it is 3rd attack (Use OnProcessSpell/SpellBook.OnCast if have to)
                {
                    Mastery Thunder = hero.GetMastery(Cunning.ThunderlordsDecree);
                    if (Thunder != null && Thunder.IsActive())
                    {
                        // amount += 10 * hero.Level + (0.2 * hero.FlatPhysicalDamageMod) + (0.1 * hero.TotalMagicalDamage);
                    }
                }
            }

            if (targetHero != null)
            {
                // Defensive masteries:

                // Double edge sword:
                //MELEE Deal an additional 3 % damage, but receive an additional 1.5 % damage
                //RANGED Deal an additional 2 % damage, but receive an additional 2 % damage
                var des = targetHero.GetMastery(Ferocity.DoubleEdgedSword);
                if (des != null && des.IsActive())
                {
                    amount *= targetHero.IsMelee() ? 1.015d : 1.02d;
                }
            }

            return amount;
        }

        /// <summary>
        /// Gets the passive flat modifier.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        private static double PassiveFlatMod(Obj_AI_Base source, Obj_AI_Base target)
        {
            var value = 0d;
            var hero = source as Obj_AI_Hero;
            var targetHero = target as Obj_AI_Hero;
            // Offensive masteries:

            //Fervor of Battle: STACKTIVATE Your basic attacks and spells give you stacks of Fervor for 5 seconds, stacking 10 times. Each stack of Fervor adds 1-8 bonus physical damage to your basic attacks against champions, based on your level.
            if (targetHero != null && hero != null)
            {
                Mastery Fervor = hero.GetMastery(Ferocity.FervorofBattle);
                if (Fervor != null && Fervor.IsActive())
                {
                    value += (0.9 + hero.Level * 0.42) * hero.GetBuffCount("MasteryOnHitDamageStacker");
                }
            }

            // Defensive masteries:

            //Tough Skin DIRT OFF YOUR SHOULDERS You take 2 less damage from champion and monster basic attacks
            if (targetHero != null && (source is Obj_AI_Hero || source is Obj_AI_Minion))
            {
                Mastery Toughskin = targetHero.GetMastery(Resolve.ToughSkin);
                if (Toughskin != null && Toughskin.IsActive())
                {
                    value -= 2;
                }
            }

            return value;
        }
 
        /// <summary>
        /// Represents a damage spell that only occurs with a passive.
        /// </summary>
        internal class PassiveDamage
        {
            /// <summary>
            /// Gets the damage dealts to the unit.
            /// </summary>
            /// <param name="source">The source.</param>
            /// <param name="target">The target.</param>
            /// <returns></returns>
            public delegate float GetDamageD(Obj_AI_Hero source, Obj_AI_Base target);

            /// <summary>
            /// Gets whether this instance is active.
            /// </summary>
            /// <param name="source">The source.</param>
            /// <param name="target">The target.</param>
            /// <returns></returns>
            public delegate bool IsActiveD(Obj_AI_Hero source, Obj_AI_Base target);

            /// <summary>
            /// The champion name
            /// </summary>
            public string ChampionName = "";

            /// <summary>
            /// The get damage delegate.
            /// </summary>
            public GetDamageD GetDamage;

            /// <summary>
            /// The is active delegate.
            /// </summary>
            public IsActiveD IsActive;
        }
    }
}
