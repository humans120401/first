using System;
using System.Collections;
using UnityEngine;

namespace Game.Gameplay
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] int maxHp = 100;
        [SerializeField] float hitFlashDuration = 0.08f;
        [SerializeField] bool disableOnDeath = true;

        int _currentHp;
        int _maxHp;
        bool _isInvincible;
        SpriteRenderer _sr;
        Color _originalColor;

        public int MaxHp => _maxHp;
        public int CurrentHp => _currentHp;
        public bool IsDead => _currentHp <= 0;
        public bool IsInvincible => _isInvincible;

        public event Action Died;
        public event Action Damaged;

        void Awake()
        {
            _maxHp = maxHp;
            _currentHp = _maxHp;
            _sr = GetComponent<SpriteRenderer>();
            if (_sr != null) _originalColor = _sr.color;
        }

        // 외부에서 최대 체력을 지정할 때 사용
        public void SetMaxHp(int value, bool healToFull = true)
        {
            _maxHp = Mathf.Max(1, value);
            if (healToFull) _currentHp = _maxHp;
            else _currentHp = Mathf.Min(_currentHp, _maxHp);
        }

        public void SetInvincible(bool value)
        {
            _isInvincible = value;
            if (_sr == null) return;

            var c = _originalColor;
            c.a = value ? 0.4f : 1f;
            _sr.color = c;
        }

        public void TakeDamage(int amount)
        {
            if (IsDead) return;

            if (_isInvincible)
            {
                Debug.Log(name + " 회피 성공");
                return;
            }

            _currentHp = Mathf.Max(0, _currentHp - amount);
            Debug.Log(name + " 피격 / 남은 체력 " + _currentHp);
            Damaged?.Invoke();

            if (_sr != null) StartCoroutine(HitFlash());

            if (IsDead)
            {
                Debug.Log(name + " 사망");
                Died?.Invoke();
                if (disableOnDeath) gameObject.SetActive(false);
            }
        }

        IEnumerator HitFlash()
        {
            _sr.color = Color.white;
            yield return new WaitForSeconds(hitFlashDuration);
            if (!_isInvincible) _sr.color = _originalColor;
        }

        void OnDeath() { }
    }
}