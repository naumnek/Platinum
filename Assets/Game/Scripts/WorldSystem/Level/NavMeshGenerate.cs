using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace naumnek.FPS
{
    public class NavMeshGenerate : MonoBehaviour
    {
        private static NavMeshGenerate instance;

        public static void Build()
        {
            print("Build Mesh");
            instance.GetComponent<NavMeshSurface>().BuildNavMesh();
        }

        private void Start()
        {
            instance = this;
        }
    }
}

