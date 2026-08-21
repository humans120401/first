using UnityEngine;
using TMPro;
using Game.Gameplay;

namespace Game.Presentation
{
    public class TrainingDummyUI : MonoBehaviour
    {
        [SerializeField] TrainingDummy dummy;
        [SerializeField] TextMeshProUGUI text;

        void OnEnable()
        {
            if (dummy != null) dummy.Changed += Refresh;
            Refresh();
        }

        void OnDisable()
        {
            if (dummy != null) dummy.Changed -= Refresh;
        }

        void Refresh()
        {
            if (text == null || dummy == null) return;

            if (dummy.HitCount == 0)
            {
                text.text = "훈련용 허수아비";
                return;
            }

            text.text = "타격 " + dummy.HitCount + "회"
                      + "   누적 " + dummy.TotalDamage
                      + "   DPS " + dummy.Dps.ToString("0.0");
        }
    }
}