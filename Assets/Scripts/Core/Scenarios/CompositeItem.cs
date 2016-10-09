using System.Collections.Generic;

namespace Assets.Scripts.Core.Scenarios
{
    public class CompositeItem : IScenarioItem
    {
        private readonly List<IScenarioItem> items;

        public bool IsComplete
        {
            get
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (!items[i].IsComplete)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public CompositeItem(List<IScenarioItem> items)
		{
			this.items = items;
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i] == null)
				{
					items.RemoveAt(i);
					i--;
				}
			}
		}

        public CompositeItem(params IScenarioItem[] items)
            : this(new List<IScenarioItem>(items))
        {
        }

        public IScenarioItem Play()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Play();
            }

            return this;
        }

        public void Stop()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Stop();
            }
        }

        public void Pause()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Pause();
            }
        }

    }
}
