using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Move")]
        [SerializeField] float moveSpeed = 8f;

        public void SetMoveSpeed(float value) => moveSpeed = value;

        [Header("Jump")]
        [SerializeField] float jumpForce = 14f;
        [SerializeField] float coyoteTime = 0.1f;
        [SerializeField] float jumpBuffer = 0.1f;

        [Header("Dash")]
        [SerializeField] float dashSpeed = 20f;
        [SerializeField] float dashDuration = 0.18f;   // 대시가 지속되는 시간
        [SerializeField] float dashCooldown = 0.6f;    // 다음 대시까지 대기
        [SerializeField] float invincibleTime = 0.14f; // 무적 구간 (대시보다 짧게)

        [Header("Ground Check")]
        [SerializeField] Transform groundCheck;
        [SerializeField] float checkRadius = 0.15f;
        [SerializeField] LayerMask groundLayer;

        PlayerControls _controls;
        Rigidbody2D _rb;
        Damageable _damageable;

        float _input;
        float _coyoteCounter;
        float _bufferCounter;
        float _dashCooldownTimer;
        bool _isGrounded;
        bool _isDashing;
        int _facing = 1;   // 1 = 오른쪽, -1 = 왼쪽

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _damageable = GetComponent<Damageable>();
            _controls = new PlayerControls();
        }

        void OnEnable()
        {
            if (_controls == null) _controls = new PlayerControls();

            _controls.Player.Enable();
            _controls.Player.Jump.started += OnJumpStarted;
            _controls.Player.Jump.canceled += OnJumpCanceled;
            _controls.Player.Dash.started += OnDashStarted;
        }

        void OnDisable()
        {
            if (_controls == null) return;

            _controls.Player.Jump.started -= OnJumpStarted;
            _controls.Player.Jump.canceled -= OnJumpCanceled;
            _controls.Player.Dash.started -= OnDashStarted;
            _controls.Player.Disable();
        }

        void OnJumpStarted(InputAction.CallbackContext ctx) => _bufferCounter = jumpBuffer;

        void OnJumpCanceled(InputAction.CallbackContext ctx)
        {
            if (_isDashing) return;
            if (_rb.linearVelocity.y > 0f)
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y * 0.5f);
        }

        void OnDashStarted(InputAction.CallbackContext ctx)
        {
            if (_isDashing || _dashCooldownTimer > 0f) return;
            StartCoroutine(DashRoutine());
        }

        IEnumerator DashRoutine()
        {
            _isDashing = true;
            _dashCooldownTimer = dashCooldown;

            float originalGravity = _rb.gravityScale;
            _rb.gravityScale = 0f;   // 대시 중에는 낙하하지 않는다

            if (_damageable != null) _damageable.SetInvincible(true);

            _rb.linearVelocity = new Vector2(_facing * dashSpeed, 0f);

            yield return new WaitForSeconds(invincibleTime);
            if (_damageable != null) _damageable.SetInvincible(false);

            yield return new WaitForSeconds(dashDuration - invincibleTime);

            _rb.gravityScale = originalGravity;
            _isDashing = false;
        }

        void Update()
        {
            _input = _controls.Player.Move.ReadValue<float>();

            _isGrounded = Physics2D.OverlapCircle(
                groundCheck.position, checkRadius, groundLayer);

            _coyoteCounter = _isGrounded ? coyoteTime : _coyoteCounter - Time.deltaTime;
            _bufferCounter -= Time.deltaTime;
            _dashCooldownTimer -= Time.deltaTime;

            if (_isDashing) return;   // 대시 중에는 점프 입력을 처리하지 않는다

            if (_bufferCounter > 0f && _coyoteCounter > 0f)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
                _bufferCounter = 0f;
                _coyoteCounter = 0f;
            }
        }

        void FixedUpdate()
        {
            if (_isDashing) return;

            _rb.linearVelocity = new Vector2(_input * moveSpeed, _rb.linearVelocity.y);

            if (_input != 0f)
            {
                _facing = (int)Mathf.Sign(_input);
                transform.localScale = new Vector3(_facing, 1f, 1f);
            }
        }

        void OnDrawGizmosSelected()
        {
            if (groundCheck == null) return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}