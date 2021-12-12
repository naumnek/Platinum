using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;
using System;
using System.Globalization;


namespace naumnek.FPS
{
    public class MenuController : MonoBehaviour
    {
        public GameObject startMenu;
        public GameObject[] allMenu;
        public List<Slider> allSlider = new List<Slider>();
        public List<Toggle> allToggle = new List<Toggle>();
        public AudioMixer Music;
        public FileManager _fileManager;
        public bool load = true;
        //PRIVATE
        private static MenuController instance;

        public static MenuController GetMenuController()
        {
            return instance.GetComponent<MenuController>();
        }

        void Start() //запускаем самый первый процесс
        {
            instance = this;
            LoadSettings();
        }

        private void Update()
        {
            if (!FileManager.load != startMenu.activeSelf && load)
            {
                print("Load3: " + FileManager.load);
                load = false;
                startMenu.SetActive(true);
            }
        }


        private void LoadSettings() //загружаем информацию из файлов
        {
            foreach (Slider copy in allSlider)
            {
                copy.value = PlayerPrefs.GetFloat(copy.name);
                copy.gameObject.transform.GetChild(0).GetComponent<Text>().text = copy.value.ToString();
            }
            foreach (Toggle copy in allToggle)
            {
                if (PlayerPrefs.GetString(copy.name) == "True") { copy.isOn = true; };
                if (PlayerPrefs.GetString(copy.name) == "False") { copy.isOn = false; };
            }
            Music.SetFloat("musicVolume", -PlayerPrefs.GetFloat("MusicVolume"));
            if (PlayerPrefs.GetString("IsFullScreen") == "true") { Screen.fullScreen = true; };
            if (PlayerPrefs.GetString("IsFullScreen") == "False") { Screen.fullScreen = false; };
            QualitySettings.SetQualityLevel((int)PlayerPrefs.GetFloat("SelectQuality"));
        }

        private void SaveSettings() //сохраняем значения объектов в файл
        {
            foreach (Slider copy in allSlider)
            {
                PlayerPrefs.SetFloat(copy.name, copy.value);
            }
            foreach (Toggle copy in allToggle)
            {
                PlayerPrefs.SetString(copy.name, copy.isOn.ToString());
            }
            PlayerPrefs.SetString("CheckSave", "yes");
            print("Save");
        }

        public void SaveButton() //кнопка сохранения
        {
            SaveSettings();
            LoadSettings();
        }

        public void Language(string lang)
        {

        }

        public void Slider(Slider slider)
        {
            slider.gameObject.transform.GetChild(0).GetComponent<Text>().text = slider.value.ToString(); //находим текстовый дочерний обьект и приваеваем ему значение слайдера
        }

        public void MusicVolume(Slider slider) //установка громкости звука
        {
            slider.gameObject.transform.GetChild(0).GetComponent<Text>().text = (100 - (slider.value * 2)).ToString();
            Music.SetFloat("musicVolume", -slider.value);
        }

        public void Scenes(string scene) //загрузка уровня Tutorial
        {
            FileManager.SwitchScene(scene);
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
            Application.Quit();
        }

        public void FullScreenToggle(Toggle toggle) // вкл/выкл полноэкранный режим
        {
            Screen.fullScreen = toggle.isOn;
        }

        /*public void JoystickToggle(Toggle toggle) // вкл/выкл полноэкранный режим
        {
            foreach (GameObject copy in AllJoystick)
            {
                if (copy != AllJoystick[0])
                {
                    copy.gameObject.SetActive(toggle.isOn);
                }
            }
        }*/

    }
}

