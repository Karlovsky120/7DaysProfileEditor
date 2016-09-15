using SevenDaysSaveManipulator.GameData;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    class LabeledCoordinateFloat : TableLayoutPanel
    {
        public LabeledCoordinateFloat(string name, Vector3Df coordinate)
        {
            Label nameLabel = new Label();
            nameLabel.Text = name;
            nameLabel.AutoSize = true;

            Controls.Add(nameLabel, 0, 0);

            TableLayoutPanel coordinates = new TableLayoutPanel();
            coordinates.Size = new Size(116, 102);

            LabeledControl labeledX = new LabeledControl("X", new TextBoxFloat(coordinate.x, float.MinValue, float.MaxValue, 80), 110);
            coordinates.Controls.Add(labeledX, 0, 0);

            LabeledControl labeledY = new LabeledControl("Y", new TextBoxFloat(coordinate.y, float.MinValue, float.MaxValue, 80), 110);
            coordinates.Controls.Add(labeledY, 0, 1);

            LabeledControl labeledZ = new LabeledControl("Z", new TextBoxFloat(coordinate.z, float.MinValue, float.MaxValue, 80), 110);
            coordinates.Controls.Add(labeledZ, 0, 2);

            Controls.Add(coordinates, 0, 1);
            Size = new Size(122, 122);
        }
    }
}
