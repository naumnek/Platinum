using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;*/
using System;
using UnityEngine.UI;


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
        public Text ValueLoading;
        public Image ValueLoadingBar;
        public GameObject loading;
        public GameObject clock;
        public CharacterManager _CharacterManager;
        public ItemsManager _ItemsManager;
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

        public static FileManager GetFileManager()
        {
            return instance.GetComponent<FileManager>();
        }

        public static void SwitchScene(string scene)
        {
            instance.anim.SetTrigger("Visibly");
            instance.clockanim.SetTrigger("ClockWait");
            instance.loadingSceneOperation = SceneManager.LoadSceneAsync(scene);
            instance.loadingSceneOperation.allowSceneActivation = false;
            instance.loadscene = scene;
        }

        public void EndLoadScene()
        {
            print("Load1: " + load);
            if (loadscene == "MainMenu")
            {

            }
            else
            {
                LoadManager.Load();
            }
        }

        public void StartLoadScene()
        {
            print("Load2: " + load);
            if (loadscene != "MainMenu")
            {
                mainMenu = MenuController.GetMenuController();
                mainMenu.gameObject.SetActive(false);
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

        void Start() //запускаем самый первый процесс
        {
            instance = this;
            clockanim = clock.GetComponent<Animator>();
            anim = loading.GetComponent<Animator>();
            Manager();
        }

        private void Manager()
        {
            Cursor.lockState = CursorLockMode.Locked; //заблокировать курсор
            Cursor.visible = false; //скрыть курсор
            Cursor.lockState = CursorLockMode.None; //разблокировать курсор
            Cursor.visible = true; //показать курсор
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


