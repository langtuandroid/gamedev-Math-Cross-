using stickin;

namespace stickin.mathcross
{
    public interface IGameView
    {
        void Init(LevelDifficult difficult, Board board, Pocket pocket, RewardResourceModule rewardResourceModule);
    }
}