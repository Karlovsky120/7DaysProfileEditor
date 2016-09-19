using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    internal class ScrollPanel : TableLayoutPanel
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