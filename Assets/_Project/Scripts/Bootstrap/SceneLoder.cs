using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Core;
using Game.Data;

namespace Game.Bootstrap
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        public int CurrentFloor { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void OnEnable()
        {
            GameEvents.OnStageRequested += LoadStage;
            GameEvents.OnLobbyRequested += LoadLobby;
            GameEvents.OnRetryRequested += RetryCurrentStage;
        }

        void OnDisable()
        {
            GameEvents.OnStageRequested -= LoadStage;
            GameEvents.OnLobbyRequested -= LoadLobby;
            GameEvents.OnRetryRequested -= RetryCurrentStage;
        }

        public void LoadLobby()
        {
            CurrentFloor = 0;
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneNames.Lobby);
        }

        public void LoadStage(int floor)
        {
            if (!ProgressStore.Current.IsFloorUnlocked(floor))
            {
                Debug.LogWarning($"{floor}층은 아직 잠겨 있습니다");
                return;
            }

            CurrentFloor = floor;
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneNames.Stage(floor));
        }

        public void RetryCurrentStage()
        {
            if (CurrentFloor <= 0)
            {
                LoadLobby();
                return;
            }
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneNames.Stage(CurrentFloor));
        }

        public void ClearAndReturn()
        {
            if (CurrentFloor > 0)
                ProgressStore.RecordClear(CurrentFloor);

            Debug.Log($"{CurrentFloor}층 클리어 / 최고 기록 {ProgressStore.Current.clearedFloor}층");
            LoadLobby();
        }
    }
}