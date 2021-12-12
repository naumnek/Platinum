using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace naumnek.FPS
{
    public class Test : MonoBehaviour
    {
        public bool okey;

        void OnTriggerEnter(Collider other)
        {
            print(other.name);
        }
    }
}