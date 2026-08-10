using System.Collections;
using UnityEngine;

namespace Game.Gameplay
{
    public class DummyAttacker : MonoBehaviour
    {
        [Header("Timing")]
        [SerializeField] float interval = 3f;      // 공격 주기
        [SerializeField] float telegraph = 0.6f;   // 예고 시간
        [SerializeField] float activeTime = 0.15f; // 판정이 살아있는 시간

        [Header("Refs")]
        [SerializeField] GameObject hitbox;
        [SerializeField] SpriteRenderer bodyRenderer;
        [SerializeField] Color telegraphColor = new Color(1f, 0.6f, 0.2f);

        Color _originalColor;

        void Start()
        {
            if (bodyRenderer != null) _originalColor = bodyRenderer.color;
            if (hitbox != null) hitbox.SetActive(false);
            StartCoroutine(AttackLoop());
        }

        IEnumerator AttackLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(interval);

                // 예고 - 색이 변하는 동안 플레이어가 반응할 시간
                if (bodyRenderer != null) bodyRenderer.color = telegraphColor;
                yield return new WaitForSeconds(telegraph);

                // 판정 발생
                if (bodyRenderer != null) bodyRenderer.color = _originalColor;
                if (hitbox != null) hitbox.SetActive(true);
                yield return new WaitForSeconds(activeTime);
                if (hitbox != null) hitbox.SetActive(false);
            }
        }
    }
}