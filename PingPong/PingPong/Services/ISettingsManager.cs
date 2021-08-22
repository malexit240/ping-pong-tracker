using PingPong.Models;
using System.Collections.Generic;

namespace PingPong.Services
{
    public interface ISettingsManager
    {
        List<IRule> GetRules();

        void SaveRules(List<IRule> rules);

        public int PlayersAmount { get; set; }
    }
}
