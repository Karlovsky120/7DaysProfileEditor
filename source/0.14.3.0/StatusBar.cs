using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    class BottomStatusBar : StatusBar
    {
        public StatusBarPanel statusPanel;

        public BottomStatusBar() : base()
        {
            SizingGrip = false;

            statusPanel = new StatusBarPanel();

            statusPanel.BorderStyle = StatusBarPanelBorderStyle.Sunken;
            statusPanel.Text = "Ready!";
            statusPanel.ToolTipText = "Last Activity";
            statusPanel.AutoSize = StatusBarPanelAutoSize.Spring;

            Panels.Add(statusPanel);

            ShowPanels = true;
        }

        public void setText(string text)
        {
            statusPanel.Text = text;
        }

        public void reset()
        {
            setText("Ready!");
        }
    }
}
