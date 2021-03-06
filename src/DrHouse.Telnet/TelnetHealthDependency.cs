﻿using DrHouse.Core;
using System;
using System.Net.Sockets;

namespace DrHouse.Telnet
{
    /// <summary>
    /// Telnet Health Dependency uses telnet to check if it is possible to connect
    /// to a given hostname and port.
    /// </summary>
    public class TelnetHealthDependency : IHealthDependency
    {
        private readonly string _hostname;
        private readonly int _port;
        private readonly string _serviceName;

        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="hostname">The hostname to test the connection</param>
        /// <param name="port">The port to test the connection</param>
        /// <param name="serviceName">Optional service name, to enchant result readability</param>
        public TelnetHealthDependency(string hostname, int port, string serviceName = null)
        {
            _hostname = hostname;
            _port = port;
            _serviceName = serviceName;
        }

        public HealthData CheckHealth()
        {
            HealthData healthData = new HealthData($"{_hostname}:{_port} [{_serviceName ?? ""}]");
            healthData.Type = "Telnet";

            try
            {
                using (Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Connect(_hostname, _port);

                    if (socket.Connected)
                    {
                        healthData.IsOK = true;
                    }
                }
            }
            catch (Exception ex)
            {
                healthData.IsOK = false;
                healthData.ErrorMessage = ex.Message;
            }

            return healthData;
        }

        public HealthData CheckHealth(Action check)
        {
            throw new NotImplementedException();
        }
    }
}
