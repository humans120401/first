using UnityEngine;

namespace Game.Gameplay
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] int maxHp = 100;
        [SerializeField] float hitFlashDuration = 0.08f;

        int _currentHp;
        SpriteRenderer _sr;
        Color _originalColor;

        public int CurrentHp => _currentHp;
        public bool IsDead => _currentHp <= 0;

        void Awake()
        {
            _currentHp = maxHp;
            _sr = GetComponent<SpriteRenderer>();
            if (_sr != null) _originalColor = _sr.color;
        }

        public void TakeDamage(int amount)
        {
            if (IsDead) return;

            _currentHp = Mathf.Max(0, _currentHp - amount);
            Debug.Log($"{name} 피격 / 남은 체력 {_currentHp}");

            if (_sr != null) StartCoroutine(HitFlash());

            if (IsDead) OnDeath();
        }

        System.Collections.IEnumerator HitFlash()
        {
            _sr.color = Color.white;
            yield return new WaitForSeconds(hitFlashDuration);
            _sr.color = _originalColor;
        }

        void OnDeath()
        {
            Debug.Log($"{name} 사망");
            gameObject.SetActive(false);
        }
    }
}