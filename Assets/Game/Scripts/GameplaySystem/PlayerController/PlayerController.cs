using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
    //PUBLIC
    public AttributesManager am;
    public GameObject Camera;
    public GameObject body;
    //PRIVATE
    private DataManager dm = new DataManager();
    private Rigidbody rigBody;
    private Animator anim;
    private bool b = false;

    void Start()
    {
        getComponent(); //загрузка данных
    }

    private void getComponent() //загружаем данные
    {
        Cursor.visible = b; //скрыть курсор
        Cursor.lockState = CursorLockMode.Locked;
        anim = body.GetComponent<Animator>(); 
        rigBody = body.GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        posCamera(); //камера следующая за персонажем
        Controller();
    }

    private void posCamera() //камера следует за персонажем
    {
        //Camera.transform.position = new Vector3(body.transform.position.x, Camera.transform.position.y, body.transform.position.z);
    }
    
    void Controller()
    {        
        mouseCheck();
        keyCheck();
    }

    private void mouseCheck() //функции мыши
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) & anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") == false)
        {
            anim.SetTrigger("attack1");
        }
    }

    private void keyCheck() //функции клавиш
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            print("Info: ");
        }

        if (Input.GetKey(KeyCode.W) & anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") == false)
        {
            anim.SetBool("run", true);
            rigBody.AddForce(body.transform.forward * dm.getInt(am.attributes, "speed_forward") * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }

        if (Input.GetKey(KeyCode.S) & anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") == false)
        {
            anim.SetBool("walk", true);
            rigBody.AddForce(-body.transform.forward * dm.getInt(am.attributes, "speed_backward") * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }

        if (Input.GetKeyUp(KeyCode.W) | Input.GetKeyUp(KeyCode.S) | anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") == true)
        {
            anim.SetBool("run", false);
            anim.SetBool("walk", false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            body.transform.rotation *= Quaternion.Euler(0f, -dm.getInt(am.attributes, "speed_rotate") * Time.deltaTime, 0f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            body.transform.rotation *= Quaternion.Euler(0f, dm.getInt(am.attributes, "speed_rotate") * Time.deltaTime, 0f);
        }
        if (Input.GetKey(KeyCode.L))
        {
            b = !b;
            Cursor.visible = b; //скрыть курсор
            if(b) Cursor.lockState = CursorLockMode.None;
            else Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
