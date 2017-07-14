using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;

namespace AutoUpdaterDotNET {
    class Ourself {
        public static string FileName() {
            Assembly _objParentAssembly;

            if (Assembly.GetEntryAssembly() == null)
                _objParentAssembly = Assembly.GetCallingAssembly();
            else
                _objParentAssembly = Assembly.GetEntryAssembly();

            if (_objParentAssembly.CodeBase.StartsWith("http://"))
                throw new IOException("Deployed from URL");

            if (File.Exists(_objParentAssembly.Location))
                return _objParentAssembly.Location;
            if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + System.AppDomain.CurrentDomain.FriendlyName))
                return System.AppDomain.CurrentDomain.BaseDirectory + System.AppDomain.CurrentDomain.FriendlyName;
            if (File.Exists(Assembly.GetExecutingAssembly().Location))
                return Assembly.GetExecutingAssembly().Location;

            throw new IOException("Assembly not found");
        }
        public static bool Rename() {
            string currentName = FileName();
            string newName = FileName() + ".ori";
            if (File.Exists(newName)) {
                File.Delete(newName);
            }
            File.Move(currentName, newName);
            return true;
        }

    }
}