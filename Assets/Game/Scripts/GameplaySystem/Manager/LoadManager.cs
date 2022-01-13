using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Game;


namespace naumnek.FPS
{
    public class LoadManager : MonoBehaviour
    {
        public GameObject general;
        private static LoadManager instance;

        void Awake()
        {
            instance = this;
            EventManager.AddListener<EndGenerationEvent>(OnEndGeneration);
            EventManager.AddListener<ExitMenuEvent>(OnExitMenu);
        }

        // Start is called before the first frame update
        public void OnEndGeneration(EndGenerationEvent evt)
        {
            general.SetActive(true);
            NavMeshGenerate.Build();
        }
        public void OnExitMenu(ExitMenuEvent evt)
        {
            general.SetActive(false);
        }
    }
}

