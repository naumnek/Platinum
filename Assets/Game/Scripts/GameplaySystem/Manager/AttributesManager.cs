using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditorInternal;
using System.Text.RegularExpressions;
using System.Linq;

public class AttributesManager : MonoBehaviour
{
    //PUBLIC
    public bool hit = true;
    public GameObject main_body;
    public List<string> attributes = new List<string> { "strength:1" , "agility:1", "intellect:1", "stamina:10", "power_hit:5", "speed_backward:3", "speed_forward:6", "speed_rotate:2", "view_radius:30" };
    public GameObject[] own_obj;
    private DataManager dm = new DataManager();

    public void getCommand(string command)
    {
        if(command == "die")
        {
            main_body.GetComponent<Animator>().SetBool("die", true);
        }
    }

    public void CheckInteract(LogicCollision target_lc, LogicCollision own_lc)
    {
        for (int i = 0; i < own_obj.Length; i++)
        {
            if (own_obj[i] == target_lc) return;
        }
        parseAllData(target_lc, own_lc);
    }

    private void parseAllData(LogicCollision target_lc, LogicCollision own_lc)
    {

        switch (dm.getStr(own_lc.data, "object"))
        {
            case "item":
                switch (dm.getStr(own_lc.data, "usage"))
                {
                    case "damage":
                        UseWeapon(own_lc, target_lc.gameObject);
                        break;
                    case "protect":
                        break;
                    default:
                        break;
                }
                break;
            case "entity":
                switch (dm.getStr(own_lc.data, "usage"))
                {
                    case "damage":
                        UseWeapon(own_lc, target_lc.gameObject);
                        break;
                    case "protect":
                        break;
                    case "base":
                        break;
                    default:
                        break;
                }
                break;
        }
    }

    private void UseWeapon(LogicCollision own_lc, GameObject target_obj)
    {
        LogicCollision lc_obj = target_obj.GetComponent<LogicCollision>();
        if (hit == false)
        {
            dm.setData(ref lc_obj.data, "durability", Convert.ToString(dm.getInt(lc_obj.data, "durability") - (dm.getInt(own_lc.data, "sharpness") * dm.getInt(own_lc.data, "quality"))));
        }
        hit = true;
        LogicForce(own_lc.this_obj, target_obj);
    }


    private void LogicForce(GameObject obj1, GameObject obj2) //принадлежащий обьект, полученный обьект, 
    {
        Rigidbody obj3 = obj2.GetComponent<Rigidbody>();
        Vector3 forceDirection = (obj1.transform.position - obj2.transform.position).normalized;
        obj3.AddForce(forceDirection * dm.getInt(attributes, "power_hit"), ForceMode.Impulse);
    }
}
