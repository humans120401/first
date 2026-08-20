using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "StatScaleTable", menuName = "Game/Stat Scale Table")]
    public class StatScaleTable : ScriptableObject
    {
        [System.Serializable]
        public class StatEntry : IStatInfo
        {
            public StatType stat;

            [Tooltip("등급 기준 수치에 곱해질 배율")]
            public float scale = 1f;

            [Tooltip("이 능력치가 뽑힐 가중치")]
            public float weight = 100f;

            [Tooltip("표시용 단위")]
            public string suffix = "";

            public StatType Stat => stat;
            public float Scale => scale;
            public float Weight => weight;
            public string Suffix => suffix;
        }

        public List<StatEntry> entries = new List<StatEntry>
        {
            new StatEntry { stat = StatType.Attack,       scale = 1f,   weight = 100f, suffix = "" },
            new StatEntry { stat = StatType.MaxHp,        scale = 5f,   weight = 100f, suffix = "" },
            new StatEntry { stat = StatType.MoveSpeed,    scale = 0.2f, weight = 80f,  suffix = "" },
            new StatEntry { stat = StatType.AttackSpeed,  scale = 1.5f, weight = 90f,  suffix = "%" },
            new StatEntry { stat = StatType.CooldownRate, scale = 1.2f, weight = 90f,  suffix = "%" },
        };
    }
}