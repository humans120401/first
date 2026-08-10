namespace Game.Data
{
    // 저장될 플레이어 진행 상황
    [System.Serializable]
    public class PlayerProgress
    {
        public int clearedFloor = 0;   // 클리어한 최고 층 (0 = 아직 없음)
        public int currency = 0;       // 보유 재화

        public bool IsFloorUnlocked(int floor)
        {
            // 1층은 항상 열려 있고, 그 위는 이전 층을 깨야 열린다
            return floor <= clearedFloor + 1;
        }
    }
}