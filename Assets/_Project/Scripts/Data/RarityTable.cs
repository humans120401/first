using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "RarityTable", menuName = "Game/Rarity Table")]
    public class RarityTable : ScriptableObject
    {
        [System.Serializable]
        public class RarityEntry : IRarityInfo
        {
            public UpgradeRarity rarity;

            [Tooltip("등장 가중치 - 높을수록 자주 나온다")]
            public float weight = 100f;

            [Tooltip("기준 수치 - 능력치별 배율이 곱해진다")]
            public float minValue = 0.5f;
            public float maxValue = 1f;

            public UpgradeRarity Rarity => rarity;
            public float Weight => weight;
            public float MinValue => minValue;
            public float MaxValue => maxValue;
        }

        public List<RarityEntry> entries = new List<RarityEntry>
        {
            new RarityEntry { rarity = UpgradeRarity.Common,    weight = 400f, minValue = 0.5f, maxValue = 1f },
            new RarityEntry { rarity = UpgradeRarity.Uncommon,  weight = 280f, minValue = 1f,   maxValue = 2f },
            new RarityEntry { rarity = UpgradeRarity.Rare,      weight = 180f, minValue = 2f,   maxValue = 3.5f },
            new RarityEntry { rarity = UpgradeRarity.Epic,      weight = 90f,  minValue = 3.5f, maxValue = 5f },
            new RarityEntry { rarity = UpgradeRarity.Legendary, weight = 35f,  minValue = 5f,   maxValue = 7f },
            new RarityEntry { rarity = UpgradeRarity.Transcend, weight = 8f,   minValue = 7f,   maxValue = 10f },
            new RarityEntry { rarity = UpgradeRarity.Chaos,     weight = 40f,  minValue = 6f,   maxValue = 12f },
        };
    }
}