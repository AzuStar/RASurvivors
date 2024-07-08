using System.Collections.Generic;
using Godot;

namespace RA2Survivors
{
    public partial class UpgradeSelector : Node
    {
        public static UpgradeSelector instance { get; private set; }

        private List<UpgradeButton> buttons = new List<UpgradeButton>();

        public UpgradeSelector()
        {
            instance = this;
        }

        public static void CreateSelection(params UpgradeButtonSettings[] createdButtons)
        {
            PauseService.PauseGame();
            foreach (var button in createdButtons)
            {
                UpgradeButton uButton = ResourceProvider.CreateResource<UpgradeButton>(
                    button.resourcePath
                );
                uButton.Pressed += _CallBackLogic + button.callback;
                instance.buttons.Add(uButton);
                instance.AddChild(uButton);
            }
        }

        private static void _CallBackLogic()
        {
            PauseService.UnpauseGame();
            foreach (var button in instance.buttons)
            {
                button.QueueFree();
            }
            instance.buttons.Clear();
        }
    }
}