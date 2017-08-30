using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FourIB.TestTask.WebApi.Controllers
{
    /// <summary>
    /// Work with NICs
    /// </summary>
    public class NicController : ApiController
    {
        private Networking.NetworkManagement _networkManagement;

        public NicController()
        {
            _networkManagement = new Networking.NetworkManagement();
        }

        /// <summary>
        /// Get NICs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetNic()
        {
            Dictionary<string, string> nics = null;

            try
            {
                nics = _networkManagement.GetNics(false);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex));
            }

            if (nics != null && nics.Count > 0)
                return Ok(nics);
            else
                return NotFound();
        }

        //Comment for testing with swagger
        //public string Get(string nic)
        //{
        //    string selectedNic = string.Empty;

        //    if (_networkManagement.GetNics(false).TryGetValue(nic, out selectedNic))
        //        return selectedNic;
        //    else
        //        return string.Format("Oncorect param in Action:{0}", nameof(Get));
        //}
    }
}