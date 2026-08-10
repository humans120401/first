using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Move")]
        [SerializeField] float moveSpeed = 8f;

        [Header("Jump")]
        [SerializeField] float jumpForce = 14f;
        [SerializeField] float coyoteTime = 0.1f;
        [SerializeField] float jumpBuffer = 0.1f;

        [Header("Ground Check")]
        [SerializeField] Transform groundCheck;
        [SerializeField] float checkRadius = 0.15f;
        [SerializeField] LayerMask groundLayer;

        PlayerControls _controls;
        Rigidbody2D _rb;
        float _input;
        float _coyoteCounter;
        float _bufferCounter;
        bool _isGrounded;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _controls = new PlayerControls();
        }

        void OnEnable()
        {
            _controls.Player.Enable();
            _controls.Player.Jump.started += OnJumpStarted;
            _controls.Player.Jump.canceled += OnJumpCanceled;
        }

        void OnDisable()
        {
            _controls.Player.Jump.started -= OnJumpStarted;
            _controls.Player.Jump.canceled -= OnJumpCanceled;
            _controls.Player.Disable();
        }

        void OnJumpStarted(InputAction.CallbackContext ctx) => _bufferCounter = jumpBuffer;

        void OnJumpCanceled(InputAction.CallbackContext ctx)
        {
            if (_rb.linearVelocity.y > 0f)
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y * 0.5f);
        }

        void Update()
        {
            _input = _controls.Player.Move.ReadValue<float>();

            var hit = Physics2D.OverlapCircle(
                groundCheck.position, checkRadius, groundLayer);
            _isGrounded = hit;

            // 진단용 — 원인 확인 후 삭제할 블록
           /* if (Time.frameCount % 30 == 0)
            {
                Debug.Log(
                    $"원위치 {groundCheck.position} | " +
                    $"반지름 {checkRadius} | " +
                    $"마스크 {groundLayer.value} | " +
                    $"감지 {(hit ? hit.name + " (layer " + hit.gameObject.layer + ")" : "없음")}");
            }
           */
            _coyoteCounter = _isGrounded ? coyoteTime : _coyoteCounter - Time.deltaTime;
            _bufferCounter -= Time.deltaTime;

            if (_bufferCounter > 0f && _coyoteCounter > 0f)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
                _bufferCounter = 0f;
                _coyoteCounter = 0f;
            }
        }

        void FixedUpdate()
        {
            _rb.linearVelocity = new Vector2(_input * moveSpeed, _rb.linearVelocity.y);

            // 이동 방향으로 스프라이트와 자식 오브젝트를 함께 뒤집는다
            if (_input != 0f)
                transform.localScale = new Vector3(Mathf.Sign(_input), 1f, 1f);
        }
        void OnDrawGizmosSelected()
        {
            if (groundCheck == null) return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}