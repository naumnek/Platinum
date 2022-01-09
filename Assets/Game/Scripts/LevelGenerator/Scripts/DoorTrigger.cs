using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowLevelGenerator.Scripts
{
    public class DoorTrigger : MonoBehaviour
    {
        [HideInInspector]
        public DoorExit doorExit;

        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.name == "Player") 
            {
                doorExit.PlayerTrigger = true;
                doorExit.TriggerPlayer(trigger.transform); 
            }
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.name == "Player")
            {
                doorExit.PlayerTrigger = false; 
                doorExit.ClosedDoor(); 
            }
        }
    }
}
