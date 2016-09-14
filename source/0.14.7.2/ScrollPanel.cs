using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    class ScrollPanel : TableLayoutPanel
    {
        public ScrollPanel()
        {
            VerticalScroll.Maximum = 0;
            AutoScroll = false;
            HorizontalScroll.Visible = false;
            AutoScroll = true;

            MouseEnter += (sender, e) =>
            {
                Focus();
            };
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (this.VScroll)
            {
                this.VScroll = false;
                base.OnMouseWheel(e);
                this.VScroll = true;
            }
            else
            {
                base.OnMouseWheel(e);
            }
        }
    }
}
