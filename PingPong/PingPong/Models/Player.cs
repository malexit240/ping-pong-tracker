using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PingPong.Models
{
    public class PlayerViewModel : BindableBase
    {
        private static readonly List<string> _allowedNames = new List<string>()
        {
            "Зебра",
            "Бабочка",
            "Слон",
            "Носорог",
            "Змея",
            "Енот",
            "Кенгуру",
            "Пчела",
        };

        public PlayerViewModel()
        {
            GenerateName();
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public void GenerateName()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                _allowedNames.Add(Name);
            }

            if (_allowedNames.Count > 0)
            {
                Name = _allowedNames.ElementAt(new Random().Next(0, _allowedNames.Count));
                _allowedNames.Remove(Name);
            }
        }
    }

    public class SetViewModel : BindableBase
    {
        private PlayerViewModel _leftPlayer;
        public PlayerViewModel LeftPlayer
        {
            get => _leftPlayer;
            set => SetProperty(ref _leftPlayer, value);
        }

        private PlayerViewModel _rightPlayer;
        public PlayerViewModel RightPlayer
        {
            get => _rightPlayer;
            set => SetProperty(ref _rightPlayer, value);
        }

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

    public class QueuViewModel : ObservableCollection<PlayerViewModel>
    {
    }

    public class GameViewModel : BindableBase
    {
        private ObservableCollection<IRule> _rules = new ObservableCollection<IRule>();
        public ObservableCollection<IRule> Rules
        {
            get => _rules;
            set => SetProperty(ref _rules, value);
        }

        private IRule _currentRule = new RuleViewModel();
        public IRule CurrentRule
        {
            get => _currentRule;
            set => SetProperty(ref _currentRule, value);
        }

        private QueuViewModel _queu = new QueuViewModel();
        public QueuViewModel Queu
        {
            get => _queu;
            set => SetProperty(ref _queu, value);
        }

        private SetViewModel _currentSet = new SetViewModel();
        public SetViewModel CurrentSet
        {
            get => _currentSet;
            set => SetProperty(ref _currentSet, value);
        }

        private bool _isNextAllowed;
        public bool IsNextAllowed
        {
            get => _isNextAllowed;
            set => SetProperty(ref _isNextAllowed, value);
        }
    }

    public interface IRule
    {
        public bool IsChoosed { get; set; }

        void Update(GameViewModel game);

        void NextGame(GameViewModel game);
    }

    public class RuleViewModel : BindableBase, IRule
    {
        private int _winPoints = 5;
        public int WinPoints
        {
            get => _winPoints;
            set => SetProperty(ref _winPoints, value);
        }

        private bool _isChoosed;
        public bool IsChoosed
        {
            get => _isChoosed;
            set => SetProperty(ref _isChoosed, value);
        }

        public void NextGame(GameViewModel game)
        {
            if (game.CurrentSet.LeftPlayer == null && game.Queu.Count >= 2)
            {
                game.CurrentSet.LeftPlayer = game.Queu.First();

                game.Queu.RemoveAt(0);
            }

            if (game.CurrentSet.RightPlayer == null && game.Queu.Count >= 2)
            {
                game.CurrentSet.RightPlayer = game.Queu.First();

                game.Queu.RemoveAt(0);
            }

            if (game.IsNextAllowed && game.Queu.Count > 2)
            {
                if (game.CurrentSet.LeftPoints >= WinPoints)
                {
                    game.Queu.Add(game.CurrentSet.RightPlayer);
                    game.CurrentSet.RightPlayer = game.Queu[0];
                    game.Queu.RemoveAt(0);

                }
                else if (game.CurrentSet.RightPoints >= WinPoints)
                {
                    game.Queu.Add(game.CurrentSet.LeftPlayer);
                    game.CurrentSet.LeftPlayer = game.Queu[0];
                    game.Queu.RemoveAt(0);
                }

                game.CurrentSet.LeftPoints = game.CurrentSet.RightPoints = 0;
                game.IsNextAllowed = false;
            }
        }

        public void Update(GameViewModel game)
        {
            game.IsNextAllowed = game.CurrentSet.LeftPoints >= WinPoints || game.CurrentSet.RightPoints >= WinPoints;
        }
    }
}
