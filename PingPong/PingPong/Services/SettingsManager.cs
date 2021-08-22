using PingPong.Models;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PingPong.Services
{
    public class SettingsManager : ISettingsManager
    {
        public int PlayersAmount
        {
            get => Preferences.Get(nameof(PlayersAmount), 4);
            set => Preferences.Set(nameof(PlayersAmount), value);
        }

        public List<IRule> GetRules()
        {
            var rules = JsonConvert.DeserializeObject<List<IRule>>(Preferences.Get("Rules", string.Empty));

            return rules
                ?? new List<IRule>()
                {
                    { new RuleViewModel(){
                            IsChoosed = true,
                            WinPoints = 5,}
                    },
                };
        }

        public void SaveRules(List<IRule> rules)
        {
            Preferences.Set("Rules", JsonConvert.SerializeObject(rules));
        }
    }
}
