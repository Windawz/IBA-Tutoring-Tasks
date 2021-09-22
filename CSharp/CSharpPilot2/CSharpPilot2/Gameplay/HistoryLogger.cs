using System.Collections.Generic;

using CSharpPilot2.IO;

namespace CSharpPilot2.Gameplay
{
    sealed class HistoryLogger
    {
        public HistoryLogger(IList<Step> history) =>
            _history = history;

        private readonly IList<Step> _history;

        public void LogPlayerInput(Player player, Input input)
        {
            _history.Add(new Step(player, input));
        }
    }
}
