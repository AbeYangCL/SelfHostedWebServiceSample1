using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Net.Http.Headers;
using SelfHostedWebServiceSample1.Properties;
using System.Windows.Forms;

namespace SelfHostedWebServiceSample1
{
    public class MainApiController : ApiController
    {

        /// <summary>
        /// Constructor
        /// </summary>
        protected MainApiController()
        {
            // Add sharable code to Constructor if needed
        }

        /// <summary>
        /// Hello world method. Return "Hello World"
        /// </summary>
        /// <returns>IHttpActionResult. OK=Success, BadRequest=error</returns>
        [HttpGet]
        [Route("api/helloworld")]
        public IHttpActionResult GetHelloWorld()
        {
            try
            {
                // If localhost only and not localhost URL, bail out
                if (Settings.Default.LocalhostOnly)
                {
                    if (!IsRequestLocalHost())
                        throw new Exception(Settings.Default.LocalHostErrorMessage);
                }

                return Ok("Hello World");
            }
            catch (Exception ex)
            {
                string rtnerror = "{ { \"error\", \"" + ex.Message + "\" } }";
                return BadRequest(rtnerror);
            }
        }

        /// <summary>
        /// Check current Request to see if URL is using 
        /// localhost or 127.0.0.1
        /// </summary>
        /// <returns>True=localhost, False=not localhost</returns>
        private bool IsRequestLocalHost()
        {
            try
            {
                // Get request URL
                string requesturi = Request.RequestUri.ToString();

                // Make sure request is from localhost
                if (requesturi.ToLower().StartsWith("http://localhost") ||
                    requesturi.StartsWith("http://127.0.0.1"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
