using Prism.Mvvm;
using System;
using System.Collections.Generic;
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

    public class RuleViewModel : BindableBase
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
    }
}
