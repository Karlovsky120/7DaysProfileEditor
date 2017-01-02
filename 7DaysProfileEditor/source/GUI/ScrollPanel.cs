using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI {

    /// <summary>
    /// Panel used for vertical scrolling
    /// </summary>
    internal class ScrollPanel : TableLayoutPanel {

        /// <summary>
        ///  Default constructor.
        /// </summary>
        public ScrollPanel() {
            VerticalScroll.Maximum = 0;
            AutoScroll = false;
            HorizontalScroll.Visible = false;
            AutoScroll = true;

            MouseEnter += (sender, e) => {
                Focus();
            };
        }

        /// <summary>
        /// Event handler for mouse wheel.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e) {
            if (this.VScroll) {
                this.VScroll = false;
                base.OnMouseWheel(e);
                this.VScroll = true;
            }
            else {
                base.OnMouseWheel(e);
            }
        }
    }
}