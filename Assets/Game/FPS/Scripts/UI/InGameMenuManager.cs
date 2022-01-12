using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using naumnek.FPS;
using UnityEngine.Audio;

namespace Unity.FPS.UI
{
    public class InGameMenuManager : MonoBehaviour
    {
        [Tooltip("Root GameObject of the menu used to toggle its activation")]
        public GameObject MenuRoot;

        [Tooltip("Master volume when menu is open")] [Range(0.001f, 1f)]
        public float VolumeWhenMenuOpen = 0.5f;

        public Slider MusicVolume;
        public TMP_InputField SwitchMusic;
        public AudioMixer MusicMixer;

        [Tooltip("Slider component for look sensitivity")]
        public Slider LookSensitivity;

        [Tooltip("Toggle component for shadows")]
        public Toggle ShadowsToggle;

        [Tooltip("Toggle component for framerate display")]
        public Toggle FramerateToggle;

        [Tooltip("Inputfield for the show seed")]
        public TMP_InputField Seed;


        public string LevelSeed;
        PlayerInputHandler m_PlayerInputsHandler;
        Health m_PlayerHealth;
        FramerateCounter m_FramerateCounter;

        void Awake()
        {
            EventManager.AddListener<SwitchMusicEvent>(OnSwitchMusic);
        }

        void Start()
        {
            m_PlayerInputsHandler = FindObjectOfType<PlayerInputHandler>();
            DebugUtility.HandleErrorIfNullFindObject<PlayerInputHandler, InGameMenuManager>(m_PlayerInputsHandler,
                this);

            m_PlayerHealth = m_PlayerInputsHandler.GetComponent<Health>();
            DebugUtility.HandleErrorIfNullGetComponent<Health, InGameMenuManager>(m_PlayerHealth, this, gameObject);

            m_FramerateCounter = FindObjectOfType<FramerateCounter>();
            DebugUtility.HandleErrorIfNullFindObject<FramerateCounter, InGameMenuManager>(m_FramerateCounter, this);

            LevelSeed = FileManager.GetSeed().ToString();

            ShadowsToggle.isOn = QualitySettings.shadows != ShadowQuality.Disable;
            FramerateToggle.isOn = m_FramerateCounter.UIText.gameObject.activeSelf;

            if (PlayerPrefs.GetString("CheckSave") == "yes") LoadSettings();

            MenuRoot.SetActive(false);
        }
        private void LoadSettings() //загружаем информацию из файлов
        {
            MusicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
            LookSensitivity.value = PlayerPrefs.GetFloat("LookSensitivity");
        }


        void Update()
        {
            // Lock cursor when clicking outside of menu
            if (!MenuRoot.activeSelf && Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (Input.GetButtonDown(GameConstants.k_ButtonNamePauseMenu) || (MenuRoot.activeSelf && Input.GetButtonDown(GameConstants.k_ButtonNameCancel)))
            {
                if (Seed.gameObject.activeSelf)
                {
                    Seed.gameObject.SetActive(false);
                    return;
                }

                SetPauseMenuActivation(!MenuRoot.activeSelf);

            }

            if (Input.GetAxisRaw(GameConstants.k_AxisNameVertical) != 0)
            {
                if (EventSystem.current.currentSelectedGameObject == null)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    LookSensitivity.Select();
                }
            }
        }

        public void Exit()
        {
            EventManager.Broadcast(Events.ExitMenu);
            SetPauseMenuActivation(false);
        }

        public void ClosePauseMenu()
        {
            SetPauseMenuActivation(false);
        }

        void SetPauseMenuActivation(bool active)
        {
            GamePauseEvent evt = Events.GamePauseEvent;
            evt.Pause = active;
            EventManager.Broadcast(evt);

            MenuRoot.SetActive(active);

            if (MenuRoot.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
                //AudioUtility.SetMasterVolume(VolumeWhenMenuOpen);

                EventSystem.current.SetSelectedGameObject(null);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
                //AudioUtility.SetMasterVolume(1);
            }

        }

        public void SetMusicVolume(Slider slider) //установка громкости звука
        {
            slider.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (slider.value * 4).ToString();
            MusicMixer.SetFloat("musicVolume", -(25 - slider.value));
        }

        void OnSwitchMusic(SwitchMusicEvent evt)
        {
            SwitchMusic.text = evt.Music.name;
        }

        public void SetSwitchMusic(string Switch) //установка громкости звука
        {
            SwitchMusicEvent evt = Events.SwitchMusicEvent;
            evt.SwitchMusic = Switch;
            EventManager.Broadcast(evt);
        }



        public void SetLookSensitivity(Slider slider)
        {
            slider.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = slider.value.ToString();
            m_PlayerInputsHandler.LookSensitivity = slider.value / 50;
        }

        public void SetShadows(Toggle toggle)
        {
            QualitySettings.shadows = toggle.isOn ? ShadowQuality.All : ShadowQuality.Disable;
        }

        public void SetInvincibility(Toggle toggle)
        {
            m_PlayerHealth.Invincible = toggle.isOn;
        }

        public void SetFramerateCounter(Toggle toggle)
        {
            m_FramerateCounter.UIText.gameObject.SetActive(toggle.isOn);
        }

        public void SetShowSeed()
        {
            Seed.gameObject.SetActive(!Seed.gameObject.activeSelf);
            Seed.text = LevelSeed;
        }
        public void SetSeed()
        {
            Seed.text = LevelSeed;
        }
    }
}