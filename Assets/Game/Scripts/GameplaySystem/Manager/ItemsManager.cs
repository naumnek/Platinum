using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    //PUBLIC
    public List<Item> items = new List<Item> { };
    //PRIVATE
    private Item first_item;

    // Start is called before the first frame update
    void Start()
    {
        /*if (items == null)
        {
            print("item: " + items.Count);
            items.Add(new Item());
            items[items.Count - 1].lvl = 1;
            items[items.Count - 1].bonus = 1;
            items[items.Count - 1].type = "other";
            items[items.Count - 1].species = "none";
            items[items.Count - 1].GenerateItem();
        }*/
    }

    public void GetItem(GameObject obj, out Item item)
    {
        item = null;
        foreach (Item copy in items)
        {
            if (copy.item_obj == obj) item = copy;
        }
    }

    public void CreateItem(GameObject obj, int lvl, string type, string species, out Item item)
    {
        items.Add(new Item());
        items[items.Count - 1].item_obj = obj;
        items[items.Count - 1].lvl = lvl;
        items[items.Count - 1].type = type;
        items[items.Count - 1].species = species;
        item_species(species);
        items[items.Count - 1].GenerateItem();
        item = items[items.Count - 1];
    }

    private void item_species(string species)
    {
        switch (species)
        {
            //weapon
            case ("melee"):
                bonus(0, 0);
                break;
            case ("sword"):
                bonus(24, 24);
                break;
            //protection
            case ("body"):
                bonus(0, 0);
                break;
            case ("shield"):
                bonus(5, 5);
                break;
        }
    }

    private void bonus(int bonus1, int bonus2)
    {
        items[items.Count - 1].bonus1 = bonus1;
        items[items.Count - 1].bonus2 = bonus2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Item
{
    //PUBLIC
    public GameObject item_obj;
    public int lvl;
    public int quality;
    public int bonus1;
    public int bonus2;
    public string type;
    public string species;
    //PRIVATE
    private System.Random ran = new System.Random(); //система рандома 0 или 1 для поворота вправо или влево

    public void GenerateItem()
    {
        quality = ran.Next(quality * (lvl + bonus1), quality * (lvl + bonus2));
    }
}
