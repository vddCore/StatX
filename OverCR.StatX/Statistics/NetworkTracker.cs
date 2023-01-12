﻿using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;

namespace OverCR.StatX.Statistics
{
    public class NetworkTracker
    {
        private Thread CounterThread { get; set; }
        private NetworkInterface[] Interfaces { get; }
        private Dictionary<NetworkInterface, long> InterfaceSentData { get; }
        private Dictionary<NetworkInterface, long> InterfaceReceivedData { get; }

        public double TotalBytesSent { get; private set; }
        public double TotalBytesReceived { get; private set; }

        public event EventHandler SentDataChanged;
        public event EventHandler ReceivedDataChanged;

        public NetworkTracker()
        {
            Interfaces = NetworkInterface.GetAllNetworkInterfaces();
            InterfaceSentData = new Dictionary<NetworkInterface, long>();
            InterfaceReceivedData = new Dictionary<NetworkInterface, long>();

            foreach (var iface in Interfaces)
            {
                InterfaceSentData.Add(iface, 0);
                InterfaceReceivedData.Add(iface, 0);
            }
        }

        public void StartMonitoring()
        {
            CounterThread = new Thread(UpdateData);
            CounterThread.Start();
        }

        public void StopMonitoring()
        {
            if (CounterThread.IsAlive)
                CounterThread.Abort();
        }

        public void ReloadStats()
        {
            double totalBytesSent;
            TotalBytesSent = !double.TryParse(App.StatisticsSaveFile.Section("Main").Entry("TotalBytesSent"), out totalBytesSent) ? 0 : totalBytesSent;

            double totalBytesReceived;
            TotalBytesReceived = !double.TryParse(App.StatisticsSaveFile.Section("Main").Entry("TotalBytesReceived"), out totalBytesReceived) ? 0 : totalBytesSent;
        }

        private void UpdateData()
        {
            while (true)
            {
                foreach (var iface in Interfaces)
                {
                    var dataSent = iface.GetIPStatistics().BytesSent;
                    var dataReceived = iface.GetIPStatistics().BytesReceived;

                    if (InterfaceSentData[iface] == 0)
                    {
                        InterfaceSentData[iface] = dataSent;
                        continue;
                    }

                    if (InterfaceReceivedData[iface] == 0)
                    {
                        InterfaceReceivedData[iface] = dataReceived;
                        continue;
                    }

                    if (InterfaceSentData[iface] != dataSent)
                    {
                        var bytesSent = dataSent - InterfaceSentData[iface];
                        TotalBytesSent += bytesSent;

                        InterfaceSentData[iface] = dataSent;
                        SentDataChanged?.Invoke(this, EventArgs.Empty);
                    }


                    if (InterfaceReceivedData[iface] != dataReceived)
                    {
                        var bytesReceived = dataReceived - InterfaceReceivedData[iface];
                        TotalBytesReceived += bytesReceived;

                        InterfaceReceivedData[iface] = dataReceived;
                        ReceivedDataChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
                Thread.Sleep(100);
            }
        }
    }
}