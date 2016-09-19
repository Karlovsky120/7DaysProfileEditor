using SevenDaysSaveManipulator.GameData;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    internal class LabeledCoordinateInt : TableLayoutPanel
    {
        private Vector3Di coordinate;

        private NumericTextBox<int> coordinateX;
        private NumericTextBox<int> coordinateY;
        private NumericTextBox<int> coordinateZ;

        public LabeledCoordinateInt(string name, Vector3Di coordinate)
        {
            this.coordinate = coordinate;

            Label nameLabel = new Label();
            nameLabel.Text = name;
            nameLabel.AutoSize = true;

            Controls.Add(nameLabel, 0, 0);

            TableLayoutPanel coordinates = new TableLayoutPanel();
            coordinates.Size = new Size(116, 102);

            coordinateX = new NumericTextBox<int>(coordinate.x, int.MinValue, int.MaxValue, 80);
            LabeledControl labeledX = new LabeledControl("X", coordinateX, 110);
            coordinates.Controls.Add(labeledX, 0, 0);

            coordinateY = new NumericTextBox<int>(coordinate.y, int.MinValue, int.MaxValue, 80);
            LabeledControl labeledY = new LabeledControl("Y", coordinateY, 110);
            coordinates.Controls.Add(labeledY, 0, 1);

            coordinateZ = new NumericTextBox<int>(coordinate.z, int.MinValue, int.MaxValue, 80);
            LabeledControl labeledZ = new LabeledControl("Z", coordinateZ, 110);
            coordinates.Controls.Add(labeledZ, 0, 2);

            Controls.Add(coordinates, 0, 1);
            Size = new Size(122, 122);
        }

        public void LockToZero()
        {
            coordinateX.UpdateMin(0);
            coordinateY.UpdateMin(0);
            coordinateZ.UpdateMin(0);

            coordinateX.UpdateMax(0);
            coordinateY.UpdateMax(0);
            coordinateZ.UpdateMax(0);
        }

        public void LockToCurrent()
        {
            coordinateX.UpdateMax(coordinate.x.Get());
            coordinateY.UpdateMax(coordinate.y.Get());
            coordinateZ.UpdateMax(coordinate.z.Get());

            coordinateX.UpdateMin(coordinate.x.Get());
            coordinateY.UpdateMin(coordinate.y.Get());
            coordinateZ.UpdateMin(coordinate.z.Get());
        }

        public void Unlock()
        {
            coordinateX.UpdateMax(int.MaxValue);
            coordinateY.UpdateMax(int.MaxValue);
            coordinateZ.UpdateMax(int.MaxValue);
        }
    }
}