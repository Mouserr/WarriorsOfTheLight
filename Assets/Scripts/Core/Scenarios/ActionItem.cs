using System;

namespace Assets.Scripts.Core.Scenarios
{
    public class ActionItem : IScenarioItem
    {
        private readonly Action action;

        public ActionItem(Action action)
        {
            this.action = action;
        }

        public IScenarioItem Play()
        {
            action();
            IsComplete = true;

            return this;
        }

        public void Stop()
        {
        }

        public void Pause()
        {
        }

        public bool IsComplete { get; private set; }
    }
}