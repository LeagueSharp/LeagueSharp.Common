namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp.Common.Data;

    /// <summary>
    ///     Gets the damage done to a target.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="target">The target.</param>
    /// <param name="level">The level.</param>
    /// <returns></returns>
    public delegate double SpellDamageDelegate(Obj_AI_Base source, Obj_AI_Base target, int level);

    /// <summary>
    ///     Represents a spell that deals damage.
    /// </summary>
    public class DamageSpell
    {
        #region Fields

        /// <summary>
        ///     The calculated damage
        /// </summary>
        public double CalculatedDamage;

        /// <summary>
        ///     The damage delegate
        /// </summary>
        public SpellDamageDelegate Damage;

        /// <summary>
        ///     The damage type
        /// </summary>
        public Damage.DamageType DamageType;

        /// <summary>
        ///     The slot
        /// </summary>
        public SpellSlot Slot;

        /// <summary>
        ///     The stage
        /// </summary>
        public int Stage;

        #endregion
    }

    /// <summary>
    ///     Calculates damage to units.
    /// </summary>
    public static class Damage
    {
        #region Static Fields

        /// <summary>
        ///     The spells
        /// </summary>
        public static Dictionary<string, List<DamageSpell>> Spells =
            new Dictionary<string, List<DamageSpell>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     The attack passives
        /// </summary>
        private static readonly List<PassiveDamage> AttackPassives = new List<PassiveDamage>();

        #endregion

        #region Constructors and Destructors

        //attack passives are handled in the orbwalker, it will be changed in the future :^)

        /// <summary>
        ///     Initializes static members of the <see cref="Damage" /> class.
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

            #region Akali

            p = new PassiveDamage
                    {
                        ChampionName = "Akali", IsActive = (source, target) => true,
                        GetDamage =
                            (source, target) =>
                            (float)
                            source.CalcDamage(
                                target,
                                DamageType.Magical,
                                (0.06 + Math.Abs(source.TotalMagicalDamage / 100) * 0.16667) * source.TotalAttackDamage)
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = "Akali", IsActive = (source, target) => target.HasBuff("AkaliMota"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.Q, 1)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Alistar

            p = new PassiveDamage
                    {
                        ChampionName = "Alistar", IsActive = (source, target) => (source.HasBuff("alistartrample")),
                        GetDamage =
                            (source, target) =>
                            (float)
                            source.CalcDamage(
                                target,
                                DamageType.Magical,
                                6d + source.Level + 0.1d * source.TotalMagicalDamage),
                    };
            AttackPassives.Add(p);

            #endregion

            #region Ashe

            p = new PassiveDamage
                    {
                        ChampionName = "Ashe", IsActive = (source, target) => target.HasBuff("ashepassiveslow"),
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Physical,
                                source.TotalAttackDamage * (0.1 + (source.Crit * (1 + source.CritDamageMultiplier))))
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = "Ashe", IsActive = (source, target) => source.HasBuff("asheqattack"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.Q)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Bard

            p = new PassiveDamage
                    {
                        ChampionName = "Bard",
                        IsActive = (source, target) => source.GetBuffCount("bardpspiritammocount") > 0,
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Magical,
                                new[] { 30, 55, 80, 110, 140, 175, 210, 245, 280, 315, 345, 375, 400, 425, 445, 465 }[
                                    Math.Min(source.GetBuffCount("bardpdisplaychimecount") / 10, 15)]
                                + (source.GetBuffCount("bardpdisplaychimecount") > 150
                                       ? Math.Truncate((source.GetBuffCount("bardpdisplaychimecount") - 150) / 5d) * 20
                                       : 0) + 0.3 * source.TotalMagicalDamage)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Blatzcrink

            p = new PassiveDamage
                    {
                        ChampionName = "Blitzcrank", IsActive = (source, target) => source.HasBuff("PowerFist"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.E)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Braum

            p = new PassiveDamage
                    {
                        ChampionName = "Braum", IsActive = (source, target) => source.HasBuff("braummarkstunreduction"),
                        GetDamage =
                            (source, target) => source.CalcDamage(target, DamageType.Magical, 6.4 + (1.6 * source.Level))
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = string.Empty, IsActive = (source, target) => target.GetBuffCount("braummark") == 3,
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Magical,
                                32 + (8 * ((Obj_AI_Hero)target.GetBuff("braummark").Caster).Level))
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

            #region ChoGath

            p = new PassiveDamage
                    {
                        ChampionName = "ChoGath", IsActive = (source, target) => source.HasBuff("VorpalSpikes"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.E)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Darius

            p = new PassiveDamage
                    {
                        ChampionName = "Darius", IsActive = (source, target) => true,
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Physical,
                                ((9 + source.Level + (source.FlatPhysicalDamageMod * 0.3))
                                 * Math.Min(target.GetBuffCount("dariushemo") + 1, 5))
                                * (target.Type == GameObjectType.obj_AI_Minion ? 0.25 : 1))
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = "Darius", IsActive = (source, target) => source.HasBuff("DariusNoxianTacticsONH"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.W)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Dianna

            p = new PassiveDamage
                    {
                        ChampionName = "Diana", IsActive = (source, target) => source.HasBuff("dianaarcready"),
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Magical,
                                15
                                + ((source.Level < 6
                                        ? 5
                                        : (source.Level < 11
                                               ? 10
                                               : (source.Level < 14 ? 15 : (source.Level < 16 ? 20 : 25)))) * source.Level)
                                + (source.TotalMagicalDamage * 0.8))
                    };

            #endregion

            #region DrMundo

            p = new PassiveDamage
                    {
                        ChampionName = "DrMundo", IsActive = (source, target) => source.HasBuff("Masochism"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.E)
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

            #region Ekko

            p = new PassiveDamage
                    {
                        ChampionName = "Ekko", IsActive = (source, target) => (target.GetBuffCount("EkkoStacks") == 2),
                        GetDamage =
                            (source, target) =>
                            (float)
                            source.CalcDamage(
                                target,
                                DamageType.Magical,
                                10 + (source.Level * 10) + (source.TotalMagicalDamage * 0.8)),
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = "Ekko", IsActive = (source, target) => (target.HealthPercent < 30),
                        GetDamage = (source, target) =>
                            {
                                var dmg =
                                    (float)
                                    source.CalcDamage(
                                        target,
                                        DamageType.Magical,
                                        (target.MaxHealth - target.Health)
                                        * (5 + Math.Floor(source.TotalMagicalDamage / 100) * 2.2f) / 100);
                                if (!(target is Obj_AI_Hero) && dmg > 150f) dmg = 150f;
                                return dmg;
                            }
                    };
            AttackPassives.Add(p);

            #endregion

            #region Fizz

            p = new PassiveDamage
                    {
                        ChampionName = "Fizz", IsActive = (source, target) => source.GetSpell(SpellSlot.W).Level > 0,
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.W) / 6
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = "Fizz", IsActive = (source, target) => source.HasBuff("FizzSeastonePassive"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.W)
                    };

            AttackPassives.Add(p);

            #endregion

            /*
            #region Gangplank

            p = new PassiveDamage
            {
                ChampionName = "Gangplank",
                IsActive = (source, target) => source.HasBuff("gangplankpassiveattack"),
                GetDamage =
                    (source, target) =>
                    source.CalcDamage(
                        target,
                        DamageType.True,
                        20 + (10 * source.Level) + source.FlatPhysicalDamageMod)
            };

            AttackPassives.Add(p);

            #endregion
*/

            #region Garen

            p = new PassiveDamage
                    {
                        ChampionName = "Garen", IsActive = (source, target) => source.HasBuff("GarenQ"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.Q)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Gnar

            p = new PassiveDamage
                    {
                        ChampionName = "Gnar", IsActive = (source, target) => (target.GetBuffCount("gnarwproc") == 2),
                        GetDamage = (source, target) => ((float)source.GetSpellDamage(target, SpellSlot.W)),
                    };
            AttackPassives.Add(p);

            #endregion

            #region Gragas

            p = new PassiveDamage
                    {
                        ChampionName = "Gragas", IsActive = (source, target) => source.HasBuff("gragaswattackbuff"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.W)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Graves

            p = new PassiveDamage
                    {
                        ChampionName = "Graves", IsActive = (source, target) => true,
                        GetDamage =
                            (source, target) =>
                            (float)
                            (((72 + 3 * source.Level) / 100f)
                             * source.CalcDamage(target, DamageType.Physical, source.TotalAttackDamage)
                             - source.CalcDamage(target, DamageType.Physical, source.TotalAttackDamage)),
                    };
            AttackPassives.Add(p);

            #endregion

            #region Hecarim

            p = new PassiveDamage
                    {
                        ChampionName = "Hecarim", IsActive = (source, target) => source.HasBuff("hecarimrampspeed"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.E)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Illaoi

            p = new PassiveDamage
                    {
                        ChampionName = "Illaoi", IsActive = (source, target) => source.HasBuff("IllaoiW"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.W)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Irelia

            p = new PassiveDamage
                    {
                        ChampionName = "Irelia", IsActive = (source, target) => source.HasBuff("ireliahitenstylecharged"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.W)
                    };

            AttackPassives.Add(p);

            #endregion

            #region JarvanIV

            p = new PassiveDamage
                    {
                        ChampionName = "JarvanIV",
                        IsActive = (source, target) => !target.HasBuff("jarvanivmartialcadencecheck"),
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(target, DamageType.Physical, Math.Min(target.Health * 0.1, 400))
                    };

            AttackPassives.Add(p);

            #endregion

            #region Jax

            p = new PassiveDamage
                    {
                        ChampionName = "Jax", IsActive = (source, target) => source.HasBuff("JaxEmpowerTwo"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.W)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Jayce

            p = new PassiveDamage
                    {
                        ChampionName = "Jayce",
                        IsActive =
                            (source, target) =>
                            Math.Abs(source.Crit - 1) < float.Epsilon && !source.HasBuff("jaycehypercharge"),
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Physical,
                                source.GetCritMultiplier() * source.TotalAttackDamage)
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = "Jayce", IsActive = (source, target) => source.HasBuff("jaycehypercharge"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.W, 1)
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = "Jayce", IsActive = (source, target) => source.HasBuff("jaycepassivemeleeattack"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.R)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Jhin

            p = new PassiveDamage
                    {
                        ChampionName = "Jhin", IsActive = (source, target) => (source.HasBuff("jhinpassiveattackbuff")),
                        GetDamage =
                            (source, target) =>
                            ((float)
                             source.CalcDamage(
                                 target,
                                 DamageType.Physical,
                                 source.TotalAttackDamage * 0.5f
                                 + (target.MaxHealth - target.Health)
                                 * new float[] { 0.15f, 0.20f, 0.25f }[Math.Min(2, (source.Level - 1) / 5)])),
                    };
            AttackPassives.Add(p);

            p = new PassiveDamage()
                    {
                        ChampionName = "Jhin", IsActive = (source, target) => Math.Abs(source.Crit - 1) < float.Epsilon,
                        GetDamage =
                            (source, target) =>
                            (float)
                            source.CalcDamage(
                                target,
                                DamageType.Physical,
                                (Items.HasItem((int)ItemId.Infinity_Edge, source) ? 0.875 : 0.5)
                                * (source.TotalAttackDamage
                                   * (1
                                      + (new[] { 2, 3, 4, 5, 6, 7, 8, 10, 12, 14, 16, 18, 20, 24, 28, 32, 36, 40 }[
                                          source.Level - 1] + (Math.Floor(source.Crit * 100 / 10) * 4)
                                         + (Math.Floor((source.AttackSpeedMod - 1) * 100 / 10) * 2.5)) / 100)))
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

            #region Kalista

            p = new PassiveDamage
                    {
                        ChampionName = "Kalista",
                        IsActive = (source, target) => target.HasBuff("kalistacoopstrikemarkally"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.W)
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = string.Empty,
                        IsActive =
                            (source, target) =>
                            target.HasBuff("kalistacoopstrikemarkbuff") && source.HasBuff("kalistacoopstrikeally"),
                        GetDamage =
                            (source, target) =>
                            ((Obj_AI_Hero)target.GetBuff("kalistacoopstrikemarkbuff").Caster).GetSpellDamage(
                                target,
                                SpellSlot.W)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Kassadin

            p = new PassiveDamage
                    {
                        ChampionName = "Kassadin", IsActive = (source, target) => source.GetSpell(SpellSlot.W).Level > 0,
                        GetDamage =
                            (source, target) =>
                            source.GetSpellDamage(target, SpellSlot.W, source.HasBuff("NetherBlade") ? 1 : 0)
                    };

            #endregion

            #region Katarina

            p = new PassiveDamage
                    {
                        ChampionName = "Katarina", IsActive = (source, target) => (target.HasBuff("katarinaqmark")),
                        GetDamage = (source, target) => ((float)source.GetSpellDamage(target, SpellSlot.Q, 1)),
                    };
            AttackPassives.Add(p);

            #endregion

            #region Kayle

            p = new PassiveDamage
                    {
                        ChampionName = "Kayle", IsActive = (source, target) => source.GetSpell(SpellSlot.E).Level > 0,
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.E)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Kennen

            p = new PassiveDamage
                    {
                        ChampionName = "Kennen", IsActive = (source, target) => source.HasBuff("kennendoublestrikelive"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.W)
                    };

            AttackPassives.Add(p);

            #endregion

            #region KhaZix

            p = new PassiveDamage
                    {
                        ChampionName = "KhaZix",
                        IsActive =
                            (source, target) =>
                            source.HasBuff("khazixpdamage") && target.Type == GameObjectType.obj_AI_Hero,
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Physical,
                                10
                                + ((source.Level < 6 ? 5 : (source.Level < 11 ? 10 : (source.Level < 14 ? 15 : 20)))
                                   * source.Level) + (0.5 * source.TotalMagicalDamage))
                    };

            AttackPassives.Add(p);

            #endregion

            #region Kindred

            p = new PassiveDamage
                    {
                        ChampionName = "Kindred",
                        IsActive =
                            (source, target) =>
                            source.HasBuff("KindredLegendPassive")
                            && source.GetBuffCount("kindredmarkofthekindredstackcounter") > 0,
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Physical,
                                Math.Min(
                                    (0.125 * source.GetBuffCount("kindredmarkofthekindredstackcounter")) * target.Health,
                                    target is Obj_AI_Minion
                                        ? 75 + (10 * source.GetBuffCount("kindredmarkofthekindredstackcounter"))
                                        : target.MaxHealth))
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

            #region Leona

            p = new PassiveDamage
                    {
                        ChampionName = string.Empty,
                        IsActive =
                            (source, target) =>
                            target.HasBuff("leonasunlight")
                            && target.GetBuff("leonasunlight").Caster.NetworkId != source.NetworkId,
                        GetDamage = (source, target) =>
                            {
                                var lvl = ((Obj_AI_Hero)target.GetBuff("leonasunlight").Caster).Level - 1;
                                if ((lvl / 2) % 1 > 0)
                                {
                                    lvl -= 1;
                                }
                                return source.CalcDamage(target, DamageType.Magical, 20 + (15 * lvl / 2));
                            }
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = "Leona", IsActive = (source, target) => source.HasBuff("LeonaShieldOfDaybreak"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.Q)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Lucian

            p = new PassiveDamage
                    {
                        ChampionName = "Lucian", IsActive = (source, target) => source.HasBuff("lucianpassivebuff"),
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Physical,
                                ((target.Type == GameObjectType.obj_AI_Minion
                                      ? 1
                                      : (source.Level < 6
                                             ? 0.3
                                             : (source.Level < 11 ? 0.4 : (source.Level < 16 ? 0.5 : 0.6))))
                                 * source.TotalAttackDamage) * source.GetCritMultiplier(true))
                    };

            AttackPassives.Add(p);

            #endregion

            #region Lux

            p = new PassiveDamage
                    {
                        ChampionName = "Lux", IsActive = (source, target) => target.HasBuff("LuxIlluminatingFraulein"),
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Magical,
                                10 + (8 * source.Level) + (0.2 * source.TotalMagicalDamage))
                    };

            AttackPassives.Add(p);

            #endregion

            #region Malphite

            p = new PassiveDamage
                    {
                        ChampionName = "Malphite", IsActive = (source, target) => source.HasBuff("malphitecleave"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.W)
                    };

            AttackPassives.Add(p);

            #endregion

            #region MasterYi

            p = new PassiveDamage
                    {
                        ChampionName = "MasterYi", IsActive = (source, target) => source.HasBuff("doublestrike"),
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Physical,
                                (0.5 * source.TotalAttackDamage) * source.GetCritMultiplier(true))
                    };

            AttackPassives.Add(p);

            #endregion

            #region MonkeyKing

            p = new PassiveDamage
                    {
                        ChampionName = "MonkeyKing",
                        IsActive = (source, target) => source.HasBuff("MonkeyKingDoubleAttack"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.Q)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Mordekaiser

            p = new PassiveDamage
                    {
                        ChampionName = "Mordekaiser",
                        IsActive = (source, target) => source.Buffs.Any(x => x.Name.Contains("mordekaisermaceofspades")),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.Q)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Nami

            p = new PassiveDamage
                    {
                        ChampionName = string.Empty, IsActive = (source, target) => source.HasBuff("NamiE"),
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Magical,
                                new[] { 25, 40, 55, 70, 85 }[
                                    ((Obj_AI_Hero)source.GetBuff("NamiE").Caster).Spellbook.GetSpell(SpellSlot.E).Level
                                    - 1] + (0.2 * ((Obj_AI_Hero)source.GetBuff("NamiE").Caster).TotalMagicalDamage))
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

            #region Nautilus

            p = new PassiveDamage
                    {
                        ChampionName = "Nautilus", IsActive = (source, target) => !target.HasBuff("nautiluspassivecheck"),
                        GetDamage =
                            (source, target) => source.CalcDamage(target, DamageType.Magical, 2 + (6 * source.Level))
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = "Nautilus",
                        IsActive = (source, target) => source.HasBuff("nautiluspiercinggazeshield"),
                        GetDamage =
                            (source, target) =>
                            source.GetSpellDamage(target, SpellSlot.W)
                            / (target.Type == GameObjectType.obj_AI_Hero ? 1 : 2)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Nidalee

            p = new PassiveDamage
                    {
                        ChampionName = "Nidalee", IsActive = (source, target) => source.HasBuff("Takedown"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.Q, 1)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Noctune

            p = new PassiveDamage
                    {
                        ChampionName = "Nocturne", IsActive = (source, target) => source.HasBuff("nocturneumbrablades"),
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(target, DamageType.Physical, 0.2 * source.TotalAttackDamage)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Nunu

            p = new PassiveDamage
                    {
                        ChampionName = "Nunu", IsActive = (source, target) => source.HasBuff("nunuqbufflizard"),
                        GetDamage =
                            (source, target) => source.CalcDamage(target, DamageType.Magical, 0.01 * source.MaxHealth)
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

            #region Pantheon

            p = new PassiveDamage
                    {
                        ChampionName = "Pantheon",
                        IsActive =
                            (source, target) =>
                            (target.HealthPercent < 15 && source.Spellbook.GetSpell(SpellSlot.E).Level > 0)
                            || Math.Abs(source.Crit - 1) < float.Epsilon,
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Physical,
                                source.GetCritMultiplier() * source.TotalAttackDamage)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Poppy

            p = new PassiveDamage
                    {
                        ChampionName = "Poppy", IsActive = (source, target) => source.HasBuff("PoppyPassiveBuff"),
                        GetDamage =
                            (source, target) => source.CalcDamage(target, DamageType.Physical, 10 + (10 * source.Level))
                    };

            AttackPassives.Add(p);

            #endregion

            #region Quinn

            p = new PassiveDamage
                    {
                        ChampionName = "Quinn", IsActive = (source, target) => (target.HasBuff("quinnw")),
                        GetDamage =
                            (source, target) =>
                            ((float)source.CalcDamage(target, DamageType.Physical, 0.5d * source.TotalAttackDamage)),
                    };
            AttackPassives.Add(p);

            #endregion

            #region RekSai

            p = new PassiveDamage
                    {
                        ChampionName = "RekSai", IsActive = (source, target) => source.HasBuff("RekSaiq"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.Q)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Renekton

            p = new PassiveDamage
                    {
                        ChampionName = "Renekton", IsActive = (source, target) => source.HasBuff("RenektonPreExecute"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.Q)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Rengar

            p = new PassiveDamage
                    {
                        ChampionName = "Rengar", IsActive = (source, target) => source.HasBuff("rengarqbase"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.Q)
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = "Rengar", IsActive = (source, target) => source.HasBuff("rengarqemp"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.Q, 1)
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
                                 (source.Level < 3
                                      ? 0.25
                                      : (source.Level < 6
                                             ? 0.29167
                                             : (source.Level < 9
                                                    ? 0.3333
                                                    : (source.Level < 12
                                                           ? 0.375
                                                           : (source.Level < 15
                                                                  ? 0.4167
                                                                  : (source.Level < 18 ? 0.4583 : 0.5))))))
                                 * source.TotalAttackDamage)),
                    };

            AttackPassives.Add(p);

            #endregion

            #region Rumble

            p = new PassiveDamage
                    {
                        ChampionName = "Rumble", IsActive = (source, target) => source.HasBuff("rumbleoverheat"),
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Magical,
                                0 + (5 * source.Level) + (0.3 * source.TotalMagicalDamage))
                    };

            AttackPassives.Add(p);

            #endregion

            #region Sejuani

            p = new PassiveDamage
                    {
                        ChampionName = "Sejuani",
                        IsActive = (source, target) => source.HasBuff("sejuaninorthernwindsenrage"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.W)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Shaco

            p = new PassiveDamage
                    {
                        ChampionName = "Shaco",
                        IsActive =
                            (source, target) => Math.Abs(source.Crit - 1) < float.Epsilon && !source.HasBuff("Deceive"),
                        GetDamage = (source, target) => source.GetCritMultiplier() * source.TotalAttackDamage
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = "Shaco",
                        IsActive = (source, target) => source.IsFacing(target) && !source.IsFacing(target),
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Physical,
                                (source.TotalAttackDamage * 0.2) * source.GetCritMultiplier(true))
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = "Shaco", IsActive = (source, target) => source.HasBuff("Deceive"),
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Physical,
                                (source.GetCritMultiplier()
                                 + new[] { -0.6, -0.4, -0.2, 0, 0.2 }[source.Spellbook.GetSpell(SpellSlot.Q).Level - 1])
                                * source.TotalAttackDamage)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Shen

            p = new PassiveDamage
                    {
                        ChampionName = "Shen", IsActive = (source, target) => source.HasBuff("shenqbuff"),
                        GetDamage = (source, target) =>
                            {
                                double dmg = 0;
                                if (source.HasBuff("shenqbuffweak"))
                                {
                                    dmg = source.GetSpellDamage(target, SpellSlot.Q);
                                }
                                if (source.HasBuff("shenqbuffstrong"))
                                {
                                    dmg = source.GetSpellDamage(target, SpellSlot.Q, 1);
                                }
                                return dmg;
                            }
                    };

            AttackPassives.Add(p);

            #endregion

            #region Shyvana

            p = new PassiveDamage
                    {
                        ChampionName = "Shyvana", IsActive = (source, target) => source.HasBuff("ShyvanaDoubleAttack"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.Q)
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = "Shyvana",
                        IsActive =
                            (source, target) =>
                            source.HasBuff("ShyvanaImmolationAura") || source.HasBuff("shyvanaimmolatedragon"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.W) / 4
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = "Shyvana", IsActive = (source, target) => target.HasBuff("ShyvanaFireballMissile"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.E, 1)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Sion

            p = new PassiveDamage
                    {
                        ChampionName = "Sion", IsActive = (source, target) => source.HasBuff("sionpassivezombie"),
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Physical,
                                Math.Min(
                                    0.1 * target.MaxHealth,
                                    target.Type == GameObjectType.obj_AI_Minion ? 75 : target.MaxHealth))
                    };

            AttackPassives.Add(p);

            #endregion

            #region Skarner

            p = new PassiveDamage
                    {
                        ChampionName = "Skarner", IsActive = (source, target) => target.HasBuff("skarnerpassivebuff"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.E, 1)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Sona

            p = new PassiveDamage
                    {
                        ChampionName = "Sona", IsActive = (source, target) => source.HasBuff("SonaPassiveReady"),
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Magical,
                                (6
                                 + ((source.Level < 4
                                         ? 7
                                         : (source.Level < 6 ? 8 : (source.Level < 7 ? 9 : (source.Level < 15 ? 10 : 15))))
                                    * source.Level)) + (0.2 * target.TotalMagicalDamage))
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = "Sona", IsActive = (source, target) => source.HasBuff("SonaQProcAttacker"),
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Physical,
                                new[] { 20, 30, 40, 50, 60 }[
                                    ((Obj_AI_Hero)source.GetBuff("SonaQProcAttacker").Caster).Spellbook.GetSpell(
                                        SpellSlot.Q).Level - 1]
                                + (0.2 * ((Obj_AI_Hero)source.GetBuff("SonaQProcAttacker").Caster).TotalMagicalDamage)
                                + new[] { 0, 10, 20, 30 }[
                                    ((Obj_AI_Hero)source.GetBuff("SonaQProcAttacker").Caster).Spellbook.GetSpell(
                                        SpellSlot.R).Level])
                    };

            AttackPassives.Add(p);

            #endregion

            #region TahmKench

            p = new PassiveDamage
                    {
                        ChampionName = "TahmKench", IsActive = (source, target) => source.GetSpell(SpellSlot.R).Level > 0,
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.R)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Talon

            p = new PassiveDamage
                    {
                        ChampionName = "Talon",
                        IsActive =
                            (source, target) =>
                            target.HasBuffOfType(BuffType.Slow) || target.HasBuffOfType(BuffType.Stun)
                            || target.HasBuffOfType(BuffType.Snare) || target.HasBuffOfType(BuffType.Suppression),
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Physical,
                                (source.TotalAttackDamage * 0.1) * source.GetCritMultiplier(true))
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = "Talon", IsActive = (source, target) => source.HasBuff("talonnoxiandiplomacybuff"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.Q)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Taric

            p = new PassiveDamage
                    {
                        ChampionName = "Taric", IsActive = (source, target) => source.HasBuff("taricgemcraftbuff"),
                        GetDamage = (source, target) => source.CalcDamage(target, DamageType.Magical, source.Armor * 0.2)
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

            #region Thresh

            p = new PassiveDamage
                    {
                        ChampionName = "Thresh",
                        IsActive = (source, target) => source.Buffs.Any(x => x.Name.Contains("threshqpassive")),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.E, 1)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Tristana

            p = new PassiveDamage
                    {
                        ChampionName = "Tristana",
                        IsActive = (source, target) => target.GetBuffCount("tristanaecharge") == 3,
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.E)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Trundle

            p = new PassiveDamage
                    {
                        ChampionName = "Trundle", IsActive = (source, target) => source.HasBuff("TrundleTrollSmash"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.Q)
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

            #region Twitch

            /*
            p = new PassiveDamage
            {
                ChampionName = "Twitch",
                IsActive = (source, target) => true,
                GetDamage =
                    (source, target) =>
                    source.CalcDamage(
                        target,
                        DamageType.True,
                        (source.Level < 5
                             ? 12
                             : (source.Level < 9 ? 18 : (source.Level < 13 ? 24 : (source.Level < 17 ? 30 : 36))))
                        * Math.Min(target.GetBuffCount("twitchdeadlyvenom") + 1, 6)
                        / (target.Type == GameObjectType.obj_AI_Minion ? 1 : 6d))
            };

            AttackPassives.Add(p);
        */

            #endregion

            #region Udyr

            p = new PassiveDamage
                    {
                        ChampionName = "Udyr", IsActive = (source, target) => source.HasBuff("UdyrTigerStance"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.Q)
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
                        IsActive = (source, target) => source.GetBuffCount("vaynesilvereddebuff") == 2,
                        GetDamage = (source, target) => ((float)source.GetSpellDamage(target, SpellSlot.W)),
                    };

            AttackPassives.Add(p);

            #endregion

            #region Vi

            p = new PassiveDamage
                    {
                        ChampionName = "Vi", IsActive = (source, target) => target.GetBuffCount("viwproc") == 2,
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.W)
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = "Vi", IsActive = (source, target) => source.HasBuff("ViE"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.W)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Viktor

            p = new PassiveDamage
                    {
                        ChampionName = "Viktor",
                        IsActive = (source, target) => (source.HasBuff("viktorpowertransferreturn")),
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

            #region Volibear

            p = new PassiveDamage
                    {
                        ChampionName = "Volibear", IsActive = (source, target) => source.HasBuff("VolibearQ"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.Q)
                    };

            AttackPassives.Add(p);

            p = new PassiveDamage
                    {
                        ChampionName = "Volibear", IsActive = (source, target) => source.HasBuff("volibearrapllicator"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.R)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Warwick

            p = new PassiveDamage
                    {
                        ChampionName = "Warwick", IsActive = (source, target) => true,
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Magical,
                                2.5 + (source.Level < 10 ? 0.5 : 1) * source.Level)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Yasuo

            p = new PassiveDamage
                    {
                        ChampionName = "Yasuo", IsActive = (source, target) => Math.Abs(source.Crit - 1) < float.Epsilon,
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Physical,
                                (Items.HasItem((int)ItemId.Infinity_Edge, source) ? 1.25 : 0.8) * source.TotalAttackDamage)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Yorick

            p = new PassiveDamage
                    {
                        ChampionName = "Yorick", IsActive = (source, target) => source.HasBuff("YorickUnholySymbiosis"),
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Physical,
                                (0.05
                                 * MinionManager.GetMinions(float.MaxValue)
                                       .Count(
                                           g =>
                                           g.Team == source.Team
                                           && (g.Name.Equals("Clyde") || g.Name.Equals("Inky") || g.Name.Equals("Blinky")
                                               || (g.HasBuff("yorickunholysymbiosis")
                                                   && g.GetBuff("yorickunholysymbiosis").Caster.NetworkId
                                                   == source.NetworkId)))) * source.TotalAttackDamage)
                    };

            AttackPassives.Add(p);

            #endregion

            #region Zed

            p = new PassiveDamage
                    {
                        ChampionName = "Zed",
                        IsActive = (source, target) => target.HealthPercent < 50 && !target.HasBuff("ZedPassiveCD"),
                        GetDamage =
                            (source, target) =>
                            source.CalcDamage(
                                target,
                                DamageType.Magical,
                                (source.Level < 7 ? 0.06 : (source.Level < 17 ? 0.08 : 0.1)) * target.MaxHealth)
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
                                (float)0.3d * source.TotalMagicalDamage
                                + new float[]
                                      { 20, 24, 28, 32, 36, 40, 48, 56, 64, 72, 80, 88, 100, 112, 124, 136, 148, 160 }[
                                          source.Level - 1]),
                    };

            AttackPassives.Add(p);

            #endregion

            #region XinZhao

            p = new PassiveDamage
                    {
                        ChampionName = "XinZhao", IsActive = (source, target) => source.HasBuff("XenZhaoComboTarget"),
                        GetDamage = (source, target) => source.GetSpellDamage(target, SpellSlot.Q)
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
                                    new double[] { 60, 85, 110, 135, 160 }[level]
                                    + 0.4 * source.TotalMagicalDamage
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
                                     + 0.5 * source.TotalMagicalDamage)
                                    * (target.HasBuff("chilled") ? 2 : 1)
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
                                    new double[] { 150, 275, 400 }[level]
                                    + new double[] { 10, 15, 20 }[level] /* Aura */
                                    + new double[] { 50, 75, 100 }[level] /* Tibbers ʕ•͡ᴥ•ʔ */
                                    + (0.65 + 0.1 + 0.15) * source.TotalMagicalDamage
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
                                    new double[] { 80, 110, 140, 170, 200 }[level]
                                    + 0.55 * source.TotalMagicalDamage
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
                                    new double[] { 70, 90, 110, 130, 150 }[level]
                                    + 0.35 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 100, 200, 300 }[level]
                                    + 0.25 * source.TotalMagicalDamage
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
                                    + new double[] { 1.3, 1.4, 1.5, 1.6, 1.7 }[level]
                                    * (source.TotalAttackDamage)
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
                                    new double[] { 75, 120, 165, 210, 255 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 35, 50, 65, 80 }[level]
                                    + 0.15 * source.TotalMagicalDamage
                            },
                        //E 
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    (48 + 4 * ((Obj_AI_Hero)source).Level)
                                    + 0.1 * source.TotalMagicalDamage
                                    + (target.HasBuffOfType(BuffType.Poison)
                                           ? new double[] { 10, 40, 70, 100, 130 }[level]
                                             + 0.35 * source.TotalMagicalDamage
                                           : 0)
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
                                    new double[] { 70, 115, 150, 205, 250 }[level]
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
                                    new double[] { 30, 45, 60, 75, 90 }[level]
                                    + (1.5 * source.FlatPhysicalDamageMod)
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
                                    new[] { 40, 70, 100, 130, 160 }[level]
                                    + (new[] { 0.5, 1.1, 1.2, 1.3, 1.4 }[level]
                                       * source.TotalAttackDamage)
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
                                    new double[] { 100, 200, 300 }[level]
                                    + 0.75 * source.FlatPhysicalDamageMod
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
                                                        new double[] { 15, 17.5, 20, 22.5, 25 }[level]
                                                        / 100 * target.Health));
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
                                    + new[] { 0.95, 1, 1.05, 1.1, 1.15 }[level]
                                    * source.FlatPhysicalDamageMod
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
                                    new double[] { 50, 70, 90 }[level] + 0.1 * source.TotalMagicalDamage
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
                                    new double[] { 15, 25, 35, 45, 55 }[level]
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
                                    + 8 / 100f * target.MaxHealth + 0.3 * source.TotalMagicalDamage
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
                                    new double[] { 80, 125, 170, 215, 260 }[level]
                                    + new double[] { 0.4, 0.6, 0.8, 1, 1.2 }[level]
                                    * source.FlatPhysicalDamageMod
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
                                    + 1.2 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)
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
                "Jhin",
                new List<DamageSpell>
                    {
                        //Q
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 75, 100, 125, 150 }[level]
                                    + new double[] { 0.3, 0.35, 0.4, 0.45, 0.5 }[level]
                                    * source.FlatPhysicalDamageMod + 0.6 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 85, 120, 155, 190 }[level]
                                    + 0.7 * source.FlatPhysicalDamageMod
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 20, 80, 140, 200, 260 }[level]
                                    + 1.20 * source.FlatPhysicalDamageMod
                                    + 1 * source.TotalMagicalDamage
                            },
                        //R - Normal Shot
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 125, 200 }[level]
                                    + 0.25 * source.FlatPhysicalDamageMod
                                    * (1 + (100 - target.HealthPercent) * 1.02)
                            },
                        //R - Final Shot
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, Stage = 1, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 125, 200 }[level]
                                    + 0.25 * source.FlatPhysicalDamageMod
                                    * (1 + (100 - target.HealthPercent) * 1.02) * 2
                                    + 0.01 * source.FlatCritDamageMod
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
                                Damage = (source, target, level) => 0.1 * source.TotalAttackDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 10, 60, 110, 160, 210 }[level]
                                    + 1.4 * source.FlatPhysicalDamageMod
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
                                    source.HasBuff("judicatorrighteousfury")
                                        ? new double[] { 20, 30, 40, 50, 60 }[level]
                                          + 0.30 * source.TotalMagicalDamage
                                        : new double[] { 10, 15, 20, 25, 30 }[level]
                                          + 0.15 * source.TotalMagicalDamage
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
                                        var dmg = (0.02
                                                   + (Math.Truncate(source.TotalMagicalDamage / 100)
                                                      * 0.75)) * target.MaxHealth;

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
                                     + 0.25 * source.TotalMagicalDamage)
                                    * (target.HealthPercent < 25
                                           ? 3
                                           : (target.HealthPercent < 50 ? 2 : 1))
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
                                    new double[] { 60, 90, 120, 150, 180 }[level]
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
                                    new double[] { 30, 55, 80, 105, 130 }[level]
                                    + 0.3 * source.TotalMagicalDamage
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 60, 100, 140, 180, 220 }[level]
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
                                    new double[] { 100, 175, 250 }[level]
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
                                    new double[] { 50, 100, 150, 200, 250 }[level]
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
                                    new double[] { 70, 110, 150, 190, 230 }[level]
                                    + 0.7 * source.TotalMagicalDamage
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
                                    new double[] { 80, 115, 150, 185, 220 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                        //R
                        new DamageSpell
                            //1.5% of the target’s maximum health per 100 ability power, per second
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    2.5
                                    * (new double[] { 6, 8, 10 }[level] / 100
                                       + 0.015 * source.TotalMagicalDamage / 100) * target.MaxHealth
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
                                Damage = (source, target, level) =>
                                    {
                                        var dmg =
                                            (new double[] { 4, 20, 50, 90 }[
                                                source.Spellbook.GetSpell(SpellSlot.R).Level - 1]
                                             + 0.36 * source.TotalMagicalDamage
                                             + 0.75
                                             * (source.BaseAttackDamage + source.FlatPhysicalDamageMod))
                                            * ((target.MaxHealth - target.Health) / target.MaxHealth
                                               * 1.5 + 1);
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
                                    new double[] { 60, 110, 160, 210 }[
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
                                    new double[] { 35, 55, 75, 95, 115 }[level]
                                    + 0.80 * source.FlatPhysicalDamageMod + 0.07 * target.MaxHealth
                            },
                        //Q - both hits
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Physical,
                                Damage =
                                    (source, target, level) =>
                                    (new double[] { 70, 110, 150, 190, 230 }[level]
                                     + 1.6 * source.FlatPhysicalDamageMod + 0.14 * target.MaxHealth)
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
                                Damage = (source, target, level) =>
                                    {
                                        var damage = (new double[] { 20, 45, 70, 95, 120 }[level]
                                                      + (new double[] { 0.8, 0.9, 1.0, 1.1, 1.2 }[level]
                                                         * source.TotalAttackDamage)
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
                                Damage = (source, target, level) => source.TotalAttackDamage
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
                                    (new double[] { 60, 85, 110, 135, 160, 185 }[level]
                                     + 0.45 * source.TotalMagicalDamage
                                     + 0.03
                                     * (source.MaxMana - 392.4 - 52 * (source as Obj_AI_Hero).Level))
                                    * (1
                                       + (target.HasBuff("RyzeE")
                                              ? new double[] { 40, 55, 70, 85, 100 }[
                                                  ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E)
                                                      .Level - 1] / 100
                                              : 0))
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 100, 120, 140, 160 }[level]
                                    + 0.2 * source.TotalMagicalDamage
                                    + 0.01
                                    * (source.MaxMana - 392.4 - 52 * (source as Obj_AI_Hero).Level)
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 50, 75, 100, 125, 150 }[level]
                                    + 0.3 * source.TotalMagicalDamage
                                    + 0.02
                                    * (source.MaxMana - 392.4 - 52 * (source as Obj_AI_Hero).Level)
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
                                Damage = (source, target, level) =>
                                    {
                                        var dmg = (new double[] { 3, 3.5, 4, 4.5, 5 }[level]
                                                   + 0.015 * source.TotalMagicalDamage)
                                                  * target.MaxHealth / 100;
                                        if (target is Obj_AI_Hero)
                                        {
                                            return dmg;
                                        }
                                        return
                                            Math.Min(
                                                new double[] { 30, 50, 70, 90, 110 }[level] + dmg,
                                                new double[] { 75, 100, 125, 150, 175 }[level]);
                                    }
                            },
                        //Q - Enhanced
                        new DamageSpell
                            {
                                Slot = SpellSlot.Q, Stage = 1, DamageType = DamageType.Magical,
                                Damage = (source, target, level) =>
                                    {
                                        var dmg = (new double[] { 5, 5.5, 6, 6.6, 7 }[level]
                                                   + 0.02 * source.TotalMagicalDamage)
                                                  * target.MaxHealth / 100;
                                        if (target is Obj_AI_Hero)
                                        {
                                            return dmg;
                                        }
                                        return
                                            Math.Min(
                                                new double[] { 30, 50, 70, 90, 110 }[level] + dmg,
                                                new double[] { 75, 100, 125, 150, 175 }[level]);
                                    }
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
                                     + 0.5 * source.TotalMagicalDamage)
                                    + (0.60 * source.TotalAttackDamage)
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
                                    new double[] { 40, 70, 100, 130, 160 }[level]
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
                                    new double[] { 50, 80, 110, 140, 170 }[level]
                                    + 1 * source.TotalMagicalDamage
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
                                     + 0.75 * source.TotalMagicalDamage)
                            },
                        //W
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 80, 120, 160, 200, 240 }[level]
                                    + 0.8 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 115, 160, 205, 250 }[level]
                                    + 0.5 * source.TotalMagicalDamage
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
                                    new double[] { 80, 130, 180, 230, 280 }[level]
                                    + 0.7 * source.TotalMagicalDamage
                            },
                        //W - Devour 
                        new DamageSpell
                            {
                                Slot = SpellSlot.W, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    target is Obj_AI_Minion
                                        ? new double[] { 400, 450, 500, 550, 600 }[level]
                                        : (new double[] { 0.20, 0.23, 0.26, 0.29, 0.32 }[level]
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
                                    new double[] { 10, 20, 30, 40, 50 }[level]
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
                                    (source, target, level) =>
                                    Math.Max(
                                        new double[] { 40, 60, 80, 100, 120 }[level],
                                        (new double[] { 6, 7.5, 9, 10.5, 12 }[level] / 100)
                                        * target.MaxHealth)
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
                                    (source, target, level) => new double[] { 175, 250, 325 }[level]
                                                               // TODO: figure out how fast it scales, 175-350/250-500/325-650 (based on target’s missing health)
                                                               + 0.8 * target.TotalMagicalDamage
                                                               + 0.75 * source.TotalMagicalDamage
                                //0.75 - 1.5 ability power (based on target’s missing health)
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
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 100, 130, 160, 190 }[level]
                                    + 0.3 * source.TotalMagicalDamage
                            },
                        //R - max
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.True,
                                Damage =
                                    (source, target, level) =>
                                    target.HasBuff("velkozresearchedstack")
                                        ? new double[] { 500, 725, 950 }[level]
                                          + 1 * source.TotalMagicalDamage
                                        : source.CalcDamage(
                                            target,
                                            DamageType.Magical,
                                            new double[] { 500, 725, 950 }[level]
                                            + 1 * source.TotalMagicalDamage)
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
                                    new double[] { 60, 80, 100, 120, 140 }[level]
                                    + 0.4 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 70, 110, 150, 190, 230 }[level]
                                    + 0.5 * source.TotalMagicalDamage
                            },
                        //E
                        new DamageSpell
                            {
                                Slot = SpellSlot.E, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 90, 170, 250, 330, 410 }[level]
                                    + 1.2 * source.TotalMagicalDamage
                            },
                        //R - summon damage
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 100, 175, 250 }[level]
                                    + 0.50 * source.TotalMagicalDamage
                            },
                        //R - per bolt
                        new DamageSpell
                            {
                                Slot = SpellSlot.R, Stage = 1, DamageType = DamageType.Magical,
                                Damage =
                                    (source, target, level) =>
                                    new double[] { 150, 250, 350 }[level]
                                    + 0.6 * source.TotalMagicalDamage
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
                                    new double[] { 75, 90, 105, 120, 135 }[level]
                                    + 0.55 * source.TotalMagicalDamage
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
                                    new double[] { 60, 80, 100, 120, 140 }[level]
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
                                    (new double[] { 60, 110, 160, 210, 260 }[level])
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
                                    new double[] { 200, 230, 260 }[level]
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
                                    new double[] { 300, 450, 600 }[level]
                                    + 1.1 * source.TotalMagicalDamage
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
                                    new double[] { 60, 90, 120, 150, 180 }[level]
                                    + 0.55 * source.TotalMagicalDamage
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

        #endregion

        #region Enums

        /// <summary>
        ///     Represents items that deal damage.
        /// </summary>
        public enum DamageItems
        {
            /// <summary>
            ///     The hexgun
            /// </summary>
            Hexgun,

            /// <summary>
            ///     The Dfg
            /// </summary>
            Dfg,

            /// <summary>
            ///     The botrk
            /// </summary>
            Botrk,

            /// <summary>
            ///     The bilgewater
            /// </summary>
            Bilgewater,

            /// <summary>
            ///     The tiamat
            /// </summary>
            Tiamat,

            /// <summary>
            ///     The hydra
            /// </summary>
            Hydra,

            /// <summary>
            ///     The black fire torch
            /// </summary>
            BlackFireTorch,

            /// <summary>
            ///     The oding veils
            /// </summary>
            OdingVeils,

            /// <summary>
            ///     The frost queen claim
            /// </summary>
            FrostQueenClaim,

            /// <summary>
            ///     The liandrys torment
            /// </summary>
            LiandrysTorment,
        }

        /// <summary>
        ///     The type of damage.
        /// </summary>
        public enum DamageType
        {
            /// <summary>
            ///     Physical damage. (AD)
            /// </summary>
            Physical,

            /// <summary>
            ///     Magical damage. (AP)
            /// </summary>
            Magical,

            /// <summary>
            ///     True damage
            /// </summary>
            True
        }

        /// <summary>
        ///     Represnets summoner spells that deal damage.
        /// </summary>
        public enum SummonerSpell
        {
            /// <summary>
            ///     The ignite spell.
            /// </summary>
            Ignite,

            /// <summary>
            ///     The smite spell.
            /// </summary>
            Smite,
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Calculates the damage.
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
        ///     Gets the automatic attack damage.
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
            var k = 1d;
            if (source.CharData.BaseSkinName == "Kalista")
            {
                k = 0.9d;
            }
            if (source.CharData.BaseSkinName == "Kled" && 
                ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "KledRiderQ")
            {
                k = 0.8d;
            }

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
                        p => (p.ChampionName == "" || p.ChampionName == hero.ChampionName) && p.IsActive(hero, target))
                        .Sum(passive => passive.GetDamage(hero, target));

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
                return CalcMixedDamage(source, target, (result - reduction) * k, result * k);
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
        ///     Gets the damage spell.
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
        ///     Gets the damage spell.
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
        ///     Gets the item damage.
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
        ///     Gets the spell damage.
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
        ///     Gets the spell damage.
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
        ///     Gets the summoner spell damage.
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

        #endregion

        #region Methods

        /// <summary>
        ///     Calculates the magic damage.
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
        ///     Calculates the mixed damage.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="amountPhysical">The amount of physical damage after modifiers.</param>
        /// <param name="amountMagic">The amount of magical damage after modifiers.</param>
        /// <param name="magic">% magic dmg</param>
        /// <param name="physical">% physical dmg</param>
        /// <param name="trueDmg">% trueDmg dmg</param>
        /// <returns></returns>
        private static double CalcMixedDamage(
            Obj_AI_Base source,
            Obj_AI_Base target,
            double amountPhysical,
            double amountMagic,
            int magic = 50,
            int physical = 50,
            int trueDmg = 0)
        {
            return CalcMagicDamage(source, target, (amountMagic * magic) / 100)
                   + CalcPhysicalDamage(source, target, (amountPhysical * physical) / 100)
                   + PassiveFlatMod(source, target) + (amountMagic * trueDmg) / 100;
        }

        /// <summary>
        ///     Calculates the physical damage.
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
            else if ((armor * armorPenetrationPercent) - (bonusArmor * (1 - bonusArmorPenetrationMod))
                     - armorPenetrationFlat < 0)
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

        /// <summary>
        ///     Gets the damage reduction modifier.
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

        private static float GetCritMultiplier(this Obj_AI_Hero hero, bool checkCrit = false)
        {
            var crit = Items.HasItem((int)ItemId.Infinity_Edge, hero) ? 1.5f : 1;
            return !checkCrit ? crit : (Math.Abs(hero.Crit - 1) < float.Epsilon ? 1 + crit : 1);
        }

        /// <summary>
        ///     Gets the passive flat modifier.
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

        /// <summary>
        ///     Gets the passive percent modifier.
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

        #endregion

        /// <summary>
        ///     Represents a damage spell that only occurs with a passive.
        /// </summary>
        internal class PassiveDamage
        {
            #region Fields

            /// <summary>
            ///     The champion name
            /// </summary>
            public string ChampionName = "";

            /// <summary>
            ///     The get damage delegate.
            /// </summary>
            public GetDamageD GetDamage;

            /// <summary>
            ///     The is active delegate.
            /// </summary>
            public IsActiveD IsActive;

            #endregion

            #region Delegates

            /// <summary>
            ///     Gets the damage dealts to the unit.
            /// </summary>
            /// <param name="source">The source.</param>
            /// <param name="target">The target.</param>
            /// <returns></returns>
            public delegate double GetDamageD(Obj_AI_Hero source, Obj_AI_Base target);

            /// <summary>
            ///     Gets whether this instance is active.
            /// </summary>
            /// <param name="source">The source.</param>
            /// <param name="target">The target.</param>
            /// <returns></returns>
            public delegate bool IsActiveD(Obj_AI_Hero source, Obj_AI_Base target);

            #endregion
        }
    }
}