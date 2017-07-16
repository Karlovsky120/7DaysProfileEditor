using System;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Xml;
using System.IO;
using System.Management;
using System.Reflection;
using System.IO.Compression;

namespace SevenDaysProfileEditor {
    public partial class frmErrorReport : Form {

        string _message;
        Exception _ex;
        string _ttpFile;
        public frmErrorReport(string message, Exception ex, string ttpFile) {
            InitializeComponent();

            lblError.Text = message;
            _message = message;
            _ttpFile = ttpFile;
            _ex = ex;
        }

        private void cmdCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void cmdSubmit_Click(object sender, EventArgs e) {
            SubmitReport();
        }
        private void SubmitReport() {

            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("<b>Message:</b><br>{0}<br>", _message));
            sb.Append(String.Format("<b>Comment: </b>{0}<br>", txtComment.Text));
            sb.Append(String.Format("<b>Email: </b><br>{0}<br>", txtEmail.Text));
            sb.Append(String.Format("<b>Profile: </b><br>{0}<br>", _ttpFile));
            sb.Append(String.Format("<b>Editor Version: </b>{0}<br>", Assembly.GetEntryAssembly().GetName().Version));
            sb.Append(String.Format("<b>Date & Time: </b>{0}<br>", DateTime.Now));
            sb.Append(String.Format("<b>Process: </b>{0}<br>", Environment.Is64BitProcess ? "64Bit" : "32bit"));
            sb.Append(String.Format("<b>OS Arch: </b>{0}<br>", Environment.Is64BitOperatingSystem ? "64Bit" : "32bit"));
            sb.Append(String.Format("<b>OS Version: </b>{0}<br>", Environment.OSVersion));
            sb.Append(String.Format("<b>Runtime Version: </b>{0}<br>", Environment.Version));
            
            sb.Append("<hr />");
            sb.Append("<h3>Exception:</h3>");

            if (_ex != null)
                sb.Append(_ex.ToString());
            else
                sb.Append(String.Format("<b>No Exception Recorded</b><br>"));

            byte[] filebytes = CreateReport();

            SubmitFile(sb.ToString(), "error.rpt", filebytes);

            MessageBox.Show("Thank you, your submission as been sent!");

            this.Close();
        }

        private byte[] CreateReport() {

            string reportDir = Path.GetTempPath() + Guid.NewGuid();
            Directory.CreateDirectory(reportDir);

            // Get program log, list of mods and user comment
            File.Copy("ProfileEditor.log", reportDir + "\\Info.txt", true);

            string gameRoot = Config.GetSetting("gameRoot");
            string modspath = gameRoot + "\\Mods";

            if (Directory.Exists(modspath))
                File.Copy(modspath, reportDir + "\\Mods");

            // Get xml's
            File.Copy(gameRoot + "\\Data\\Config\\items.xml", reportDir + "\\items.xml", true);
            File.Copy(gameRoot + "\\Data\\Config\\blocks.xml", reportDir + "\\blocks.xml", true);
            File.Copy(gameRoot + "\\Data\\Config\\progression.xml", reportDir + "\\progression.xml", true);

            //Do we have an TTP Loaded?
            if (File.Exists(_ttpFile)) {

                //Did the consent to all TTPs?
                if (chkTTP.Checked) {
                    string ttpPath = Path.GetDirectoryName(_ttpFile);
                    var ttpFiles = Directory.EnumerateFiles(ttpPath, "*.ttp");
                    foreach (string ttpFile in ttpFiles) {
                        File.Copy(ttpFile, reportDir + "\\" + ttpFile.Substring(ttpFile.LastIndexOf('\\') + 1), true);
                    }
                }
                else { //If not, only include the current loaded TTP.
                    File.Copy(_ttpFile, reportDir + "\\" + _ttpFile.Substring(_ttpFile.LastIndexOf('\\') + 1), true);
                }
            }

            // Create the zip
            if (File.Exists(Path.GetTempPath() + "\\error.rpt"))
                File.Delete(Path.GetTempPath() + "\\error.rpt");

            ZipFile.CreateFromDirectory(reportDir, Path.GetTempPath() + "\\error.rpt", CompressionLevel.Optimal, false);

            byte[] filebytes = File.ReadAllBytes(Path.GetTempPath() + "\\error.rpt");

            Directory.Delete(reportDir, true);
            File.Delete(System.IO.Path.GetTempPath() + "\\error.rpt");
            return filebytes;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static string GetAssemblyInfo(Assembly assembly) {
            string assname = assembly.ToString();
            string assversion = assembly.GetName().Version.ToString();
            string asstime = File.GetCreationTime(assembly.Location).ToString();
            string asslocation = assembly.Location;
            return String.Format("{0} {1} ({2})\n{3}", assname, asstime, assversion, asslocation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="filename"></param>
        /// <param name="filecontent"></param>
        private static void SubmitFile(string message, string filename, byte[] filecontent) {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://7daysprofileeditor.azurewebsites.net/Submission.asmx?op=SubmitFile");
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://localhost:51737/Submission.asmx?op=SubmitFile");

            request.Headers.Add(@"SOAP:Action");
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Accept = "text/xml";
            request.Method = "POST";

            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Body><SubmitFile xmlns=""http://tempuri.org/""><message><![CDATA[" + message + "]]]]></message><filename>" + filename + "</filename><filecontent>" + Convert.ToBase64String(filecontent) + "</filecontent></SubmitFile></soap:Body></soap:Envelope>");

            using (Stream stream = request.GetRequestStream()) {
                soapEnvelopeXml.Save(stream);
            }

            using (WebResponse response = request.GetResponse()) {
                using (StreamReader rd = new StreamReader(response.GetResponseStream())) {
                    string soapResult = rd.ReadToEnd();
                    Console.WriteLine(soapResult);
                }
            }
        }
    }
}
