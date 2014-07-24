#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace LeagueSharp.Common
{
    public static class Utility
    {
        static Utility()
        {
            if (Common.isInitialized == false)
            {
                Common.InitializeCommonLib();
            }
        }

        /// <summary>
        /// Returns if the target is valid (not dead, targetable, visible...).
        /// </summary>
        public static bool IsValidTarget(this Obj_AI_Base unit, float range = float.MaxValue, bool checkTeam = true)
        {
            if (unit == null || !unit.IsValid || unit.IsDead || !unit.IsVisible || !unit.IsTargetable ||
                unit.IsInvulnerable)
                return false;

            if (checkTeam && unit.Team == ObjectManager.Player.Team)
                return false;

            if (range != float.MaxValue &&
                Vector2.DistanceSquared(ObjectManager.Player.ServerPosition.To2D(), unit.ServerPosition.To2D()) >
                range * range)
                return false;

            return true;
        }

        public static bool LevelUpSpell(this Spellbook book, SpellSlot slot)
        {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);

            bw.Write((byte)0x39);
            bw.Write((uint)ObjectManager.Player.NetworkId);
            switch (slot.ToString())
            {
                case "Q":
                    bw.Write(0);
                    break;
                case "W":
                    bw.Write(1);
                    break;
                case "E":
                    bw.Write(2);
                    break;
                case "R":
                    bw.Write(3);
                    break;
                default:
                    return false;
            }

            Game.SendPacket(ms.ToArray(), PacketChannel.C2S, PacketProtocolFlags.NoFlags);
            return true;
        }

        /// <summary>
        /// Returns the path of the unit appending the ServerPosition at the start.
        /// </summary>
        public static List<Vector2> GetWaypoints(this Obj_AI_Base unit)
        {
            var result = new List<Vector2>();
            result.Add(unit.ServerPosition.To2D());
            foreach (var point in unit.Path)
            {
                result.Add(point.To2D());
            }
            return result;
        }

        /// <summary>
        /// Returns if the unit has the buff and it is active
        /// </summary>
        public static bool HasBuff(this Obj_AI_Base unit, string buffName, bool dontUseDisplayName = false)
        {
            foreach (var buff in unit.Buffs)
            {
                if (((!dontUseDisplayName && buff.DisplayName == buffName) ||
                     (dontUseDisplayName && buff.Name == buffName)) && buff.IsActive && buff.EndTime - Game.Time >= 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the spell slot with the name.
        /// </summary>
        public static SpellSlot GetSpellSlot(this Obj_AI_Hero unit, string name, bool searchInSummoners = true)
        {
            foreach (var spell in unit.Spellbook.Spells)
            {
                if (spell.Name == name)
                    return spell.Slot;
            }

            if (searchInSummoners)
            {
                foreach (var spell in unit.SummonerSpellbook.Spells)
                {
                    if (spell.Name == name)
                        return spell.Slot;
                }
            }

            return SpellSlot.Unknown;
        }

        /// <summary>
        /// Returns true if Player is under tower range.
        /// </summary>
        public static bool UnderTurret()
        {
            return UnderTurret(ObjectManager.Player, true);
        }

        /// <summary>
        /// Returns true if the unit is under tower range.
        /// </summary>
        public static bool UnderTurret(Obj_AI_Base unit)
        {
            return UnderTurret(unit, true);
        }

        /// <summary>
        /// Returns true if the unit is under turret range.
        /// </summary>
        public static bool UnderTurret(Obj_AI_Base unit, bool enemyTurretsOnly)
        {
            foreach (var turret in ObjectManager.Get<Obj_AI_Turret>())
            {
                if (enemyTurretsOnly)
                {
                    if (turret != null && turret.IsValid && turret.IsEnemy && turret.Health > 0)
                    {
                        if (Vector2.Distance(unit.Position.To2D(), turret.Position.To2D()) < 950)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (turret != null && turret.IsValid && turret.Health > 0)
                    {
                        if (Vector2.Distance(unit.Position.To2D(), turret.Position.To2D()) < 950)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Counts the enemies in range of Player.
        /// </summary>
        public static int CountEnemysInRange(int range)
        {
            return CountEnemysInRange(range, ObjectManager.Player);
        }

        /// <summary>
        /// Counts the enemies in range of Unit.
        /// </summary>
        public static int CountEnemysInRange(int range, Obj_AI_Base unit)
        {
            return
                ObjectManager.Get<Obj_AI_Hero>()
                    .Where(units => units.IsValidTarget())
                    .Count(units => Vector2.Distance(unit.Position.To2D(), units.Position.To2D()) <= range);
        }

        /// <summary>
        /// Returns true if Player is in shop range.
        /// </summary>
        /// <returns></returns>
        public static bool InShopRange()
        {
            return
                ObjectManager.Get<Obj_Shop>()
                    .Where(shop => shop.IsAlly)
                    .Any(shop => Vector2.Distance(ObjectManager.Player.Position.To2D(), shop.Position.To2D()) < 1000);
        }

        /// <summary>
        /// Returns the cursor position on the screen.
        /// </summary>
        public static Vector2 GetCursorPos()
        {
            var rpos = Drawing.WorldToScreen(Game.CursorPos);
            var de = Drawing.Direct3DDevice;

            return new Vector2(rpos[0], rpos[1]);
        }

        /// <summary>
        /// Returns true if the point is under the rectangle
        /// </summary>
        public static bool IsUnderRectangle(Vector2 point, float x, float y, float width, float height)
        {
            return (point.X > x && point.X < x + width && point.Y > y && point.Y < y + height);
        }


        public static string KeyToText(uint vKey)
        {
            /*A-Z */
            if (vKey >= 65 && vKey <= 90)
                return ("" + (char)vKey);

            /*F1-F12*/
            if (vKey >= 112 && vKey <= 123)
                return ("F" + (vKey - 111));

            switch (vKey)
            {
                case 9:
                    return "Tab";
                case 16:
                    return "Shift";
                case 17:
                    return "Ctrl";
                case 20:
                    return "CAPS";
                case 27:
                    return "ESC";

                case 32:
                    return "Space";
                case 45:
                    return "Insert";
                case 220:
                    return "º";
                default:
                    return vKey.ToString();
            }
        }

        /// <summary>
        /// Returns the md5 hash from a string.
        /// </summary>
        public static string Md5Hash(string s)
        {
            var sb = new StringBuilder();
            HashAlgorithm algorithm = MD5.Create();
            var h = algorithm.ComputeHash(Encoding.UTF8.GetBytes(s));

            foreach (var b in h)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }


        /// <summary>
        /// Draws a "lag-free" circle
        /// </summary>
        public static void DrawCircle(Vector3 center, float radius, Color color, int thickness = 2, int quality = 30,
            bool onMinimap = false)
        {
            var pointList = new List<Vector3>();
            for (var i = 0; i < quality; i++)
            {
                var angle = i * Math.PI * 2 / quality;
                pointList.Add(new Vector3(center.X + radius * (float)Math.Cos(angle),
                    center.Y + radius * (float)Math.Sin(angle), center.Z));
            }

            for (var i = 0; i < pointList.Count; i++)
            {
                var A = pointList[i];
                var B = pointList[i == pointList.Count - 1 ? 0 : i + 1];

                float[] AonScreen;
                float[] BonScreen;
                if (onMinimap)
                {
                    AonScreen = Drawing.WorldToMinimap(A);
                    BonScreen = Drawing.WorldToMinimap(B);
                }
                else
                {
                    AonScreen = Drawing.WorldToScreen(A);
                    BonScreen = Drawing.WorldToScreen(B);
                }

                if (onMinimap ||
                    (AonScreen[0] < Drawing.Width * 1.5 && AonScreen[0] > -Drawing.Width * 1.5 &&
                     BonScreen[0] < Drawing.Width * 1.5 && BonScreen[0] > -Drawing.Width * 1.5 &&
                     AonScreen[1] < Drawing.Height * 1.5 && BonScreen[1] > -Drawing.Height * 1.5 &&
                     BonScreen[1] < Drawing.Height * 1.5 && BonScreen[1] > -Drawing.Height * 1.5
                        ))
                {
                    Drawing.DrawLine(AonScreen[0], AonScreen[1], BonScreen[0], BonScreen[1], thickness, color);
                }
            }
        }


        public static class DelayAction
        {
            public delegate void Callback();

            public static List<Action> actionList = new List<Action>();

            static DelayAction()
            {
                Game.OnGameUpdate += GameOnOnGameUpdate;
            }

            private static void GameOnOnGameUpdate(EventArgs args)
            {
                for (var i = actionList.Count - 1; i >= 0; i--)
                {
                    if (actionList[i].Time <= Environment.TickCount)
                    {
                        actionList[i].CallbackObject();
                        actionList.RemoveAt(i);
                    }
                }
            }

            public static void Add(int time, Callback func)
            {
                var action = new Action(time, func);
                actionList.Add(action);
            }

            public struct Action
            {
                public Callback CallbackObject;
                public int Time;

                public Action(int time, Callback callback)
                {
                    Time = time + Environment.TickCount;
                    CallbackObject = callback;
                }
            }
        }
    }
}