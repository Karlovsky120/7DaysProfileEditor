using SevenDaysSaveManipulator.GameData;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    class LabeledCoordinateInt : TableLayoutPanel
    {
        public LabeledCoordinateInt(string name, Vector3Di coordinate)
        {
            Label nameLabel = new Label();
            nameLabel.Text = name;
            nameLabel.AutoSize = true;

            Controls.Add(nameLabel, 0, 0);

            TableLayoutPanel coordinates = new TableLayoutPanel();
            coordinates.Size = new Size(116, 102);

            LabeledControl labeledX = new LabeledControl("X", new TextBoxInt(coordinate.x, int.MinValue, int.MaxValue, 80), 110);
            coordinates.Controls.Add(labeledX, 0, 0);

            LabeledControl labeledY = new LabeledControl("Y", new TextBoxInt(coordinate.y, int.MinValue, int.MaxValue, 80), 110);
            coordinates.Controls.Add(labeledY, 0, 1);

            LabeledControl labeledZ = new LabeledControl("Z", new TextBoxInt(coordinate.z, int.MinValue, int.MaxValue, 80), 110);
            coordinates.Controls.Add(labeledZ, 0, 2);

            Controls.Add(coordinates, 0, 1);
            Size = new Size(122, 122);
        }
    }
}
