using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowLevelGenerator.Scripts
{
    public class DoorTrigger : MonoBehaviour
    {
        [HideInInspector]
        public DoorExit doorExit;

        private void OnTriggerStay(Collider trigger)
        {
            if (trigger.name == "Player") 
            {
                doorExit.OpenDoor(); 
            }
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.name == "Player")
            {
                doorExit.ClosedDoor(); 
            }
        }
    }
}
