namespace Game.Data
{
    // 게임 실행 중 진행도를 들고 있는 곳
    // 나중에 파일 저장/불러오기가 여기에 붙는다
    public static class ProgressStore
    {
        public static PlayerProgress Current { get; private set; } = new PlayerProgress();

        public static void Reset()
        {
            Current = new PlayerProgress();
        }

        public static void RecordClear(int floor)
        {
            if (floor > Current.clearedFloor)
                Current.clearedFloor = floor;
        }

        public static void AddCurrency(int amount)
        {
            Current.currency += amount;
        }
        public static bool SpendCurrency(int amount)
        {
            if (Current.currency < amount) return false;
            Current.currency -= amount;
            return true;
        }
    }
}