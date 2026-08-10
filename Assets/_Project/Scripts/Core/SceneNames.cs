namespace Game.Core
{
    // 씬 이름을 문자열로 흩뿌리지 않기 위한 상수 모음
    public static class SceneNames
    {
        public const string Boot = "Boot";
        public const string Lobby = "Lobby";

        // Stage1 ~ Stage5
        public static string Stage(int floor) => $"Stage{floor}";
    }
}