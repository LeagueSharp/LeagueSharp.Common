// <copyright file="Damage.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

    using LeagueSharp.Common.Data;

    /// <summary>
    ///     Damage calculations and data.
    /// </summary>
    [Export(typeof(Damage))]
    public partial class Damage
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Calculates the damage.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <param name="damageType">
        ///     The damage type.
        /// </param>
        /// <param name="amount">
        ///     The amount.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        public double CalculateDamage(Obj_AI_Base source, Obj_AI_Base target, DamageType damageType, double amount)
        {
            var damage = amount;

            switch (damageType)
            {
                case DamageType.Magical:
                    damage = this.CalculateMagicDamage(source, target, amount);
                    break;
                case DamageType.Physical:
                    damage = this.CalculatePhysicalDamage(source, target, amount);
                    break;
            }

            return Math.Max(damage, 0d);
        }

        /// <summary>
        ///     Gets the auto attack damage.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <param name="includePassive">
        ///     A value indicating whether to include the passive.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        public double GetAutoAttackDamage(Obj_AI_Base source, Obj_AI_Base target, bool includePassive = false)
        {
            double result = source.TotalAttackDamage;
            var k = 1d;
            if (source.CharData.BaseSkinName == "Kalista")
            {
                k = 0.9d;
            }
            if (source.CharData.BaseSkinName == "Kled"
                && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "KledRiderQ")
            {
                k = 0.8d;
            }

            if (!includePassive)
            {
                return this.CalculatePhysicalDamage(source, target, result * k);
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

                // Champions passive damages:
                result += (from passiveDamage in this.PassiveLazies
                           where
                           passiveDamage.Metadata.ChampionName == string.Empty
                           || passiveDamage.Metadata.ChampionName == hero.ChampionName
                           where passiveDamage.Value.IsActive(hero, target)
                           select passiveDamage.Value.GetDamage(hero, target)).Sum();

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
                    var f = new int[] { 4, 6, 8, 10, 12, 14 };
                    reduction += f[(targetHero.Level - 1) / 3];
                }
            }

            //TODO: need to check if there are items or spells in game that reduce magical dmg % or by amount
            if (hero != null && hero.ChampionName == "Corki")
            {
                return this.CalculateMixedDamage(source, target, (result - reduction) * k, result * k);
            }

            return this.CalculatePhysicalDamage(
                source,
                target,
                (result - reduction) * k + PassiveFlatMod(source, target));
        }

        /// <summary>
        ///     Calculates the combo damage.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <param name="spellCombo">
        ///     The spell combo.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        public double GetComboDamage(Obj_AI_Hero source, Obj_AI_Base target, IEnumerable<SpellSlot> spellCombo)
            => this.GetComboDamage(source, target, spellCombo.Select(s => Tuple.Create(s, 0)).ToArray());

        /// <summary>
        ///     Calculates the combo damage.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <param name="spellCombo">
        ///     The spell combo.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        public double GetComboDamage(
                Obj_AI_Hero source,
                Obj_AI_Base target,
                IEnumerable<Tuple<SpellSlot, int>> spellCombo)
            => spellCombo.Sum(s => this.GetSpellDamage(source, target, s.Item1, s.Item2));

        /// <summary>
        ///     Calculates the spell damage.
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
        ///     The <see cref="double" />.
        /// </returns>
        public double GetSpellDamage(Obj_AI_Base source, Obj_AI_Base target, string spellName)
            => this.GetDamageSpell(source, target, spellName)?.CalculatedDamage ?? 0d;

        /// <summary>
        ///     Calculates the spell damage.
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
        ///     The <see cref="double" />.
        /// </returns>
        public double GetSpellDamage(Obj_AI_Hero source, Obj_AI_Base target, SpellSlot slot, int stage = 0)
            => this.GetDamageSpell(source, target, slot, stage)?.CalculatedDamage ?? 0d;

        /// <summary>
        ///     Determines if the target is killable by source.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <param name="spellCombo">
        ///     The spell combo.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool IsKillable(Obj_AI_Hero source, Obj_AI_Base target, IEnumerable<Tuple<SpellSlot, int>> spellCombo)
            => this.GetComboDamage(source, target, spellCombo) >= target.Health;

        #endregion

        #region Methods

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
                var BondofStones = targetHero.GetMastery(MasteryData.Resolve.BondofStones);
                if (BondofStones != null && BondofStones.IsActive())
                {
                    var closebyenemies =
                        HeroManager.Enemies.Any(x => x.NetworkId != target.NetworkId && x.Distance(target) <= 500);
                    //500 is not the real value
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

        private static double PassiveFlatMod(Obj_AI_Base source, Obj_AI_Base target)
        {
            var value = 0d;
            var hero = source as Obj_AI_Hero;
            var targetHero = target as Obj_AI_Hero;
            // Offensive masteries:

            //Fervor of Battle: STACKTIVATE Your basic attacks and spells give you stacks of Fervor for 5 seconds, stacking 10 times. Each stack of Fervor adds 1-8 bonus physical damage to your basic attacks against champions, based on your level.
            if (targetHero != null && hero != null)
            {
                var Fervor = hero.GetMastery(MasteryData.Ferocity.FervorofBattle);
                if (Fervor != null && Fervor.IsActive())
                {
                    value += (0.9 + hero.Level * 0.42) * hero.GetBuffCount("MasteryOnHitDamageStacker");
                }
            }

            // Defensive masteries:

            //Tough Skin DIRT OFF YOUR SHOULDERS You take 2 less damage from champion and monster basic attacks
            if (targetHero != null && (source is Obj_AI_Hero || source is Obj_AI_Minion))
            {
                var Toughskin = targetHero.GetMastery(MasteryData.Resolve.ToughSkin);
                if (Toughskin != null && Toughskin.IsActive())
                {
                    value -= 2;
                }
            }

            return value;
        }

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
                var Opressor = hero.GetMastery(MasteryData.Ferocity.Oppresor);
                if (targetHero != null && Opressor != null && Opressor.IsActive() && targetHero.IsMovementImpaired())
                {
                    amount *= 1.025;
                }

                //Merciless DAMAGE AMPLIFICATION 1 / 2 / 3 / 4 / 5 % increased damage to champions below 40 % health
                if (targetHero != null)
                {
                    var Merciless = hero.GetMastery(MasteryData.Cunning.Merciless);
                    if (Merciless != null && Merciless.IsActive() && targetHero.HealthPercent < 40)
                    {
                        amount *= 1 + Merciless.Points / 100f;
                    }
                }

                //Thunderlord's Decree: RIDE THE LIGHTNING Your 3rd ability or basic attack on an enemy champion shocks them, dealing 10 - 180(+0.2 bonus attack damage)(+0.1 ability power) magic damage in an area around them
                if (false)
                    // Need a good way to check if it is 3rd attack (Use OnProcessSpell/SpellBook.OnCast if have to)
                {
                    var Thunder = hero.GetMastery(MasteryData.Cunning.ThunderlordsDecree);
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
                var des = targetHero.GetMastery(MasteryData.Ferocity.DoubleEdgedSword);
                if (des != null && des.IsActive())
                {
                    amount *= targetHero.IsMelee() ? 1.015d : 1.02d;
                }
            }

            return amount;
        }

        private double CalculateMagicDamage(Obj_AI_Base source, Obj_AI_Base target, double amount)
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

        private double CalculateMixedDamage(
            Obj_AI_Base source,
            Obj_AI_Base target,
            double amountPhysical,
            double amountMagic,
            int magic = 50,
            int physical = 50,
            int trueDmg = 0)
        {
            return this.CalculateMagicDamage(source, target, (amountMagic * magic) / 100)
                   + this.CalculatePhysicalDamage(source, target, (amountPhysical * physical) / 100)
                   + PassiveFlatMod(source, target) + (amountMagic * trueDmg) / 100;
        }

        private double CalculatePhysicalDamage(Obj_AI_Base source, Obj_AI_Base target, double amount)
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
            else if ((armor * armorPenetrationPercent) - (bonusArmor * (1 - bonusArmorPenetrationMod)) - armorPenetrationFlat
                     < 0)
            {
                value = 1;
            }
            else
            {
                value = 100
                        / (100 + (armor * armorPenetrationPercent) - (bonusArmor * (1 - bonusArmorPenetrationMod))
                           - armorPenetrationFlat);
            }

            var damage = DamageReductionMod(
                source,
                target,
                PassivePercentMod(source, target, value) * amount,
                DamageType.Physical);

            // Take into account the percent passives, flat passives and damage reduction.
            return damage;
        }

        #endregion
    }
}