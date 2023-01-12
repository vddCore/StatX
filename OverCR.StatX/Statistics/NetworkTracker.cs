using System;
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
            TotalBytesSent = App.StatisticsSaveFile.Section("Main").Entry<double>("TotalBytesSent");
            TotalBytesReceived = App.StatisticsSaveFile.Section("Main").Entry<double>("TotalBytesReceived");
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

                    if (InterfaceSentData[iface] != dataSent && InterfaceSentData[iface] > 0)
                    {
                        var bytesSent = dataSent - InterfaceSentData[iface];
                        if (bytesSent > 0)
                        {
                            TotalBytesSent += bytesSent;

                            InterfaceSentData[iface] = dataSent;
                            SentDataChanged?.Invoke(this, EventArgs.Empty);
                        }
                    }


                    if (InterfaceReceivedData[iface] != dataReceived && InterfaceReceivedData[iface] > 0)
                    {
                        var bytesReceived = dataReceived - InterfaceReceivedData[iface];
                        if (bytesReceived > 0)
                        {
                            TotalBytesReceived += bytesReceived;

                            InterfaceReceivedData[iface] = dataReceived;
                            ReceivedDataChanged?.Invoke(this, EventArgs.Empty);
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }
    }
}
