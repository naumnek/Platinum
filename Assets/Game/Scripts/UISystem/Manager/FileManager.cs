using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using LowLevelGenerator.Scripts.Helpers;
using TMPro;
using Unity.FPS.Game;


namespace naumnek.FPS
{
    public class FileManager : MonoBehaviour
    {
        [Header("General")]
        //PUBLIC
        public bool checkLoad;
        [Tooltip("Versions determines which scripts the file manager should use")]
        public string GameVersion = "fps_1";
        [Header("References")]
        public int LevelSeed;
        public GameObject LoadingCanvas;
        public TMP_Text ValueLoading;
        public Image ValueLoadingBar;
        public GameObject loading;
        public GameObject clock;
        //PRIVATE
        private string loadscene = "FirstLoadMenu";
        private GameObject Canvas;
        private MenuController mainMenu;
        private AsyncOperation loadingSceneOperation;
        private static FileManager instance;
        public static bool load = false;
        private Animator background_anim;
        private Animator clockanim;

        void Awake() //запускаем самый первый процесс
        {
            instance = this;
            clockanim = clock.GetComponent<Animator>();
            background_anim = loading.GetComponent<Animator>();
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (loadscene == "FirstLoadMenu") return;
            if (scene.name == "MainMenu")
            {

            }
            else
            {
                EventManager.AddListener<EndGenerationEvent>(OnEndGeneration);
                StartGenerationEvent evt = Events.StartGenerationEvent;
                evt.Seed = LevelSeed;
                EventManager.Broadcast(evt);
            }
            background_anim.SetTrigger("Unvisibly");
        }

        public static FileManager GetFileManager()
        {
            return instance.GetComponent<FileManager>();
        }
        public static int GetSeed()
        {
            return instance.LevelSeed;
        }
        
        public static void LoadScene(string scene, int seed)
        {
            instance.SwitchSceme(scene, seed);
        }

        void SwitchSceme(string scene, int seed)
        {
            LoadingCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
            loadscene = scene;
            LevelSeed = seed;
            background_anim.SetTrigger("Visibly");
            clockanim.SetTrigger("ClockWait");
            EventManager.RemoveListener<EndGenerationEvent>(OnEndGeneration);
            if (loadscene == "MainMenu")
            {
                EventManager.Broadcast(Events.ExitMenuEvent);
            }
            loadingSceneOperation = SceneManager.LoadSceneAsync(scene);
            loadingSceneOperation.allowSceneActivation = false;
        }

        public void EndLoadScene()
        {
            if (loadscene == "MainMenu")
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {

            }
            LoadingCanvas.SetActive(false);
        }

        public void StartLoadScene()
        {
            loadingSceneOperation.allowSceneActivation = true;
            load = false;
        }

        public void OnEndGeneration(EndGenerationEvent evt)
        {
            background_anim.SetTrigger("Unvisibly");
        }

        public void LoadMenu(bool active)
        {
            mainMenu = MenuController.GetMenuController();
            mainMenu.gameObject.SetActive(active);
            mainMenu.startMenu.SetActive(active);
        }



        void Update()
        {
            checkLoad = load;
            if (load)
            {
                ValueLoading.text = (Mathf.RoundToInt(loadingSceneOperation.progress * 100)).ToString() + "%";
                ValueLoadingBar.fillAmount = loadingSceneOperation.progress;
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
            EventManager.RemoveListener<EndGenerationEvent>(OnEndGeneration);
        }
    }

}


