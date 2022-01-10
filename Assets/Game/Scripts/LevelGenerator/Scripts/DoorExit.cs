using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Gameplay;
using System.Linq;

namespace LowLevelGenerator.Scripts
{
    public class DoorExit : MonoBehaviour
    {
        public GameObject Structure;
        public DoorTrigger doorTrigger;
        public bool LockedRoom = false;
        public bool isClosed = false;
        public AnimationClip ClosedDoorAnimation;
        public Animator anim;
        DoorExit door;
        public List<Section> Sections = new List<Section>();
        public Section PlayerSection => Sections.Where(s => s.Bound.player).First();

        private void Start()
        {
            anim = GetComponent<Animator>();
            door = GetComponent<DoorExit>();
            doorTrigger.doorExit = this;
        }

        public void OpenDoor()
        {
            if (!isClosed && PlayerSection.Matched)
            {
                Sections[0].SetActiveSection(this, true);
                isClosed = true;
                anim.SetBool("Open", true);
            }
        }
        public void ClosedDoor()
        {
            anim.SetBool("Open", false);
        }

        public void EndClosedDoor()
        {
            isClosed = false;
            Sections[0].SetActiveSection(this, false);
        }
    }
}