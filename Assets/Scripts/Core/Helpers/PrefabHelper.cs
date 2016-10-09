using UnityEngine;

namespace Assets.Scripts.Core.Helpers
{
    public static class PrefabHelper
    {
        public static GameObject AddChild(this GameObject parent, string name)
        {
            GameObject go = new GameObject(name);
            if (parent != null)
            {
                go.transform.parent = parent.transform;
                go.transform.localScale = Vector3.one;
            }

            return go;
        }

        public static T Intantiate<T>(T prefab, GameObject parent) where T : Component
        {
            T instance = Object.Instantiate(prefab) as T;
            if (instance == null) return null;

            instance.gameObject.AppendTo(parent);
        
            return instance;
        }

        public static void AppendTo(this GameObject go, GameObject parent)
        {
            go.transform.parent = parent.transform;
            go.transform.localScale = Vector3.one;
            go.gameObject.layer = parent.layer;
        }
    }
}
