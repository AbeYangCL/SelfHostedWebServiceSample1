using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Net.Http.Headers;
using SelfHostedWebServiceSample1.Properties;

namespace SelfHostedWebServiceSample1
{
    public partial class FormMain : Form
    {
        HttpSelfHostConfiguration config = null;
        HttpSelfHostServer server = null;
        string _appTitle = "";

        public FormMain()
        {
            InitializeComponent();
        }

        private void buttonStartServer_Click(object sender, EventArgs e)
        {

            try
            {
                buttonStartServer.Enabled = false;

                // https://stackoverflow.com/questions/17260766/how-do-i-get-asp-net-web-api-self-hosted-to-listen-on-only-localhost
                config = new HttpSelfHostConfiguration(Settings.Default.ServerURLAndPort);

                // Web API routes. Use route attributes in controller classes
                // Ex: [Route("api/info1")]
                config.MapHttpAttributeRoutes();

                // Allow web page routing ?
                if (Settings.Default.AllowWebPages)
                {

                    config.Routes.MapHttpRoute(
                        "Static", "{*url}",
                        new { controller = "StaticFiles", action = "Index" });
                }

                // Remove XML formatters so we return JSON by default from all WEBAPI calls.
                config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(
                config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml"));
                
                // Initiate server object
                server = new HttpSelfHostServer(config);
                
                // Open/start server 
                server.OpenAsync().Wait();

                // Set message
                tsStatusMessage.Text = _appTitle + " Server Started";
                tsStatusMessage.ForeColor = Color.DarkGreen;
            }
            catch (Exception ex)
            {
                // Write first level exception
                textLogMessages.Text = ex.Message + "\r\n";
                
                // Write inner exception if found
                if (ex.InnerException.Message != null)
                {
                    textLogMessages.Text = textLogMessages.Text + ex.InnerException.Message + "\r\n";
                }
                MessageBox.Show(textLogMessages.Text, _appTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                buttonStartServer.Enabled = true;
            }
            finally
            {
                // nothing yet
            }
        }

        private void buttonStopServer_Click(object sender, EventArgs e)
        {

            // Let's attempt to stop the server
            try
            {
                // Close web server if enabled
                if (server != null)
                {
                    // Close server
                    server.CloseAsync().Wait();

                    // Dispose of server object
                    server.Dispose();
                    server = null;

                    // Enable start button
                    buttonStartServer.Enabled = true;

                    // Set message
                    tsStatusMessage.Text = _appTitle + " Server Stopped";
                    tsStatusMessage.ForeColor = Color.DarkRed;

                }
                else
                {
                    tsStatusMessage.Text = _appTitle + " Server Not Active";
                    tsStatusMessage.ForeColor = Color.DarkRed;
                    MessageBox.Show("Server Not Active", _appTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _appTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            // Let's attempt to stop the server
            try
            {

                // Exit if confirmed
                if (MessageBox.Show(String.Format("Do you want to exit the {0} ?",_appTitle), _appTitle, 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    //Environment.Exit(0);
                    Application.Exit();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _appTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // App startup stuff
            try
            {
                // Load ans set app title
                _appTitle = Settings.Default.AppTitle;
                this.Text = _appTitle;
                labelHost.Text = "Server host and port info - " + Settings.Default.ServerURLAndPort;

                // Click Start button if auto-start
                if (Settings.Default.AutoStartServer)
                {
                    buttonStartServer.PerformClick();
                }

                // Minimize if auto-minimize selected
                if (Settings.Default.AutoMinimize)
                {
                    this.WindowState = FormWindowState.Minimized;
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}


