#region LICENSE
/*
Copyright 2014 - 2014 LeagueSharp
Global.cs is part of LeagueSharp.Common.
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
using System.ServiceModel;
#endregion

namespace LeagueSharp.Common
{
    public static class Shared
    {
        #region Interface definitions


        #endregion Interface definitions

        public static TInterfaceType GetInterface<TInterfaceType>() where TInterfaceType : class
        {
            try
            {
                return new ChannelFactory<TInterfaceType>(new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/" + typeof(TInterfaceType).Name)).CreateChannel();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to connect to assembly pipe for Common.Shared communication. The targetted assembly may not be loaded yet. Desired interface: " + typeof(TInterfaceType).Name, e);
            }
        }

        public static ServiceHost ShareInterface<TImplementationType>(bool open = true)
        {
            ServiceHost host = new ServiceHost(typeof(TImplementationType), new Uri("net.pipe://localhost"));

            host.AddServiceEndpoint(typeof(TImplementationType).GetInterfaces()[0], new NetNamedPipeBinding(), typeof(TImplementationType).GetInterfaces()[0].Name);

            if (open)
            {
                host.Open();
            }

            return host;
        }
    }
}
