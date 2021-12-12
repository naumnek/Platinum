using System.Collections.Generic;
using UnityEngine;

namespace LowLevelGenerator.Scripts
{
    public class Bounds : MonoBehaviour
    {
            public IEnumerable<Collider> Colliders => GetComponentsInChildren<Collider>();
    }
}
