using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public GameObject AllManagers;

    void Start()
    {
        if (GameObject.FindGameObjectsWithTag(tag).Length > 1) Destroy(this.gameObject);
        else
        {
            AllManagers.SetActive(true);
            DontDestroyOnLoad(this);
        }
    }
}
