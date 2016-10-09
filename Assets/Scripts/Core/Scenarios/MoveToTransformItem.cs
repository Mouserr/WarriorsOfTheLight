using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Scenarios
{
    public class MoveToTransformItem : IScenarioItem
    {
        private readonly Transform follower;
        private readonly Transform target;
        private readonly float speed;
        private readonly float sqrMinRange;
        private bool isPaused;
        private IEnumerator followCoroutine;

        public bool IsComplete { get; private set; }

        public MoveToTransformItem(Transform follower, Transform target, float speed, float minRange = 1f)
        {
            this.follower = follower;
            this.target = target;
            this.speed = speed;
            sqrMinRange = minRange * minRange;
        }

        public IScenarioItem Play()
        {
            isPaused = false;
            if (followCoroutine == null)
            {
                followCoroutine = getFollowCoroutine();
                ScenariosRoot.Instance.StartCoroutine(followCoroutine);
            }
            return this;
        }

        public void Stop()
        {
            if (followCoroutine != null && ScenariosRoot.Instance != null)
            {
                ScenariosRoot.Instance.StopCoroutine(followCoroutine);
                followCoroutine = null;
            }
            IsComplete = true;
        }

        public void Pause()
        {
            isPaused = true;
        }

        private IEnumerator getFollowCoroutine()
        {
            while (true)
            {
                Vector3 delta = target.position - follower.position;
                if (delta.sqrMagnitude < sqrMinRange)
                {
                    Stop();
                    yield break;
                }
                follower.rotation = Quaternion.LookRotation(-delta);
                follower.position += delta.normalized * speed;

                do
                {
                    yield return null;
                } while (isPaused);
            }
        }
    }
}