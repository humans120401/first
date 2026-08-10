using UnityEngine;

namespace Game.Gameplay
{
    public class AttackHitbox : MonoBehaviour
    {
        [SerializeField] int damage = 15;
        [SerializeField] LayerMask targetLayer;

        void OnTriggerEnter2D(Collider2D other)
        {
            if ((targetLayer.value & (1 << other.gameObject.layer)) == 0) return;

            if (other.TryGetComponent<Damageable>(out var target))
                target.TakeDamage(damage);
        }
    }
}