using SevenDaysSaveManipulator.GameData;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI {

    /// <summary>
    /// Class used to display coordinates.
    /// </summary>
    internal class LabeledCoordinate<T> : TableLayoutPanel where T : IComparable<T> {

        /// <summary>
        /// Creates a labeled coordinate.
        /// </summary>
        /// <param name="name">Name to be displayed</param>
        /// <param name="coordinate">Coordinate to be displayed</param>
        /// <param name="min">Min value for each coordinate</param>
        /// <param name="max">Max value for each coordinate</param>
        public LabeledCoordinate(string name, Vector3D<T> coordinate, T min, T max) {
            Label nameLabel = new Label() {
                Text = name,
                AutoSize = true
            };

            Controls.Add(nameLabel, 0, 0);

            TableLayoutPanel coordinates = new TableLayoutPanel() {
                Size = new Size(116, 102)
            };

            LabeledControl labeledX = new LabeledControl("X", new NumericTextBox<T>(coordinate.x, min, max, 80), 110);
            coordinates.Controls.Add(labeledX, 0, 0);

            LabeledControl labeledY = new LabeledControl("Y", new NumericTextBox<T>(coordinate.y, min, max, 80), 110);
            coordinates.Controls.Add(labeledY, 0, 1);

            LabeledControl labeledZ = new LabeledControl("Z", new NumericTextBox<T>(coordinate.z, min, max, 80), 110);
            coordinates.Controls.Add(labeledZ, 0, 2);

            Controls.Add(coordinates, 0, 1);
            Size = new Size(122, 122);
        }
    }
}