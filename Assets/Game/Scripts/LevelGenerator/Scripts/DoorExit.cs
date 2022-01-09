using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Gameplay;

namespace LowLevelGenerator.Scripts
{
    public class DoorExit : MonoBehaviour
    {
        public GameObject Structure;
        public DoorTrigger doorTrigger;
        public bool PlayerTrigger = false;
        public bool LockedRoom = false;
        public bool Locked = false;
        public bool isClosed = false;
        public AnimationClip ClosedDoorAnimation;
        public Animator anim;
        DoorExit door;
        public List<Section> Sections = new List<Section>();

        private void Start()
        {
            anim = GetComponent<Animator>();
            door = GetComponent<DoorExit>();
            doorTrigger.doorExit = this;
        }

        public void TriggerPlayer(Transform target)
        {
            if (!anim.GetBool("Open"))
            {
                target.GetComponent<PlayerCharacterController>().Door = door;
            }
        }

        public void ClosedDoor()
        {
            anim.SetBool("Open", false);
        }
        public void OpenDoor()
        {
            Sections[0].SetActiveSection(this, true);
            isClosed = true;
            Locked = true;
            anim.SetBool("Open", true);
        }

        public void EndClosedDoor()
        {
            isClosed = false;
            Sections[0].SetActiveSection(this, false);
        }
    }
}