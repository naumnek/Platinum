using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAI : MonoBehaviour
{
    // Detects manually if obj is being seen by the main camera
    public float checkRadius = 6;
    public GameObject target;
    Collider targetCollider;
    Animator anim;

    GameObject player;
    Camera playerCamera;
    Plane[] planes;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCamera = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(playerCamera);
        targetCollider = target.GetComponent<Collider>();
        anim = target.GetComponent<Animator>();
    }

    void Update()
    {
        if (player != null && (player.transform.position - target.transform.position).sqrMagnitude < checkRadius * checkRadius)
        {
            if (GeometryUtility.TestPlanesAABB(planes, targetCollider.bounds))
            {
                anim.SetBool("Open", true);
                Debug.Log(target.name + " has been detected!");
            }
            else
            {
                anim.SetBool("Open", false);
                Debug.Log("Nothing has been detected");
            }
        }
        else
        {
            if (player != null) player = GameObject.FindGameObjectWithTag("Player");          
        }
    }
}
