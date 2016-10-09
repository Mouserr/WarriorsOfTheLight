using System.Collections.Generic;
using Assets.Scripts.Core.Helpers;
using UnityEngine;

namespace Assets.Scripts.Core.Pools
{
    public class GameObjectPool<T> where T : Component
    {
        private readonly GameObject container;
        private readonly T prefab;
        private readonly int addingCount;
        private readonly Stack<T> instances;

        public GameObjectPool(GameObject container, T prefabObj, int startCount, int addingCount = 5)
        {
            prefab = prefabObj;
            this.addingCount = addingCount;
            this.container = container;
            instances = new Stack<T>(startCount);
            addInstances(startCount);
        }

        public T GetObject()
        {
            if (instances.Count == 0)
            {
                addInstances(addingCount);
            }
            return instances.Pop();
        }

        public void ReleaseObject(T objectToRelease)
        {
            objectToRelease.gameObject.AppendTo(container);
            objectToRelease.gameObject.SetActive(false);
            instances.Push(objectToRelease);
        }

        public void ClearPull()
        {
            while (instances.Count > 0)
            {
                GameObject.Destroy(instances.Pop());
            }
        }


        private void addInstances(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T instance = PrefabHelper.Intantiate(prefab, container);
                instance.gameObject.SetActive(false);
                instances.Push(instance);
            }
        }

    }
}
