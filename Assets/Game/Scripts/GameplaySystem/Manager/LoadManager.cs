using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace naumnek.FPS
{
    public class LoadManager : MonoBehaviour
    {
        public GameObject general;
        private static LoadManager instance;

        void Start()
        {
            instance = this;
        }

        // Start is called before the first frame update
        public static void Load()
        {
            instance.general.SetActive(true);
        }
    }
}

