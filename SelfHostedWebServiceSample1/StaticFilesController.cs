using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Net.Http.Headers;
using SelfHostedWebServiceSample1.Properties;

namespace SelfHostedWebServiceSample1
{
    /// <summary>
    /// Serve up static files. Currently only serves index.html
    /// TODO - Allow other static HTML pages and content to be served possibly ?
    /// </summary>
    public class StaticFilesController : ApiController
    {
        /// <summary>
        /// Display index page
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Index(string url)
        {
            try
            {

                     
                if (string.IsNullOrWhiteSpace(url))
                    url = "index.html";

                var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\index.html";
                var response = new HttpResponseMessage();
                // Replace servicename tag
                string filecontent=File.ReadAllText(path).Replace("@@SERVICENAME",Settings.Default.AppTitle);
                response.Content = new StringContent(filecontent);
                // https://stackoverflow.com/questions/10679214/how-do-you-set-the-content-type-header-for-an-httpclient-request
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                return response;

            }
            catch (Exception ex)
            {
                var response = new HttpResponseMessage();
                response.Content = new StringContent("Error displaying page");
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                return response;

            }
        }
    }
}
