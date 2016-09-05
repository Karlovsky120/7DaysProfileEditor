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
            ComboBoxToPopulate<QuestState> currentState = new ComboBoxToPopulate<QuestState>(questStates, (int)binderQuest.currentState.get());
            currentState.DropDownStyle = ComboBoxStyle.DropDownList;
            currentState.DropDownClosed += (sender, e) =>
            {
                binderQuest.currentState.set((QuestState)currentState.SelectedItem);
            };

            LabeledBox currentStateBox = new LabeledBox("Current state", currentState);
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
