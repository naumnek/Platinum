﻿using System.Collections.Generic;
using System.Linq;
using LowLevelGenerator.Scripts.Exceptions;
using LowLevelGenerator.Scripts.Helpers;
using LowLevelGenerator.Scripts.Structure;
using UnityEngine;
using naumnek.FPS;
using Unity.FPS.Game;
using System;
using System.Collections;

namespace LowLevelGenerator.Scripts
{
    public class LevelGenerator : MonoBehaviour
    {
        [Header("General")]

        /// <summary>
        /// LevelGenerator seed
        /// </summary>
        public int Seed;

        /// <summary>
        /// Container for all sections in hierarchy
        /// </summary>
        public Transform SectionContainer;

        /// <summary>
        /// Maximum level size measured in sections
        /// </summary>
        public int MaxLevelSize;
        public int LevelSize = 0;

        public float WaitGenerateSection = 0.1f;

        /// <summary>
        /// Maximum allowed distance from the original section
        /// </summary>
        public int MaxAllowedOrder;
        public bool EnableMinAllowedOrder;

        [Header("Other")]
        public bool DisableSectionsParser = false;

        public bool NewGeneration = false;

        public bool WhileNewGeneration = false;

        public bool CustomStartGeneration = false;

        [Header("Lists Objects")]
        /// <summary>
        /// Spawnable section prefabs
        /// </summary>
        public Section[] Sections;

        /// <summary>
        /// Spawnable dead ends
        /// </summary>
        public GameObject[] DeadEnds;

        /// <summary>
        /// Spawnable doors
        /// </summary>
        public DoorExit[] Doors;

        /// <summary>
        /// Tags that will be taken into consideration when building the first section
        /// </summary>
        public string[] InitialSectionTags;
        
        /// <summary>
        /// Special section rules, limits and forces the amount of a specific tag
        /// </summary>
        public TagRule[] SpecialRules;

        [HideInInspector]
        public List<Collider> RegisteredColliders = new List<Collider>();

        public List<Section> RegisteredSections = new List<Section>();

        Section StartSection;
        System.Random ran = new System.Random();

        private void Awake()
        {
            EventManager.AddListener<StartGenerationEvent>(OnStartGeneration);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<StartGenerationEvent>(OnStartGeneration);
        }

        void Start()
        {
            if (CustomStartGeneration) OnStartGeneration(null);
        }

        protected void CreateInitialSection()
        {
            StartSection = Instantiate(PickSectionWithTag(InitialSectionTags), transform);
            StartSection.transform.SetParent(SectionContainer, false);
            StartSection.Initialize(this, 0);
        }

        protected void OnStartGeneration(StartGenerationEvent evt)
        {
            LevelSize = 0;

            if (EnableMinAllowedOrder && MaxAllowedOrder > MaxLevelSize / 2) MaxAllowedOrder = MaxLevelSize / 2;
            if (SectionContainer == null)
            {
                SectionContainer = new GameObject("SectionContainer").transform;
                SectionContainer.SetParent(transform);
            }

            if (evt != null) Seed = evt.Seed;
            if (Seed == 0) Seed = ran.Next(Int32.MinValue, Int32.MaxValue);
            RandomService.SetSeed(Seed);

            CreateInitialSection();
        }
        public void RegisterNewSection(Section newSection)
        {            
            newSection.transform.SetParent(SectionContainer);
            RegisteredSections.Add(newSection);
            if (RegisteredSections.Count == LevelSize)
            {
                if (LevelSize >= MaxLevelSize)
                {
                    if (CustomStartGeneration)
                    {
                        if(NewGeneration) WhileNewGeneration = true;
                        print("EndGeneration: " + Seed);
                    }
                    else EndGeneration();
                }
                else
                {

                    if (CustomStartGeneration)
                    {
                        print("ErrorSeed: " + Seed);
                    }
                    CheckRandomSection();
                }
            }
        }     

        public void CheckRandomSection()
        {
            bool RespawnSection = true;
            for (int i = 0; i < RegisteredSections.Count; i++)
            {
                if (RegisteredSections[i].ValidDeadEnds.Count > 0)
                {
                    for (int ii = 0; ii < RegisteredSections[i].ValidDeadEnds.Count; ii++)
                    {
                        if (RespawnSection)
                        {
                            RespawnSection = false;
                            GameObject obj = RegisteredSections[i].ValidDeadEnds[ii];
                            RegisteredSections[i].DeadEnds.Remove(obj);
                            RegisteredSections[i].ValidDeadEnds.Remove(obj);
                            RegisteredSections[i].ExitsCompleted--;
                            StartCoroutine(RegisteredSections[i].GenerateSection(RegisteredSections[i].TransformValidDeadEnds[ii]));
                            Destroy(obj.gameObject);
                        }
                    }
                }
            }
        }

        private void Update()
        {
            if(LevelSize >= MaxLevelSize && WhileNewGeneration)
            {
                WhileNewGeneration = false;
                StartCoroutine(NewGenerationLevels());
            }
        }
        IEnumerator NewGenerationLevels()
        {
            yield return new WaitForSeconds(WaitGenerateSection * 10);
            RegisteredSections.Clear();
            RegisteredColliders.Clear();
            Destroy(SectionContainer.gameObject);
            SectionContainer = new GameObject("SectionContainer").transform;
            SectionContainer.SetParent(transform);
            Seed = 0;
            OnStartGeneration(null);
        }

        void EndGeneration()
        {
            print("EndGeneration!");
            if (StartSection.Structure.activeSelf)
            {
                foreach (DoorExit copy2 in StartSection.FlankDoors) copy2.Structure.SetActive(true);
            }
            EventManager.Broadcast(Events.EndGenerationEvent);
        }

        public void AddSectionTemplate() => Instantiate(Resources.Load("SectionTemplate"), Vector3.zero, Quaternion.identity);
        public void AddDeadEndTemplate() => Instantiate(Resources.Load("DeadEndTemplate"), Vector3.zero, Quaternion.identity);

        public bool IsSectionsIntersect(BoundSection newSection, BoundSection sectionToIgnore)
        {
            List<Collider> Colliders = RegisteredColliders;
            List<Collider> IgnoreColliders = new List<Collider>() { sectionToIgnore.MainBound };
            return Colliders.Except(IgnoreColliders).Any(c => c != null && c.bounds.Intersects(newSection.MainBound.bounds));
        }
        

        public Section PickSectionWithTag(string[] tags)
        {
            foreach (string copy in tags)
            {
                return Sections.Where(s => s.Tags.Contains(copy)).PickOne();
            }
            return Sections.Where(s => s.Tags.Contains(tags.PickOne())).PickOne();
        }

        protected string PickFromExcludedTags(string[] tags)
        {
            var tagsToExclude = SpecialRules.Where(r => r.Completed).Select(rs => rs.Tag);
            return tags.Except(tagsToExclude).PickOne();
        }

        protected bool RulesContainTargetTags(string[] tags) => tags.Intersect(SpecialRules.Where(r => r.NotSatisfied).Select(r => r.Tag)).Any();
    }
}