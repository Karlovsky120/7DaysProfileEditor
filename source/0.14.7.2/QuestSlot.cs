using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.GameData;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Quests
{
    class QuestSlot : TableLayoutPanel
    {
        private QuestBinder questBinder;

        private void GenerateSlot()
        {
            Label name = new Label();
            name.AutoSize = true;
            name.Text = questBinder.id;
            Controls.Add(name);

            QuestState[] questStates = { QuestState.InProgress, QuestState.Completed, QuestState.Failed };
            ComboBox currentState = new ComboBox();
            currentState.BindingContext = new BindingContext();
            currentState.DataSource = questStates;
            currentState.SelectedIndex = (int)questBinder.currentState.Get();


            currentState.DropDownStyle = ComboBoxStyle.DropDownList;
            currentState.DropDownClosed += (sender, e) =>
            {
                questBinder.currentState.Set((QuestState)currentState.SelectedItem);
            };

            LabeledControl currentStateBox = new LabeledControl("Current state", currentState, 200);
            Controls.Add(currentStateBox);
        }

        public QuestSlot(QuestBinder binderQuest)
        {
            Dock = DockStyle.Fill;

            this.questBinder = binderQuest;

            GenerateSlot();
        }
    }
}
