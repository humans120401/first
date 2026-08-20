using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Data
{
    // 두 테이블을 하나로 묶어 Progression에 넘긴다
    [CreateAssetMenu(fileName = "UpgradeTables", menuName = "Game/Upgrade Tables")]
    public class UpgradeTables : ScriptableObject, IUpgradeTables
    {
        public RarityTable rarityTable;
        public StatScaleTable statScaleTable;

        List<IRarityInfo> _rarities;
        List<IStatInfo> _stats;

        public IReadOnlyList<IRarityInfo> Rarities
        {
            get
            {
                if (_rarities == null)
                {
                    _rarities = new List<IRarityInfo>();
                    foreach (var e in rarityTable.entries) _rarities.Add(e);
                }
                return _rarities;
            }
        }

        public IReadOnlyList<IStatInfo> Stats
        {
            get
            {
                if (_stats == null)
                {
                    _stats = new List<IStatInfo>();
                    foreach (var e in statScaleTable.entries) _stats.Add(e);
                }
                return _stats;
            }
        }
    }
}