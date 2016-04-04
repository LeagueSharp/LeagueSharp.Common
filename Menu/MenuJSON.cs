using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueSharp.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class MenuJSON
    {
        public MenuJSON(Menu Settings)
        {
            this.Settings = Settings;
            this.AssemblyName = Settings.Assemblyname;
        }

        [JsonProperty]
        public string AssemblyName;

        [JsonProperty]
        public Menu Settings;

    }
}
