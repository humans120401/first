using System.Collections;
using UnityEngine;

namespace Game.Gameplay
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] int maxHp = 100;
        [SerializeField] float hitFlashDuration = 0.08f;

        int _currentHp;
        bool _isInvincible;
        SpriteRenderer _sr;
        Color _originalColor;

        public int CurrentHp => _currentHp;
        public bool IsDead => _currentHp <= 0;
        public bool IsInvincible => _isInvincible;

        void Awake()
        {
            _currentHp = maxHp;
            _sr = GetComponent<SpriteRenderer>();
            if (_sr != null) _originalColor = _sr.color;
        }

        public void SetInvincible(bool value)
        {
            _isInvincible = value;
            if (_sr == null) return;

            // 무적 중에는 반투명하게 표시
            var c = _originalColor;
            c.a = value ? 0.4f : 1f;
            _sr.color = c;
        }

        public void TakeDamage(int amount)
        {
            if (IsDead) return;

            if (_isInvincible)
            {
                Debug.Log($"{name} 회피 성공");
                return;
            }

            _currentHp = Mathf.Max(0, _currentHp - amount);
            Debug.Log($"{name} 피격 / 남은 체력 {_currentHp}");

            if (_sr != null) StartCoroutine(HitFlash());

            if (IsDead) OnDeath();
        }

        IEnumerator HitFlash()
        {
            _sr.color = Color.white;
            yield return new WaitForSeconds(hitFlashDuration);
            if (!_isInvincible) _sr.color = _originalColor;
        }

        void OnDeath()
        {
            Debug.Log($"{name} 사망");
            gameObject.SetActive(false);
        }
    }
}