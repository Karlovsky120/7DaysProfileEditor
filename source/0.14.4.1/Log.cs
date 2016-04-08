using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysProfileEditor
{
    class Log
    {
        public static void writeError(Exception e)
        {
            StreamWriter writer = new StreamWriter("ProfileEditorCrash.log", true);

            writer.WriteLine("Time Stamp: " + DateTime.Now.ToString());
            writer.WriteLine("Message: " + e.Message);
            writer.WriteLine("Source: " + e.Source);           
            writer.WriteLine("Inner Exception: " + e.InnerException);                      
            writer.WriteLine("Stack Trace: " + e.StackTrace);
            writer.WriteLine("Target Site: " + e.TargetSite);
            writer.WriteLine("Data: " + e.Data);
            writer.WriteLine();
        }

    }
}
