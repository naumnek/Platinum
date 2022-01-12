using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowLevelGenerator.Scripts
{
    public class DoorTrigger : MonoBehaviour
    {
        [HideInInspector]
        public bool player;
        public DoorExit doorExit;

        private void OnTriggerStay(Collider trigger)
        {
            if (!player && trigger.tag == "Player") 
            {
                player = true;
                doorExit.OpenDoor(); 
            }
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (player && trigger.tag == "Player")
            {
                player = false;
                doorExit.ClosedDoor(); 
            }
        }
    }
}
