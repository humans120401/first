using UnityEngine;
using Game.Core;

namespace Game.Gameplay
{
    public enum NpcAction
    {
        OpenUpgrade,
        OpenStageSelect
    }

    [RequireComponent(typeof(Collider2D))]
    public class InteractableNpc : MonoBehaviour, IInteractable
    {
        [SerializeField] string promptText = "대화하기";
        [SerializeField] NpcAction action = NpcAction.OpenUpgrade;
        [SerializeField] bool enabledForNow = true;

        public string PromptText => promptText;
        public bool CanInteract => enabledForNow;

        public void Interact()
        {
            switch (action)
            {
                case NpcAction.OpenUpgrade:
                    GameEvents.RequestUpgradeUI();
                    break;
                case NpcAction.OpenStageSelect:
                    GameEvents.RequestStageSelectUI();
                    break;
            }
        }
    }
}