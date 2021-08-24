using Prism.Mvvm;
using System.Collections.ObjectModel;
using PingPong.Enums;
using System.ComponentModel;

namespace PingPong.Models
{
    public class GameViewModel : BindableBase
    {
        private int _leftPoints;
        public int LeftPoints
        {
            get => _leftPoints;
            set => SetProperty(ref _leftPoints, value);
        }

        private int _rightPoints;
        public int RightPoints
        {
            get => _rightPoints;
            set => SetProperty(ref _rightPoints, value);
        }

        private int _pointsToWin;
        public int PointsToWin
        {
            get => _pointsToWin;
            set => SetProperty(ref _pointsToWin, value);
        }

        private bool _isNextAllowed;
        public bool IsNextAllowed
        {
            get => _isNextAllowed;
            set => SetProperty(ref _isNextAllowed, value);
        }

        public EGameWinState WinState => (LeftPoints >= PointsToWin, RightPoints >= PointsToWin) switch
        {
            (true, true) => EGameWinState.None,
            (true, false) => EGameWinState.LeftPlayer,
            (false, true) => EGameWinState.RightPlayer,
            (false, false) => EGameWinState.None,
        };

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case nameof(LeftPoints):
                case nameof(RightPoints):
                case nameof(PointsToWin):
                    RaisePropertyChanged(nameof(WinState));
                    break;
            }
        }

        public static explicit operator GameModel(GameViewModel game)
        {
            return new GameModel()
            {
                LeftPoints = game.LeftPoints,
                RightPoints = game.RightPoints,
                PointsToWin = game.PointsToWin,
                IsNextAllowed = game.IsNextAllowed,
            };
        }
    }

    public class GameModel
    {
        public int LeftPoints { get; set; }

        public int RightPoints { get; set; }

        public int PointsToWin { get; set; }

        public bool IsNextAllowed { get; set; }

        public static explicit operator GameViewModel(GameModel game)
        {
            return new GameViewModel()
            {
                LeftPoints = game.LeftPoints,
                RightPoints = game.RightPoints,
                PointsToWin = game.PointsToWin,
                IsNextAllowed = game.IsNextAllowed,
            };
        }
    }
}
