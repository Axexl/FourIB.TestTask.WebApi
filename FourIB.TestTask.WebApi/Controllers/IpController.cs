using System.Web.Http;

namespace FourIB.TestTask.WebApi.Controllers
{
    /// <summary>
    /// API work with IP addresses NICs
    /// </summary>
    public class IpController : ApiController
    {
        private Networking.NetworkManagement _networkManagement;

        public IpController()
        {
            _networkManagement = new Networking.NetworkManagement();
        }

        //Comment for testing with swagger
        //public IEnumerable<string> Get()
        //{
        //    return _networkManagement.GetIp(_networkManagement.GetNics().FirstOrDefault().Key);
        //}

        // GET: api/Ip
        /// <summary>
        /// Return IP addresses for Network intarface card
        /// </summary>
        /// <param name="nic"></param>
        /// <returns></returns>
        public IHttpActionResult Get(string nic)
        {
            if (string.IsNullOrEmpty(nic))
            {
                return BadRequest();
            }
            else
            {
                var ips = _networkManagement.GetIp(nic);
                if (ips != null && !string.IsNullOrEmpty(ips[0]))
                    return Ok(ips);
            }
            return NotFound();
        }

        /// <summary>
        /// Set IP addresses and Subnet mask for NIC
        /// </summary>
        /// <param name="nic"></param>
        /// <param name="ipAddress"></param>
        /// <param name="subnetMask"></param>
        /// <returns></returns>
        public IHttpActionResult Put(string nic, string ipAddress, string subnetMask)
        {
            if (string.IsNullOrEmpty(nic)
                || string.IsNullOrEmpty(ipAddress)
                || string.IsNullOrEmpty(subnetMask))
            {
                return BadRequest();
            }
            else
            {
                _networkManagement.SetIp(nic, ipAddress, subnetMask);
                return Ok("Ip address is set");
                // TODO : add real check
            }
        }

        // DELETE: api/Ip/5
        /// <summary>
        /// Enable DHCP for NIC
        /// </summary>
        /// <param name="nic"></param>
        /// <returns></returns>
        public IHttpActionResult Delete(string nic)
        {
            _networkManagement.EnableDhcp(nic);
            return Ok("Dhcp enabled");
            // TODO : add real check
        }
    }
}