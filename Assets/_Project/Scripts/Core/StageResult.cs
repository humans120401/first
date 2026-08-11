namespace Game.Core
{
    // 스테이지를 끝냈을 때의 성적
    public struct StageResult
    {
        public int Floor;          // 몇 층인지
        public float ClearTime;    // 걸린 시간 (초)
        public int TimesHit;       // 맞은 횟수
        public bool NoHit;         // 무피격 클리어인지

        public StageResult(int floor, float clearTime, int timesHit)
        {
            Floor = floor;
            ClearTime = clearTime;
            TimesHit = timesHit;
            NoHit = timesHit == 0;
        }
    }
}