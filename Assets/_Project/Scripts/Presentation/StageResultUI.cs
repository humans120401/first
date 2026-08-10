using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Core;

namespace Game.Presentation
{
    public class StageResultUI : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] GameObject panel;
        [SerializeField] TextMeshProUGUI titleText;

        [Header("Buttons")]
        [SerializeField] Button retryButton;
        [SerializeField] Button lobbyButton;

        void Awake()
        {
            if (panel != null) panel.SetActive(false);

            if (retryButton != null)
                retryButton.onClick.AddListener(() => GameEvents.RequestRetry());

            if (lobbyButton != null)
                lobbyButton.onClick.AddListener(() => GameEvents.RequestLobby());
        }

        void OnEnable()
        {
            GameEvents.OnStageCleared += ShowCleared;
            GameEvents.OnPlayerDied += ShowFailed;
        }

        void OnDisable()
        {
            GameEvents.OnStageCleared -= ShowCleared;
            GameEvents.OnPlayerDied -= ShowFailed;
        }

        void ShowCleared()
        {
            Show("스테이지 클리어", showRetry: false);
        }

        void ShowFailed()
        {
            Show("실패", showRetry: true);
        }

        void Show(string title, bool showRetry)
        {
            if (panel != null) panel.SetActive(true);
            if (titleText != null) titleText.text = title;
            if (retryButton != null) retryButton.gameObject.SetActive(showRetry);

            Time.timeScale = 0f;   // 결과창이 뜨면 게임을 멈춘다
        }
    }
}