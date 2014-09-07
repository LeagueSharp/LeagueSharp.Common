#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Orbwalking.cs is part of LeagueSharp.Common.
 
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

/*
 * Library: GenericDamageLib
 * Author: Vis, iMeh
 * Date: 21.04.2014
 * Version: 3 / 10.08.2014
 * Kludger: Crisdmc
 */

#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace LeagueSharp.Common
{
    public static class GenericDamageLib
    {
        public enum SpellType
        {
            Q,
            W,
            E,
            R,
            AD,
            IGNITE,
            HEXGUN,
            DFG,
            BOTRK,
            BILGEWATER,
            TIAMAT,
            HYDRA,
            BLACKFIRETORCH,
            ODINVEILS,
            FROSTQUEENSCLAIM,
        }

        public enum StageType
        {
            FirstDamage,
            SecondDamage,
            ThirdDamage,
            FourthDamage,
            Default,
        }

        private static readonly List<Champion> champList = new List<Champion>();
        private static ChampDamage Champ;

        static GenericDamageLib()
        {
            if (Common.isInitialized == false)
            {
                Common.InitializeCommonLib();
            }

            // Get champs masteries
            foreach (var hero in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (hero != null && hero.IsValid)
                {
                    var unyielding = 0;
                    var block = 0;

                    // offsensive masteries
                    var doubleedgedsword = false;
                    var havoc = false;
                    var executioner = 0;
                    var arcaneblade = false;
                    var butcher = false;

                    foreach (var mastery in hero.Masteries)
                    {
                        if (mastery.Page == MasteryPage.Offense)
                        {
                            switch (mastery.Id)
                            {
                                case 65:
                                    doubleedgedsword = (mastery.Points == 1);
                                    break;
                                case 146:
                                    havoc = (mastery.Points == 1);
                                    break;
                                case 132:
                                    arcaneblade = (mastery.Points == 1);
                                    break;
                                case 100:
                                    executioner = mastery.Points;
                                    break;
                                case 68:
                                    butcher = (mastery.Points == 1);
                                    break;
                            }
                        }
                        else if (mastery.Page == MasteryPage.Defense)
                        {
                            switch (mastery.Id)
                            {
                                case 65:
                                    block = mastery.Points;
                                    break;

                                case 81:
                                    if (mastery.Points == 1)
                                    {
                                        unyielding = (hero.CombatType == GameObjectCombatType.Melee) ? 2 : 1;
                                    }
                                    break;
                            }
                        }
                    }

                    champList.Add(new Champion
                    {
                        NetworkId = hero.NetworkId,
                        block = block,
                        unyielding = unyielding,
                        doubleedgedsword = doubleedgedsword,
                        havoc = havoc,
                        executioner = executioner,
                        arcaneblade = arcaneblade,
                        butcher = butcher
                    });
                }
            }
        }

        public static void obtainChamp(Obj_AI_Hero attacker)
        {
            // Get Hero
            switch (attacker.ChampionName)
            {
                case "Aatrox":
                    Champ = Aatrox;
                    break;
                case "Ahri":
                    Champ = Ahri;
                    break;
                case "Akali":
                    Champ = Akali;
                    break;
                case "Alistar":
                    Champ = Alistar;
                    break;
                case "Amumu":
                    Champ = Amumu;
                    break;
                case "Anivia":
                    Champ = Anivia;
                    break;
                case "Annie":
                    Champ = Annie;
                    break;
                case "Ashe":
                    Champ = Ashe;
                    break;
                case "Blitzcrank":
                    Champ = Blitzcrank;
                    break;
                case "Brand":
                    Champ = Brand;
                    break;
                case "Braum":
                    Champ = Braum;
                    break;
                case "Caitlyn":
                    Champ = Caitlyn;
                    break;
                case "Cassiopeia":
                    Champ = Cassiopeia;
                    break;
                case "Chogath":
                    Champ = ChoGath;
                    break;
                case "Corki":
                    Champ = Corki;
                    break;
                case "Darius":
                    Champ = Darius;
                    break;
                case "Diana":
                    Champ = Diana;
                    break;
                case "Draven":
                    Champ = Draven;
                    break;
                case "DrMundo":
                    Champ = DrMundo;
                    break;
                case "Elise":
                    Champ = Elise;
                    break;
                case "Evelynn":
                    Champ = Evelynn;
                    break;
                case "Ezreal":
                    Champ = Ezreal;
                    break;
                case "Fiddlesticks":
                    Champ = Fiddlesticks;
                    break;
                case "Fiora":
                    Champ = Fiora;
                    break;
                case "Fizz":
                    Champ = Fizz;
                    break;
                case "Galio":
                    Champ = Galio;
                    break;
                case "Gangplank":
                    Champ = Gangplank;
                    break;
                case "Garen":
                    Champ = Garen;
                    break;
                case "Gnar":
                    Champ = Gnar;
                    break;
                case "Gragas":
                    Champ = Gragas;
                    break;
                case "Graves":
                    Champ = Graves;
                    break;
                case "Hecarim":
                    Champ = Hecarim;
                    break;
                case "Heimerdinger":
                    Champ = Heimerdinger;
                    break;
                case "Irelia":
                    Champ = Irelia;
                    break;
                case "Janna":
                    Champ = Janna;
                    break;
                case "JarvanIV":
                    Champ = JarvanIV;
                    break;
                case "Jax":
                    Champ = Jax;
                    break;
                case "Jayce":
                    Champ = Jayce;
                    break;
                case "Jinx":
                    Champ = Jinx;
                    break;
                case "Karma":
                    Champ = Karma;
                    break;
                case "Karthus":
                    Champ = Karthus;
                    break;
                case "Kassadin":
                    Champ = Kassadin;
                    break;
                case "Katarina":
                    Champ = Katarina;
                    break;
                case "Kayle":
                    Champ = Kayle;
                    break;
                case "Kennen":
                    Champ = Kennen;
                    break;
                case "Khazix":
                    Champ = Khazix;
                    break;
                case "KogMaw":
                    Champ = KogMaw;
                    break;
                case "Leblanc":
                    Champ = LeBlanc;
                    break;
                case "LeeSin":
                    Champ = LeeSin;
                    break;
                case "Leona":
                    Champ = Leona;
                    break;
                case "Lissandra":
                    Champ = Lissandra;
                    break;
                case "Lucian":
                    Champ = Lucian;
                    break;
                case "Lulu":
                    Champ = Lulu;
                    break;
                case "Lux":
                    Champ = Lux;
                    break;
                case "Malphite":
                    Champ = Malphite;
                    break;
                case "Malzahar":
                    Champ = Malzahar;
                    break;
                case "Maokai":
                    Champ = Maokai;
                    break;
                case "MasterYi":
                    Champ = MasterYi;
                    break;
                case "MissFortune":
                    Champ = MissFortune;
                    break;
                case "Mordekaiser":
                    Champ = Mordekaiser;
                    break;
                case "Morgana":
                    Champ = Morgana;
                    break;
                case "Nami":
                    Champ = Nami;
                    break;
                case "Nasus":
                    Champ = Nasus;
                    break;
                case "Nautilus":
                    Champ = Nautilus;
                    break;
                case "Nidalee":
                    Champ = Nidalee;
                    break;
                case "Nocturne":
                    Champ = Nocturne;
                    break;
                case "Nunu":
                    Champ = Nunu;
                    break;
                case "Olaf":
                    Champ = Olaf;
                    break;
                case "oriannanoball":
                    Champ = Orianna;
                    break;
                case "Orianna":
                    Champ = Orianna;
                    break;
                case "Pantheon":
                    Champ = Pantheon;
                    break;
                case "Poppy":
                    Champ = Poppy;
                    break;
                case "Quinn":
                    Champ = Quinn;
                    break;
                case "Rammus":
                    Champ = Rammus;
                    break;
                case "Renekton":
                    Champ = Renekton;
                    break;
                case "Rengar":
                    Champ = Rengar;
                    break;
                case "Riven":
                    Champ = Riven;
                    break;
                case "Rumble":
                    Champ = Rumble;
                    break;
                case "Ryze":
                    Champ = Ryze;
                    break;
                case "Sejuani":
                    Champ = Sejuani;
                    break;
                case "Shaco":
                    Champ = Shaco;
                    break;
                case "Shen":
                    Champ = Shen;
                    break;
                case "Shyvanna":
                    Champ = Shyvana;
                    break;
                case "Singed":
                    Champ = Singed;
                    break;
                case "Sion":
                    Champ = Sion;
                    break;
                case "Sivir":
                    Champ = Sivir;
                    break;
                case "Skarner":
                    Champ = Skarner;
                    break;
                case "Sona":
                    Champ = Sona;
                    break;
                case "Soraka":
                    Champ = Soraka;
                    break;
                case "Swain":
                    Champ = Swain;
                    break;
                case "Syndra":
                    Champ = Syndra;
                    break;
                case "Talon":
                    Champ = Talon;
                    break;
                case "Taric":
                    Champ = Taric;
                    break;
                case "Teemo":
                    Champ = Teemo;
                    break;
                case "Thresh":
                    Champ = Thresh;
                    break;
                case "Tristana":
                    Champ = Tristana;
                    break;
                case "Trundle":
                    Champ = Trundle;
                    break;
                case "Tryndamere":
                    Champ = Tryndamere;
                    break;
                case "TwistedFate":
                    Champ = TwistedFate;
                    break;
                case "Twitch":
                    Champ = Twitch;
                    break;
                case "Udyr":
                    Champ = Udyr;
                    break;
                case "Urgot":
                    Champ = Urgot;
                    break;
                case "Varus":
                    Champ = Varus;
                    break;
                case "Vayne":
                    Champ = Vayne;
                    break;
                case "Veigar":
                    Champ = Veigar;
                    break;
                case "Velkoz":
                    Champ = Velkoz;
                    break;
                case "Vi":
                    Champ = Vi;
                    break;
                case "Viktor":
                    Champ = Viktor;
                    break;
                case "Vladimir":
                    Champ = Vladimir;
                    break;
                case "Volibear":
                    Champ = Volibear;
                    break;
                case "Warwick":
                    Champ = Warwick;
                    break;
                case "MonkeyKing":
                    Champ = MonkeyKing;
                    break;
                case "Xerath":
                    Champ = Xerath;
                    break;
                case "XinZhao":
                    Champ = XinZhao;
                    break;
                case "Yasuo":
                    Champ = Yasuo;
                    break;
                case "Yorick":
                    Champ = Yorick;
                    break;
                case "Zac":
                    Champ = Zac;
                    break;
                case "Zed":
                    Champ = Zed;
                    break;
                case "Ziggs":
                    Champ = Ziggs;
                    break;
                case "Zilean":
                    Champ = Zilean;
                    break;
                case "Zyra":
                    Champ = Zyra;
                    break;
                default:
                    Console.WriteLine(
                        "GenericDamageLib: Could not find the champion '" + attacker.ChampionName +
                        "'. Please report this in the forums/IRC (@LeagueSharp), it's either a wrong typed name or a new hero that needs to be added!");
                    break;
            }
        }

        private static bool IsBetween<T>(this T item, T start, T end)
        {
            return Comparer<T>.Default.Compare(item, start) >= 0 && Comparer<T>.Default.Compare(item, end) <= 0;
        }

        public static double CalcPhysicalMinionDmg(Obj_AI_Hero attacker, double dmg, Obj_AI_Minion minion, bool isAA)
        {
            double additionaldmg = 0;
            double bonusmagicdmg = 0;

            Champion champ = champList.First<Champion>(chmp => chmp.NetworkId == attacker.NetworkId);

            if (champ.doubleedgedsword)
            {
                if (attacker.CombatType == GameObjectCombatType.Melee)
                {
                    additionaldmg += dmg * 0.02;
                }
                else
                {
                    additionaldmg += dmg * 0.015;
                }
            }
            if (champ.havoc)
            {
                additionaldmg += dmg * 0.03;
            }

            if (champ.butcher)
            {
                additionaldmg += 2;
            }

            if (champ.arcaneblade)
            {
                bonusmagicdmg = CalcMagicDmg(attacker, 0.05 * attacker.FlatMagicDamageMod, minion);
            }

            if (isAA)
            {
                foreach (var slot in attacker.InventoryItems)
                {
                    if ((int)slot.Id == 3153) // BOTRK
                    {
                        var tmpdmg = minion.Health * 0.05;
                        if (tmpdmg >= 60)
                        {
                            tmpdmg = 60;
                        }
                        additionaldmg += tmpdmg;
                    }
                }
            }

            double newarmor = minion.Armor * attacker.PercentArmorPenetrationMod;
            var dmgreduction = 100 / (100 + newarmor - attacker.FlatArmorPenetrationMod);
            return (((dmg + additionaldmg) * dmgreduction)) + bonusmagicdmg;
        }

        public static double CalcMagicMinionDmg(Obj_AI_Hero attacker, double dmg, Obj_AI_Minion minion, bool isSingleTargetSpell)
        {
            double additionaldmg = 0;

            Champion champ = champList.First<Champion>(chmp => chmp.NetworkId == attacker.NetworkId);

            if (champ.doubleedgedsword)
            {
                if (attacker.CombatType == GameObjectCombatType.Melee)
                {
                    additionaldmg += dmg * 0.02;
                }
                else
                {
                    additionaldmg += dmg * 0.015;
                }
            }
            if (champ.havoc)
            {
                additionaldmg += dmg * 0.03;
            }
            if (champ.butcher && isSingleTargetSpell) // AA or single target spell deals +2
            {
                additionaldmg += 2;
            }

            double newmr = minion.SpellBlock * attacker.PercentMagicPenetrationMod;
            var dmgreduction = 100 / (100 + newmr - attacker.FlatMagicPenetrationMod);
            return (((dmg + additionaldmg) * dmgreduction));
        }

        public static double CalcObjectToObjectDmg(Obj_AI_Base attackminion,
            Obj_AI_Base shotminion,
            double extraDamage = 0)
        {
            double armorPenPercent = attackminion.PercentArmorPenetrationMod;
            double armorPen = attackminion.FlatArmorPenetrationMod;

            if (attackminion is Obj_AI_Minion)
            {
                armorPen = 0;
                armorPenPercent = 1;
            }

            if (attackminion is Obj_AI_Turret)
            {
                armorPen = 0;
                armorPenPercent = 0.7f;
            }

            var newarmor = shotminion.Armor * armorPenPercent;
            var dmgreduction = 100 / (100 + Math.Max(newarmor - armorPen, 0));

            if ((attackminion is Obj_AI_Turret) &&
                (shotminion.BaseSkinName == "Red_Minion_MechCannon" ||
                 shotminion.BaseSkinName == "Blue_Minion_MechCannon"))
            {
                dmgreduction = 0.8 * dmgreduction;
            }

            if (attackminion is Obj_AI_Turret &&
                (shotminion.BaseSkinName == "Red_Minion_Wizard" || shotminion.BaseSkinName == "Blue_Minion_Wizard" ||
                 shotminion.BaseSkinName == "Red_Minion_Basic" || shotminion.BaseSkinName == "Blue_Minion_Basic"))
            {
                dmgreduction = (1 / 0.875) * dmgreduction;
            }

            if (attackminion is Obj_AI_Turret)
            {
                dmgreduction = 1.05 * dmgreduction;
            }

            return (((attackminion.BaseAttackDamage + attackminion.FlatPhysicalDamageMod + extraDamage) * dmgreduction));
        }

        /// <summary>
        /// Calculates the damage into the physical damage using Armor, Armorpenetration and Masteries
        /// </summary>
        /// <param name="attacker">The attacker object</param>
        /// <param name="dmg">The basic damage</param>
        /// <param name="target">The target object</param>
        /// <returns>Returns the physical damage</returns>
        public static double CalcPhysicalDmg(Obj_AI_Hero attacker, double dmg, Obj_AI_Base target)
        {
            double additionaldmg = 0;

            Champion champ = champList.First<Champion>(chmp => chmp.NetworkId == attacker.NetworkId);

            if (champ.doubleedgedsword)
            {
                if (attacker.CombatType == GameObjectCombatType.Melee)
                {
                    additionaldmg += dmg * 0.02;
                }
                else
                {
                    additionaldmg += dmg * 0.015;
                }
            }

            if (champ.havoc)
            {
                additionaldmg += dmg * 0.03;
            }

            if (champ.executioner > 0)
            {
                if (champ.executioner == 1)
                {
                    if ((target.Health / target.MaxHealth) * 100 < 20)
                    {
                        additionaldmg += dmg * 0.05;
                    }
                }
                else if (champ.executioner == 2)
                {
                    if ((target.Health / target.MaxHealth) * 100 < 35)
                    {
                        additionaldmg += dmg * 0.05;
                    }
                }
                else if (champ.executioner == 3)
                {
                    if ((target.Health / target.MaxHealth) * 100 < 50)
                    {
                        additionaldmg += dmg * 0.05;
                    }
                }
            }

            var reduceDmg = 0;

            if (target is Obj_AI_Hero)
            {
                var currenttarget = champList.FirstOrDefault(e => e.NetworkId == target.NetworkId);
                if (currenttarget != null)
                {
                    reduceDmg = currenttarget.unyielding;
                }
            }

            double newArmor = target.Armor * attacker.PercentArmorPenetrationMod;
            var dmgReduction = 100 / (100 + newArmor - attacker.FlatArmorPenetrationMod);
            return (((dmg + additionaldmg) * dmgReduction)) - reduceDmg;
        }

        /// <summary>
        /// Calculates the damage into the magic damage using MR, Magicpenetration and Masteries
        /// </summary>
        /// <param name="attacker">The attacker object</param>
        /// <param name="dmg">The basic damage</param>
        /// <param name="target">The target object</param>
        /// <returns>Returns the magic damage</returns>
        public static double CalcMagicDmg(Obj_AI_Hero attacker, double dmg, Obj_AI_Base target)
        {
            double additionaldmg = 0;

            Champion champ = champList.First<Champion>(chmp => chmp.NetworkId == attacker.NetworkId);

            if (champ.doubleedgedsword)
            {
                if (attacker.CombatType == GameObjectCombatType.Melee)
                {
                    additionaldmg = dmg * 0.02;
                }
                else
                {
                    additionaldmg = dmg * 0.015;
                }
            }
            if (champ.havoc)
            {
                additionaldmg += dmg * 0.03;
            }
            if (champ.executioner > 0)
            {
                if (champ.executioner == 1)
                {
                    if ((target.Health / target.MaxHealth) * 100 < 20)
                    {
                        additionaldmg += dmg * 0.05;
                    }
                }
                else if (champ.executioner == 2)
                {
                    if ((target.Health / target.MaxHealth) * 100 < 35)
                    {
                        additionaldmg += dmg * 0.05;
                    }
                }
                else if (champ.executioner == 3)
                {
                    if ((target.Health / target.MaxHealth) * 100 < 50)
                    {
                        additionaldmg += dmg * 0.05;
                    }
                }
            }

            var reducedmg = 0;

            if (target is Obj_AI_Hero)
            {
                var currentenemy = champList.FirstOrDefault(e => e.NetworkId == target.NetworkId);
                if (currentenemy != null)
                {
                    reducedmg = currentenemy.unyielding;
                }
            }

            double newspellblock = target.SpellBlock * attacker.PercentMagicPenetrationMod;
            var dmgreduction = 100 / (100 + newspellblock - attacker.FlatMagicPenetrationMod);
            return (((dmg + additionaldmg) * dmgreduction)) - reducedmg;
        }

        /// <summary>
        /// Calculates the damage of a Spell, Auto Attack or Item for a champion.
        /// </summary>
        /// <param name="attacker">The attacker object</param>
        /// <param name="target">The target object</param>
        /// <param name="type">The type of Spell</param>
        /// <param name="stagetype">The stage of the Spell</param>
        /// <returns>Returns the physical/magic damage</returns>
        public static double getDmg(Obj_AI_Hero attacker, Obj_AI_Base target, SpellType type, StageType stagetype = StageType.Default)
        {
            obtainChamp(attacker);
            Champion champ = champList.First<Champion>(chmp => chmp.NetworkId == attacker.NetworkId);

            switch (type)
            {
                case SpellType.AD:
                    if (champ.arcaneblade == false)
                    {
                        var dmgreduce = 0;
                        if (target is Obj_AI_Hero)
                        {
                            var currentenemy = champList.FirstOrDefault(e => e.NetworkId == target.NetworkId);
                            if (currentenemy != null)
                            {
                                dmgreduce = currentenemy.block;
                            }
                        }

                        var reduce2 = 0;
                        double multiplier = 1;
                        double plusdmg = 0;
                        if (target is Obj_AI_Hero)
                        {
                            foreach (var inv in target.InventoryItems)
                            {
                                if ((int)inv.Id == 1054) // Dorans Shield -> Basic attacks -8 dmg
                                {
                                    reduce2 += 8;
                                }
                                if ((int)inv.Id == 3047) // Ninja Tabi
                                {
                                    multiplier -= 0.095;
                                }
                            }
                        }

                        foreach (var slot in attacker.InventoryItems)
                        {
                            if ((int)slot.Id == 3153) // BOTRK
                            {
                                plusdmg = target.Health * 0.05;
                            }
                        }

                        return
                            CalcPhysicalDmg(
                                attacker, (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod) *
                                multiplier - reduce2 + plusdmg, target) - dmgreduce;
                    }
                    else
                    {
                        var dmgreduce = 0;
                        if (target is Obj_AI_Hero)
                        {
                            var currentenemy = champList.FirstOrDefault(e => e.NetworkId == target.NetworkId);
                            if (currentenemy != null)
                            {
                                dmgreduce = currentenemy.block;
                            }
                        }
                        var reduce2 = 0;
                        double multiplier = 1;
                        double plusdmg = 0;
                        if (target is Obj_AI_Hero)
                        {
                            foreach (var inv in target.InventoryItems)
                            {
                                if ((int)inv.Id == 1054) // Dorans Shield -> Basic attacks -8 dmg
                                {
                                    reduce2 += 8;
                                }
                                if ((int)inv.Id == 3047) // Ninja Tabi
                                {
                                    multiplier -= 0.095;
                                }
                            }
                        }

                        foreach (var slot in attacker.InventoryItems)
                        {
                            if ((int)slot.Id == 3153) // BOTRK
                            {
                                plusdmg = target.Health * 0.05;
                            }
                        }
                        var bonusmagicdmg = CalcMagicDmg(attacker, 0.05 * attacker.FlatMagicDamageMod, target);
                        return
                            CalcPhysicalDmg(
                                attacker, (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod) *
                                multiplier - reduce2 + plusdmg, target) + bonusmagicdmg - dmgreduce;
                    }
                case SpellType.IGNITE:
                    return (50 + (attacker.Level * 20)) - (target.HPRegenRate * 5) / 2;
                case SpellType.HEXGUN:
                    return CalcMagicDmg(attacker, 150 + (0.4 * attacker.FlatMagicDamageMod), target);
                case SpellType.DFG:
                    return CalcMagicDmg(attacker, (0.15 * target.MaxHealth), target);
                case SpellType.BOTRK:
                    return CalcPhysicalDmg(attacker, 0.1 * target.MaxHealth, target);
                case SpellType.BILGEWATER:
                    return CalcMagicDmg(attacker, 100, target);
                case SpellType.TIAMAT:
                    return
                        CalcPhysicalDmg(
                            attacker, attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod, target);
                case SpellType.HYDRA:
                    return
                        CalcPhysicalDmg(
                            attacker, attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod, target);
                case SpellType.BLACKFIRETORCH:
                    return CalcMagicDmg(attacker, 0.2 * target.MaxHealth, target);
                case SpellType.ODINVEILS:
                    return CalcMagicDmg(attacker, 200, target); //TODO:Check the buff to get the stored damage
                case SpellType.FROSTQUEENSCLAIM:
                    return CalcMagicDmg(attacker, 50 + 5 * attacker.Level, target);
                case SpellType.Q:
                    if (attacker.Spellbook.GetSpell(SpellSlot.Q).Level > 0)
                    {
                        return Champ(attacker, target, type, stagetype);
                    }
                    return 0;
                case SpellType.W:
                    if (attacker.Spellbook.GetSpell(SpellSlot.W).Level > 0)
                    {
                        return Champ(attacker, target, type, stagetype);
                    }
                    return 0;
                case SpellType.E:
                    if (attacker.Spellbook.GetSpell(SpellSlot.E).Level > 0)
                    {
                        return Champ(attacker, target, type, stagetype);
                    }
                    return 0;
                case SpellType.R:
                    if (attacker.Spellbook.GetSpell(SpellSlot.R).Level > 0)
                    {
                        return Champ(attacker, target, type, stagetype);
                    }
                    return 0;
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        /// <summary>
        /// Calculates the combo damage of the given spell combo on the given target.
        /// </summary>
        /// <param name="target">The target object</param>
        /// <param name="spellCombo">SpellType array containing the combo spells</param>
        /// <returns>Returns the calculated combo damage</returns>
        public static double GetComboDamage(Obj_AI_Hero attacker, Obj_AI_Base target, IEnumerable<SpellType> spellCombo)
        {
            return GetComboDamage(attacker, target, spellCombo.Select(spell => Tuple.Create(spell, StageType.Default)).ToArray());
        }

        /// <summary>
        /// Calculates the combo damage of the given spell combo on the given target respecting the stage type of each spell
        /// </summary>
        /// <param name="target">The target object</param>
        /// <param name="spellCombo">SpellType/StageType tuple containing the combo spells</param>
        /// <returns>Returns the calculated combo damage</returns>
        public static double GetComboDamage(Obj_AI_Hero attacker, Obj_AI_Base target, IEnumerable<Tuple<SpellType, StageType>> spellCombo)
        {
            return spellCombo.Sum(spell => getDmg(attacker, target, spell.Item1, spell.Item2));
        }

        /// <summary>
        /// Calculates the combo damage of the given spell combo on the given target and returns if that damage would kill the target.
        /// </summary>
        /// <param name="target">The target object</param>
        /// <param name="spellCombo">SpellType array containing the combo spells</param>
        /// <returns>true if target is killable, false if not.</returns>
        public static bool IsKillable(Obj_AI_Hero attacker, Obj_AI_Base target, IEnumerable<SpellType> spellCombo)
        {
            return GetComboDamage(attacker, target, spellCombo) > target.Health;
        }

        /// <summary>
        /// Calculates the combo damage of the given spell combo on the given target and returns if that damage would kill the target.
        /// </summary>
        /// <param name="target">The target object</param>
        /// <param name="spellCombo">SpellType/StageType tuple containing the combo spells</param>
        /// <returns>true if target is killable, false if not.</returns>
        public static bool IsKillable(Obj_AI_Hero attacker, Obj_AI_Base target, IEnumerable<Tuple<SpellType, StageType>> spellCombo)
        {
            return GetComboDamage(attacker, target, spellCombo) - spellCombo.Count() * 20 > target.Health;
        }

        private static double Aatrox(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(
                            attacker, (25 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                            (0.6 * attacker.FlatPhysicalDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcPhysicalDmg(
                            attacker, (25 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 35)) +
                            (1.0 * attacker.FlatPhysicalDamageMod), enemy); // 3rd hit Damage
                case SpellType.E:
                    return
                        CalcMagicDmg(
                            attacker, (40 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 35)) +
                            (0.6 * attacker.FlatPhysicalDamageMod) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(
                            attacker, (100 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy); // magic dmg when casted
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Ahri(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(
                                    attacker, (15 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                    (0.35 * attacker.FlatMagicDamageMod), enemy); // way to the enemy
                        case StageType.SecondDamage:
                            return (15 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                   (0.35 * attacker.FlatMagicDamageMod); // way back true dmg
                        case StageType.Default:
                            var waytoenemy =
                                CalcMagicDmg(
                                    attacker, (15 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                    (0.35 * attacker.FlatMagicDamageMod), enemy);
                            var wayback = (15 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                          (0.35 * attacker.FlatMagicDamageMod);
                            return waytoenemy + wayback; // both
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(
                                    attacker, (24 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 40)) +
                                    (0.64 * attacker.FlatMagicDamageMod), enemy); // all 3 stacks on 1 unit
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(
                                    attacker, (40 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 25)) +
                                    (0.4 * attacker.FlatMagicDamageMod), enemy); // 1 w hitting
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.E:
                    return
                        CalcMagicDmg(
                            attacker, (30 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                            (0.35 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(
                                    attacker, (30 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 40)) +
                                    (0.3 * attacker.FlatMagicDamageMod), enemy); // 1 ult 
                        case StageType.Default:
                            return
                                CalcMagicDmg(
                                    attacker, (90 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 120)) +
                                    (0.9 * attacker.FlatMagicDamageMod), enemy); // max dmg to 1 unit
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Akali(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(
                                    attacker, (15 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 20)) +
                                    (0.4 * attacker.FlatMagicDamageMod), enemy); // q throw
                        case StageType.Default:
                            return
                                CalcMagicDmg(
                                    attacker, (20 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                    (0.5 * attacker.FlatMagicDamageMod), enemy);
                        // q throw + hitted with something
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcPhysicalDmg(
                            attacker, (5 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                            (0.6 * (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage)) +
                            (0.3 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(
                            attacker, (25 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 75)) +
                            (0.5 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Alistar(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(
                            attacker, (15 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                            (0.5 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(
                            attacker, (0 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 55)) +
                            (0.7 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Amumu(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(
                            attacker, (30 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                            (0.7 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    var basedmg = CalcMagicDmg(
                        attacker, (4 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 4)), enemy);
                    var percentofmaxhealth = (1.2 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 0.3));
                    double additionalpercentper100ap = 0;
                    if (attacker.FlatMagicDamageMod < 100)
                    {
                        additionalpercentper100ap = 0;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(100, 199))
                    {
                        additionalpercentper100ap = 1;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(200, 299))
                    {
                        additionalpercentper100ap = 2;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(300, 399))
                    {
                        additionalpercentper100ap = 3;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(400, 499))
                    {
                        additionalpercentper100ap = 4;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(500, 599))
                    {
                        additionalpercentper100ap = 5;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(600, 699))
                    {
                        additionalpercentper100ap = 6;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(700, 799))
                    {
                        additionalpercentper100ap = 7;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(800, 899))
                    {
                        additionalpercentper100ap = 8;
                    }
                    else if (attacker.FlatMagicDamageMod >= 900)
                    {
                        additionalpercentper100ap = 9;
                    }
                    var healthbase = enemy.MaxHealth / 100 * (percentofmaxhealth + additionalpercentper100ap);
                    return basedmg + CalcMagicDmg(attacker, healthbase, enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(
                            attacker, (50 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                            (0.5 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(
                            attacker, (50 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (0.8 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Anivia(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(
                                    attacker, (60 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 60)) +
                                    (1.0 * attacker.FlatMagicDamageMod), enemy);
                        // when stunned (both of dmg)
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(
                                    attacker, (30 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 30)) +
                                    (0.5 * attacker.FlatMagicDamageMod), enemy); // when going through
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(
                                    attacker, (50 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 60)) +
                                    (1.0 * attacker.FlatMagicDamageMod), enemy); // when "Chilled"
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(
                                    attacker, (25 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                                    (0.5 * attacker.FlatMagicDamageMod), enemy); // basic dmg
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.R:
                    return
                        CalcMagicDmg(
                            attacker, (40 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 40)) +
                            (0.25 * attacker.FlatMagicDamageMod), enemy); // per tick
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Annie(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(
                            attacker, (45 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 35)) +
                            (0.8 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(
                            attacker, (25 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 45)) +
                            (0.85 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(
                            attacker, (10 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 10)) +
                            (0.2 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(
                                    attacker, (85 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                                    (1.0 * attacker.FlatMagicDamageMod), enemy);
                        // max damage with first tick of tibbers sunfire
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(
                                    attacker, (50 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                                    (0.8 * attacker.FlatMagicDamageMod), enemy); // basic ult summoner dmg
                        case StageType.SecondDamage:
                            return CalcMagicDmg(attacker, 35 + (0.2 * attacker.FlatMagicDamageMod), enemy);
                        // per tick of tibbers sunfire
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Ashe(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    throw new InvalidSpellTypeException();
                case SpellType.W:
                    return
                        CalcPhysicalDmg(
                            attacker, (30 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 10)) +
                            (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage), enemy);
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(
                                    attacker, (75 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 175)) +
                                    (1.0 * attacker.FlatMagicDamageMod), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(
                                    attacker, (37.5 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 87.5)) +
                                    (0.5 * attacker.FlatMagicDamageMod), enemy);
                        // dmg around the explode radius
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Blitzcrank(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(
                            attacker, (25 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 55)) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcPhysicalDmg(
                            attacker, (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod), enemy);
                // only the additional dmg
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(
                                    attacker, (125 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                                    (1.0 * attacker.FlatMagicDamageMod), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(
                                    attacker, (0 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (0.2 * attacker.FlatMagicDamageMod), enemy); // passive dmg
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Brand(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(
                            attacker, (40 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                            (0.65 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(
                                    attacker, (30 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 45)) +
                                    (0.6 * attacker.FlatMagicDamageMod), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(
                                    attacker, (37.5 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 56.25)) +
                                    (0.75 * attacker.FlatMagicDamageMod), enemy); // blazed dmg
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.E:
                    return
                        CalcMagicDmg(
                            attacker, (35 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 35)) +
                            (0.55 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(
                                    attacker, (150 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 300)) +
                                    (1.5 * attacker.FlatMagicDamageMod), enemy);
                        // Max possible dmg to one unit
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(
                                    attacker, (50 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (0.5 * attacker.FlatMagicDamageMod), enemy); // damage per bounce
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Braum(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(
                            attacker, (15 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                            (0.25 * attacker.MaxHealth), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    return
                        CalcMagicDmg(
                            attacker, (50 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Caitlyn(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(
                                    attacker, (-20 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 40)) +
                                    (1.3 *
                                     (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage)),
                                    enemy); // first hit dmg
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(
                                    attacker, (-10 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 20)) +
                                    (0.65 *
                                     (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage)),
                                    enemy); // min damage
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    return
                        CalcMagicDmg(
                            attacker, (30 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(
                            attacker, (30 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 50)) +
                            (0.8 * (attacker.FlatMagicDamageMod)), enemy);
                case SpellType.R:
                    return
                        CalcPhysicalDmg(
                            attacker, (25 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 225)) +
                            (2.0 * attacker.FlatPhysicalDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Cassiopeia(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(
                                    attacker, (12 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 13)) +
                                    (0.2666 * attacker.FlatMagicDamageMod), enemy); // first sec
                        case StageType.SecondDamage:
                            return
                                CalcMagicDmg(
                                    attacker, (24 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 26)) +
                                    (0.5333 * attacker.FlatMagicDamageMod), enemy); // second sec
                        case StageType.Default:
                            return
                                CalcMagicDmg(
                                    attacker, (35 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (0.8 * attacker.FlatMagicDamageMod), enemy); // all dmg
                        case StageType.ThirdDamage:
                            return
                                CalcMagicDmg(
                                    attacker, (35 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (0.8 * attacker.FlatMagicDamageMod), enemy); // 3 hits -> all dmg
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (135 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 90)) +
                                    (1.35 * attacker.FlatMagicDamageMod), enemy); // complete w dmg
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (15 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 10)) +
                                    (0.15 * attacker.FlatMagicDamageMod), enemy); // per second
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (15 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 35)) +
                            (0.55 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (75 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double ChoGath(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 55)) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                            (0.7 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (5 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 15)) +
                            (0.3 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return (125 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 175));
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Corki(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                            (0.5 * attacker.FlatMagicDamageMod) +
                            (0.5 * attacker.FlatPhysicalDamageMod), enemy);
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (75 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 75)) +
                                    (1.0 * attacker.FlatMagicDamageMod), enemy); // dmg complete
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (30 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 30)) +
                                    (0.4 * attacker.FlatMagicDamageMod), enemy); // per second dmg
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.E:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    (32 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 48)) +
                                    (1.6 * attacker.FlatPhysicalDamageMod), enemy); // dmg complete
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (8 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 12)) +
                                    (0.4 * attacker.FlatPhysicalDamageMod), enemy); // per sec
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (20 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 80)) +
                                    (0.3 * attacker.FlatMagicDamageMod) +
                                    ((0.1 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 0.1)) *
                                     attacker.FlatPhysicalDamageMod), enemy); // normal missile
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (30 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 120)) +
                                    (0.45 * attacker.FlatMagicDamageMod) +
                                    ((0.15 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 0.15)) *
                                     attacker.FlatPhysicalDamageMod), enemy); // every 4th
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Darius(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(
                                    attacker, (52.5 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 52.5)) +
                                    (1.05 * attacker.FlatPhysicalDamageMod), enemy);
                        // max damage (outer half)
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (35 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 35)) +
                                    (0.7 * attacker.FlatPhysicalDamageMod), enemy); // inner half
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    //double basicattack = CalcPhysicalDmg( attacker,ObjectManager.Unit.FlatPhysicalDamageMod + ObjectManager.Unit.BaseAttackDamage, enemy);
                    var bonusdmg = CalcPhysicalDmg(attacker,
                        0.2 * attacker.Spellbook.GetSpell(SpellSlot.W).Level, enemy); // only the bonus dmg
                    //return basicattack + bonusdmg;
                    return bonusdmg;
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return ((140 + (180 * attacker.Spellbook.GetSpell(SpellSlot.R).Level)) +
                                    (1.5 * attacker.FlatPhysicalDamageMod)); // at 5 stacks
                        case StageType.FirstDamage:
                            return ((70 + (90 * attacker.Spellbook.GetSpell(SpellSlot.R).Level)) +
                                    (0.75 * attacker.FlatPhysicalDamageMod)); // foreach stack
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Diana(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 35)) +
                            (0.7 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (30 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 36)) +
                                    (0.6 * attacker.FlatMagicDamageMod), enemy); // all on one target_
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (10 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 12)) +
                                    (0.2 * attacker.FlatMagicDamageMod), enemy); // single orb
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (40 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 60)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double DrMundo(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    var tmpdmg =
                        CalcMagicDmg(attacker,
                            (enemy.Health / 100) *
                            (12 + (3 * attacker.Spellbook.GetSpell(SpellSlot.Q).Level)), enemy);
                    var mindmg = CalcMagicDmg(attacker,
                        30 + (50 * attacker.Spellbook.GetSpell(SpellSlot.Q).Level), enemy);
                    if (tmpdmg > mindmg)
                    {
                        return tmpdmg;
                    }
                    return mindmg;
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (20 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 15)) +
                            (0.2 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Draven(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    //double baseattack = CalcPhysicalDmg( attacker,ObjectManager.Unit.BaseAttackDamage + ObjectManager.Unit.FlatPhysicalDamageMod, enemy);
                    var bonusdmg =
                        CalcPhysicalDmg(attacker,
                            (0.35 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 0.1)) *
                            (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod), enemy);
                    //return baseattack + bonusdmg;
                    return bonusdmg; // only the bonus dmg
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcPhysicalDmg(attacker,
                            (35 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 35)) +
                            (0.5 * (attacker.FlatPhysicalDamageMod)), enemy);
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    (150 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 200)) +
                                    (2.2 * attacker.FlatPhysicalDamageMod), enemy); // both hit
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (75 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (1.1 * attacker.FlatPhysicalDamageMod), enemy); // way to enemy
                        case StageType.SecondDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (75 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (1.1 * attacker.FlatPhysicalDamageMod), enemy); // way back
                        case StageType.ThirdDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (30 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 40)) +
                                    (0.44 * attacker.FlatPhysicalDamageMod), enemy); // minimum damage 1 hit
                        case StageType.FourthDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (60 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 80)) +
                                    (0.88 * attacker.FlatPhysicalDamageMod), enemy);
                        // minimum damage 2 hits
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Elise(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    if (attacker.Spellbook.GetSpell(SpellSlot.Q).Name == "EliseHumanQ")
                    {
                        var basedmg =
                            CalcMagicDmg(attacker, (5 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 35)), enemy);
                        double percentofcurrenthealth = 8;
                        double additionalpercentper100ap = 0;
                        if (attacker.FlatMagicDamageMod < 100)
                        {
                            additionalpercentper100ap = 0;
                        }
                        else if (attacker.FlatMagicDamageMod.IsBetween(100, 199))
                        {
                            additionalpercentper100ap = 3;
                        }
                        else if (attacker.FlatMagicDamageMod.IsBetween(200, 299))
                        {
                            additionalpercentper100ap = 6;
                        }
                        else if (attacker.FlatMagicDamageMod.IsBetween(300, 399))
                        {
                            additionalpercentper100ap = 9;
                        }
                        else if (attacker.FlatMagicDamageMod.IsBetween(400, 499))
                        {
                            additionalpercentper100ap = 12;
                        }
                        else if (attacker.FlatMagicDamageMod.IsBetween(500, 599))
                        {
                            additionalpercentper100ap = 15;
                        }
                        else if (attacker.FlatMagicDamageMod.IsBetween(600, 699))
                        {
                            additionalpercentper100ap = 18;
                        }
                        else if (attacker.FlatMagicDamageMod.IsBetween(700, 799))
                        {
                            additionalpercentper100ap = 21;
                        }
                        else if (attacker.FlatMagicDamageMod.IsBetween(800, 899))
                        {
                            additionalpercentper100ap = 24;
                        }
                        else if (attacker.FlatMagicDamageMod >= 900)
                        {
                            additionalpercentper100ap = 27;
                        }
                        var healthbase = enemy.Health / 100 * (percentofcurrenthealth + additionalpercentper100ap);
                        return basedmg + CalcMagicDmg(attacker, healthbase, enemy);
                    }
                    else
                    {
                        // Spider Q
                        var basedmg =
                            CalcMagicDmg(attacker,
                                (20 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)), enemy);
                        double percentofcurrenthealth = 8;
                        double additionalpercentper100ap = 0;
                        if (attacker.FlatMagicDamageMod < 100)
                        {
                            additionalpercentper100ap = 0;
                        }
                        else if (attacker.FlatMagicDamageMod.IsBetween(100, 199))
                        {
                            additionalpercentper100ap = 3;
                        }
                        else if (attacker.FlatMagicDamageMod.IsBetween(200, 299))
                        {
                            additionalpercentper100ap = 6;
                        }
                        else if (attacker.FlatMagicDamageMod.IsBetween(300, 399))
                        {
                            additionalpercentper100ap = 9;
                        }
                        else if (attacker.FlatMagicDamageMod.IsBetween(400, 499))
                        {
                            additionalpercentper100ap = 12;
                        }
                        else if (attacker.FlatMagicDamageMod.IsBetween(500, 599))
                        {
                            additionalpercentper100ap = 15;
                        }
                        else if (attacker.FlatMagicDamageMod.IsBetween(600, 699))
                        {
                            additionalpercentper100ap = 18;
                        }
                        else if (attacker.FlatMagicDamageMod.IsBetween(700, 799))
                        {
                            additionalpercentper100ap = 21;
                        }
                        else if (attacker.FlatMagicDamageMod.IsBetween(800, 899))
                        {
                            additionalpercentper100ap = 24;
                        }
                        else if (attacker.FlatMagicDamageMod >= 900)
                        {
                            additionalpercentper100ap = 27;
                        }
                        var healthbase = (enemy.MaxHealth - enemy.Health) / 100 *
                                         (percentofcurrenthealth + additionalpercentper100ap); // of missing health
                        return basedmg + CalcMagicDmg(attacker, healthbase, enemy);
                    }
                case SpellType.W:
                    if (attacker.Spellbook.GetSpell(SpellSlot.Q).Name == "EliseHumanW")
                    {
                        return
                            CalcMagicDmg(attacker,
                                (25 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                                (0.8 * attacker.FlatMagicDamageMod), enemy);
                    }
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    throw new InvalidSpellTypeException(); // switchting to spider / human
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Evelynn(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (15 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 15)) +
                            ((0.3 + attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 0.05) *
                             attacker.FlatMagicDamageMod) +
                            ((0.45 + attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 0.05) *
                             attacker.FlatPhysicalDamageMod), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcPhysicalDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 40)) +
                            (1.0 * attacker.FlatMagicDamageMod) +
                            (1.0 * attacker.FlatPhysicalDamageMod), enemy);
                case SpellType.R:
                    double percentage = 15 + (5 * attacker.Spellbook.GetSpell(SpellSlot.R).Level);
                    double additionalpercentper100ap = 0;
                    if (attacker.FlatMagicDamageMod < 100)
                    {
                        additionalpercentper100ap = 0;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(100, 199))
                    {
                        additionalpercentper100ap = 1;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(200, 299))
                    {
                        additionalpercentper100ap = 2;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(300, 399))
                    {
                        additionalpercentper100ap = 3;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(400, 499))
                    {
                        additionalpercentper100ap = 4;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(500, 599))
                    {
                        additionalpercentper100ap = 5;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(600, 699))
                    {
                        additionalpercentper100ap = 6;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(700, 799))
                    {
                        additionalpercentper100ap = 7;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(800, 899))
                    {
                        additionalpercentper100ap = 8;
                    }
                    else if (attacker.FlatMagicDamageMod >= 900)
                    {
                        additionalpercentper100ap = 9;
                    }
                    var healthbase = enemy.MaxHealth / 100 * (percentage + additionalpercentper100ap);
                    return CalcMagicDmg(attacker, healthbase, enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Ezreal(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (15 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 20)) +
                            (1.0 * (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod)) +
                            (0.4 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 45)) +
                            (0.8 * (attacker.FlatMagicDamageMod)), enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 50)) +
                            (0.75 * (attacker.FlatMagicDamageMod)), enemy);
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (200 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 150)) +
                                    (1.0 *
                                     (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage)) +
                                    (0.9 * attacker.FlatMagicDamageMod), enemy); // basic dmg
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (60 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 45)) +
                                    (0.3 *
                                     (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage)) +
                                    (0.27 * attacker.FlatMagicDamageMod), enemy);
                        // minimum damage after multiple targets were hitted
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Fiddlesticks(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    throw new InvalidSpellTypeException();
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (150 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 150)) +
                                    (2.25 * (attacker.FlatMagicDamageMod)), enemy); // complete dmg
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (30 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 30)) +
                                    (0.45 * (attacker.FlatMagicDamageMod)), enemy); // per second
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.E:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (45 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 20)) +
                                    (0.45 * (attacker.FlatMagicDamageMod)), enemy); // damage per bounce
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (135 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 60)) +
                                    (1.35 * (attacker.FlatMagicDamageMod)), enemy);
                        // max damage to the same target_
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (25 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (0.45 * (attacker.FlatMagicDamageMod)), enemy); // damage per sec
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (125 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 500)) +
                                    (2.25 * (attacker.FlatMagicDamageMod)), enemy); // dmg complete
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Fiora(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    (30 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                                    (1.2 * attacker.FlatPhysicalDamageMod), enemy); // for both jumps
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (15 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                    (0.6 * attacker.FlatPhysicalDamageMod), enemy); // for 1 jump
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (10 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    (-20 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 340)) +
                                    (2.4 * attacker.FlatPhysicalDamageMod), enemy);
                        // max dmg to 1 target_ // TODO: Check if correct, Wiki giving some shit to me
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (-10 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 170)) +
                                    (1.2 * attacker.FlatPhysicalDamageMod), enemy); // 1 hit damage
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Fizz(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    var addmg =
                        CalcPhysicalDmg(attacker,
                            attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod, enemy);
                    var mdmg =
                        CalcMagicDmg(attacker,
                            (-20 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 30)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                    return addmg + mdmg;
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (5 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 5)) +
                                    (0.25 * attacker.FlatMagicDamageMod), enemy); // active dmg
                        case StageType.FirstDamage:
                            var basedmg =
                                CalcMagicDmg(attacker,
                                    (20 + (10 * attacker.Spellbook.GetSpell(SpellSlot.W).Level) +
                                     (0.35 * attacker.FlatMagicDamageMod)), enemy); // passive dmg
                            double percentage = 3 + attacker.Spellbook.GetSpell(SpellSlot.W).Level;
                            return basedmg + ((enemy.MaxHealth - enemy.Health) / 100) * percentage;
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (20 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 50)) +
                            (0.75 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (75 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Galio(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 55)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (15 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                            (0.5 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (110 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 110)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Gangplank(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (-5 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                            (1.0 * (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage)),
                            enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 45)) +
                            (0.2 * attacker.FlatMagicDamageMod), enemy);
                // per canonball, 25 max but randomly
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Garen(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (5 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                            (0.4 * (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage)),
                            enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    (-15 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 75)) +
                                    ((1.8 + (0.3 * attacker.Spellbook.GetSpell(SpellSlot.E).Level)) *
                                     (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage)),
                                    enemy); // complete e dmg
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (-5 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                                    ((0.6 + (0.1 * attacker.Spellbook.GetSpell(SpellSlot.E).Level)) *
                                     (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage)),
                                    enemy); // per sec
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.R:
                    var basedmg = CalcMagicDmg(attacker,
                        175 + (175 * attacker.Spellbook.GetSpell(SpellSlot.R).Level), enemy);
                    double hpbonus = 0;
                    if (attacker.Spellbook.GetSpell(SpellSlot.R).Level == 1)
                    {
                        hpbonus = (enemy.MaxHealth - enemy.Health) / 3.5;
                    }
                    else if (attacker.Spellbook.GetSpell(SpellSlot.R).Level == 2)
                    {
                        hpbonus = (enemy.MaxHealth - enemy.Health) / 3;
                    }
                    else if (attacker.Spellbook.GetSpell(SpellSlot.R).Level == 2)
                    {
                        hpbonus = (enemy.MaxHealth - enemy.Health) / 2.5;
                    }
                    return basedmg + CalcMagicDmg(attacker, hpbonus, enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Gnar(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.Default: //Mini Gnar
                            return
                                CalcPhysicalDmg(attacker,
                                    -25 + attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 35 +
                                    attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage,
                                    enemy);
                        case StageType.FirstDamage: //MEGA Gnar
                            return
                                CalcPhysicalDmg(attacker,
                                    -30 + attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40 +
                                    1.2 *
                                    (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage),
                                    enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default: //Mini Gnar
                            return
                                CalcMagicDmg(attacker,
                                    20 + attacker.Spellbook.GetSpell(SpellSlot.W).Level * 5 +
                                    attacker.FlatMagicDamageMod +
                                    enemy.MaxHealth *
                                    (0.04 + 0.02 * attacker.Spellbook.GetSpell(SpellSlot.W).Level), enemy);
                        case StageType.FirstDamage: //MEGA Gnar
                            return
                                CalcPhysicalDmg(attacker,
                                    5 + attacker.Spellbook.GetSpell(SpellSlot.W).Level * 20 +
                                    (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage),
                                    enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.E:
                    switch (stagetype)
                    {
                        case StageType.Default: //Mini Gnar
                            return
                                CalcPhysicalDmg(attacker,
                                    -20 + attacker.Spellbook.GetSpell(SpellSlot.E).Level * 40 +
                                    attacker.MaxHealth * 0.06, enemy);
                        case StageType.FirstDamage: //MEGA Gnar
                            return
                                CalcPhysicalDmg(attacker,
                                    -20 + attacker.Spellbook.GetSpell(SpellSlot.E).Level * 40 +
                                    attacker.MaxHealth * 0.06, enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default: //Default damage
                            return
                                CalcPhysicalDmg(attacker,
                                    100 + attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100 +
                                    attacker.FlatPhysicalDamageMod * 0.2 + attacker.FlatMagicDamageMod * 0.5, enemy);
                        case StageType.FirstDamage: //Max damage.
                            return
                                CalcPhysicalDmg(attacker,
                                    100 + attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100 +
                                    attacker.FlatPhysicalDamageMod * 0.2 + attacker.FlatMagicDamageMod * 0.5, enemy) * 1.5;
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Gragas(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (40 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    double hpbonus = (enemy.MaxHealth / 100) * 7 +
                                     attacker.Spellbook.GetSpell(SpellSlot.W).Level;
                    return
                        CalcMagicDmg(attacker,
                            (-10 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 30)) +
                            (0.3 * attacker.FlatMagicDamageMod) + hpbonus, enemy);
                case SpellType.E:
                    return
                        CalcPhysicalDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 50)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (100 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (0.7 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Graves(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    (42.5 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 59.5)) +
                                    (1.36 * attacker.FlatPhysicalDamageMod), enemy); // max dmg
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (25 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 35)) +
                                    (0.8 * attacker.FlatPhysicalDamageMod), enemy); // for each struck hit
                        default:
                            throw new InvalidCastException();
                    }
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (10 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    return
                        CalcPhysicalDmg(attacker,
                            (150 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (1.5 * attacker.FlatPhysicalDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Hecarim(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 35)) +
                            (0.6 * attacker.FlatPhysicalDamageMod), enemy);
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (10 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 10)) +
                                    (0.2 * attacker.FlatMagicDamageMod), enemy); // complete dmg
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (10 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                                    (0.6 * attacker.FlatMagicDamageMod), enemy); // per sec
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.E:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    (10 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 70)) +
                                    (1.0 * attacker.FlatPhysicalDamageMod), enemy); // max e dmg
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (5 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 35)) +
                                    (0.5 * attacker.FlatPhysicalDamageMod), enemy); // min e dmg
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (50 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy);

                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Heimerdinger(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (6 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 6)) +
                                    (0.15 * attacker.FlatMagicDamageMod), enemy); // per hit dmg
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (20 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 20)) +
                                    (0.55 * attacker.FlatMagicDamageMod), enemy); // energy blast dmg
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (54 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 54)) +
                                    (0.92 * attacker.FlatMagicDamageMod), enemy); // max dmg to 1 target_
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (30 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 30)) +
                                    (0.45 * attacker.FlatMagicDamageMod), enemy); // foreach hitting missile
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (20 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 40)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Irelia(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (-10 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 30)) +
                            (1.0 * (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod)),
                            enemy);
                case SpellType.W:
                    return 15 * attacker.Spellbook.GetSpell(SpellSlot.W).Level;
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 50)) +
                            (0.5 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    (160 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 160)) +
                                    (2.4 * attacker.FlatPhysicalDamageMod) +
                                    (2.0 * attacker.FlatMagicDamageMod), enemy); // max dmg to 1 target_
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (40 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 40)) +
                                    (0.6 * attacker.FlatPhysicalDamageMod) +
                                    (0.5 * attacker.FlatMagicDamageMod), enemy); // dmg per blade
                        default:
                            throw new InvalidCastException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Janna(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (65 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (0.65 * attacker.FlatMagicDamageMod), enemy); // max dmg
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (35 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                    (0.35 * attacker.FlatMagicDamageMod), enemy);
                        // damage when directly casted
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (5 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 55)) +
                            (0.5 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double JarvanIV(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                            (1.2 * attacker.FlatPhysicalDamageMod), enemy);
                case SpellType.W:
                    throw new InvalidCastException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (15 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                            (0.8 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcPhysicalDmg(attacker,
                            (75 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                            (1.5 * attacker.FlatPhysicalDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Jax(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                            (1.0 * attacker.FlatPhysicalDamageMod) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (5 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 35)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                            (0.5 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (40 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 60)) +
                            (0.7 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Jayce(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    if (attacker.Spellbook.GetSpell(SpellSlot.Q).Name == "JayceToTheSkies")
                    {
                        return
                            CalcPhysicalDmg(attacker,
                                (-25 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                                (1.0 * attacker.FlatPhysicalDamageMod), enemy);
                    }
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    (7 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 77)) +
                                    (1.68 * attacker.FlatPhysicalDamageMod), enemy); // e + q
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (5 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 55)) +
                                    (1.2 * attacker.FlatPhysicalDamageMod), enemy); // q only
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    if (attacker.Spellbook.GetSpell(SpellSlot.Q).Name == "JayceStaticField")
                    {
                        switch (stagetype)
                        {
                            case StageType.Default:
                                return
                                    CalcMagicDmg(attacker,
                                        (30 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 70)) +
                                        (1.0 * attacker.FlatMagicDamageMod), enemy); // complete dmg
                            case StageType.FirstDamage:
                                return
                                    CalcMagicDmg(attacker,
                                        (7.5 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 17.5)) +
                                        (0.25 * attacker.FlatMagicDamageMod), enemy); // per sec
                            default:
                                throw new InvalidSpellTypeException();
                        }
                    }
                    return 0;
                // return 0, no exception as when switching the name isn't directly changed and ppl will already try to calculate
                case SpellType.E:
                    if (attacker.Spellbook.GetSpell(SpellSlot.Q).Name == "JayceThunderingBlow")
                    {
                        double percentage = 5 + (3 * attacker.Spellbook.GetSpell(SpellSlot.E).Level);
                        return
                            CalcMagicDmg(attacker,
                                ((enemy.MaxHealth / 100) * percentage) + (attacker.FlatPhysicalDamageMod),
                                enemy);
                    }
                    return 0;
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Jinx(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    throw new InvalidSpellTypeException();
                case SpellType.W:
                    return
                        CalcPhysicalDmg(attacker,
                            (-40 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                            (1.4 * (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod)),
                            enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 55)) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            var percentage =
                                CalcPhysicalDmg(attacker,
                                    ((enemy.MaxHealth - enemy.Health) / 100) *
                                    (20 + (5 * attacker.Spellbook.GetSpell(SpellSlot.R).Level)), enemy);
                            return percentage +
                                   CalcPhysicalDmg(attacker,
                                       (150 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                       (1.0 * attacker.FlatPhysicalDamageMod), enemy); // max dmg
                        case StageType.FirstDamage:
                            var percentage2 =
                                CalcPhysicalDmg(attacker,
                                    ((enemy.MaxHealth - enemy.Health) / 100) *
                                    (20 + (5 * attacker.Spellbook.GetSpell(SpellSlot.R).Level)), enemy);
                            return percentage2 +
                                   CalcPhysicalDmg(attacker,
                                       (75 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 50)) +
                                       (0.5 * attacker.FlatPhysicalDamageMod), enemy); // min dmg
                        default:
                            throw new InvalidCastException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Karma(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (35 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                                    (0.6 * attacker.FlatMagicDamageMod), enemy); // basic q
                        case StageType.FirstDamage:
                            var baseqdmg =
                                CalcMagicDmg(attacker,
                                    (35 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                                    (0.6 * attacker.FlatMagicDamageMod), enemy);
                            // mantra q (with bonus, not with detonation)
                            var bonusqdmg =
                                CalcMagicDmg(attacker,
                                    (-25 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level)) +
                                    (0.3 * attacker.FlatMagicDamageMod), enemy);
                            return baseqdmg + bonusqdmg;
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (10 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                                    (0.6 * attacker.FlatMagicDamageMod), enemy); // basic w
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (0 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 75)) +
                                    (0.6 * attacker.FlatMagicDamageMod), enemy); // mantra w
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (-20 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 80)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy); // mantra e (shield with dmg)
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Karthus(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (40 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (0.6 * attacker.FlatMagicDamageMod), enemy); // single target_ dmg
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (20 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 20)) +
                                    (0.3 * attacker.FlatMagicDamageMod), enemy); // multi target_ dmg
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (10 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 20)) +
                            (0.2 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (100 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 150)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Kassadin(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (55 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                            (0.7 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (15 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 25)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (55 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                            (0.7 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (60 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 20)) +
                            (0.02 * attacker.MaxMana), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Katarina(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (35 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                    (0.6 * attacker.FlatMagicDamageMod), enemy);
                        // total dmg (mark + detonation)
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (35 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                    (0.45 * attacker.FlatMagicDamageMod), enemy); // mark damage
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (5 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 35)) +
                            (0.25 * attacker.FlatMagicDamageMod) +
                            (0.6 * attacker.FlatPhysicalDamageMod), enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (35 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                            (0.4 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (225 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 175)) +
                                    (2.5 * attacker.FlatMagicDamageMod) +
                                    (3.75 * attacker.FlatPhysicalDamageMod), enemy); // complete ult dmg
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (22.5 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 17.5)) +
                                    (0.25 * attacker.FlatMagicDamageMod) +
                                    (0.375 * attacker.FlatPhysicalDamageMod), enemy); // for each blade
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Kayle(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (10 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                            (0.6 * attacker.FlatMagicDamageMod) +
                            (1.0 * attacker.FlatPhysicalDamageMod), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (10 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 10)) +
                            (0.25 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Kennen(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (35 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                            (0.75 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (35 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 30)) +
                                    (0.55 * attacker.FlatMagicDamageMod), enemy); // active dmg
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    ((0.3 + (0.1 * attacker.Spellbook.GetSpell(SpellSlot.W).Level)) *
                                     attacker.FlatPhysicalDamageMod), enemy); // pasive dmg
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (45 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 40)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (45 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 195)) +
                                    (1.2 * attacker.FlatMagicDamageMod), enemy); // max dmg to 1 target_
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (15 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 65)) +
                                    (0.4 * attacker.FlatMagicDamageMod), enemy); // dmg per bolt
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Khazix(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    (30 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                    (1.2 * attacker.FlatPhysicalDamageMod), enemy); // basic q dmg
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    ((30 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                     (1.2 * attacker.FlatPhysicalDamageMod)) * 1.3, enemy); // isolated q
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    return
                        CalcPhysicalDmg(attacker,
                            (35 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 40)) +
                            (1.0 * attacker.FlatPhysicalDamageMod), enemy);
                case SpellType.E:
                    return
                        CalcPhysicalDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 35)) +
                            (0.2 * attacker.FlatPhysicalDamageMod), enemy);
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double KogMaw(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                            (0.5 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    var percentofmaxhealth = (1.0 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level));
                    double additionalpercentper100ap = 0;
                    if (attacker.FlatMagicDamageMod < 100)
                    {
                        additionalpercentper100ap = 0;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(100, 199))
                    {
                        additionalpercentper100ap = 1;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(200, 299))
                    {
                        additionalpercentper100ap = 2;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(300, 399))
                    {
                        additionalpercentper100ap = 3;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(400, 499))
                    {
                        additionalpercentper100ap = 4;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(500, 599))
                    {
                        additionalpercentper100ap = 5;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(600, 699))
                    {
                        additionalpercentper100ap = 6;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(700, 799))
                    {
                        additionalpercentper100ap = 7;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(800, 899))
                    {
                        additionalpercentper100ap = 8;
                    }
                    else if (attacker.FlatMagicDamageMod >= 900)
                    {
                        additionalpercentper100ap = 9;
                    }
                    var healthbase = enemy.MaxHealth / 100 * (percentofmaxhealth + additionalpercentper100ap);
                    return CalcMagicDmg(attacker, healthbase, enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (10 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 50)) +
                            (0.7 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (80 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 80)) +
                            (0.3 * attacker.FlatMagicDamageMod) +
                            (0.5 * attacker.FlatPhysicalDamageMod), enemy); // 100% bonus dmg to champs
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double LeBlanc(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (60 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                                    (0.8 * attacker.FlatMagicDamageMod), enemy); // total q dmg
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (30 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                                    (0.4 * attacker.FlatMagicDamageMod), enemy);
                        // first q or detonation, same dmg
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (45 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 40)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (30 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 50)) +
                                    (1.0 * attacker.FlatMagicDamageMod), enemy); // total e dmg
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (15 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                                    (0.5 * attacker.FlatMagicDamageMod), enemy);
                        // first e or detonation, same dmg
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            throw new InvalidSpellTypeException();
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (0 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (0.65 * attacker.FlatMagicDamageMod), enemy); // q as ulted version
                        case StageType.SecondDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (0 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 150)) +
                                    (0.975 * attacker.FlatMagicDamageMod), enemy); // w as ulted version
                        case StageType.ThirdDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (0 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (0.65 * attacker.FlatMagicDamageMod), enemy); // e as ulted version
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double LeeSin(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    40 + ((attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 60)) +
                                    (1.8 * attacker.FlatPhysicalDamageMod) +
                                    (8 * ((enemy.MaxHealth / enemy.Health) / 100)), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    20 + ((attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 30)) +
                                    (0.9 * attacker.FlatPhysicalDamageMod), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 35)) +
                            attacker.FlatMagicDamageMod, enemy);
                case SpellType.R:
                    return
                        CalcPhysicalDmg(attacker,
                            (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 200) +
                            (2.0 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Leona(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (10 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 30)) +
                            (0.30 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (10 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                            (0.40 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (20 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 40)) +
                            (0.40 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (50 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (0.80 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Lissandra(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (40 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 35)) +
                            (0.65 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 40)) +
                            (0.40 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                            (0.60 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (50 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (0.70 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Lucian(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (50 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 30)) +
                            (45 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 15)) +
                            attacker.FlatPhysicalDamageMod, enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (20 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 40)) +
                            (0.3 * attacker.FlatPhysicalDamageMod) +
                            (0.9 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    return
                        CalcPhysicalDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 10)) +
                            (0.1 * attacker.FlatMagicDamageMod) +
                            (0.3 * attacker.FlatPhysicalDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Lulu(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return CalcMagicDmg(attacker, (35 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 45)), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (50 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                            (0.4 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Lux(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (10 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                            (0.7 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (15 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (200 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (0.75 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Malphite(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (20 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (20 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 40)) +
                            (0.2 * attacker.FlatMagicDamageMod) + (0.3 * attacker.Armor), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (100 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (1.3 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }


        private static double Malzahar(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 55)) +
                            (0.8 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            ((attacker.Spellbook.GetSpell(SpellSlot.W).Level + 3) +
                             (0.1 * attacker.FlatMagicDamageMod)) * (enemy.MaxHealth / 100), enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (20 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 60)) +
                            (0.8 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (100 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (1.3 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Maokai(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                            (0.4 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    var percentage = ((7.5 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 1.5)) +
                                      (0.04 * attacker.FlatMagicDamageMod));
                    return CalcMagicDmg(attacker, (enemy.MaxHealth / 100) * percentage, enemy);
                case SpellType.E:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (60 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 60)) +
                                    (1.0 * attacker.FlatMagicDamageMod), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (20 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 20)) +
                                    (0.4 * attacker.FlatMagicDamageMod), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (200 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (0.5 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }


        private static double MasterYi(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (10 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 35)) +
                            (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double MissFortune(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (5 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 15)) +
                                    (0.85 *
                                     (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage)) +
                                    (0.35 * attacker.FlatMagicDamageMod), enemy);
                        case StageType.SecondDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (10 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 30)) +
                                    (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage) +
                                    (0.5 * attacker.FlatMagicDamageMod), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (35 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 55)) +
                            (0.8 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    (200 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 200)) +
                                    (1.6 * attacker.FlatMagicDamageMod), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (25 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 25)) +
                                    (0.2 * attacker.FlatMagicDamageMod), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Mordekaiser(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (82.5 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 49.5)) +
                                    (0.66 * attacker.FlatMagicDamageMod) +
                                    (1.65 * attacker.FlatPhysicalDamageMod), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (50 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 30)) +
                                    (0.40 * attacker.FlatMagicDamageMod) +
                                    (1.0 * attacker.FlatPhysicalDamageMod), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (60 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 84)) +
                                    (1.2 * attacker.FlatMagicDamageMod), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (10 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 14)) +
                                    (0.20 * attacker.FlatMagicDamageMod), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            ((19 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 5)) +
                             (0.4 * attacker.FlatMagicDamageMod)) * (1 - (enemy.MaxHealth / 100)), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Morgana(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 55)) +
                            (0.60 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (75 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 105)) +
                                    (1.65 * attacker.FlatMagicDamageMod), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (50 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 70)) +
                                    (1.10 * attacker.FlatMagicDamageMod), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (150 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 150)) +
                                    (1.40 * attacker.FlatMagicDamageMod), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (75 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 75)) +
                                    (0.7 * attacker.FlatMagicDamageMod), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Nami(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (20 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 55)) +
                            (0.50 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 40)) +
                            (0.50 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (10 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 15)) +
                            (0.20 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (50 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (0.60 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Nasus(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (from buff in attacker.Buffs
                             where buff.DisplayName == "NasusQStacks"
                             select buff.Count).FirstOrDefault() +
                            (10 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 20)), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (30 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 80)) +
                                    (1.2 * attacker.FlatMagicDamageMod), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (3 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 8)) +
                                    (0.12 * attacker.FlatMagicDamageMod), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (((attacker.Spellbook.GetSpell(SpellSlot.R).Level + 2) +
                                      (0.01 * attacker.FlatMagicDamageMod)) * (enemy.MaxHealth / 100)) * 15,
                                    enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    ((attacker.Spellbook.GetSpell(SpellSlot.R).Level + 2) +
                                     (0.01 * attacker.FlatMagicDamageMod)) * (enemy.MaxHealth / 100), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Nautilus(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (15 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                            (0.75 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 15)) +
                            (0.40 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (40 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 80)) +
                                    (1.0 * attacker.FlatMagicDamageMod), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (20 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 40)) +
                                    (0.50 * attacker.FlatMagicDamageMod), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (75 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                            (0.80 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Nidalee(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    if (attacker.Spellbook.GetSpell(SpellSlot.Q).Name == "JavelinToss")
                    {
                        switch (stagetype)
                        {
                            case StageType.Default:
                                return
                                    CalcMagicDmg(attacker,
                                        (37 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 100)) +
                                        (1.625 * attacker.FlatMagicDamageMod), enemy); // max dmg
                            case StageType.FirstDamage:
                                return
                                    CalcMagicDmg(attacker,
                                        (15 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                                        (0.65 * attacker.FlatMagicDamageMod), enemy); // min dmg
                            default:
                                throw new InvalidSpellTypeException();
                        }
                    }
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    (30 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 90)) +
                                    (3.0 * attacker.FlatPhysicalDamageMod), enemy); // max dmg
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (10 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 30)) +
                                    (1.0 * attacker.FlatPhysicalDamageMod), enemy); // base dmg
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    if (attacker.Spellbook.GetSpell(SpellSlot.W).Name == "Bushwhack")
                    {
                        return
                            CalcMagicDmg(attacker,
                                (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 20) +
                                (0.1 + (0.02 * attacker.Spellbook.GetSpell(SpellSlot.W).Level)), enemy);
                    }
                    return
                        CalcMagicDmg(attacker,
                            (75 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 50)) +
                            (0.4 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    if (attacker.Spellbook.GetSpell(SpellSlot.E).Name == "PrimalSurge")
                    {
                        return 0; // no exception as switchting won't change the name directly
                    }
                    return
                        CalcMagicDmg(attacker,
                            (75 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 75)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Nocturne(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (15 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                            (0.75 * attacker.FlatPhysicalDamageMod), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 50) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcPhysicalDmg(attacker,
                            (50 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (1.2 * attacker.FlatPhysicalDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Nunu(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return (250 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 150));
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (40 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (375 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 250)) +
                                    (2.5 * attacker.FlatMagicDamageMod), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (46.875 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 31.25)) +
                                    (0.3125 * attacker.FlatMagicDamageMod), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Olaf(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                            (1.0 * attacker.FlatPhysicalDamageMod), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return (25 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 45) +
                            (0.4 * (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage)));
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Orianna(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (60.0 + (30.0 * attacker.Spellbook.GetSpell(SpellSlot.Q).Level) +
                             (0.5 * attacker.FlatMagicDamageMod)), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (70.0 + (45.0 * attacker.Spellbook.GetSpell(SpellSlot.W).Level) +
                             (0.7 * attacker.FlatMagicDamageMod)), enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (60.0 + (30.0 * attacker.Spellbook.GetSpell(SpellSlot.E).Level) +
                             (0.3 * attacker.FlatMagicDamageMod)), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (150.0 + (75.0 * attacker.Spellbook.GetSpell(SpellSlot.R).Level) +
                             (0.7 * attacker.FlatMagicDamageMod)), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Pantheon(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                            (1.4 * attacker.FlatPhysicalDamageMod) +
                            (0.5 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 25)) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    (20 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 60)) +
                                    (3.6 * attacker.FlatPhysicalDamageMod), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (6 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 20)) +
                                    (1.2 * attacker.FlatMagicDamageMod), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (100 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 300)) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Poppy(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 20) +
                            (1.0 * (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage)) +
                            (0.6 * attacker.FlatMagicDamageMod) + (0.08 * enemy.MaxHealth), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (25 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                                    (0.4 * attacker.FlatMagicDamageMod), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (50 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 75)) +
                                    (0.8 * attacker.FlatMagicDamageMod), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Quinn(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                            (0.65 * attacker.FlatPhysicalDamageMod) +
                            (0.5 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (10 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                            (0.20 * attacker.FlatPhysicalDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            ((70 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 50)) +
                             (0.50 * attacker.FlatPhysicalDamageMod)) * (2 - enemy.Health / enemy.MaxHealth),
                            enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Rammus(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (50 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (5 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 10)) +
                            (0.1 * attacker.Armor), enemy);
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (520 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 520)) +
                                    (2.4 * attacker.FlatMagicDamageMod), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 65) +
                                    (0.3 * attacker.FlatMagicDamageMod), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Renekton(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    30 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 30) +
                                    ((0.8 * attacker.FlatPhysicalDamageMod)), enemy); // basic q
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    45 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 45) +
                                    ((1.2 * attacker.FlatPhysicalDamageMod)), enemy); // empowered q
                        default:
                            throw new InvalidCastException();
                    }
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    -10 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 20) +
                                    ((1.5 *
                                      (attacker.FlatPhysicalDamageMod +
                                       attacker.BaseAttackDamage))), enemy); // basic w
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    -15 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 30) +
                                    ((2.25 *
                                      (attacker.FlatPhysicalDamageMod +
                                       attacker.BaseAttackDamage))), enemy); // empowered w
                        default:
                            throw new InvalidCastException();
                    }
                case SpellType.E:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    0 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 30) +
                                    ((0.9 * (attacker.FlatPhysicalDamageMod))), enemy); // basic e
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    0 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 45) +
                                    ((1.35 * (attacker.FlatPhysicalDamageMod))), enemy); // empowered e
                        default:
                            throw new InvalidCastException();
                    }
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (450 * attacker.Spellbook.GetSpell(SpellSlot.R).Level) +
                                    (1.5 * attacker.FlatMagicDamageMod), enemy); // complete dmg
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (30 * attacker.Spellbook.GetSpell(SpellSlot.R).Level) +
                                    (0.1 * attacker.FlatMagicDamageMod), enemy); // per sec
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Rengar(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    30 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 30) +
                                    ((0.95 * (5 * attacker.Spellbook.GetSpell(SpellSlot.Q).Level)) *
                                     (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage)),
                                    enemy); // basic q
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    17.65 + (attacker.Level * 12.35) +
                                    (1.5 * attacker.FlatPhysicalDamageMod +
                                     attacker.BaseAttackDamage), enemy); // empowered q
                        default:
                            throw new InvalidCastException();
                    }
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    20 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 30) +
                                    (0.8 * attacker.FlatMagicDamageMod), enemy); // basic w
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    25 + (attacker.Level * 15) +
                                    (0.8 * attacker.FlatMagicDamageMod), enemy); // empowered w
                        default:
                            throw new InvalidCastException();
                    }
                case SpellType.E:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    50 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 50) +
                                    (0.7 * attacker.FlatPhysicalDamageMod), enemy); // basic e
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    25 + (attacker.Level * 25) +
                                    (0.7 * attacker.FlatPhysicalDamageMod), enemy); // empowered e
                        default:
                            throw new InvalidCastException();
                    }
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Riven(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    30 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 60) +
                                    (105 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 15)) *
                                    (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage),
                                    enemy);
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    -10 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 20) +
                                    (0.35 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 0.5)) *
                                    (attacker.FlatPhysicalDamageMod + attacker.BaseAttackDamage),
                                    enemy);
                        default:
                            throw new InvalidCastException();
                    }
                case SpellType.W:
                    return
                        CalcPhysicalDmg(attacker,
                            20 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 30) +
                            (1.0 * attacker.FlatPhysicalDamageMod), enemy);
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    if ((enemy.Health / enemy.MaxHealth) * 100 > 25)
                    {
                        return
                            CalcPhysicalDmg(attacker,
                                40 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 40) +
                                (0.6 * attacker.FlatPhysicalDamageMod), enemy);
                    }
                    return
                        CalcPhysicalDmg(attacker,
                            120 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 120) +
                            (1.8 * attacker.FlatPhysicalDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Rumble(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    15 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 60) +
                                    (1.0 * attacker.FlatMagicDamageMod), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    5 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 20) +
                                    (0.33 * attacker.FlatMagicDamageMod), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            20 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 25) +
                            (0.4 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    325 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 325) +
                                    (1.5 * attacker.FlatMagicDamageMod), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    75 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 55) +
                                    (0.30 * attacker.FlatMagicDamageMod), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Ryze(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            35 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 25) +
                            (0.4 * attacker.FlatMagicDamageMod) + (0.065 * attacker.MaxMana),
                            enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            25 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 35) +
                            (0.6 * attacker.FlatMagicDamageMod) + (0.045 * attacker.MaxMana),
                            enemy);
                case SpellType.E:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    90 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 60) +
                                    (1.05 * attacker.FlatMagicDamageMod) +
                                    (0.03 * attacker.MaxMana), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    30 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 20) +
                                    (0.35 * attacker.FlatMagicDamageMod) +
                                    (0.01 * attacker.MaxMana), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Sejuani(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            10 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 30) +
                            (2 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 2)) *
                            (enemy.MaxHealth / 100), enemy);
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    60 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 60) +
                                    (0.9 * attacker.FlatMagicDamageMod) +
                                    ((attacker.ScriptHealthBonus / 100) * 10), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    10 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 10) +
                                    (0.15 * attacker.FlatMagicDamageMod) +
                                    ((attacker.ScriptHealthBonus / 100) * 0.25), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            10 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 50) +
                            (0.5 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            50 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100) +
                            (0.8 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Shaco(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            0.20 +
                            (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 0.20) *
                            (attacker.FlatPhysicalDamageMod + attacker.BaseAbilityDamage), enemy);
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    20 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 15) +
                                    (0.2 * attacker.FlatMagicDamageMod), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    200 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 135) +
                                    (1.8 * attacker.FlatMagicDamageMod), enemy);
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            10 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 40) +
                            (1.0 * attacker.FlatMagicDamageMod) +
                            (1.0 * attacker.FlatPhysicalDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            150 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 150) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Shen(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            20 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            15 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 35) +
                            (0.5 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Shyvana(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            0.75 +
                            (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 0.05) *
                            (attacker.FlatPhysicalDamageMod + attacker.BaseAbilityDamage), enemy);
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    35 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 105) +
                                    (1.4 * attacker.FlatPhysicalDamageMod), enemy);
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    5 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 15) +
                                    (0.2 * attacker.FlatPhysicalDamageMod), enemy);
                        default:
                            throw new InvalidCastException();
                    }
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            20 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 40) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            50 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 125) +
                            (0.7 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Singed(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    switch (stagetype)
                    {
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (10 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 12)) +
                                    (0.3 * attacker.FlatMagicDamageMod), enemy); // per sec
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (30 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 36)) +
                                    (0.9 * attacker.FlatMagicDamageMod), enemy); // complete dmg
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (35 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                            (0.75 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Sion(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (12 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 57.5)) +
                            (0.9 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (50 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                            (0.9 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Sivir(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (5 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 20)) +
                            ((0.6 + (0.1 * attacker.Spellbook.GetSpell(SpellSlot.Q).Level)) *
                             attacker.FlatPhysicalDamageMod) +
                            (0.5 * attacker.FlatMagicDamageMod), enemy); // basic physical dmg
                case SpellType.W:
                    return
                        CalcPhysicalDmg(attacker,
                            (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod) *
                            (0.45 + (0.05 * attacker.Spellbook.GetSpell(SpellSlot.W).Level)), enemy);
                // for each of 3
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Skarner(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (8 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 10)) +
                            (0.4 * attacker.FlatPhysicalDamageMod), enemy); // basic bonus dmg
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (20 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 20)) +
                            (0.4 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (100 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Sona(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40) +
                            (0.5 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (50 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (0.5 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Soraka(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (15 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                            (0.4 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (10 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                            (0.4 * attacker.FlatMagicDamageMod) + (0.5 * attacker.MaxMana),
                            enemy);
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }


        private static double Swain(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (10 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 15)) +
                            (0.3 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (40 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 40)) +
                            (0.7 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (35 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 40)) +
                            (0.8 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 20)) +
                            (0.2 * attacker.FlatMagicDamageMod), enemy); //x sec
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Syndra(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                            (0.7 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (40 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 40)) +
                            (0.7 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                            (0.4 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (45 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 45)) +
                                    (0.2 * attacker.FlatMagicDamageMod), enemy); // for each orb
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (135 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 135)) +
                                    (0.6 * attacker.FlatMagicDamageMod), enemy); // minimum dmg (3 balls)
                        case StageType.SecondDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (285 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 345)) +
                                    (1.4 * attacker.FlatMagicDamageMod), enemy); // max. dmg (7 balls)
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Talon(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (0 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                            (1.3 * (attacker.FlatPhysicalDamageMod)), enemy); // bonus dmg
                case SpellType.W:
                    return
                        CalcPhysicalDmg(attacker,
                            (5 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 25)) +
                            (0.60 * (attacker.FlatPhysicalDamageMod)), enemy);
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    return
                        CalcPhysicalDmg(attacker,
                            (70 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 50)) +
                            (0.75 * (attacker.FlatPhysicalDamageMod)), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Taric(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    throw new InvalidSpellTypeException();
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 40) +
                            (0.2 * attacker.Armor), enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (10 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                            (0.2 * attacker.FlatMagicDamageMod), enemy); // min e dmg
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (50 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (0.5 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Teemo /*Satan*/(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (35 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                            (0.80 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (0 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 34)) +
                            (0.7 * attacker.FlatMagicDamageMod), enemy); // total dmg for one hit
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (75 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                            (0.5 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Thresh(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (40 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                            (0.50 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (35 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                            (0.4 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (100 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 150)) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Tristana(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    throw new InvalidSpellTypeException();
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 45)) +
                            (0.80 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (35 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                                    (1.0 * attacker.FlatMagicDamageMod), enemy); // active
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (25 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                                    (0.25 * attacker.FlatMagicDamageMod), enemy); // passive
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (200 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (1.5 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Trundle(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (0 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 20)) +
                            ((0.95 + (0.05 * attacker.Spellbook.GetSpell(SpellSlot.Q).Level)) *
                             attacker.FlatPhysicalDamageMod), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    double basepercent = 16 + (4 * attacker.Spellbook.GetSpell(SpellSlot.R).Level);
                    double additionalpercentper100ap = 0;
                    if (attacker.FlatMagicDamageMod < 100)
                    {
                        additionalpercentper100ap = 0;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(100, 199))
                    {
                        additionalpercentper100ap = 2;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(200, 299))
                    {
                        additionalpercentper100ap = 4;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(300, 399))
                    {
                        additionalpercentper100ap = 6;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(400, 499))
                    {
                        additionalpercentper100ap = 8;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(500, 599))
                    {
                        additionalpercentper100ap = 10;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(600, 699))
                    {
                        additionalpercentper100ap = 12;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(700, 799))
                    {
                        additionalpercentper100ap = 14;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(800, 899))
                    {
                        additionalpercentper100ap = 16;
                    }
                    else if (attacker.FlatMagicDamageMod >= 900)
                    {
                        additionalpercentper100ap = 18;
                    }
                    var healthbase = enemy.MaxHealth / 100 * (basepercent + additionalpercentper100ap);
                    return CalcMagicDmg(attacker, healthbase, enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Tryndamere(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    throw new InvalidSpellTypeException();
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcPhysicalDmg(attacker,
                            (40 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                            (1.2 * (attacker.FlatPhysicalDamageMod)) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double TwistedFate(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (10 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                            (0.65 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            throw new InvalidSpellTypeException(); // W itself deals no dmg, card must be specified
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (20 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 20)) +
                                    (0.5 * attacker.FlatMagicDamageMod) +
                                    (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod),
                                    enemy); // Blue Card
                        case StageType.SecondDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (15 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 15)) +
                                    (0.5 * attacker.FlatMagicDamageMod) +
                                    (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod),
                                    enemy); // Red Card
                        case StageType.ThirdDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (7.5 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 7.5)) +
                                    (0.5 * attacker.FlatMagicDamageMod) +
                                    (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod),
                                    enemy); // Gold Card
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                            (0.5 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Twitch(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    throw new InvalidSpellTypeException();
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    var basedmg = CalcPhysicalDmg(attacker,
                        5 + (15 * attacker.Spellbook.GetSpell(SpellSlot.E).Level), enemy);
                    var perstack =
                        CalcPhysicalDmg(attacker,
                            10 + (5 * attacker.Spellbook.GetSpell(SpellSlot.E).Level) +
                            (0.2 * attacker.FlatMagicDamageMod) +
                            (0.25 * attacker.FlatPhysicalDamageMod), enemy);
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return basedmg + (5 * perstack); // complete dmg 5 stacks
                        case StageType.FirstDamage:
                            return basedmg + perstack; // foreach stack
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Udyr(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    var percentadbonus = 1.1 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 0.1);
                    return
                        CalcPhysicalDmg(attacker,
                            (-20 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                            (percentadbonus *
                             (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod)),
                            enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 50)) +
                            (1.25 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Urgot(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 30) - 20 +
                            (0.85 * (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod)),
                            enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcPhysicalDmg(attacker,
                            (20 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 55)) +
                            (0.60 * (attacker.FlatPhysicalDamageMod)), enemy);
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Varus(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (-40 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 55)) +
                            (1.6 * (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod)),
                            enemy); // max dmg, first target_
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (6 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 4)) +
                            (0.25 * attacker.FlatMagicDamageMod), enemy); // passive magic dmg
                case SpellType.E:
                    return
                        CalcPhysicalDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 35)) +
                            (0.60 * attacker.FlatPhysicalDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (50 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Vayne(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    var percentofbonusad = 0.25 + (0.05 * attacker.Spellbook.GetSpell(SpellSlot.Q).Level);
                    return
                        CalcPhysicalDmg(attacker,
                            percentofbonusad *
                            (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod), enemy);
                // ony the bonus dmg
                case SpellType.W:
                    double flattruedmg = 10 + (10 * attacker.Spellbook.GetSpell(SpellSlot.W).Level);
                    double percentofenemyhp = 3 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level);
                    return flattruedmg + ((enemy.MaxHealth / 100) * percentofenemyhp);
                case SpellType.E:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    (20 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 70)) +
                                    (1.0 * attacker.FlatPhysicalDamageMod), enemy);
                        // Damage when knock + against wall
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (10 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 35)) +
                                    (0.5 * attacker.FlatPhysicalDamageMod), enemy);
                        // Damage when knock starts
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Veigar(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (35 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                            (0.60 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (70 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (125 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                            (1.2 * attacker.FlatMagicDamageMod) + (0.8 * enemy.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Velkoz(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (40 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                            (0.60 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (25 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 50)) +
                                    (0.625 * attacker.FlatMagicDamageMod), enemy); // complete dmg
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (10 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 20)) +
                                    (0.25 * attacker.FlatMagicDamageMod), enemy); // first dmg
                        case StageType.SecondDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (15 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 30)) +
                                    (0.375 * attacker.FlatMagicDamageMod), enemy); // detonation
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (40 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                            (0.50 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 20)) +
                            (0.06 * attacker.FlatMagicDamageMod), enemy); //x 0,25sec
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Vi(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 25)) +
                            (0.80 * (attacker.FlatPhysicalDamageMod)), enemy);
                case SpellType.W:
                    var percentage = 2.5 * (1.5 * attacker.Spellbook.GetSpell(SpellSlot.W).Level);
                    var bonusadpercentage = percentage + (attacker.FlatPhysicalDamageMod / 34);
                    return (enemy.MaxHealth / 100 * bonusadpercentage);
                case SpellType.E:
                    return
                        CalcPhysicalDmg(attacker,
                            (-10 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 15)) +
                            (0.70 * attacker.FlatMagicDamageMod) +
                            (1.15 * attacker.FlatPhysicalDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcPhysicalDmg(attacker,
                            (75 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                            (1.4 * attacker.FlatPhysicalDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Viktor(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (55 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 35) +
                             (0.60 * attacker.FlatMagicDamageMod)), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                            (0.70 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (70 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 120)) +
                                    (0.79 * attacker.FlatMagicDamageMod), enemy);
                        // Total Initial Damage (initial + first dot)
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (50 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                                    (0.55 * attacker.FlatMagicDamageMod), enemy); // Initial Damage
                        case StageType.SecondDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (20 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 20)) +
                                    (0.24 * attacker.FlatMagicDamageMod), enemy); // Damage per Tick
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Vladimir(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (50 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 35) +
                             (0.60 * attacker.FlatMagicDamageMod)), enemy);
                case SpellType.W:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (25 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 55) +
                                     (15 * attacker.ScriptHealthBonus)), enemy); // complete w dmg
                        case StageType.FourthDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (6.25 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 13.75) +
                                     (3.75 * attacker.ScriptHealthBonus)), enemy); // per 0.5 sec
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.E:
                    var edmg =
                        CalcMagicDmg(attacker,
                            (35 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                            (0.45 * attacker.FlatMagicDamageMod), enemy);
                    switch (stagetype)
                    {
                        case StageType.Default: // without any stacks
                            return edmg;
                        case StageType.FirstDamage: // 1 stack
                            return edmg * 1.25;
                        case StageType.SecondDamage: // 2 stacks
                            return edmg * 1.5;
                        case StageType.ThirdDamage: // 3 stacks
                            return edmg * 1.75;
                        case StageType.FourthDamage: // 4 stacks
                            return edmg * 2.0;
                        default:
                            throw new InvalidSpellTypeException();
                    }
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (50 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (0.70 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Volibear(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return CalcPhysicalDmg(attacker, (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 30), enemy);
                case SpellType.W:
                    var basedmg =
                        CalcPhysicalDmg(attacker,
                            (35 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 45)) +
                            (0.15 * attacker.ScriptHealthBonus), enemy);
                    double percentmissinghealth = 100 - ((enemy.Health / enemy.MaxHealth) * 100);
                    return basedmg * percentmissinghealth;
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (15 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                            (0.60 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 80) - 5 +
                            (0.30 * attacker.FlatMagicDamageMod), enemy); //RÃœBERGUCKEN
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Warwick(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    var percentagedmg =
                        CalcMagicDmg(attacker,
                            (enemy.MaxHealth / 100 *
                             (6 + (2 * attacker.Spellbook.GetSpell(SpellSlot.Q).Level))) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy);
                    var flatdmg =
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 50)) +
                            (1.0 * attacker.FlatMagicDamageMod), enemy);
                    if (percentagedmg > flatdmg)
                    {
                        return percentagedmg;
                    }
                    return flatdmg;
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (165 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 85)) +
                                    (2.0 * attacker.FlatPhysicalDamageMod), enemy);
                        // 5 hits => complete ult
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (33 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 17)) +
                                    (0.4 * attacker.FlatPhysicalDamageMod), enemy); // per hit
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double MonkeyKing(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 30) +
                            (1.1 * (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod)),
                            enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 45)) +
                            (0.60 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    return
                        CalcPhysicalDmg(attacker,
                            (15 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 45)) +
                            (0.8 * (attacker.FlatPhysicalDamageMod)), enemy);
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcPhysicalDmg(attacker,
                                    (-280 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 360)) +
                                    (4.4 *
                                     (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod)),
                                    enemy); // max damage
                        case StageType.FirstDamage:
                            return
                                CalcPhysicalDmg(attacker,
                                    (-70 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 90)) +
                                    (1.1 *
                                     (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod)),
                                    enemy); // per second
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Xerath(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (40 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                            (0.75 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (60 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 30)) +
                            (0.90 * attacker.FlatMagicDamageMod), enemy); //NOT EMPOWERED
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (50 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                            (0.45 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (405 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 165)) +
                                    (1.3 * attacker.FlatMagicDamageMod), enemy); // 3 hits on target_
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (135 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 55)) +
                                    (0.43 * attacker.FlatMagicDamageMod), enemy); // per hit
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double XinZhao(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 15) +
                            (1.0 * (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod)),
                            enemy); // per hit
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (50 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 20)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcPhysicalDmg(attacker,
                            (-25 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100)) +
                            (1.0 * attacker.FlatPhysicalDamageMod) + ((enemy.Health / 100) * 15), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Yasuo(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 20) +
                            (1.0 * (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod)),
                            enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (50 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 20)) +
                            (0.6 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcPhysicalDmg(attacker,
                            (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 100) +
                            (1.5 * (attacker.FlatPhysicalDamageMod)), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Yorick(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 30) +
                            (1.2 * (attacker.BaseAttackDamage + attacker.FlatPhysicalDamageMod)),
                            enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 35) +
                             (1.0 * attacker.FlatMagicDamageMod)), enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                            (1.0 * (attacker.FlatPhysicalDamageMod)), enemy);
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Zac(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                            (0.50 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    var basedmg =
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 15)) +
                            (0.35 * attacker.FlatMagicDamageMod), enemy);
                    double percentofmaxhealth = (3 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level));
                    double additionalpercentper100ap = 0;
                    if (attacker.FlatMagicDamageMod < 100)
                    {
                        additionalpercentper100ap = 0;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(100, 199))
                    {
                        additionalpercentper100ap = 2;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(200, 299))
                    {
                        additionalpercentper100ap = 4;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(300, 399))
                    {
                        additionalpercentper100ap = 6;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(400, 499))
                    {
                        additionalpercentper100ap = 8;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(500, 599))
                    {
                        additionalpercentper100ap = 10;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(600, 699))
                    {
                        additionalpercentper100ap = 12;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(700, 799))
                    {
                        additionalpercentper100ap = 14;
                    }
                    else if (attacker.FlatMagicDamageMod.IsBetween(800, 899))
                    {
                        additionalpercentper100ap = 16;
                    }
                    else if (attacker.FlatMagicDamageMod >= 900)
                    {
                        additionalpercentper100ap = 18;
                    }
                    var healthbase = enemy.MaxHealth / 100 * (percentofmaxhealth + additionalpercentper100ap);
                    return basedmg + CalcMagicDmg(attacker, healthbase, enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (40 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 40)) +
                            (0.7 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    switch (stagetype)
                    {
                        case StageType.FirstDamage:
                            return
                                CalcMagicDmg(attacker,
                                    (70 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 70)) +
                                    (0.4 * attacker.FlatMagicDamageMod), enemy); // first jump on enemy
                        case StageType.Default:
                            return
                                CalcMagicDmg(attacker,
                                    (175 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 175)) +
                                    (1.0 * attacker.FlatMagicDamageMod), enemy); // all jumps on enemy
                        default:
                            throw new InvalidSpellTypeException();
                    }
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Zed(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcPhysicalDmg(attacker,
                            (35 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 40)) +
                            (1.0 * attacker.FlatPhysicalDamageMod), enemy); // 1 hit
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcPhysicalDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 30)) +
                            (0.8 * attacker.FlatPhysicalDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcPhysicalDmg(attacker,
                            1.0 * (attacker.FlatMagicDamageMod + attacker.BaseAttackDamage),
                            enemy);
                // base dmg
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Ziggs(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (30 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 45)) +
                            (0.35 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    return
                        CalcMagicDmg(attacker,
                            (35 + (attacker.Spellbook.GetSpell(SpellSlot.W).Level * 35)) +
                            (0.35 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (15 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 25)) +
                            (0.30 * attacker.FlatMagicDamageMod), enemy); // per mine
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (125 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 125)) +
                            (0.35 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Zyra(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (35 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 35)) +
                            (0.65 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    return
                        CalcMagicDmg(attacker,
                            (25 + (attacker.Spellbook.GetSpell(SpellSlot.E).Level * 35)) +
                            (0.50 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.R:
                    return
                        CalcMagicDmg(attacker,
                            (95 + (attacker.Spellbook.GetSpell(SpellSlot.R).Level * 85)) +
                            (0.70 * attacker.FlatMagicDamageMod), enemy);
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private static double Zilean(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype)
        {
            switch (type)
            {
                case SpellType.Q:
                    return
                        CalcMagicDmg(attacker,
                            (35 + (attacker.Spellbook.GetSpell(SpellSlot.Q).Level * 55)) +
                            (0.9 * attacker.FlatMagicDamageMod), enemy);
                case SpellType.W:
                    throw new InvalidSpellTypeException();
                case SpellType.E:
                    throw new InvalidSpellTypeException();
                case SpellType.R:
                    throw new InvalidSpellTypeException();
                default:
                    throw new InvalidSpellTypeException();
            }
        }

        private delegate double ChampDamage(Obj_AI_Hero attacker, Obj_AI_Base enemy, SpellType type, StageType stagetype);
    }

    internal class Champion
    {
        public int NetworkId;
        public int block;
        public int unyielding;
        public bool doubleedgedsword;
        public bool havoc;
        public int executioner;
        public bool arcaneblade;
        public bool butcher;
    }
}
