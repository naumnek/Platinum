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
        public float PlayerRecheckTime = 0.5f;
        bool LockedRoom = false;
        [HideInInspector]
        public bool isClosing = false;
        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public List<Section> Sections = new List<Section>();
        Section PlayerSection => Sections.Where(s => s.Bound.player).First();
        DoorExit door;

        bool EndOpened = true;

        private void Start()
        {
            anim = GetComponent<Animator>();
            door = GetComponent<DoorExit>();
            doorTrigger.doorExit = this;
        }

        public void OpenDoor()
        {
            if (PlayerSection != null && PlayerSection.Matched)
            {
                if (!isClosing && EndOpened)
                {
                    Sections[0].SetActiveSection(this, true);
                    isClosing = true;
                    EndOpened = false;
                    anim.SetBool("Open", true);
                }
            }
            else RecheckPlayer();
        }

        IEnumerator RecheckPlayer()
        {
            yield return new WaitForSeconds(PlayerRecheckTime);
            OpenDoor();

        }

        public void ClosedDoor()
        {
            if(EndOpened) anim.SetBool("Open", false);
        }

        public void EndOpenedDoor()
        {
            print("EndOpened");
            EndOpened = true;
            if (!doorTrigger.player) ClosedDoor();
        }

        public void EndClosedDoor()
        {
            isClosing = false;
            Sections[0].SetActiveSection(this, false);
        }
    }
}