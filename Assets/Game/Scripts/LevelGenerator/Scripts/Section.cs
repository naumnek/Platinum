using System.Linq;
using LowLevelGenerator.Scripts.Helpers;
using System.Collections.Generic;
using UnityEngine;
using naumnek.FPS;
using Unity.FPS.Game;
using Unity.FPS.AI;

namespace LowLevelGenerator.Scripts
{
    public class Section : MonoBehaviour
    {
        public bool Matched = true;

        /// <summary>
        /// Section tags
        /// </summary>
        public string[] Tags;

        /// <summary>
        /// Tags that this section can annex
        /// </summary>
        public string[] CreatesTags;

        /// <summary>
        /// Exits node in hierarchy
        /// </summary>
        public List<Transform> Exits = new List<Transform>();
        //check end generation exits in the section 
        public bool EndGenerateAnnexes = false;

        public BoundSection Bound;

        public GameObject ParentCollider;

        public GameObject Structure;

        public Transform Spawner;

        /// <summary>
        /// Chances of the section spawning a dead end
        /// </summary>
        public int DeadEndChance;

        [HideInInspector]
        public List<DoorExit> Doors = new List<DoorExit>();
        [HideInInspector]
        public List<DeadEnd> DeadEnds = new List<DeadEnd>();
        [HideInInspector]
        public List<Section> FlankSections = new List<Section>();
        [HideInInspector]
        public List<DoorExit> FlankDoors = new List<DoorExit>();
        [HideInInspector]
        public List<DeadEnd> RandomDeadEnds = new List<DeadEnd>();
        [HideInInspector]
        public List<Transform> Enemys = new List<Transform>();

        Transform ContainerDoors;
        Transform ContainerDeadEnds;
        protected LevelGenerator m_LevelGenerator;
        protected int order;

        private void Awake()
        {
            Bound.section = this;
        }

        public void Initialize(LevelGenerator levelGenerator, int sourceOrder)
        {
            Bound = GetComponentInChildren<BoundSection>();
            m_LevelGenerator = levelGenerator;

            ContainerDoors = new GameObject("Doors").transform;
            ContainerDoors.SetParent(this.transform);
            ContainerDeadEnds = new GameObject("DeadEnds").transform;
            ContainerDeadEnds.SetParent(this.transform);

            if (Tags.First() == "spawn") EnemySpawner.InitializePlayer(Spawner);
            if(Tags.First() == "room")
            {
                Transform obj = EnemySpawner.ActivateSpawner(Spawner);
                obj.GetComponent<EnemyController>().SpawnSection = this;
                Enemys.Add(obj);
            }

            order = sourceOrder + 1;

            GenerateAnnexes();
        }

        protected void GenerateAnnexes()
        {
            foreach (Transform copy in Exits)
            {
                if (m_LevelGenerator.LevelSize > 0 && order < m_LevelGenerator.MaxAllowedOrder)
                {
                    if (DeadEndChance != 0 && RandomService.RollD100(DeadEndChance))
                    {
                        PlaceDeadEnd(copy,true); 
                    }
                    else
                    {
                        GenerateSection(copy);
                    }
                }
                else 
                {
                    PlaceDeadEnd(copy, false); 
                }
            }
            EndGenerateAnnexes = true;
            m_LevelGenerator.CheckRandomSection();
        }

        public void GenerateSection(Transform exit)
        {
            Section section = Instantiate(m_LevelGenerator.PickSectionWithTag(CreatesTags), exit);
            if (m_LevelGenerator.IsSectionValid(section.Bound, Bound))
            {
                m_LevelGenerator.LevelSize--;
                section.transform.SetParent(m_LevelGenerator.SectionContainer);
                m_LevelGenerator.RegisterNewSection(section);
                section.FlankSections.Add(this);
                FlankSections.Add(section);
                PlaceDoor(exit, section);
                section.Initialize(m_LevelGenerator, order);
            }
            else
            {
                Destroy(section.gameObject);
                PlaceDeadEnd(exit,false);
            }
        }

        //создаем на месте полученого Transform exit одну из стен в DeadEnds и заносим её в список DeadEndColliders 
        protected void PlaceDeadEnd(Transform exit, bool random)
        {
            DeadEnd obj = Instantiate(m_LevelGenerator.DeadEnds.PickOne(), exit);
            obj.transform.SetParent(ContainerDeadEnds);
            DeadEnds.Add(obj);
            if (random) RandomDeadEnds.Add(obj);
        }

        //создаем на месте полученого Transform exit одну из стен в DeadEnds и заносим её в список DeadEndColliders 
        protected void PlaceDoor(Transform exit, Section section)
        {
            DoorExit obj = Instantiate(m_LevelGenerator.Doors.PickOne(), exit);
            obj.transform.SetParent(ContainerDoors);
            Doors.Add(obj);
            obj.Sections.Add(this);
            obj.Sections.Add(FlankSections.Last());
            FlankDoors.Add(obj);
            section.FlankDoors.Add(obj);
        }

        public void OnEnemyInSectionKill(Transform enemy)
        {
            Enemys.Remove(enemy);
            if(Enemys.Count == 0 && !Matched)
            {
                EventManager.Broadcast(Events.RoomMatchedEvent);
                Matched = true;
                LockedAllDoors(false);
            }
        }
        private void LockedIsOpenDoors(DoorExit openDoor, bool action)
        {
            for (int ii = 0; ii < FlankDoors.Count(); ii++)
            {
                if(FlankDoors[ii] != openDoor) FlankDoors[ii].LockedRoom = action;
            }
            for (int ii = 0; ii < Doors.Count(); ii++)
            {
                if(Doors[ii] != openDoor) Doors[ii].LockedRoom = action;
            }
        }

        private void LockedAllDoors(bool action)
        {
            for (int ii = 0; ii < FlankDoors.Count(); ii++)
            {
                FlankDoors[ii].LockedRoom = action;
            }
            for (int ii = 0; ii < Doors.Count(); ii++)
            {
                Doors[ii].LockedRoom = action;
            }
        }

        public void SetActiveSection(DoorExit door, bool action)
        {
            List<Section> playerSection = new List<Section>();
            playerSection.AddRange(m_LevelGenerator.RegisteredSections.Where(s => s.Bound.player || s.FlankDoors.Any(d => d.isClosed) || s.Doors.Any(d => d.isClosed)));

            List<Section> section = new List<Section>();
            section.AddRange(door.Sections.Where(s => !playerSection.Any(ss => ss == s)));
            for (int i = 0; i < section.Count(); i++)
            {
                section[i].Structure.SetActive(action);
                //
                for (int ii = 0; ii < section[i].DeadEnds.Count(); ii++)
                {
                    section[i].DeadEnds[ii].Structure.SetActive(action);
                }
                //
                for (int ii = 0; ii < section[i].FlankDoors.Count(); ii++)
                {
                    if (action || !playerSection.Any(s => s.FlankDoors.Any(d => d == section[i].FlankDoors[ii])))
                    {
                        section[i].FlankDoors[ii].Structure.SetActive(action);
                        section[i].FlankDoors[ii].enabled = action;
                    }
                }
            }
            Section triggerPlayerSection = m_LevelGenerator.RegisteredSections.Where(s => s.Bound.player).First();
            if (action)
            {
                Section notTriggerPlayerSection = section.Where(s => s != triggerPlayerSection).First();
                if (!notTriggerPlayerSection.Matched)
                {
                    notTriggerPlayerSection.LockedIsOpenDoors(door, true);
                }
            }
            else
            {
                if (!triggerPlayerSection.Matched)
                {
                    triggerPlayerSection.LockedAllDoors(true);
                }
                door.Locked = false;
            }
        }
    }
}