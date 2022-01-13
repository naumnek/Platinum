using UnityEngine;
using UnityEngine.SceneManagement;
using naumnek.FPS;
using System.Collections.Generic;

namespace Unity.FPS.Game
{
    public class GameFlowManager : MonoBehaviour
    {
        [Header("Parameters")]
        public AudioSource MusicSource;
        public List<AudioClip> AllMusics = new List<AudioClip>();
        int NumberMusic;
        System.Random ran = new System.Random();

        [Tooltip("Duration of the fade-to-black at the end of the game")]
        public float EndSceneLoadDelay = 3f;


        [Tooltip("The canvas group of the fade-to-black screen")]
        public CanvasGroup EndGameFadeCanvasGroup;

        [Header("Win")] [Tooltip("This string has to be the name of the scene you want to load when winning")]
        public string WinSceneName = "MainMenu";

        [Tooltip("Duration of delay before the fade-to-black, if winning")]
        public float DelayBeforeFadeToBlack = 4f;

        [Tooltip("Win game message")]
        public string WinGameMessage;
        [Tooltip("Duration of delay before the win message")]
        public float DelayBeforeWinMessage = 2f;

        [Tooltip("Sound played on win")] public AudioClip VictorySound;

        [Header("Lose")] [Tooltip("This string has to be the name of the scene you want to load when losing")]
        public string LoseSceneName = "MainMenu";


        public bool GameIsEnding { get; private set; }

        float m_TimeLoadEndGameScene;
        string m_SceneToLoad;

        void Awake()
        {
            EventManager.AddListener<SwitchMusicEvent>(OnSwitchMusic);
            EventManager.AddListener<GamePauseEvent>(OnGamePause);
            EventManager.AddListener<ExitMenuEvent>(OnExitMenu);
            EventManager.AddListener<AllObjectivesCompletedEvent>(OnAllObjectivesCompleted);
            EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
        }

        void Start()
        {
            AudioUtility.SetMasterVolume(1);
            NumberMusic = ran.Next(0, AllMusics.Count);
            int l = AllMusics.Count;
            string random = "/";
            for (int i = 0; i < 10; i++)
            {
                random += ran.Next(0, l);
            }
            SetMusic();
            MusicSource.Play();
        }

        void OnGamePause(GamePauseEvent evt)
        {
            if (evt.Pause) MusicSource.Pause();
            else MusicSource.Play();
        }

        void OnSwitchMusic(SwitchMusicEvent evt)
        {
            switch (evt.SwitchMusic)
            {
                case ("left"):
                    NumberMusic--;
                    SetMusic();
                    break;
                case ("right"):
                    NumberMusic++;
                    SetMusic();
                    break;
            }
        }

        void SetMusic()
        {
            if (NumberMusic < 0) NumberMusic = AllMusics.Count - 1;
            if (NumberMusic > AllMusics.Count - 1) NumberMusic = 0;
            MusicSource.clip = AllMusics[NumberMusic];
            SwitchMusicEvent evt = Events.SwitchMusicEvent;
            evt.Music = AllMusics[NumberMusic];
            evt.SwitchMusic = "none";
            EventManager.Broadcast(evt);
        }

        void Update()
        {
            if (GameIsEnding)
            {
                float timeRatio = 1 - (m_TimeLoadEndGameScene - Time.time) / EndSceneLoadDelay;
                //EndGameFadeCanvasGroup.alpha = timeRatio;

                AudioUtility.SetMasterVolume(1 - timeRatio);

                // See if it's time to load the end scene (after the delay)
                if (Time.time >= m_TimeLoadEndGameScene)
                {
                    FileManager.LoadScene(LoseSceneName, 0);
                    FileManager.load = true;
                    GameIsEnding = false;
                }
            }
        }
        void OnExitMenu(ExitMenuEvent evt) => ResultEndGame(false);

        void OnAllObjectivesCompleted(AllObjectivesCompletedEvent evt)
        {
            ResultEndGame(true); 
        }

        void OnPlayerDeath(PlayerDeathEvent evt) => ResultEndGame(false);

        void ResultEndGame(bool win)
        {
            // unlocks the cursor before leaving the scene, to be able to click buttons
            //Cursor.lockState = CursorLockMode.None;
           // Cursor.visible = true;

            // Remember that we need to load the appropriate end scene after a delay
            GameIsEnding = true;
            //EndGameFadeCanvasGroup.gameObject.SetActive(true);
            if (win)
            {
                PlayerPrefs.SetString("ResultEndGame", "win");

                m_SceneToLoad = WinSceneName;
                m_TimeLoadEndGameScene = Time.time + EndSceneLoadDelay + DelayBeforeFadeToBlack;

                MusicSource.Pause();

                // play a sound on win
                var audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = VictorySound;
                audioSource.playOnAwake = false;
                audioSource.outputAudioMixerGroup = AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.HUDVictory);
                audioSource.PlayScheduled(AudioSettings.dspTime + DelayBeforeWinMessage);

                // create a game message
                //var message = Instantiate(WinGameMessagePrefab).GetComponent<DisplayMessage>();
                //if (message)
                //{
                //    message.delayBeforeShowing = delayBeforeWinMessage;
                //    message.GetComponent<Transform>().SetAsLastSibling();
                //}

                //DisplayMessageEvent displayMessage = Events.DisplayMessageEvent;
                //displayMessage.Message = WinGameMessage;
                //displayMessage.DelayBeforeDisplay = DelayBeforeWinMessage;
                //EventManager.Broadcast(displayMessage);
            }
            else
            {
                PlayerPrefs.SetString("ResultEndGame", "lose");

                m_SceneToLoad = LoseSceneName;
                m_TimeLoadEndGameScene = Time.time + EndSceneLoadDelay;
            }
        }

        void OnDestroy()
        {
            EventManager.RemoveListener<AllObjectivesCompletedEvent>(OnAllObjectivesCompleted);
            EventManager.RemoveListener<PlayerDeathEvent>(OnPlayerDeath);
        }
    }
}