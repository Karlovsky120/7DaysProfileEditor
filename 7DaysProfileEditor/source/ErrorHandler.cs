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
            Log.Write(text);

            if (report) {
                frmErrorReport frm = new frmErrorReport(text, e, pathToSaveFile);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
