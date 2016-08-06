namespace LeagueSharp.Common
{
    using System;
    using System.ServiceModel;

    /// <summary>
    ///     Helps share an instance of a class across assemblies.
    /// </summary>
    public static class Shared
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the interface.
        /// </summary>
        /// <typeparam name="InterfaceType">The type of the interface type.</typeparam>
        /// <returns>InterfaceType.</returns>
        /// <exception cref="Exception">
        ///     Failed to connect to assembly pipe for Common.Shared communication. The targetted assembly
        ///     may not be loaded yet. Desired interface:  + typeof(InterfaceType).Name
        /// </exception>
        public static InterfaceType GetInterface<InterfaceType>() where InterfaceType : class
        {
            try
            {
                return
                    new ChannelFactory<InterfaceType>(
                        new NetNamedPipeBinding(),
                        new EndpointAddress("net.pipe://localhost/" + typeof(InterfaceType).Name)).CreateChannel();
            }
            catch (Exception e)
            {
                throw new Exception(
                    "Failed to connect to assembly pipe for Common.Shared communication. The targetted assembly may not be loaded yet. Desired interface: "
                    + typeof(InterfaceType).Name,
                    e);
            }
        }

        /// <summary>
        ///     Shares the interface.
        /// </summary>
        /// <typeparam name="ImplementationType">The type of the implementation type.</typeparam>
        /// <param name="open">if set to <c>true</c> [open].</param>
        /// <returns>ServiceHost.</returns>
        public static ServiceHost ShareInterface<ImplementationType>(bool open = true)
        {
            var host = new ServiceHost(typeof(ImplementationType), new Uri("net.pipe://localhost"));

            host.AddServiceEndpoint(
                typeof(ImplementationType).GetInterfaces()[0],
                new NetNamedPipeBinding(),
                typeof(ImplementationType).GetInterfaces()[0].Name);

            if (open)
            {
                host.Open();
            }

            return host;
        }

        #endregion
    }
}