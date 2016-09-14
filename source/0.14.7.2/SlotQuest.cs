using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.GameData;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Quests
{
    class SlotQuest : TableLayoutPanel
    {
        private BinderQuest binderQuest;

        private void generateSlot()
        {
            Label name = new Label();
            name.AutoSize = true;
            name.Text = binderQuest.id;
            Controls.Add(name);

            QuestState[] questStates = { QuestState.InProgress, QuestState.Completed, QuestState.Failed };
            ComboBox currentState = new ComboBox();
            currentState.BindingContext = new BindingContext();
            currentState.DataSource = questStates;
            currentState.SelectedIndex = (int)binderQuest.currentState.Get();


            currentState.DropDownStyle = ComboBoxStyle.DropDownList;
            currentState.DropDownClosed += (sender, e) =>
            {
                binderQuest.currentState.Set((QuestState)currentState.SelectedItem);
            };

            LabeledControl currentStateBox = new LabeledControl("Current state", currentState, 200);
            Controls.Add(currentStateBox);
        }

        public SlotQuest(BinderQuest binderQuest)
        {
            Dock = DockStyle.Fill;

            this.binderQuest = binderQuest;

            generateSlot();
        }
    }
}
