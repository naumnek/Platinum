using System.Collections.Generic;
using System.Linq;
using LowLevelGenerator.Scripts.Exceptions;
using LowLevelGenerator.Scripts.Helpers;
using LowLevelGenerator.Scripts.Structure;
using UnityEngine;
using naumnek.FPS;

namespace LowLevelGenerator.Scripts
{
    public class LevelGenerator : MonoBehaviour
    {
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

        public int MaxLevels = 1;

        /// <summary>
        /// Maximum allowed distance from the original section
        /// </summary>
        public int MaxAllowedOrder;
        public bool EnableMinAllowedOrder;

        /// <summary>
        /// Spawnable section prefabs
        /// </summary>
        public Section[] Sections;

        /// <summary>
        /// Spawnable dead ends
        /// </summary>
        public DeadEnd[] DeadEnds;

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

        public int LevelSize;

        [HideInInspector]
        protected List<Collider> RegisteredColliders = new List<Collider>();
        [HideInInspector]
        protected List<Collider> DoorColliders = new List<Collider>();

        public List<Section> RegisteredSections = new List<Section>();

        protected bool HalfLevelBuilt => RegisteredSections.Count > LevelSize;

        protected void Start()
        {
            Seed = FileManager.GetFileManager().LevelSeed;
            if (EnableMinAllowedOrder && MaxAllowedOrder > MaxLevelSize / 2) MaxAllowedOrder = MaxLevelSize / 2;
            LevelSize = MaxLevelSize;
            if (SectionContainer == null) SectionContainer = transform;
            if (Seed != 0)
                RandomService.SetSeed(Seed);
            else
                Seed = RandomService.Seed;
            
            CreateInitialSection();
        }
        public void RegisterNewSection(Section newSection)
        {
            newSection.transform.SetParent(SectionContainer);
            RegisteredSections.Add(newSection);
            RegisteredColliders.AddRange(newSection.Bound.GetColliders);
            if (LevelSize < 1 && SectionContainer.childCount == RegisteredSections.Count)
            {
                print("EndGeneration!");
                if (GameObject.FindGameObjectWithTag("SaveObjects") != null)
                {
                    EndGeneration();
                }

            }
        }

        public void CheckRandomSection()
        {
            if (RegisteredSections.All(s => s.EndGenerateAnnexes && RegisteredSections.Count < MaxLevelSize))
            {
                for (int i = 0; i < RegisteredSections.Count; i++)
                {
                    if (RegisteredSections[i].RandomDeadEnds.Count != 0)
                    {
                        for (int ii = 0; ii < RegisteredSections[i].RandomDeadEnds.Count; ii++)
                        {
                            DeadEnd obj = RegisteredSections[i].RandomDeadEnds[ii];
                            RegisteredSections[i].DeadEnds.Remove(obj);
                            RegisteredSections[i].RandomDeadEnds.Remove(obj);
                            RegisteredSections[i].GenerateSection(obj.transform);
                            Destroy(obj.gameObject);

                        }
                    }
                }
            }
        }

        public void EndGeneration()
        {

            DeactivateBounds();
            EnemySpawner.MainStart();
        }

        protected void CreateInitialSection()
        {
            Section sec = Instantiate(PickSectionWithTag(InitialSectionTags), transform);
            LevelSize--;
            sec.transform.SetParent(SectionContainer, false);
            RegisterNewSection(sec);
            sec.Initialize(this, 0);
            if (sec.Structure.activeSelf)
            {
                foreach (DoorExit copy2 in sec.Doors) copy2.Structure.SetActive(true);
                foreach (DeadEnd copy2 in sec.DeadEnds) copy2.Structure.SetActive(true);
            }
        }

        public void AddSectionTemplate() => Instantiate(Resources.Load("SectionTemplate"), Vector3.zero, Quaternion.identity);
        public void AddDeadEndTemplate() => Instantiate(Resources.Load("DeadEndTemplate"), Vector3.zero, Quaternion.identity);

        public bool IsSectionValid(BoundSection newSection, BoundSection sectionToIgnore) => 
            !RegisteredColliders.Except(sectionToIgnore.GetColliders).Any(c => c.bounds.Intersects(newSection.GetColliders.First().bounds));


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

        protected void DeactivateBounds()
        {
            /*foreach (Collider copy in RegisteredColliders)
            {
                copy.enabled = false;
            }*/
        }
    }
}