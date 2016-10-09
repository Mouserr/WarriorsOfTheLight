using System.Collections.Generic;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts
{
    public class CastContext
    {
        public List<Unit> Targets;
        public Unit MainTarget;
        public Vector3 TargetPoint;
        public Transform TargetTransform;
        public Vector3 CasterPoint;
    }
}