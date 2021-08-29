using Prism.Mvvm;
using System.Collections.ObjectModel;
using PingPong.Enums;
using System.ComponentModel;
using System;
using System.Timers;

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

    public class TimeLogger : BindableBase
    {
        private Timer _timer = new Timer(1000);
        private DateTime _startTimerPoint;

        public TimeLogger()
        {
            _timer.Elapsed += OnTimerElapsed;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            ElapsedTime = e.SignalTime - _startTimerPoint;
        }

        private TimeSpan _elapsedTime;
        public TimeSpan ElapsedTime
        {
            get => _elapsedTime;
            set => SetProperty(ref _elapsedTime, value);
        }

        private int _gamesCount;
        public int GamesCount
        {
            get => _gamesCount;
            set => SetProperty(ref _gamesCount, value);
        }

        private bool _isTimerStarted;
        public bool IsTimerStarted
        {
            get => _isTimerStarted;
            set => SetProperty(ref _isTimerStarted, value);
        }

        public void StartTimer()
        {
            IsTimerStarted = true;
            _startTimerPoint = DateTime.Now;
            _timer.Start();
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
