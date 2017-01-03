using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace SevenDaysProfileEditor
{
    /// <summary>
    /// Handles error reporting
    /// </summary>
    class ErrorHandler {

        /// <summary>
        /// Main public method for handling error specifically
        /// </summary>
        /// <param name="text">Text to be shown in the window</param>
        /// <param name="e">Exeption</param>
        /// <param name="report">true if user should be asked to create a report</param>
        /// <param name="pathToSaveFile">Path to the used save file</param>
        public static void HandleError(string text, Exception e, bool report = false, string pathToSaveFile = "") {

            Log.WriteException(e);

            if (report) {

                string messageText;

                if (pathToSaveFile.Equals("")) {
                    messageText = text + "\n\nDo you wish to create an error report package? This will create a file to send to developers containing xml configuration, mod list and error log.";
                }

                else {
                    messageText = text + "\n\nDo you wish to create an error report package? This will create a file to send to developers containing your current save file, xml configuration, mod list and error log.";
                }

                DialogResult result = MessageBox.Show(messageText, "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                if (result == DialogResult.Yes) {
                    SaveReport(pathToSaveFile);
                }
            }

            else
            {
                MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Prompts user to write how the error occured.
        /// </summary>
        /// <returns>User's comment</returns>
        private static string GetComment()
        {
            Form commentDialog = new Form();

            commentDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            commentDialog.MaximizeBox = false;
            commentDialog.MinimizeBox = false;

            commentDialog.StartPosition = FormStartPosition.CenterScreen;
            commentDialog.Size = new System.Drawing.Size(320, 320);          

            TableLayoutPanel panel = new TableLayoutPanel();
            panel.AutoSize = true;

            Label commentText = new Label();
            commentText.Text = "What went wrong?";
            commentText.Size = new System.Drawing.Size(100, 20);
            panel.Controls.Add(commentText, 0, 0);

            TextBox comment = new TextBox();
            comment.Multiline = true;
            comment.Size = new System.Drawing.Size(300, 220);
            panel.Controls.Add(comment, 0, 1);
            panel.SetColumnSpan(comment, 3);

            Button ok = new Button();
            ok.Text = "OK";
            ok.Click += (sender, e1) => {
                commentDialog.Close();
            };

            panel.Controls.Add(ok, 1, 2);

            commentDialog.Controls.Add(panel);

            commentDialog.ShowDialog();

            return comment.Text;
        }

        /// <summary>
        /// Minor public methd that creates save report.
        /// </summary>
        /// <param name="pathToSaveFile">path to the user's save file.</param>
        public static void SaveReport(string pathToSaveFile) {

            string comment = GetComment();

            SaveFileDialog saveErrorReport = new SaveFileDialog();
            saveErrorReport.Filter = "7DaysProfileEditor error report|*.rpt.zip";

            saveErrorReport.FileOk += (sender1, e1) => {
                CreateErrorReport(pathToSaveFile, comment, saveErrorReport.FileName);
            };

            saveErrorReport.ShowDialog();
        }

        /// <summary>
        /// Method that generates the report package zip
        /// </summary>
        /// <param name="pathToSaveFile">Path to user's save file</param>
        /// <param name="comment">User's comment.</param>
        /// <param name="reportLocation">Save location for the package.</param>
        private static void CreateErrorReport(string pathToSaveFile, string comment, string reportLocation) {

            Directory.CreateDirectory("errorReport");

            // Get program log, list of mods and user comment
            File.Copy("ProfileEditor.log", "errorReport\\Info.txt", true);

            StreamWriter writer = new StreamWriter("errorReport\\Info.txt", true);
            writer.WriteLine();

            writer.WriteLine("Currently installed mods (iconwise):");

            string gameRoot = Config.GetSetting("gameRoot");

            string[] modList = Directory.GetDirectories(Config.GetSetting("gameRoot") + "\\Mods");
            foreach (string modName in modList) {
                writer.WriteLine(modName.Substring(modName.LastIndexOf('\\') + 1));
            }

            writer.WriteLine();

            writer.WriteLine("Comment:");
            writer.WriteLine(comment);

            writer.Close();

            // Get xml's
            File.Copy(gameRoot + "\\Data\\Config\\items.xml", "errorReport\\items.xml", true);
            File.Copy(gameRoot + "\\Data\\Config\\blocks.xml", "errorReport\\blocks.xml", true);
            File.Copy(gameRoot + "\\Data\\Config\\progression.xml", "errorReport\\progression.xml", true);

            // Get players save file
            if (!pathToSaveFile.Equals(""))
            {
                File.Copy(pathToSaveFile, "errorReport\\saveFile.ttp", true);
            }

            if (!reportLocation.EndsWith(".rpt.zip")) {
                reportLocation += ".rpt.zip";
            }

            // Create the zip
            ZipFile.CreateFromDirectory("errorReport", reportLocation, CompressionLevel.Optimal, false);

            // Clean up
            Directory.Delete("errorReport", true);

            MessageBox.Show("Report saved to " + reportLocation + "! You can now send that file to the developers on GitHub or the 7 Days to die forums for them to try to fix your problem!", "Report saved!", MessageBoxButtons.OK, MessageBoxIcon.Information);        
        }
    }
}
