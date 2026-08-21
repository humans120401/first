using UnityEngine;
using UnityEngine.InputSystem;
using Game.Core;

namespace Game.Gameplay
{
    // 플레이어 주변의 상호작용 대상을 찾고 입력을 처리한다
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("Detection")]
        [SerializeField] float radius = 1.5f;
        [SerializeField] LayerMask interactableLayer;

        PlayerControls _controls;
        IInteractable _current;

        void Awake() => _controls = new PlayerControls();

        void OnEnable()
        {
            if (_controls == null) _controls = new PlayerControls();

            _controls.Player.Enable();
            _controls.Player.Interact.started += OnInteractPressed;
        }

        void OnDisable()
        {
            if (_controls == null) return;

            _controls.Player.Interact.started -= OnInteractPressed;
            _controls.Player.Disable();
            GameEvents.HidePrompt();
        }

        void Update()
        {
            var found = FindNearest();

            if (found != _current)
            {
                _current = found;

                if (_current != null && _current.CanInteract)
                    GameEvents.ShowPrompt(_current.PromptText);
                else
                    GameEvents.HidePrompt();
            }
        }

        IInteractable FindNearest()
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, radius, interactableLayer);

            IInteractable nearest = null;
            float minDist = float.MaxValue;

            foreach (var hit in hits)
            {
                if (!hit.TryGetComponent<IInteractable>(out var target)) continue;
                if (!target.CanInteract) continue;

                float dist = Vector2.Distance(transform.position, hit.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = target;
                }
            }

            return nearest;
        }

        void OnInteractPressed(InputAction.CallbackContext ctx)
        {
            if (_current == null || !_current.CanInteract) return;
            _current.Interact();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}