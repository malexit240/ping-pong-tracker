using PingPong.Models;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PingPong.Services
{
    public class SettingsManager : ISettingsManager
    {
        public int PointsToWin
        {
            get => Preferences.Get(nameof(PointsToWin), 5);
            set => Preferences.Set(nameof(PointsToWin), value);
        }
    }
}
