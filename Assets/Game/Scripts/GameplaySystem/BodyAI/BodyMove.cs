using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BodyMove : MonoBehaviour
{
    //PUBLIC
    public AttributesManager am;
    public GameObject body; //тело персонажа
    public List<GameObject> interest_obj = new List<GameObject> { };
    //PRIVATE
    private DataManager dm = new DataManager();
    private List<GameObject> interest_body = new List<GameObject> { }; //интересующие нас обьекты
    private Animator anim;    

    void Start() //метод для активации обьектов
    {
        anim = body.GetComponent<Animator>();     //присваеваем переменной targetI координаты interest
        foreach (GameObject copy in interest_obj)
        {
            addTarget(copy);
        }
    }

    void FixedUpdate() //метод обращающийся ко всем методам в скрипте каждый тик
    {
        CheckObj();
    }

    private void addTarget(GameObject obj)
    {
        interest_body.Add(obj.GetComponent<AttributesManager>().main_body);
    }


    private void CheckObj()
    {
        if(interest_body != null)
        {
            for (int i = 0; i < interest_body.Count; i++)
            {
                if (Vector3.Distance(body.transform.position, interest_body[i].transform.position) < dm.getInt(am.attributes, "view_radius"))
                {
                    CheckRadius(); //сверяем нет ли кого в зоне досягаемости
                }
            }
        }
    }

    private void CheckRadius()
    {
        for (int i = 0; i < interest_body.Count; i++)
        {
            if (am.hit == true)
            {
                anim.SetBool("walk", false);
                anim.SetBool("attack", true);
            }
            else
            {
                if (Vector3.Distance(body.transform.position, interest_body[i].transform.position) < dm.getInt(am.attributes, "view_radius"))
                {
                    anim.SetBool("attack", false);
                    anim.SetBool("walk", true);
                    MyBodyMove(interest_body[i]);
                }
                else
                {
                    Stop();
                }
            }
        }
    }

    private void MyBodyMove(GameObject i_body) //передвижение тела
    {
        Vector3 direction = (i_body.transform.position - body.transform.position).normalized;
        Quaternion lookR = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        body.transform.rotation = Quaternion.Slerp(body.transform.rotation, lookR, Time.deltaTime * dm.getInt(am.attributes, "speed_rotate"));
        body.transform.Translate(new Vector3(0, 0, dm.getInt(am.attributes, "speed_forward") * Time.deltaTime)); //двигаем его вперёд  
    }

    void Stop()
    {
        anim.SetBool("walk", false);
        anim.SetBool("attack", false);
    }
}
