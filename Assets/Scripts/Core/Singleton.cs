using UnityEngine;

namespace Assets.Scripts.Core
{
    /// <summary>
    /// As a note, this is made as MonoBehaviour because we need Coroutines.
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;
        private static object lockObject = new object();
        private static bool applicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    return null;
                }

                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError("Something went really wrong  - there should never be more than 1 singleton! Reopenning the scene might fix it.");
                            return instance;
                        }

                        if (instance == null)
                        {
                            GameObject singleton = new GameObject();
                            singleton.name = "(singleton) " + typeof(T).ToString();
                            DontDestroyOnLoad(singleton);

                            instance = singleton.AddComponent<T>();
                            instance.init();


                            Debug.Log("An instance of " + typeof(T) + " is needed in the scene, so '" + singleton + "' was created with DontDestroyOnLoad.");
                        }
                        else
                        {
                            Debug.Log("Using instance already created: " + instance.gameObject.name);
                        }
                    }

                    return instance;
                }
            }
        }

        protected virtual void init() { }

        protected virtual void OnDestroy()
        {
            applicationIsQuitting = true;
        }
    
    }
}