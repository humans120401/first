using Game.Core;

namespace Game.Progression
{
    // 실제 게임에서 쓰는 난수 - 매번 다른 결과
    public class UnityRandomProvider : IRandomProvider
    {
        readonly System.Random _random;

        public UnityRandomProvider()
        {
            _random = new System.Random();
        }

        // 시드를 주면 항상 같은 순서로 나온다 - 테스트용
        public UnityRandomProvider(int seed)
        {
            _random = new System.Random(seed);
        }

        public float Value01() => (float)_random.NextDouble();
    }
}