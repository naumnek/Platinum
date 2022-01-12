using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
using System.Linq;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;


namespace naumnek.FPS
{
    public class MenuController : MonoBehaviour
    {
        public GameObject startMenu;
        public GameObject[] allMenu;
        public Slider MusicVolume;
        public Slider LookSensitivity;
        public TMP_Dropdown GraphicsQuality;
        public TMP_Dropdown ScreenResolution;
        public Toggle FullScreen;
        public TMP_InputField Seed;
        public AudioMixer MusicMixer;
        public AudioSource MusicSource;
        public List<AudioClip> WinMusics = new List<AudioClip>();
        public List<AudioClip> LoseMusics = new List<AudioClip>();
        public Image Background;
        public List<Sprite> BackgroundSprites = new List<Sprite>();
        public bool load = true;
        //PRIVATE
        PlayerInputHandler m_PlayerInputsHandler;
        private static MenuController instance;
        FileManager _fileManager;
        System.Random ran = new System.Random();
        List<Resolution> ScreenResolutions = new List<Resolution>();

        public static MenuController GetMenuController()
        {
            return instance.GetComponent<MenuController>();
        }

        void Start() //запускаем самый первый процесс
        {
            instance = this;
            m_PlayerInputsHandler = FindObjectOfType<PlayerInputHandler>();

            SetMusic();
            LoadUI();
        }

        void SetMusic()
        {
            if (PlayerPrefs.GetString("FirstStart") == "yes") MusicSource.clip = WinMusics[ran.Next(0, WinMusics.Count)];
            else
            {
                switch (PlayerPrefs.GetString("ResultEndGame"))
                {
                    case ("win"):
                        MusicSource.clip = WinMusics[ran.Next(0, WinMusics.Count)];
                        break;
                    case ("lose"):
                        MusicSource.clip = LoseMusics[ran.Next(0, WinMusics.Count)];
                        break;
                    case ("none"):
                        MusicSource.clip = WinMusics[ran.Next(0, WinMusics.Count)];
                        break;
                }
            }


            if (MusicSource.clip.name == "Cafofo - AMB - Muffled Pop Music") MusicSource.volume = 1;
            MusicSource.Play();
        }

        private void Update()
        {
            if (!FileManager.load != startMenu.activeSelf && load)
            {
                load = false;
                startMenu.SetActive(true);
            }
        }

        private void LoadUI()
        {
            Background.sprite = BackgroundSprites[ran.Next(0, BackgroundSprites.Count)];

            ScreenResolutions.AddRange(Screen.resolutions);
            foreach (Resolution resolution in ScreenResolutions)
            {
                ScreenResolution.options.Add(new TMP_Dropdown.OptionData(resolution.ToString()));
            }
            if (PlayerPrefs.GetString("CheckSave") == "yes") LoadSettings();
        }

        private void LoadSettings() //загружаем информацию из файлов
        {
            MusicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
            LookSensitivity.value = PlayerPrefs.GetFloat("LookSensitivity");
            GraphicsQuality.value = PlayerPrefs.GetInt("GraphicsQuality");
            ScreenResolution.value = PlayerPrefs.GetInt("ScreenResolution");
            FullScreen.isOn = PlayerPrefs.GetString("FullScreen") == "True";
            print("EndLoad");
        }

        private void SaveSettings() //сохраняем значения объектов в файл
        {
            PlayerPrefs.SetFloat("MusicVolume", MusicVolume.value);
            PlayerPrefs.SetFloat("LookSensitivity", LookSensitivity.value);
            PlayerPrefs.SetInt("GraphicsQuality", GraphicsQuality.value);
            PlayerPrefs.SetInt("ScreenResolution", ScreenResolution.value);
            PlayerPrefs.SetString("FullScreen", FullScreen.isOn.ToString());
            PlayerPrefs.SetString("CheckSave", "yes");
            print("EndSave");
        }

        public void SetMusicVolume(Slider slider) //установка громкости звука
        {
            slider.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (slider.value * 4).ToString();
            MusicMixer.SetFloat("musicVolume", -(25 - slider.value));
        }

        public void SetLookSensitivity(Slider slider)
        {
            slider.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = slider.value.ToString();
        }

        public void SetQualitySettings(TMP_Dropdown dropdown)
        {
            QualitySettings.SetQualityLevel(dropdown.value, Screen.fullScreen);
        }

        public void SetScreenResolution(TMP_Dropdown ResolutionDropdown) // вкл/выкл полноэкранный режим
        {
            for (int i = 0; i < ScreenResolutions.Count; i++)
            {
                if (i == ResolutionDropdown.value) Screen.SetResolution(ScreenResolutions[i].width, ScreenResolutions[i].height, Screen.fullScreen);
            }
        }

        public void SetFullScreen(Toggle toggle) // вкл/выкл полноэкранный режим
        {
            Screen.fullScreen = toggle.isOn;
        }

        public void SaveButton() //кнопка сохранения
        {
            SaveSettings();
        }

        public void Scenes(string scene) //загрузка уровня Tutorial
        {
            PlayerPrefs.SetString("FirstStart", "no");
            foreach (GameObject copy in allMenu)
            {
                copy.gameObject.SetActive(false);
            }
            if (Seed.text == "" || !int.TryParse(Seed.text, out _))
            {
                FileManager.LoadScene(scene, 0);
            }
            else
            {
                FileManager.LoadScene(scene, Convert.ToInt32(Seed.text));
            }
            FileManager.load = true;
        }

        public void SelectMenu(GameObject menu) //открыть главное меню и закрыть все остальные
        {
            foreach (GameObject copy in allMenu)
            {
                if (copy != menu) copy.gameObject.SetActive(false);
            }
            menu.gameObject.SetActive(true);
        }

        public void Exit() //выход из игры
        {
            PlayerPrefs.SetString("FirstStart", "yes");
            Application.Quit();
        }
    }
}

