using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;
using System;

public class CharacterManager : MonoBehaviour
{
    //PUBLIC
    public List<GameObject> Characters = new List<GameObject> { };
    public List<string> users = new List<string> { "player", "mob1" };
    //PRIVATE
    private DataManager dm = new DataManager();

    void Start()
    {
        GameObject own_obj_char = GameObject.FindGameObjectWithTag("Characters");
        for (int i = 0; i < own_obj_char.transform.childCount; i++)
        {
            Characters.Add(own_obj_char.transform.GetChild(i).gameObject);
        }
        CharactersLife();
    }

    private void SearchCharacter()
    {
        //print("Проверка наличия персонажей НАЧАЛАСЬ");
        foreach (GameObject copy in Characters)
        {
            if (PlayerPrefs.HasKey(copy.gameObject.name))
            {
                // персонажи есть
                print("Проверка целостности файлов НАЧАТА");
                CharactersLife();
            }
            else
            {
                // персонажей нет
                print("Персонажи не обнаружены");
                CreateCharacter();
            }
        }

    }

    private void CharactersLife()
    {
        foreach (GameObject copy in Characters)
        {
            print("Оживление: " + copy.gameObject.name);
            copy.gameObject.GetComponent<IneerMind>().enabled = true;
            copy.gameObject.GetComponent<AttributesManager>().enabled = true;
            charControllers(copy, true);
        }
        //print("Проверка целостности файлов ОКОНЧЕНА");
    }

    private void RecoveryCharacter(string CreateFolder)
    {
        CharactersLife();
    }

    private void CreateCharacter() //создаём персонажа
    {

    }

    private void FixedUpdate()
    {
        foreach (GameObject copy in Characters)
        {
            AttributesManager am = copy.GetComponent<AttributesManager>();
            if (copy != null && dm.getInt(am.main_body.GetComponent<LogicCollision>().data, "durability") <= 0)
            {
                charControllers(copy, false);
                am.getCommand("die");
                //Destroy(copy);
            }
        }
    }

    private void charControllers(GameObject character, bool controller)
    {
        foreach (string copy in users)
        {
            switch (character.GetComponent<IneerMind>().user)
            {
                case "player":
                    character.gameObject.GetComponent<PlayerController>().enabled = controller;
                    break;
                case "mob1":
                    character.gameObject.GetComponent<BodyMove>().enabled = controller;
                    break;
                default:
                    Console.WriteLine("Обнаружен неизвестный персонаж: " + character.name);
                    break;
            }
        }
    }
}

