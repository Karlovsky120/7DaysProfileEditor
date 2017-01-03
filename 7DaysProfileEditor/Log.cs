using System;
using System.IO;
using System.Text;

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
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Time Stamp: " + DateTime.Now.ToString());
            sb.AppendLine("Message: " + e.Message);
            sb.AppendLine("Source: " + e.Source);
            sb.AppendLine("Inner Exception: " + e.InnerException);
            sb.AppendLine("Stack Trace: " + e.StackTrace);
            sb.AppendLine(string.Format("Target Site: {0}\n\n", e.TargetSite));
            sb.AppendLine();
            sb.AppendLine();

            StreamWriter writer = new StreamWriter("ProfileEditor.log", true);

            writer.WriteLine(sb.ToString());
            writer.Close();
        }

        /// <summary>
        /// A method to be called on program startup.
        /// </summary>
        public static void startLog() {
            StreamWriter writer = new StreamWriter("ProfileEditor.log", true);

            writer.WriteLine("-----------------------------------------------------");
            writer.WriteLine("Log started at: " + DateTime.Now.ToString());
            writer.WriteLine("-----------------------------------------------------");
            writer.WriteLine();

            writer.Close();
        }
    }
}