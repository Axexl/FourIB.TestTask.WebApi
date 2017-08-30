using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FourIB.TestTask.WebApi.Controllers
{
    /// <summary>
    /// Work with DNS
    /// </summary>
    public class DnsController : ApiController
    {
        private Networking.NetworkManagement _networkManagement;

        public DnsController()
        {
            _networkManagement = new Networking.NetworkManagement();
        }

        //Comment for testing with swagger
        ///// <summary>
        ///// Get DNS for first NIC
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<string> Get()
        //{
        //    return _networkManagement.GetDns(_networkManagement.GetNics().FirstOrDefault().Key);
        //}

        /// <summary>
        /// Get DNS for NIC
        /// </summary>
        /// <param name="nic"></param>
        /// <returns></returns>
        public IHttpActionResult Get(string nic)
        {
            IEnumerable<string> dns = null;

            try
            {
                dns = _networkManagement.GetDns(nic);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex));
            }

            if (dns != null)
                return Ok(dns);
            else
                return NotFound();
        }

        /// <summary>
        /// Add DNS server
        /// </summary>
        /// <param name="nic"></param>
        /// <param name="value"></param>
        public IHttpActionResult Put(string nic, string value)
        {
            try
            {
                _networkManagement.SetDns(nic, value);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex));
            }

            return Ok("DNS server added");
        }
    }
}