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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    /// Helps share an instance of a class across assemblies.
    /// </summary>
    public static class Shared
    {
        #region Interface definitions


        #endregion Interface definitions

        /// <summary>
        /// Gets the interface.
        /// </summary>
        /// <typeparam name="InterfaceType">The type of the interface type.</typeparam>
        /// <returns>InterfaceType.</returns>
        /// <exception cref="Exception">Failed to connect to assembly pipe for Common.Shared communication. The targetted assembly may not be loaded yet. Desired interface:  + typeof(InterfaceType).Name</exception>
        public static InterfaceType GetInterface<InterfaceType>() where InterfaceType : class
        {
            try
            {
                return new ChannelFactory<InterfaceType>(new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/" + typeof(InterfaceType).Name)).CreateChannel();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to connect to assembly pipe for Common.Shared communication. The targetted assembly may not be loaded yet. Desired interface: " + typeof(InterfaceType).Name, e);
            }
        }

        /// <summary>
        /// Shares the interface.
        /// </summary>
        /// <typeparam name="ImplementationType">The type of the implementation type.</typeparam>
        /// <param name="open">if set to <c>true</c> [open].</param>
        /// <returns>ServiceHost.</returns>
        public static ServiceHost ShareInterface<ImplementationType>(bool open = true)
        {
            ServiceHost host = new ServiceHost(typeof(ImplementationType), new Uri("net.pipe://localhost"));

            host.AddServiceEndpoint(typeof(ImplementationType).GetInterfaces()[0], new NetNamedPipeBinding(), typeof(ImplementationType).GetInterfaces()[0].Name);

            if (open)
            {
                host.Open();
            }

            return host;
        }
    }
}
