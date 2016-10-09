namespace Assets.Scripts.Core.Scenarios
{
    public interface IScenarioItem
    {
        bool IsComplete { get; }

        /// <summary>
        /// Start item and return self
        /// </summary>
        IScenarioItem Play();
        void Stop();
        void Pause();
    }
}