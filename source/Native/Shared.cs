// <copyright file="Shared.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.ServiceModel;

    /// <summary>
    ///     The shared utility, assists in sharing a class across assemblies.
    /// </summary>
    public static class Shared
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the interface.
        /// </summary>
        /// <typeparam name="TInterfaceType">
        ///     The type of the interface type.
        /// </typeparam>
        /// <returns>
        ///     The <typeparamref name="TInterfaceType" />.
        /// </returns>
        /// <exception cref="Exception">
        ///     Failed to connect to assembly pipe for Common.Shared communication. The targetted assembly
        ///     may not be loaded yet. Desired interface:  + typeof(InterfaceType).Name
        /// </exception>
        public static TInterfaceType GetInterface<TInterfaceType>()
            where TInterfaceType : class
        {
            try
            {
                return
                    new ChannelFactory<TInterfaceType>(
                        new NetNamedPipeBinding(),
                        new EndpointAddress("net.pipe://localhost/" + typeof(TInterfaceType).Name)).CreateChannel();
            }
            catch (Exception e)
            {
                throw new Exception(
                          "Failed to connect to assembly pipe for Common.Shared communication. The targetted assembly may not be loaded yet. Desired interface: "
                          + typeof(TInterfaceType).Name,
                          e);
            }
        }

        /// <summary>
        ///     Shares the interface.
        /// </summary>
        /// <typeparam name="TImplementationType">
        ///     The type of the implementation type.
        /// </typeparam>
        /// <param name="open">
        ///     A value indicating whether the interface is open.
        /// </param>
        /// <returns>
        ///     The <see cref="ServiceHost" />.
        /// </returns>
        public static ServiceHost ShareInterface<TImplementationType>(bool open = true)
        {
            var host = new ServiceHost(typeof(TImplementationType), new Uri("net.pipe://localhost"));

            host.AddServiceEndpoint(
                typeof(TImplementationType).GetInterfaces()[0],
                new NetNamedPipeBinding(),
                typeof(TImplementationType).GetInterfaces()[0].Name);

            if (open)
            {
                host.Open();
            }

            return host;
        }

        #endregion
    }
}