using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace LeagueSharp.Common
{
    public static class Shared
    {
        #region Interface definitions


        #endregion Interface definitions

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
