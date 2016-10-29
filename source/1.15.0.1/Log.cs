using System;
using System.IO;

namespace SevenDaysProfileEditor {

    /// <summary>
    /// Logger for program.
    /// </summary>
    internal class Log {

        /// <summary>
        /// Writes exception to the log.
        /// </summary>
        /// <param name="e">Exception to write</param>
        public static void WriteException(Exception e) {
            StreamWriter writer = new StreamWriter("ProfileEditorCrash.log", true);

            writer.WriteLine("Time Stamp: " + DateTime.Now.ToString());
            writer.WriteLine("Message: " + e.Message);
            writer.WriteLine("Source: " + e.Source);
            writer.WriteLine("Inner Exception: " + e.InnerException);
            writer.WriteLine("Stack Trace: " + e.StackTrace);
            writer.WriteLine(string.Format("Target Site: {0}\n\n", e.TargetSite));
            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine();

            writer.Close();
        }
    }
}