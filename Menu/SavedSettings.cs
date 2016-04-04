namespace LeagueSharp.Common
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Security.Permissions;
    using System.Text;

    /// <summary>
    /// The menu settings manager (serialization, saving, etc.)
    /// /// </summary>
    [Serializable]
    internal static class SavedSettings
    {
        #region Static Fields

        /// <summary>
        ///     The loaded files collection.
        /// </summary>
        public static Dictionary<string, Dictionary<string, byte[]>> LoadedFiles =
            new Dictionary<string, Dictionary<string, byte[]>>();

        /// <summary>
        ///     The loaded configurations collection.
        /// </summary>
        public static Dictionary<string, Dictionary<string, MenuItem>> LoadedJSONS = new Dictionary<string, Dictionary<string, MenuItem>>();


        public static Dictionary<string, ReadFormat> FileFormat = new Dictionary<string, ReadFormat>();

        public enum ReadFormat
        {
            JSON,
            BIN
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the saved data.
        /// </summary>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <returns>
        ///     The <see cref="byte" /> collection of the data.
        /// </returns>
        public static byte[] GetSavedData(string name, string key)
        {
            var dic = LoadedFiles.ContainsKey(name) ? LoadedFiles[name] : LoadBIN(name);
            return dic == null ? null : dic.ContainsKey(key) ? dic[key] : null;
        }

        public static Dictionary<string, byte[]> GetDictionary(string name)
        {
            var dic = LoadedFiles.ContainsKey(name) ? LoadedFiles[name] : LoadBIN(name);
            return dic;
        }


        [PermissionSet(SecurityAction.Assert, Unrestricted = true)]
        public static void SaveToJSON(Menu menu)
        {
            try
            {
                menu.RecursiveSaveAll(ref LoadedJSONS);
                Directory.CreateDirectory(MenuSettings.MenuConfigPath);
                if (LoadedJSONS.ContainsKey("SharedConfig"))
                {
                    string shared = JsonConvert.SerializeObject(LoadedJSONS["SharedConfig"], Formatting.Indented);
                    File.WriteAllText(Path.Combine(MenuSettings.MenuConfigPath, "SharedConfig.json"), shared);
                }
                MenuJSON jsonstructure = new MenuJSON(menu);
                string json = JsonConvert.SerializeObject(jsonstructure, Formatting.Indented);
                File.WriteAllText(Path.Combine(MenuSettings.MenuConfigPath, menu.configName + ".json"), json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        /// <summary>
        ///     Loads the specific entry.
        /// </summary>
        /// <param name="name">
        ///     The name of the entry.
        /// </param>
        /// <returns>
        ///     The <see cref="Dictionary{TKey,TValue}" /> collection of the entry contents.
        /// </returns>
        public static Dictionary<string, byte[]> LoadBIN(string name)
        {
            if (LoadedFiles.ContainsKey(name))
            {
                return LoadedFiles[name];
            }

            try
            {
                var fileName = Path.Combine(MenuSettings.MenuConfigPath, name + ".bin");
                if (File.Exists(fileName))
                {
                    if (FileFormat.ContainsKey(name))
                    {
                        FileFormat[name] = ReadFormat.BIN;
                    }
                    else {
                        FileFormat.Add(name, ReadFormat.BIN);
                    }
                    var result = Utils.Deserialize<Dictionary<string, byte[]>>(File.ReadAllBytes(fileName));
                    if (!LoadedFiles.ContainsKey(name))
                    {
                        LoadedFiles.Add(name, result);
                    }
                    else
                    {
                        LoadedFiles[name] = result;
                    }
                    foreach (var item in result)
                    {
                        if (item.Value == null)
                        {
                            continue;
                        }
                        MenuItem mitem = new MenuItem(item.Key, item.Key);
                        ConvertItem(item.Key, item.Value, MenuValueType.KeyBind, ref mitem);
                        if (!LoadedJSONS.ContainsKey(name))
                        {
                            LoadedJSONS.Add(name, new Dictionary<string, MenuItem>());
                        }

                        if (!LoadedJSONS[name].ContainsKey(item.Key))
                        {
                            LoadedJSONS[name].Add(item.Key, mitem);
                        }
                        else {
                            LoadedJSONS[name][item.Key] = mitem;
                        }
                    }
                    Console.WriteLine("Loaded " + name + " using BIN");
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }



        public static MenuItem ConvertItem(string name, byte[] bytes, MenuValueType type, ref MenuItem menuitem)
        {
            try
            {
                Object value = null;
                if (type == MenuValueType.KeyBind)
                {
                    value = Utils.Deserialize<KeyBind>(bytes);
                    if (value != null)
                    {
                        menuitem.value = (KeyBind) value;
                        menuitem.ValueType = MenuValueType.KeyBind;
                        return menuitem;
                    }
                }
                if (type == MenuValueType.StringList)
                {
                    value = Utils.Deserialize<StringList>(bytes);
                    if (value != null)
                    {
                        menuitem.value = (StringList)value;
                        var kk = (StringList)menuitem.value;
                        menuitem.ValueType = MenuValueType.StringList;
                        return menuitem;
                    }
                }
                if (type == MenuValueType.Slider)
                {
                    value = Utils.Deserialize<Slider>(bytes);
                    if (value != null)
                    {
                        menuitem.value = (Slider)value;
                        menuitem.ValueType = MenuValueType.Slider;
                        return menuitem;
                    }
                }
                if (type == MenuValueType.Circle)
                {
                    value = Utils.Deserialize<Circle>(bytes);
                    if (value != null)
                    {
                        menuitem.value = (Circle)value;
                        menuitem.ValueType = MenuValueType.Circle;
                        return menuitem;
                    }
                }

                if (type == MenuValueType.Boolean)
                {
                    value = Utils.Deserialize<bool>(bytes);
                    if (value != null)
                    {
                        menuitem.value = (Boolean) value;
                        menuitem.ValueType = MenuValueType.Boolean;
                        return menuitem;
                    }
                }

                if (type == MenuValueType.Color)
                {
                    value = Utils.Deserialize<Color>(bytes);
                    if (value != null)
                    {
                        menuitem.value = (Color)value;
                        menuitem.ValueType = MenuValueType.Color;
                        return menuitem;
                    }
                }
            }

            catch (InvalidCastException e)
            {
                if (type == MenuValueType.KeyBind)
                {
                    ConvertItem(name, bytes, MenuValueType.Slider, ref menuitem);
                }
                if (type == MenuValueType.Slider)
                {
                    ConvertItem(name, bytes, MenuValueType.StringList, ref menuitem);
                }
                if (type == MenuValueType.StringList)
                {
                    ConvertItem(name, bytes, MenuValueType.Boolean, ref menuitem);
                }
                if (type == MenuValueType.Boolean)
                {
                    ConvertItem(name, bytes, MenuValueType.Color, ref menuitem);
                }
                if (type == MenuValueType.Color)
                {
                    ConvertItem(name, bytes, MenuValueType.Circle, ref menuitem);
                }
            }

            return menuitem;
        }


        [PermissionSet(SecurityAction.Assert, Unrestricted = true)]
        public static void Load(string name)
        {
            try
            {
                if (name == "SharedConfig" && !LoadedJSONS.ContainsKey("SharedConfig"))
                {
                    LoadShared();
                }

                else {

                    var load = LoadJSON(name);
                    if (!load && !LoadedFiles.ContainsKey(name))
                    {
                        LoadBIN(name);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static bool LoadJSON(string name)
        {
            if (LoadedJSONS.ContainsKey(name))
            {
                return true;
            }

            var fileName = Path.Combine(MenuSettings.MenuConfigPath, name + ".json");

            if (File.Exists(fileName))
            {
                string menujson = File.ReadAllText(fileName);

                MenuJSON json = JsonConvert.DeserializeObject<MenuJSON>(menujson);

                Dictionary<string, MenuItem> menuDict = new Dictionary<string, MenuItem>();

                json.Settings.LoadRecursively(ref menuDict);

                if (!LoadedJSONS.ContainsKey(name))
                {
                    LoadedJSONS.Add(name, menuDict);
                }

                else {
                    LoadedJSONS[name] = menuDict;
                }

                if (FileFormat.ContainsKey(name))
                {
                    FileFormat[name] = ReadFormat.JSON;
                }
                else {
                    FileFormat.Add(name, ReadFormat.JSON);
                }

                Console.WriteLine("Loaded " + name + " using JSON");
                return true;
            }
            return false;
        }


        public static void LoadShared()
        {
            var fileName = Path.Combine(MenuSettings.MenuConfigPath, "SharedConfig.json");

            if (File.Exists(fileName))
            {
                string menujson = File.ReadAllText(fileName);
                Dictionary<string, MenuItem> json = JsonConvert.DeserializeObject<Dictionary<string, MenuItem>>(menujson);

                if (LoadedJSONS.ContainsKey("SharedConfig"))
                {
                    LoadedJSONS["SharedConfig"] = json;
                }
                else
                {
                    LoadedJSONS.Add("SharedConfig", json);
                }
            }

            if (FileFormat.ContainsKey("SharedConfig"))
            {
                FileFormat["SharedConfig"] = ReadFormat.JSON;
            }
            else {
                FileFormat.Add("SharedConfig", ReadFormat.JSON);
            }
        }
    }
        #endregion
}