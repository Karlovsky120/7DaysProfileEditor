using System;
using System.IO;

namespace SevenDaysProfileEditor
{
    class Log
    {
        public static void WriteError(Exception e)
        {
            StreamWriter writer = new StreamWriter("ProfileEditorCrash.log", true);

            writer.WriteLine("Time Stamp: " + DateTime.Now.ToString());
            writer.WriteLine("Message: " + e.Message);
            writer.WriteLine("Source: " + e.Source);           
            writer.WriteLine("Inner Exception: " + e.InnerException);                      
            writer.WriteLine("Stack Trace: " + e.StackTrace);
            writer.WriteLine("Target Site: " + e.TargetSite);
            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine();
        }

    }
}
