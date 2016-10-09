using System.Collections.Generic;
using Assets.Scripts.Core;
using Assets.Scripts.Core.Helpers;
using Assets.Scripts.Core.Pools;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class EffectsPool : Singleton<EffectsPool>
    {
        private readonly Dictionary<string, GameObjectPool<ParticleSystem>> effects = new Dictionary<string, GameObjectPool<ParticleSystem>>();

        public void Register(string effectName, ParticleSystem prefab, int startCount)
        {
            if (!effects.ContainsKey(effectName))
            {
                effects[effectName] = new GameObjectPool<ParticleSystem>(gameObject.AddChild(effectName), prefab, startCount);
            }
        }

        public ParticleSystem GetEffect(string effectName)
        {
            GameObjectPool<ParticleSystem> pool;
            if (!effects.TryGetValue(effectName, out pool))
            {
                Debug.LogError(string.Format("Can't find pool for {0}", effectName));
                return null;
            }

            return pool.GetObject();
        }

        public void ReleaseEffect(string effectName, ParticleSystem effect)
        {
            GameObjectPool<ParticleSystem> pool;
            if (!effects.TryGetValue(effectName, out pool))
            {
                Debug.LogError(string.Format("Can't find pool for {0}", effectName));
                return;
            }

            pool.ReleaseObject(effect);
        }

        public void Clear()
        {
            foreach (KeyValuePair<string, GameObjectPool<ParticleSystem>> gameObjectPool in effects)
            {
                gameObjectPool.Value.ClearPull();
            }
            effects.Clear();
        }
    }
}