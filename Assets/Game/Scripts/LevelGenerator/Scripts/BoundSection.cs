using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LowLevelGenerator.Scripts
{
    public class BoundSection : MonoBehaviour
    {
        public IEnumerable<Collider> GetColliders => GetComponentsInChildren<Collider>();

        public bool player = false;
        public Section section;

        private void OnTriggerStay(Collider other)
        {
            if (!player && other.tag == "Player") player = true;           
        }

        private void OnTriggerExit(Collider other)
        {
            if (player && other.tag == "Player") player = false;           
        }
    }
}
