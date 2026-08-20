using UnityEngine;
using TMPro;
using Game.Core;

namespace Game.Presentation
{
    public class InteractionPromptUI : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        [SerializeField] TextMeshProUGUI text;

        void Awake()
        {
            if (panel != null) panel.SetActive(false);
        }

        void OnEnable()
        {
            GameEvents.OnPromptShown += Show;
            GameEvents.OnPromptHidden += Hide;
        }

        void OnDisable()
        {
            GameEvents.OnPromptShown -= Show;
            GameEvents.OnPromptHidden -= Hide;
        }

        void Show(string message)
        {
            if (panel != null) panel.SetActive(true);
            if (text != null) text.text = "[E] " + message;
        }

        void Hide()
        {
            if (panel != null) panel.SetActive(false);
        }
    }
}