using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using LowLevelGenerator.Scripts.Helpers;
using TMPro;


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
        public TMP_Text ValueLoading;
        public Image ValueLoadingBar;
        public GameObject loading;
        public GameObject clock;
        public LoadManager _LoadManager;
        //PRIVATE
        private string loadscene = "Menu";
        private GameObject Canvas;
        private MenuController mainMenu;
        private AsyncOperation loadingSceneOperation;
        private static FileManager instance;
        public static bool load = false;
        private Animator anim;
        private Animator clockanim;

        void Start() //запускаем самый первый процесс
        {
            instance = this;
            clockanim = clock.GetComponent<Animator>();
            anim = loading.GetComponent<Animator>();
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
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
            loadscene = scene;
            anim.SetTrigger("Visibly");
            clockanim.SetTrigger("ClockWait");
            if (loadscene == "MainMenu")
            {
                LoadManager.Unload();
            }
            else if (seed == 0) LevelSeed = RandomService.Seed;
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
                LoadManager.Load();
            }
        }

        public void StartLoadScene()
        {
            if (loadscene != "MainMenu")
            {
                //mainMenu = MenuController.GetMenuController();
                //mainMenu.gameObject.SetActive(false);
            }
            loadingSceneOperation.allowSceneActivation = true;
            anim.SetTrigger("Unvisibly");
            load = false;
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
    }

}


