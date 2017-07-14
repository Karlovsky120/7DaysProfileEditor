using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;
using AutoUpdaterDotNET;

namespace SevenDaysProfileEditor {
    public static class Updater {

        public static void DoUpdate() {
            #region AutoUpdater
            AutoUpdater.ShowRemindLaterButton = false;
            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.ReportErrors = true;
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
            AutoUpdater.Start("http://7daysprofileeditor.azurewebsites.net/version.xml");
            #endregion
        }
        private static void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args) {
            if (args != null) {
                if (args.IsUpdateAvailable) {
                    var dialogResult =
                        MessageBox.Show(
                            string.Format(
                                "There is new version {0} available. You are using version {1}. Do you want to update the application now?",
                                args.CurrentVersion, args.InstalledVersion), @"Update Available",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);

                    if (dialogResult.Equals(DialogResult.Yes)) {
                        try {
                            //You can use Download Update dialog used by AutoUpdater.NET to download the update.
                            AutoUpdater.DownloadUpdate();
                        }
                        catch (Exception exception) {
                            MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }
                else {
                    //MessageBox.Show(@"There is no update available please try again later.", @"No update available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else {
                MessageBox.Show(
                        @"There is a problem reaching update server please check your internet connection and try again later.",
                        @"Update check failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
