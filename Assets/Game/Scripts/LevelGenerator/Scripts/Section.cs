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
        public List<DoorExit> FlankDoors = new List<DoorExit>();
        [HideInInspector]
        public List<GameObject> DeadEnds = new List<GameObject>();
        [HideInInspector]
        public List<Section> FlankSections = new List<Section>();
        [HideInInspector]
        public List<GameObject> RandomDeadEnds = new List<GameObject>();
        [HideInInspector]
        public List<Transform> Enemys = new List<Transform>();

        public bool TriggerPlayer { get; private set; } = false;

        Transform ContainerDoors;
        Transform ContainerDeadEnds;
        protected LevelGenerator m_LevelGenerator;
        protected int order;

        private void Awake()
        {
            Bound.ParentSection = this;
        }

        public void Initialize(LevelGenerator levelGenerator, int sourceOrder)
        {
            Bound = GetComponentInChildren<BoundSection>();
            m_LevelGenerator = levelGenerator;

            ContainerDoors = new GameObject("Doors").transform;
            ContainerDoors.SetParent(transform);
            ContainerDeadEnds = new GameObject("DeadEnds").transform;
            ContainerDeadEnds.SetParent(Structure.transform);

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
            GameObject obj = Instantiate(m_LevelGenerator.DeadEnds.PickOne(), exit);
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

        public void OnTriggerPlayer(bool trigger)
        {
            TriggerPlayer = trigger;
            if (trigger)
            {
                for (int i = 0; i < FlankDoors.Count; i++)
                {
                    FlankDoors[i].PlayerSectionMatched = Matched;
                }
            }
        }

        public void OnEnemyInSectionKill(Transform enemy)
        {
            Enemys.Remove(enemy);
            if(Enemys.Count == 0 && !Matched)
            {
                Matched = true;
                for (int i = 0; i < FlankDoors.Count; i++)
                {
                    FlankDoors[i].PlayerSectionMatched = Matched;
                }
            }
        }

        public void SetActiveSection(DoorExit door, bool action)
        {
            List<Section> EmptySections = new List<Section>();
            EmptySections.AddRange(door.Sections.Where(s => !s.TriggerPlayer)); 

            for (int i = 0; i < EmptySections.Count; i++)
            {
                if(action || EmptySections[i].FlankDoors.All(d => !d.isClosing)) EmptySections[i].Structure.SetActive(action);
                
                for (int ii = 0; ii < EmptySections[i].FlankDoors.Count; ii++)
                {                  
                    if (action || EmptySections[i].FlankDoors[ii].Sections.All(s => !s.TriggerPlayer && s.FlankDoors.All(d => !d.isClosing)))
                    {
                        EmptySections[i].FlankDoors[ii].Structure.SetActive(action);
                    }
                }
            }
        }
    }
}