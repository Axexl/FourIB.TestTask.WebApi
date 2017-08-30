using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FourIB.TestTask.WebApi.Controllers
{
    /// <summary>
    /// Work with gateway
    /// </summary>
    public class GatewayController : ApiController
    {
        private Networking.NetworkManagement _networkManagement;

        public GatewayController()
        {
            _networkManagement = new Networking.NetworkManagement();
        }

        /// <summary>
        /// Get default gateway
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult Get()
        {
            string[] gateway = null;

            try
            {
                gateway = _networkManagement.GetGateway();
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex));
            }
            return Ok(gateway);
        }

        /// <summary>
        /// Set default Gateway address
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IHttpActionResult Post(string value)
        {
            try
            {
                _networkManagement.SetGateway(value);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex));
            }
            return Ok("Default Gateway address is set");
        }

        /// <summary>
        /// Remove default gateway
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult Delete()
        {
            try
            {
                _networkManagement.SetGateway(null);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex));
            }
            return Ok("Remove default gateway");
        }
    }
}