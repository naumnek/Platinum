using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class LogicCollision : MonoBehaviour
{
    public List<string> data = new List<string> { "object:item", "type:sword", "status:used", "usage:damage", "name:sword", "level:1" , "quality:1", "durability:100" };
    public GameObject this_obj;
    public AttributesManager am;
    //public ItemsManager im;
    //public Item info_item;

    void Start()
    {
        this_obj = this.gameObject;
        LogicItem();
    }

    void OnCollisionEnter(Collision collision) //активируется при столкновении обьекта и записывает его в collision
    {
        GameObject col_obj = collision.gameObject;
        //print(this_obj.name + " touch: " + collision.gameObject.name);
        if (data[1] != "inactive" && col_obj != this_obj.gameObject && col_obj.GetComponent<LogicCollision>() != null)
        {
            am.CheckInteract(col_obj.GetComponent<LogicCollision>(), this_obj.GetComponent<LogicCollision>());
        }
        collision = null;
    }

    private void LogicItem()
    {
        /*im.GetItem(this_obj, out info_item);
        if (info_item == null)
        {
            im.CreateItem(this_obj, lvl, type, species, out info_item);
        }
        else
        {
            lvl = info_item.lvl;
            quality = info_item.quality;
            type = info_item.type;
            species = info_item.species;
        }*/
    }

    public void OnAttackOver()
    {
        am.hit = false;
    }
}
