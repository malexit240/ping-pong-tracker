using PingPong.Models;

namespace PingPong.Services
{
    public interface IGameService
    {
        public void SetGame(GameModel game);

        public GameModel GetGame();
    }

    public class GameService : IGameService
    {
        private readonly ISettingsManager _settingsManager;
        private GameModel _gameModel;

        public GameService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _gameModel = new GameModel()
            {
                PointsToWin = _settingsManager.PointsToWin,
            };
        }

        public GameModel GetGame()
        {
            return _gameModel;
        }

        public void SetGame(GameModel game)
        {
            _gameModel = game;
        }
    }
}
