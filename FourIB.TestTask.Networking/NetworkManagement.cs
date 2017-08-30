using System;
using System.Collections.Generic;
using System.Management;

namespace FourIB.TestTask.Networking
{
    public class NetworkManagement
    {
        #region Fields

        private const string RNameWmiClassNetworkAdapterConf = "Win32_NetworkAdapterConfiguration";
        private const string RIPEnabled = "IPEnabled";
        private const string RDescription = "Description";
        private const string REnableStatic = "EnableStatic";
        private const string RIPAddress = "IPAddress";
        private const string RSubnetMask = "SubnetMask";
        private const string RCaption = "Caption";
        private const string REnableDHCP = "EnableDHCP";
        private const string RSetGateways = "SetGateways";
        private const string RDefaultIPGateway = "DefaultIPGateway";
        private const string RGatewayCostMetric = "GatewayCostMetric";
        private const string RSetDNSServerSearchOrder = "SetDNSServerSearchOrder";
        private const string RDNSServerSearchOrder = "DNSServerSearchOrder";
        private const string RSetWINSServer = "SetWINSServer";
        private const string RWINSPrimaryServer = "WINSPrimaryServer";
        private const string RWINSSecondaryServer = "WINSSecondaryServer";

        #endregion Fields

        #region Props

        private ManagementClass NetworkConfigManagementClass
        {
            get
            {
                return new ManagementClass(RNameWmiClassNetworkAdapterConf);
            }
        }

        #endregion Props

        #region Methods

        /// <summary>
        /// Get list all NICs
        /// Key contains 'Id' and Description
        /// Value contains Description
        /// <paramref name="withDownNic">Include NIC without IP enabled</param>
        /// </summary>
        public Dictionary<string, string> GetNics(bool withDownNic = false)
        {
            Dictionary<string, string> nics = new Dictionary<string, string>();
            ManagementObjectCollection objMOC = NetworkConfigManagementClass.GetInstances();

            foreach (var objMO in objMOC)
            {
                if (withDownNic || (bool)objMO[RIPEnabled])
                {
                    nics.Add(objMO[RCaption].ToString(), objMO[RDescription].ToString());
                }
            }

            return nics;
        }

        #region Addresses

        /// <summary>
        /// Set's a new IP Address and it's Submask of the local machine
        /// </summary>
        /// <param name="ipAddress">The NIC Caption for which the address will be set</param>
        /// <param name="ipAddress">The IP Address</param>
        /// <param name="subnetMask">The Submask IP Address</param>
        /// <remarks>Requires a reference to the System.Management namespace</remarks>
        public string[] GetIp(string nic)
        {
            ManagementClass objMC = new ManagementClass(RNameWmiClassNetworkAdapterConf);
            ManagementObjectCollection objMOC = objMC.GetInstances();
            string[] addresses = null;

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO[RIPEnabled] &&
                    objMO[RCaption].ToString() == nic)
                {
                    addresses = (string[])objMO[RIPAddress];
                }
            }

            return addresses;
        }

        /// <summary>
        /// Set's a new IP Address and it's Submask of the local machine
        /// </summary>
        /// <param name="ipAddress">The NIC Caption for which the address will be set</param>
        /// <param name="ipAddress">The IP Address</param>
        /// <param name="subnetMask">The Submask IP Address</param>
        /// <remarks>Requires a reference to the System.Management namespace</remarks>
        public void SetIp(string nic, string ipAddress, string subnetMask)
        {
            ManagementClass objMC = new ManagementClass(RNameWmiClassNetworkAdapterConf);
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO[RIPEnabled] &&
                    objMO[RCaption].ToString() == nic)
                {
                    try
                    {
                        ManagementBaseObject setIP;
                        ManagementBaseObject newIP =
                            objMO.GetMethodParameters(REnableStatic);

                        newIP[RIPAddress] = new string[] { ipAddress };
                        newIP[RSubnetMask] = new string[] { subnetMask };

                        setIP = objMO.InvokeMethod(REnableStatic, newIP, null);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Error(ex);
                        throw new ApplicationException("An exception when setting up an IP address", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Enable DHCP client for NIC
        /// </summary>
        /// <remarks>Requires a reference to the System.Management namespace</remarks>
        public void EnableDhcp(string nic)
        {
            ManagementClass objMC = new ManagementClass(RNameWmiClassNetworkAdapterConf);
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO[RIPEnabled] &&
                    !string.IsNullOrEmpty(nic) &&
                    objMO[RCaption].ToString() == nic)
                {
                    try
                    {
                        ManagementBaseObject setIP;
                        setIP = objMO.InvokeMethod(REnableDHCP, null, null);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Error(ex);
                        throw new ApplicationException("An exception when DHCP is enabled", ex);
                    }
                }
            }
        }

        #endregion Addresses

        #region Gateway

        public string[] GetGateway()
        {
            ManagementClass objMC = new ManagementClass(RNameWmiClassNetworkAdapterConf);
            ManagementObjectCollection objMOC = objMC.GetInstances();
            string[] addresses = null;

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO[RIPEnabled])
                {
                    addresses = (string[])objMO[RDefaultIPGateway];
                }
            }

            return addresses;
        }

        /// <summary>
        /// Set's a new Gateway address of the local machine
        /// </summary>
        /// <param name="gateway">The Gateway IP Address</param>
        /// <remarks>Requires a reference to the System.Management namespace</remarks>
        public void SetGateway(string gateway)
        {
            ManagementClass objMC = new ManagementClass(RNameWmiClassNetworkAdapterConf);
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO[RIPEnabled])
                {
                    try
                    {
                        ManagementBaseObject setGateway;
                        ManagementBaseObject newGateway =
                            objMO.GetMethodParameters(RSetGateways);

                        newGateway[RDefaultIPGateway] = new string[] { gateway };
                        newGateway[RGatewayCostMetric] = new int[] { 1 };

                        setGateway = objMO.InvokeMethod(RSetGateways, newGateway, null);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Error(ex);
                        throw new ApplicationException("An exception when configuring the gateway", ex);
                    }
                }
            }
        }

        #endregion Gateway

        #region Dns

        public IEnumerable<string> GetDns()
        {
            ManagementClass objMC = new ManagementClass(RNameWmiClassNetworkAdapterConf);
            ManagementObjectCollection objMOC = objMC.GetInstances();
            string[] dns = null;

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO[RIPEnabled])
                {
                    try
                    {
                        dns = (string[])objMO[RDNSServerSearchOrder];
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Error(ex);
                        throw new ApplicationException("An exception when getting DNS", ex);
                    }
                }
            }
            return dns;
        }

        public IEnumerable<string> GetDns(string nic)
        {
            ManagementClass objMC = new ManagementClass(RNameWmiClassNetworkAdapterConf);
            ManagementObjectCollection objMOC = objMC.GetInstances();
            string[] dns = null;

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO[RIPEnabled] &&
                    objMO[RCaption].Equals(nic))
                {
                    try
                    {
                        dns = (string[])objMO[RDNSServerSearchOrder];
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Error(ex);
                        throw new ApplicationException("An exception when getting DNS", ex);
                    }
                }
            }
            return dns;
        }

        #endregion Dns

        /// <summary>
        /// Set's the DNS Server of the local machine
        /// </summary>
        /// <param name="nic">NIC Caption</param>
        /// <param name="dns">DNS server address</param>
        /// <remarks>Requires a reference to the System.Management namespace</remarks>
        public void SetDns(string nic, string dns)
        {
            ManagementClass objMC = new ManagementClass(RNameWmiClassNetworkAdapterConf);
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO[RIPEnabled] &&
                    objMO[RCaption].Equals(nic))
                {
                    try
                    {
                        ManagementBaseObject newDNS =
                            objMO.GetMethodParameters(RSetDNSServerSearchOrder);
                        newDNS[RDNSServerSearchOrder] = dns.Split(',');
                        ManagementBaseObject setDNS =
                            objMO.InvokeMethod(RSetDNSServerSearchOrder, newDNS, null);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Error(ex);
                        throw new ApplicationException("An exception when configuring the DNS", ex);
                    }
                }
            }
        }

        #region Wins

        public IEnumerable<string> GetWins()
        {
            ManagementClass objMC = new ManagementClass(RNameWmiClassNetworkAdapterConf);
            ManagementObjectCollection objMOC = objMC.GetInstances();
            string[] wins = null;

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO[RIPEnabled])
                {
                    wins = (string[])objMO[RWINSPrimaryServer];
                }
            }

            return wins;
        }

        public string GetWins(string nic)
        {
            ManagementClass objMC = new ManagementClass(RNameWmiClassNetworkAdapterConf);
            ManagementObjectCollection objMOC = objMC.GetInstances();
            string wins = null;

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO[RIPEnabled] &&
                    objMO[RCaption].Equals(nic))
                {
                    wins = (string)objMO[RWINSPrimaryServer];
                }
            }

            return wins;
        }

        /// <summary>
        /// Set's WINS of the local machine
        /// </summary>
        /// <param name="nic">NIC Caption</param>
        /// <param name="primaryWins">Primary WINS server address</param>
        /// <param name="secondaryWins">Secondary WINS server address</param>
        /// <remarks>Requires a reference to the System.Management namespace</remarks>
        public void SetWins(string nic, string primaryWins, string secondaryWins)
        {
            ManagementClass objMC = new ManagementClass(RNameWmiClassNetworkAdapterConf);
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO[RIPEnabled] &&
                    objMO[RCaption].Equals(nic))
                {
                    try
                    {
                        ManagementBaseObject setWINS;
                        ManagementBaseObject wins =
                        objMO.GetMethodParameters(RSetWINSServer);
                        wins.SetPropertyValue(RWINSPrimaryServer, primaryWins);
                        wins.SetPropertyValue(RWINSSecondaryServer, secondaryWins);

                        setWINS = objMO.InvokeMethod(RSetWINSServer, wins, null);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Error(ex);
                        throw new ApplicationException("An exception when configuring the WINS", ex);
                    }
                }
            }
        }

        public void DeleteWins(string nic)
        {
            //throw new NotImplementedException();
        }

        #endregion Wins

        #endregion Methods
    }
}