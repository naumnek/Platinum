using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;
using System;

public class IneerMind : MonoBehaviour
{
    public string user;
    public string Name;
    private int ID = 0;
    public GameObject CharM;
    private CharacterManager cm;

    void Start()
    {
        Name = this.gameObject.name;
        CharM = GameObject.FindWithTag("CharacterManager");
        cm = CharM.gameObject.GetComponent<CharacterManager>();
        //Manager();
    }

    private void Manager()
    {
        if (PlayerPrefs.HasKey(Name + " " + ID.ToString())) //проверяем наличие директории персонажа
        {
            LoadSettings(); //загружаем информацию о нём, если всё окей
        }
        else
        {
            print("Не нашёлся персонаж: " + Name); //ой-ёй-ёй
        }
    }

    private void LoadSettings() //загружаем информацию из файлов
    {

    }

    private void SaveSettings() //сохраняем значения объектов в файл
    {

    }
}
