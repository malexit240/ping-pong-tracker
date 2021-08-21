using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace PingPong.Models
{
    public class PlayerViewModel : BindableBase
    {
        public string Name { get; set; }
    }

    public class SetViewModel : BindableBase
    {
        public PlayerViewModel LeftPlayer
        {
            get;
            set;
        } = new PlayerViewModel();

        public PlayerViewModel RightPlayer
        {
            get;
            set;
        } = new PlayerViewModel();

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
    }

    public class GameViewModel : BindableBase
    {
        public SetViewModel CurrentSet
        {
            get;
            set;
        } = new SetViewModel();
    }
}
