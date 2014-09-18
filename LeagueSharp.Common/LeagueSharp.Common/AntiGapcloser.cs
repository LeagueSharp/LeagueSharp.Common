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

#region

using System;
using System.Collections.Generic;
using System.Linq;

using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    public delegate void OnGapcloseH(ActiveGapcloser gapcloser);

    public enum GapcloserType
    {
        Skillshot,
        Targeted
    }

    public struct Gapcloser
    {
        public string ChampionName;
        public GapcloserType SkillType;
        public SpellSlot Slot;
        public string SpellName;
    }

    public struct ActiveGapcloser
    {
        public Vector3 End;
        public Obj_AI_Base Sender;
        public GapcloserType SkillType;
        public Vector3 Start;
        public int TickCount;
    }

    public static class AntiGapcloser
    {
        public static List<Gapcloser> Spells = new List<Gapcloser>();
        public static List<ActiveGapcloser> ActiveGapclosers = new List<ActiveGapcloser>();

        static AntiGapcloser()
        {
            #region Aatrox

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Aatrox",
                    Slot = SpellSlot.Q,
                    SpellName = "aatroxq",
                    SkillType = GapcloserType.Skillshot
                });

            #endregion

            #region Akali

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Akali",
                    Slot = SpellSlot.R,
                    SpellName = "akalishadowdance",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Alistar

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Alistar",
                    Slot = SpellSlot.E,
                    SpellName = "headbutt",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region LeeSin

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "LeeSin",
                    Slot = SpellSlot.Q,
                    SpellName = "blindmonkqtwo",
                    SkillType = GapcloserType.Skillshot
                });

            #endregion

            #region Diana

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Diana",
                    Slot = SpellSlot.R,
                    SpellName = "dianateleport",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Elise

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Elise",
                    Slot = SpellSlot.Q,
                    SpellName = "elisespiderqcast",
                    SkillType = GapcloserType.Skillshot
                });

            #endregion

            #region Fiora

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Fiora",
                    Slot = SpellSlot.Q,
                    SpellName = "fioraq",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Fizz

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Fizz",
                    Slot = SpellSlot.Q,
                    SpellName = "fizzpiercingstrike",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Gnar

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Gnar",
                    Slot = SpellSlot.E,
                    SpellName = "gnarbige",
                    SkillType = GapcloserType.Skillshot
                });

            #endregion

            #region Gragas

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Gragas",
                    Slot = SpellSlot.E,
                    SpellName = "gragase",
                    SkillType = GapcloserType.Skillshot
                });

            #endregion

            #region Hecarim

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Hecarim",
                    Slot = SpellSlot.R,
                    SpellName = "hecarimult",
                    SkillType = GapcloserType.Skillshot
                });

            #endregion

            #region Irelia

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Irelia",
                    Slot = SpellSlot.Q,
                    SpellName = "ireliagatotsu",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region JarvanIV

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "JarvanIV",
                    Slot = SpellSlot.Q,
                    SpellName = "jarvanivdragonstrike",
                    SkillType = GapcloserType.Skillshot
                });

            #endregion

            #region Jax

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Jax",
                    Slot = SpellSlot.Q,
                    SpellName = "jaxleapstrike",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Khazix

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Khazix",
                    Slot = SpellSlot.E,
                    SpellName = "khazixe",
                    SkillType = GapcloserType.Skillshot
                });

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Khazix",
                    Slot = SpellSlot.E,
                    SpellName = "khazixelong",
                    SkillType = GapcloserType.Skillshot
                });

            #endregion

            #region LeBlanc

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "LeBlanc",
                    Slot = SpellSlot.W,
                    SpellName = "leblancslide",
                    SkillType = GapcloserType.Skillshot
                });

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "LeBlanc",
                    Slot = SpellSlot.R,
                    SpellName = "leblancslidem",
                    SkillType = GapcloserType.Skillshot
                });

            #endregion

            #region Leona

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Leona",
                    Slot = SpellSlot.Q,
                    SpellName = "leonazenithblade",
                    SkillType = GapcloserType.Skillshot
                });

            #endregion

            #region MonkeyKing

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "MonkeyKing",
                    Slot = SpellSlot.E,
                    SpellName = "monkeykingnimbus",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Pantheon

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Pantheon",
                    Slot = SpellSlot.W,
                    SpellName = "pantheon_leapbash",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Poppy

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Poppy",
                    Slot = SpellSlot.E,
                    SpellName = "poppyheroiccharge",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Renekton

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Renekton",
                    Slot = SpellSlot.E,
                    SpellName = "renektonsliceanddice",
                    SkillType = GapcloserType.Skillshot
                });

            #endregion

            #region Riven

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Riven",
                    Slot = SpellSlot.Q,
                    SpellName = "riventricleave",
                    SkillType = GapcloserType.Skillshot
                });

            #endregion

            #region Sejuani

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Sejuani",
                    Slot = SpellSlot.Q,
                    SpellName = "sejuaniarcticassault",
                    SkillType = GapcloserType.Skillshot
                });

            #endregion

            #region Malphite

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Malphite",
                    Slot = SpellSlot.R,
                    SpellName = "ufslash",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Vi

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Vi",
                    Slot = SpellSlot.Q,
                    SpellName = "viq",
                    SkillType = GapcloserType.Skillshot
                });

            #endregion

            #region XinZhao

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "XinZhao",
                    Slot = SpellSlot.E,
                    SpellName = "xenzhaosweep",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Yasuo

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Yasuo",
                    Slot = SpellSlot.E,
                    SpellName = "yasuodashwrapper",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Tryndamere

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Tryndamere",
                    Slot = SpellSlot.Q,
                    SpellName = "slashcast",
                    SkillType = GapcloserType.Skillshot
                });

            #endregion

            Game.OnGameUpdate += Game_OnGameUpdate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        public static event OnGapcloseH OnEnemyGapcloser;

        private static void Game_OnGameUpdate(EventArgs args)
        {
            ActiveGapclosers.RemoveAll(entry => Environment.TickCount > entry.TickCount + 900);
            if (OnEnemyGapcloser == null)
            {
                return;
            }

            foreach (var gapcloser in ActiveGapclosers)
            {
                if (gapcloser.Sender.IsValidTarget())
                {
                    if (gapcloser.SkillType == GapcloserType.Targeted ||
                        (gapcloser.SkillType == GapcloserType.Skillshot &&
                         ObjectManager.Player.Distance(gapcloser.Sender) < 500))
                    {
                        OnEnemyGapcloser(gapcloser);
                    }
                }
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!SpellIsGapcloser(args))
            {
                return;
            }

            ActiveGapclosers.Add(
                new ActiveGapcloser
                {
                    Start = args.Start,
                    End = args.End,
                    Sender = sender,
                    TickCount = Environment.TickCount,
                    SkillType =
                        (args.Target != null && args.Target.IsMe) ? GapcloserType.Targeted : GapcloserType.Skillshot
                });
        }

        private static bool SpellIsGapcloser(GameObjectProcessSpellCastEventArgs args)
        {
            return Spells.Any(spell => spell.SpellName == args.SData.Name.ToLower());
        }
    }
}