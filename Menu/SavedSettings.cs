namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///     The menu settings manager (serialization, saving, etc.)
    /// </summary>
    [Serializable]
    internal static class SavedSettings
    {
        #region Static Fields

        /// <summary>
        ///     The loaded files collection.
        /// </summary>
        public static Dictionary<string, Dictionary<string, byte[]>> LoadedFiles =
            new Dictionary<string, Dictionary<string, byte[]>>();

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
            var dic = LoadedFiles.ContainsKey(name) ? LoadedFiles[name] : Load(name);
            return dic == null ? null : dic.ContainsKey(key) ? dic[key] : null;
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
        public static Dictionary<string, byte[]> Load(string name)
        {
            try
            {
                var fileName = Path.Combine(MenuSettings.MenuConfigPath, name + ".bin");
                if (File.Exists(fileName))
                {
                    return Utils.Deserialize<Dictionary<string, byte[]>>(File.ReadAllBytes(fileName));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        /// <summary>
        ///     Saves the specificed entry.
        /// </summary>
        /// <param name="name">
        ///     The name of the entry.
        /// </param>
        /// <param name="entires">
        ///     The entires.
        /// </param>
        public static void Save(string name, Dictionary<string, byte[]> entires)
        {
            try
            {
                Directory.CreateDirectory(MenuSettings.MenuConfigPath);
                File.WriteAllBytes(Path.Combine(MenuSettings.MenuConfigPath, name + ".bin"), Utils.Serialize(entires));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion
    }
}