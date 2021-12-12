using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDController : MonoBehaviour
{
    public int speed = 5;
    public GameObject player; //здесь ми указываем персонажа как игровой Object;
    //public GameObject go;

    void Start()
    {
        player = (GameObject)this.gameObject; //тут присваиваем персонажа к игровому Object или как-то так.
        //go = goCamera.transform.parent.gameObject;
    }
    // Ах да вместо player надо ставить имя твоего перса которое записано в Unity;
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            player.transform.position += player.transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            player.transform.position -= player.transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            player.transform.position += player.transform.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            player.transform.position -= player.transform.right * speed * Time.deltaTime;//персонаж плавно двигается на W,S,D,A;
        }                                              //всё легко и просто, как борщ(всё как Вы и просили)
    }
}
