using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
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
        public string ResultEndGame = "None";
        //PRIVATE
        private string loadscene = "FirstLoadMenu";
        private GameObject Canvas;
        private MenuController mainMenu;
        private AsyncOperation loadingSceneOperation;
        private static FileManager instance;
        public static bool load = false;
        private Animator background_anim;
        private Animator clockanim;
        System.Random ran = new System.Random();

        void Awake() //запускаем самый первый процесс
        {
            instance = this;
            clockanim = clock.GetComponent<Animator>();
            background_anim = loading.GetComponent<Animator>();
            EventManager.AddListener<EndGenerationEvent>(OnEndGeneration);
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (loadscene == "FirstLoadMenu") return;
            if (scene.name == "MainMenu")
            {
                background_anim.SetTrigger("Unvisibly");
            }
            else
            {
                if (LevelSeed == 0) LevelSeed = ran.Next(Int32.MinValue, Int32.MaxValue);
                StartGenerationEvent evt = Events.StartGenerationEvent;
                evt.Seed = LevelSeed;
                EventManager.Broadcast(evt);
            }
        }

        public static FileManager GetFileManager()
        {
            return instance.GetComponent<FileManager>();
        }
        public static int GetSeed()
        {
            return instance.LevelSeed;
        }

        public static void EndGame(string scene, string result)
        {
            instance.ResultEndGame = result;
            instance.SwitchSceme(scene);
        }

        public static void LoadScene(string scene, int seed)
        {
            instance.LevelSeed = seed;
            instance.SwitchSceme(scene);
        }

        void SwitchSceme(string scene)
        {
            load = true;
            loadscene = scene;
            if (loadscene == "MainMenu")
            {

            }
            LoadingCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            background_anim.SetTrigger("Visibly");
            clockanim.SetTrigger("ClockWait");
            loadingSceneOperation = SceneManager.LoadSceneAsync(scene);
            loadingSceneOperation.allowSceneActivation = false;
        }

        public void EndLoadScene()
        {
            load = false;
            LoadingCanvas.SetActive(false);
            if (loadscene == "MainMenu")
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {

            }
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


